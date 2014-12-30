namespace TestPipe.Common
{
	using System;
    using System.Configuration;
	using log4net;
	using log4net.Appender;
	using log4net.Config;
	using log4net.Core;
	using log4net.Layout;
	using log4net.Repository.Hierarchy;

	public class Logger : ILogManager
	{
		private const string LOGPATTERN = "%d [%t] %-5p %m%n";

		private readonly ILog logger;
		private bool isDebugEnabled;
		private bool isErrorEnabled;
		private bool isFatalEnabled;
		private bool isInfoEnabled;
		private bool isWarnEnabled;
		private PatternLayout layout = new PatternLayout();
        private string logPath = @"Logs";

		public Logger()
		{
            this.logPath = ConfigurationManager.AppSettings["log.path"];
			this.logger = log4net.LogManager.GetLogger(typeof(LogManager));
			XmlConfigurator.Configure();
			this.Configure(this.logPath);
			this.SetLoggingLevelConstants();
		}

		public Logger(ILog logger)
		{
			this.logger = logger;
			this.SetLoggingLevelConstants();
		}

		public void Debug(string message, Exception ex = null)
		{
			Log(this.isDebugEnabled, this.logger.Debug, message, ex);
		}

		public void Error(string message, Exception ex = null)
		{
			Log(this.isErrorEnabled, this.logger.Error, message, ex);
		}

		public void Fatal(string message, Exception ex = null)
		{
			Log(this.isFatalEnabled, this.logger.Fatal, message, ex);
		}

		public void Info(string message, Exception ex = null)
		{
			Log(this.isInfoEnabled, this.logger.Info, message, ex);
		}

		public void Warn(string message, Exception ex = null)
		{
			Log(this.isWarnEnabled, this.logger.Warn, message, ex);
		}

		private static void Log(bool enabled, Action<string, Exception> logAction, string message, Exception exception = null)
		{
			if (!enabled)
			{
				return;
			}

			logAction(message, exception);
		}

		private void Configure(string logPath)
		{
			Hierarchy hierarchy = (log4net.Repository.Hierarchy.Hierarchy)LogManager.GetRepository();
			TraceAppender tracer = new TraceAppender();
			PatternLayout patternLayout = new PatternLayout();

			patternLayout.ConversionPattern = LOGPATTERN;
			patternLayout.ActivateOptions();

			tracer.Layout = patternLayout;
			tracer.ActivateOptions();
			hierarchy.Root.AddAppender(tracer);

            RollingFileAppender rolling = new RollingFileAppender();
            hierarchy.Root.AddAppender(rolling);

			foreach (IAppender a in hierarchy.Root.Appenders)
			{
				if (a is FileAppender)
				{
					FileAppender fa = (FileAppender)a;
					fa.Layout = patternLayout;
					fa.AppendToFile = true;
					fa.File = logPath;
					fa.ActivateOptions();
					break;
				}

				if (a is RollingFileAppender)
				{
					RollingFileAppender rfa = (RollingFileAppender)a;
					rfa.Layout = patternLayout;
					rfa.AppendToFile = true;
					rfa.RollingStyle = RollingFileAppender.RollingMode.Date;
					rfa.MaxSizeRollBackups = 4;
					rfa.MaximumFileSize = "100KB";
					rfa.StaticLogFileName = true;
					rfa.File = logPath;
					rfa.ActivateOptions();
					break;
				}
			}

			hierarchy.Root.Level = Level.All;
			hierarchy.Configured = true;
		}

		private void SetLoggingLevelConstants()
		{
			this.isDebugEnabled = this.logger.IsDebugEnabled;
			this.isInfoEnabled = this.logger.IsInfoEnabled;
			this.isWarnEnabled = this.logger.IsWarnEnabled;
			this.isErrorEnabled = this.logger.IsErrorEnabled;
			this.isFatalEnabled = this.logger.IsFatalEnabled;
		}
	}
}