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
		IDomSearchContext BrowserSearchContext { get; }

		BrowserTypeEnum BrowserType { get; }

		string CurrentWindowHandle { get; }

		string PageSource { get; }

		string Title { get; }

		string Url { get; }

		ReadOnlyCollection<string> WindowHandles { get; }

		IElement ActiveElement();

		void AddCookie(string key, string value, string path = "/", string domain = null, DateTime? expiry = null);

		void Close();

		void Back();

		void DeleteAllCookies();

		void DeleteCookieNamed(string name);

		object ExecuteAsyncScript(string script, params object[] args);

		object ExecuteScript(string script, params object[] args);

		Dictionary<string, string> GetAllCookies();

		bool HasUrl(string pageUrl);

        void HoverElement(IElement element);

        void LoadBrowser(BrowserTypeEnum browser, BrowserConfiguration configuration = null);

		void MaximizeWindow();

		void MoveToElement(IElement element);

		void Open(string url, uint timeoutInSeconds = 0);

		void Quit();

		void Refresh();

		void SendBrowserKeys(string keys);

		void SetWindowPosition(Point point);

		void SetWindowSize(Size size);

		void SwitchTo(string window);

		void TakeScreenshot(string screenshotPath, ImageFormat format);

		void WaitForPageLoad(uint timeoutInSeconds = 0, string title = "");
	}
}