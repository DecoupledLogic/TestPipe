namespace TestPipe.Core.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using System.Drawing.Imaging;
	using TestPipe.Core.Browser;
	using TestPipe.Core.Enums;

	public interface IBrowser
	{
		IBrowserSearchContext BrowserSearchContext { get; }

		BrowserTypeEnum BrowserType { get; }

		string CurrentWindowHandle { get; }

		string PageSource { get; }

		string Title { get; }

		string Url { get; }

		ReadOnlyCollection<string> WindowHandles { get; }

		IElement ActiveElement();

		void Close();

		void DeleteAllCookies();

		void DeleteCookieNamed(string name);

		Dictionary<string, string> GetAllCookies();

		bool HasUrl(string pageUrl);

		void LoadBrowser(BrowserTypeEnum browser, BrowserConfiguration configuration = null);

		void Open(string url, uint timeoutInSeconds = 0);

		void MoveToElement(IElement element);

		void Quit();

		void Refresh();

		void SendBrowserKeys(string keys);

		void SwitchTo(string window);

		void TakeScreenshot(string screenshotPath, ImageFormat format);

		void AddCookie(string key, string value, string path = "/", string domain = null, DateTime? expiry = null);

		void WaitForPageLoad(uint timeoutInSeconds = 0);

		void MaximizeWindow();

		void SetWindowSize(Size size);

		void SetWindowPosition(Point point);

		object ExecuteAsyncScript(string script, params object[] args);

		object ExecuteScript(string script, params object[] args);
	}
}