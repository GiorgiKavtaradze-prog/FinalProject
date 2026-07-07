namespace FinalProjectApp.Logging;

public sealed class FileLogger : IAppLogger
{
    private readonly string _filePath;
    private readonly object _lock = new();

    public FileLogger(string filePath)
    {
        _filePath = filePath;
        var directory = Path.GetDirectoryName(_filePath);
        if (!string.IsNullOrWhiteSpace(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public void Info(string message) => WriteLogEntry("INFO", message);
    public void Warning(string message) => WriteLogEntry("WARN", message);
    public void Error(string message, Exception exception) => WriteLogEntry("ERROR", $"{message} | Exception: {exception.GetType().Name} - {exception.Message}");

    private void WriteLogEntry(string level, string message)
    {
        var logEntry = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss.fff} [{level}] {message}{Environment.NewLine}";
        try
        {
            lock (_lock)
            {
                File.AppendAllText(_filePath, logEntry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Logging error: {ex.Message}");
        }
    }
}