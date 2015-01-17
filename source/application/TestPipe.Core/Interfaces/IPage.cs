namespace TestPipe.Core.Interfaces
{
    using System;
    using System.Collections.Generic;
    using TestPipe.Core.Control;

    public interface IPage
    {
        IBrowser Browser { get; }

        uint DefaultTimeout { get; }

        string PageRelativeUrl { get; }

        string PageUrl { get; }

        TestEnvironment TestEnvironment { get; }

        string Title { get; }

        BaseControl ActiveControl();

        void AddCookie(string key, string value, string path = "/", string domain = null, DateTime? expiry = null);

        void DeleteCookies(string[] cookieNames = null);

        Dictionary<string, string> GetAllPageCookies();

        string GetCookieValue(string key);

        bool HasTitle(string title);

        bool HasUrl(string url);

        bool IsActiveControlId(string controlId);

        bool IsOpen(uint timeoutInSeconds = 0);

        void Open(string url, uint timeoutInSeconds = 0);

        string PageState();

        void SendKeys(string keys);
    }
}