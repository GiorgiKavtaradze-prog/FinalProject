namespace FinalProjectApp.Logging;

public sealed class FileLogger : IAppLogger
{
    private readonly string _filePath;

    public FileLogger(string filePath)
    {
        _filePath = filePath;
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public void Info(string message) => Write("INFO", message);

    public void Warning(string message) => Write("WARN", message);

    public void Error(string message, Exception exception) => Write("ERROR", $"{message} {exception}");

    private void Write(string level, string message)
    {
        try
        {
            File.AppendAllText(_filePath, $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging message: {ex.Message}");
        }
    }
}
