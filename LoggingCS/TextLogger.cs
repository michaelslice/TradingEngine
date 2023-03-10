using Microsoft.Extensions.Options;
using System.Threading.Tasks.Dataflow;
using TradingEngineServer.Logging.LoggingConfiguration;
using System.Threading;
using System.IO;

namespace TradingEngineServer.Logging
{
    public class TextLogger : AbstractLogger, ITextLogger
    {
        // PRIVATE //
        private readonly LoggerConfiguration _loggingConfiguration;


        public TextLogger(IOptions<LoggerConfiguration> loggingConfiguration) : base()
        {

            _loggingConfiguration = loggingConfiguration.Value ?? throw new ArgumentNullException(nameof(loggingConfiguration));
            if (_loggingConfiguration.LoggerType != LoggerType.Text)
                throw new InvalidOperationException($"{nameof(TextLogger)} doesn't match LoggerType of {_loggingConfiguration.LoggerType}");

            var now = DateTime.Now;
            string logdirectory = Path.Combine(_loggingConfiguration.TextLoggerConfiguration.Directory, $"{now:yyyy-MM-DD}");
            string uniqueLogName = $"{_loggingConfiguration.TextLoggerConfiguration.Filename}-{now:HH_mm_ss}";
            string baseLogName = Path.ChangeExtension(uniqueLogName, _loggingConfiguration.TextLoggerConfiguration.FileExtension);
            string filepath = Path.Combine(logdirectory, baseLogName);
            Directory.CreateDirectory(logdirectory);
            _ = Task.Run(() => LogAsync(filepath, _logQueue, _tokensource.Token));
        }

        private static async Task LogAsync(string filepath, BufferBlock<LogInformation> logQueue, CancellationToken token)
        {

            using var fs = new FileStream(filepath, FileMode.CreateNew, FileAccess.Write, FileShare.Read);
            using var sw = new StreamWriter(fs) { AutoFlush = true, };
            try
            {
                while (true)
                {
                    var logItem = await logQueue.ReceiveAsync(token).ConfigureAwait(false);
                    string formattedMessage = FormatLogItem(logItem);
                    await sw.WriteAsync(formattedMessage).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            { }
        }

        private static string FormatLogItem(LogInformation logItem)
        {
            return $"[{logItem.Now:yyyy-MM-dd HH-mm-ss.fffffff}] [{logItem.ThreadName,-30}:{logItem.ThreadId:000}] "
                   + $"[{logItem.LogLevel}] {logItem.Message}";
        }

        protected override void Log(LogLevel logevel, string module, string message)
        {
            _logQueue.Post(new LogInformation(logLevel, module, message,
                DateTime.Now, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.Name));
        }

        // Think of this as destructer 
        ~TextLogger()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (_lock)
            { 
                if (_disposed)
                return;
                _disposed = true;
            }
            if (disposing)
            {
                // Get rid of managed resources 
                _tokensource.Cancel();
                _tokensource.Dispose();
            }

            //Get rid of unmanaged resources 
        }

        private readonly System.Threading.Tasks.Dataflow.BufferBlock<LogInformation> _logQueue = new BufferBlock<LogInformation>();
        private LogLevel logLevel;
        private readonly CancellationTokenSource _tokensource = new  CancellationTokenSource();
        private readonly string logDirectory;
        private readonly object _lock = new object();
        private bool _disposed = false;
    }    
}
