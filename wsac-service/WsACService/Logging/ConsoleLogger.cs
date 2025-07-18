namespace WsACService.Logging;

public class ConsoleLogger(string name) : ILogger
{
    private static readonly Lock Lock = new();
    
    private Dictionary<LogLevel, ConsoleColor> ColorMap { get; } = new()
    {
        [LogLevel.Trace]       = ConsoleColor.Gray,
        [LogLevel.Debug]       = ConsoleColor.Cyan,
        [LogLevel.Information] = ConsoleColor.Green,
        [LogLevel.Warning]     = ConsoleColor.Yellow,
        [LogLevel.Error]       = ConsoleColor.Red,
        [LogLevel.Critical]    = ConsoleColor.Magenta,
    };
    
    public void Log<TState>(
        LogLevel                         logLevel,
        EventId                          eventId,
        TState                           state,
        Exception?                       exception,
        Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
            return;

        lock (Lock)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = ColorMap[logLevel];

            Console.WriteLine($"[{eventId.Id,2}: {logLevel,-12}]    {name} - {formatter(state, exception)}");

            Console.ForegroundColor = old;
        }
    }

    public bool IsEnabled(LogLevel _) => true;

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
}
