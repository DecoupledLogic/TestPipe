namespace TestPipe.Core.Page
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using TestPipe.Core;
	using TestPipe.Core.Control;
	using TestPipe.Core.Interfaces;

	public class BasePage : IPage
	{
		private IBrowser browser = null;

		private string pageUrl;

		public BasePage(IBrowser browser, TestEnvironment testEnvironment)
		{
			this.browser = browser;
			this.TestEnvironment = testEnvironment;
		}

		public IBrowser Browser
		{
			get
			{
				if (this.browser == null)
				{
					return TestSession.Browser;
				}
				else
				{
					return this.browser;
				}
			}
		}

		public long PageLoadTime { get; set; }

		public string PageRelativeUrl
		{
			get;
			set;
		}

		public string PageUrl
		{
			get
			{
				if (string.IsNullOrEmpty(this.pageUrl))
				{
					this.pageUrl = this.GetPageUrl();
				}
				return this.pageUrl;
			}
		}

		public TestEnvironment TestEnvironment
		{
			get;
			private set;
		}

		public string Title
		{
			get;
			set;
		}

		public BaseControl ActiveControl()
		{
			IElement element = this.Browser.ActiveElement();
			BaseControl control = new BaseControl(this.Browser, element);
			return control;
		}

		public void AddCookie(string key, string value, string path = "/", string domain = null, DateTime? expiry = null)
		{
			this.Browser.AddCookie(key, value, path, domain, expiry);
		}

		public void DeleteCookies(string[] cookieNames = null)
		{
			if (cookieNames == null || cookieNames.Length < 1)
			{
				this.Browser.DeleteAllCookies();
				return;
			}

			foreach (string name in cookieNames)
			{
				this.Browser.DeleteCookieNamed(name);
			}
		}

		public void EnterText(BaseControl control, string text)
		{
			control.Clear();

			if (string.IsNullOrWhiteSpace(text))
			{
				return;
			}

			control.TypeText(text);
		}

		public BaseControl GetActiveControlById(string controlId)
		{
			string error = string.Empty;

			BaseControl control = this.ActiveControl();
			string id = control.Id;

			if (string.IsNullOrWhiteSpace(id))
			{
				throw new InvalidOperationException("Element: " + controlId + " does not have an id.");
			}

			if (id != controlId)
			{
				throw new InvalidOperationException("Element: " + controlId + " does not have focus.");
			}

			return control;
		}

		public Dictionary<string, string> GetAllPageCookies()
		{
			return this.browser.GetAllCookies();
		}

		public BaseControl GetBaseControl(ISelect select, uint timeoutInSeconds = 0, bool displayed = false)
		{
			return new BaseControl(this.Browser, select, "", timeoutInSeconds, displayed);
		}

		public string GetCookieValue(string key)
		{
			return this.GetAllPageCookies().FirstOrDefault(x => x.Key == key).Value;
		}

		public bool HasTitle(string title)
		{
			if (title == null)
			{
				return false;
			}

			return this.Browser.Title.Trim() == title;
		}

		public bool HasUrl(string url)
		{
			if (string.IsNullOrEmpty(url))
			{
				return false;
			}

			return this.Browser.HasUrl(url);
		}

		public bool IsActiveControlId(string controlId)
		{
			return this.GetActiveControlById(controlId) != null;
		}

		public virtual bool IsOpen(uint timeoutInSeconds = 0)
		{
			if (timeoutInSeconds == 0)
			{
				return this.IsPageOpen();
			}

			int interval = 500;//half second
			long startTime = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
			long timeout = timeoutInSeconds * 1000;

			while(!this.IsPageOpen())
			{
				System.Threading.Thread.Sleep(interval);

				long elapsedTime = (DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond) - startTime;

				if (elapsedTime > timeout)
				{
					return this.IsPageOpen();
				}
			}

			return this.IsPageOpen();
		}

		private bool IsPageOpen()
		{
			bool isOpen = false;
			bool titleMatch = true;

			if (!string.IsNullOrEmpty(this.Title))
			{
				titleMatch = this.Browser.Title.Trim() == this.Title;
				isOpen = titleMatch;
			}

			if (string.IsNullOrEmpty(this.PageRelativeUrl))
			{
				return isOpen;
			}

			bool urlMatch = this.Browser.HasUrl(this.PageUrl);

			return titleMatch & urlMatch;
		}

		public virtual void Open(string url = "", uint timeoutInSeconds = 0)
		{
			if (timeoutInSeconds == 0)
			{
				timeoutInSeconds = (uint)TestSession.Timeout.TotalSeconds;
			}

			Stopwatch timer = new Stopwatch();
			if (!string.IsNullOrWhiteSpace(url))
			{
				timer.Start();
				this.Browser.Open(url, timeoutInSeconds);
			}
			else
			{
				timer.Start();
				this.Browser.Open(this.PageUrl, timeoutInSeconds);
			}

			timer.Stop();
			this.PageLoadTime = timer.ElapsedMilliseconds;
		}

		public virtual void OpenWithQueryString(string queryString, string url = "", uint timeoutInSeconds = 0)
		{
			if (string.IsNullOrWhiteSpace(queryString))
			{
				this.Open(url, timeoutInSeconds);
				return;
			}

			if (!queryString.StartsWith("?"))
			{
				queryString = "?" + queryString;
			}

			if (!string.IsNullOrWhiteSpace(url))
			{
				this.Open(url + queryString, timeoutInSeconds);
				return;
			}

			this.Browser.Open(this.PageUrl + queryString, timeoutInSeconds);
		}

		public string PageState()
		{
			return string.Format("The opend page was not expected, title: {0}\n, url: {1}\n source: {2}", this.Browser.Title, this.Browser.Url, this.Browser.PageSource);
		}

		public virtual dynamic Refresh()
		{
			this.Browser.Refresh();
			return new BasePage(this.Browser, this.TestEnvironment);
		}

		public void SendBrowserKeys(string keys)
		{
			this.browser.SendBrowserKeys(keys);
		}

		public void SendKeys(string keys)
		{
			this.browser.SendBrowserKeys(keys);
		}

		private string GetPageUrl()
		{
			if (this.TestEnvironment == null)
			{
				return string.Empty;
			}

			string baseUrl = this.TestEnvironment.BaseUrl;
			string virtualUrl = this.PageRelativeUrl;
			if (string.IsNullOrEmpty(baseUrl))
			{
				return string.Empty;
			}

			if (!baseUrl.EndsWith("/"))
			{
				baseUrl += "/";
			}

			if (virtualUrl.StartsWith("/"))
			{
				virtualUrl = virtualUrl.Substring(1);
			}

			return string.Format("{0}{1}", baseUrl, virtualUrl);
		}
	}
}