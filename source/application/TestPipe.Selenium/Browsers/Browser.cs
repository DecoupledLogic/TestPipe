namespace TestPipe.Selenium.Browsers
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.ComponentModel.Composition;
	using System.Drawing;
	using System.Drawing.Imaging;
	using System.Linq;
	using Ninject;
	using OpenQA.Selenium;
	using OpenQA.Selenium.Interactions;
	using OpenQA.Selenium.Remote;
	using TestPipe.Common;
	using TestPipe.Core.Browser;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;
	using TestPipe.Selenium.Common;

	[Export(typeof(IBrowser))]
	public class Browser : IBrowser, IDisposable
	{
		private SeleniumBrowserSearchContext searchContext;

		private IWebDriver webDriver;

		public Browser()
		{
		}

		public Browser(BrowserTypeEnum browser)
		{
			this.LoadBrowser(browser);
		}

		public IDomSearchContext BrowserSearchContext
		{
			get { return this.searchContext; }
		}

		public BrowserTypeEnum BrowserType
		{
			get;
			private set;
		}

		public string CurrentWindowHandle
		{
			get { return this.webDriver.CurrentWindowHandle; }
		}

		public ISearchContext Driver
		{
			get { return this.webDriver; }
		}

		[Inject]
		public ILogManager Logger { get; set; }

		public string PageSource
		{
			get { return this.webDriver.PageSource; }
		}

		public string Title
		{
			get { return this.webDriver.Title; }
		}

		public string Url
		{
			get { return this.webDriver.Url; }
		}

		public ReadOnlyCollection<string> WindowHandles
		{
			get { return this.webDriver.WindowHandles; }
		}

		public IElement ActiveElement()
		{
			return this.BrowserSearchContext.GetActiveElement();
		}

		public void AddCookie(string key, string value, string path = "/", string domain = null, DateTime? expiry = null)
		{
			Cookie cookie = new Cookie(key, value, domain, path, expiry);

			this.webDriver.Manage().Cookies.AddCookie(cookie);
		}

		public void Close()
		{
			this.webDriver.Close();
		}

		public void DeleteAllCookies()
		{
			this.webDriver.Manage().Cookies.DeleteAllCookies();
		}

		public void DeleteCookieNamed(string name)
		{
			this.webDriver.Manage().Cookies.DeleteCookieNamed(name);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public object ExecuteAsyncScript(string script, params object[] args)
		{
			IJavaScriptExecutor js = this.webDriver as IJavaScriptExecutor;
			return js.ExecuteAsyncScript(script, args);
		}

		public object ExecuteScript(string script, params object[] args)
		{
			IJavaScriptExecutor js = this.webDriver as IJavaScriptExecutor;
			return js.ExecuteScript(script, args);
		}

		public Dictionary<string, string> GetAllCookies()
		{
			return this.webDriver.Manage().Cookies.AllCookies.ToDictionary(cookie => cookie.Name, cookie => cookie.Value);
		}

		public bool HasUrl(string pageUrl)
		{
			return this.webDriver.Url.ToLower().Contains(pageUrl.ToLower());
		}

		public void LoadBrowser(BrowserTypeEnum browser, BrowserConfiguration configuration = null)
		{
			this.BrowserType = browser;
			this.LoadDriver(browser, configuration);
			this.LoadSearchContext();
		}

		public void MaximizeWindow()
		{
			this.webDriver.Manage().Window.Maximize();
		}

		public void MoveToElement(IElement element)
		{
			Actions actions = new Actions(this.webDriver);

			actions.MoveToElement(element.NativeElement);
		}

		public void Open(string url, uint timeoutInSeconds = 0)
		{
			this.webDriver.Navigate().GoToUrl(url);
			this.WaitForPageLoad(timeoutInSeconds);
		}

		public void Quit()
		{
			this.webDriver.Quit();
		}

		public void Refresh()
		{
			this.webDriver.Navigate().Refresh();
		}

		public void SendBrowserKeys(string keys)
		{
			Actions actions = new Actions(this.webDriver);

			actions.SendKeys(keys + Keys.Null).Perform();
		}

		public void SetWindowPosition(Point point)
		{
			this.webDriver.Manage().Window.Position = point;
		}

		public void SetWindowSize(Size size)
		{
			this.webDriver.Manage().Window.Size = size;
		}

		public void SwitchTo(string window)
		{
			this.webDriver.SwitchTo().Window(window);
		}

		public void TakeScreenshot(string screenshotPath, ImageFormat format)
		{
			try
			{
				ITakesScreenshot screenshotDriver = this.webDriver as ITakesScreenshot;
				Screenshot screenshot = screenshotDriver.GetScreenshot();
				screenshot.SaveAsFile(screenshotPath, format);
			}
			catch (Exception ex)
			{
				this.Logger.Error("Error taking screen shot.", ex);
			}
		}

		public void WaitForPageLoad(uint timeoutInSeconds = 0, string pageTitle = "")
		{
			this.webDriver.WaitForPageLoad(timeoutInSeconds, pageTitle);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.webDriver.Dispose();
			}
		}

		private DesiredCapabilities GetCapabiliities(BrowserConfiguration configuration)
		{
			if (configuration == null)
			{
				throw new ArgumentNullException("BrowserConfiguration is null and must be specified for Remote Browsers.");
			}

			DesiredCapabilities capabilities;

			switch (configuration.BrowserType)
			{
				case BrowserTypeEnum.Chrome:
					{
						capabilities = OpenQA.Selenium.Remote.DesiredCapabilities.Chrome();
						break;
					}
				case BrowserTypeEnum.FireFox:
					{
						capabilities = OpenQA.Selenium.Remote.DesiredCapabilities.Firefox();
						break;
					}
				case BrowserTypeEnum.Headless:
					{
						capabilities = OpenQA.Selenium.Remote.DesiredCapabilities.PhantomJS();
						break;
					}
				case BrowserTypeEnum.IE:
					{
						capabilities = OpenQA.Selenium.Remote.DesiredCapabilities.InternetExplorer();
						break;
					}
				default:
					{
						throw new ArgumentOutOfRangeException("BrowserType", "You must use a valid Browser Type.");
					}
			}

			capabilities.SetCapability(CapabilityType.Platform, configuration.Platform);
			string browserName = configuration.BrowserType.ToString().ToLower();

			if (configuration.BrowserType == BrowserTypeEnum.IE)
			{
				browserName = "internet explorer";
			}

			capabilities.SetCapability(CapabilityType.BrowserName, browserName);
			capabilities.SetCapability(CapabilityType.Version, configuration.Version);
			Proxy proxy = new Proxy();
			proxy.IsAutoDetect = true;
			proxy.Kind = ProxyKind.AutoDetect;
			capabilities.SetCapability(CapabilityType.Proxy, proxy);
			return capabilities;
		}

		private void LoadDriver(BrowserTypeEnum browser, BrowserConfiguration configuration = null)
		{
			DesiredCapabilities capabilities = null;

			if (configuration != null)
			{
				capabilities = this.GetCapabiliities(configuration);
			}

			switch (browser)
			{
				case BrowserTypeEnum.Chrome:
					{
						this.webDriver = new OpenQA.Selenium.Chrome.ChromeDriver();
						break;
					}
				case BrowserTypeEnum.FireFox:
					{
						this.webDriver = new OpenQA.Selenium.Firefox.FirefoxDriver(OpenQA.Selenium.Remote.DesiredCapabilities.Firefox());
						break;
					}
				case BrowserTypeEnum.Headless:
					{
						this.webDriver = new OpenQA.Selenium.PhantomJS.PhantomJSDriver();
						break;
					}
				case BrowserTypeEnum.IE:
					{
						Proxy proxy = new Proxy();

						//proxy.IsAutoDetect = true;
						proxy.Kind = ProxyKind.Manual;

						var options = new OpenQA.Selenium.IE.InternetExplorerOptions
						{
							EnsureCleanSession = true,
							Proxy = proxy,
							UsePerProcessProxy = true
						};

						this.webDriver = new OpenQA.Selenium.IE.InternetExplorerDriver(options);
						break;
					}
				case BrowserTypeEnum.Remote:
					{
						//this.webDriver = new RemoteWebDriver(configuration.RemotePath, capabilities, configuration.Timeout);
						this.webDriver = new RemoteWebDriver(capabilities);
						break;
					}
				case BrowserTypeEnum.None:
					{
						throw new ArgumentOutOfRangeException("BrowserTypeEnum.None", "You must use a valid Browser.");
					}
			}
		}

		private void LoadSearchContext()
		{
			this.searchContext = new SeleniumBrowserSearchContext(this, this.webDriver);
		}
	}
}