namespace TestPipe.Core.Browser
{
	using System;
	using System.Collections.Generic;
	using TestPipe.Core.Enums;

	public class BrowserConfiguration
	{
		private readonly Dictionary<string, object> capabilities = new Dictionary<string, object>();
		private readonly string platformDefault = "WINDOWS";
		private readonly Uri remotePathDefault = new Uri("http://localhost:4444/wd/hub");
		private string platform;
		private Uri remotePath;

		public BrowserTypeEnum BrowserType { get; private set; }

		public bool IsJavaScriptEnabled { get; private set; }

		public string Platform
		{
			get
			{
				if (string.IsNullOrWhiteSpace(this.platform))
				{
					this.platform = this.platformDefault;
				}

				return this.platform.ToUpper();
			}
			private set
			{
				this.platform = value;
			}
		}

		public Uri RemotePath
		{
			get
			{
				if (this.remotePath == null)
				{
					this.remotePath = this.remotePathDefault;
				}
				return this.remotePath;
			}
			private set
			{
				this.remotePath = value;
			}
		}

		public TimeSpan Timeout { get; private set; }

		public string Version { get; private set; }

		internal Dictionary<string, object> Capabilities
		{
			get { return this.capabilities; }
		}

		public object GetCapability(string capability)
		{
			object capabilityValue = null;
			if (this.capabilities.ContainsKey(capability))
			{
				capabilityValue = this.capabilities[capability];
			}

			return capabilityValue;
		}

		public void SetCapability(string capability, object capabilityValue)
		{
			this.capabilities[capability] = capabilityValue;
		}
	}
}