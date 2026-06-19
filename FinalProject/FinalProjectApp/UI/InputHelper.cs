using System.Globalization;

namespace FinalProjectApp.UI;

public static class InputHelper
{
    public static int GetIntInput(string message)
    {
        return ReadUntilValid(
            message,
            static input => int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
                ? ParseResult<int>.Success(value)
                : ParseResult<int>.Failure(),
            "Invalid format. Please enter an integer.");
    }

    public static int[] GetIntArrayInput(int count, string message)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count);

        return ReadUntilValid(
            message,
            input => TryParseIntArray(input, count),
            $"Please enter exactly {count} valid integer numbers separated by spaces.");
    }

    public static string GetStringInput(string message)
    {
        return ReadRequired(message);
    }

    public static string ReadRequired(string message, string errorMessage = "Value is required.")
    {
        return ReadUntilValid(
            message,
            static input => string.IsNullOrWhiteSpace(input)
                ? ParseResult<string>.Failure()
                : ParseResult<string>.Success(input.Trim()),
            errorMessage);
    }

    public static decimal ReadPositiveDecimal(
        string message,
        string errorMessage = "Please enter a positive numeric amount. Example: 100.50")
    {
        return ReadUntilValid(
            message,
            static input =>
            {
                var isValid = decimal.TryParse(
                    input,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out var amount);

                return isValid && amount > 0
                    ? ParseResult<decimal>.Success(decimal.Round(amount, 2, MidpointRounding.AwayFromZero))
                    : ParseResult<decimal>.Failure();
            },
            errorMessage);
    }

    public static TEnum ReadEnumChoice<TEnum>(
        string message,
        IReadOnlyList<TEnum> values,
        string errorMessage = "Invalid selection.")
        where TEnum : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(values);

        if (values.Count == 0)
        {
            throw new ArgumentException("At least one value is required.", nameof(values));
        }

        return ReadUntilValid(
            message,
            input => TryParseEnumChoice(input, values),
            errorMessage);
    }

    private static TValue ReadUntilValid<TValue>(
        string message,
        Func<string, ParseResult<TValue>> parser,
        string errorMessage,
        TextReader? reader = null,
        TextWriter? writer = null)
    {
        ArgumentNullException.ThrowIfNull(parser);

        reader ??= Console.In;
        writer ??= Console.Out;

        while (true)
        {
            writer.Write(message);
            var input = reader.ReadLine();

            if (input is not null)
            {
                var result = parser(input);
                if (result.IsValid)
                {
                    return result.Value;
                }
            }

            writer.WriteLine(errorMessage);
        }
    }

    private static ParseResult<int[]> TryParseIntArray(string input, int expectedCount)
    {
        var parsedNumbers = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(TryParseInt)
            .ToArray();

        return parsedNumbers.Length == expectedCount && parsedNumbers.All(number => number.IsValid)
            ? ParseResult<int[]>.Success(parsedNumbers.Select(number => number.Value).ToArray())
            : ParseResult<int[]>.Failure();
    }

    private static ParseResult<TEnum> TryParseEnumChoice<TEnum>(string input, IReadOnlyList<TEnum> values)
        where TEnum : struct, Enum
    {
        if (!int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var selectedNumber))
        {
            return ParseResult<TEnum>.Failure();
        }

        return values
            .Select((value, index) => new { Number = index + 1, Value = value })
            .Where(option => option.Number == selectedNumber)
            .Select(option => ParseResult<TEnum>.Success(option.Value))
            .DefaultIfEmpty(ParseResult<TEnum>.Failure())
            .Single();
    }

    private static ParseResult<int> TryParseInt(string input)
    {
        return int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out var value)
            ? ParseResult<int>.Success(value)
            : ParseResult<int>.Failure();
    }

    private readonly record struct ParseResult<T>(bool IsValid, T Value)
    {
        public static ParseResult<T> Success(T value)
        {
            return new ParseResult<T>(true, value);
        }

        public static ParseResult<T> Failure()
        {
            return new ParseResult<T>(false, default!);
        }
    }
}
