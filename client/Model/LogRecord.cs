namespace client.Model;

public class LogRecord
{
    public string Message { get; set; }

    public LogRecord(string message)
    {
        Message = message;
    }
}