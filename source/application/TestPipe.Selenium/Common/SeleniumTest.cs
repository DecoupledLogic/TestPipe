namespace TestPipe.SeleniumTestInstance.Common
{
    using System;
    using System.Collections.Specialized;
    using TestPipe.Common.Enum;
    using TestPipe.Controller;
    using TestPipe.Design.Application;
    using TestPipe.Design.Domain;
    using TestPipe.SeleniumTestInstance.Browsers;

    public class SeleniumTest
    {
        public static readonly string ConfigBrowser = "browser";

        private NameValueCollection appSettings;
        private DesignQuery designQuery;

        public SeleniumTest(NameValueCollection appSettings, string featureTitle)
        {
            this.appSettings = appSettings;
            this.Runner = new TestRunner(appSettings, featureTitle);
            this.designQuery = new DesignQuery();
            this.SetBrowser();
        }

        public Browser Browser { get; private set; }

        public BrowserTypeEnum BrowserType { get; private set; }

        public TestRunner Runner { get; private set; }

        public void InitizlizePages(ScenarioTest scenario)
        {
        }

        private void SetBrowser()
        {
            string browserName = this.appSettings[ConfigBrowser];
            BrowserTypeEnum browserType = BrowserTypeEnum.IE;
            Enum.TryParse<BrowserTypeEnum>(browserName, true, out browserType);
            this.BrowserType = browserType;
            this.Browser = new Browser(this.BrowserType);
        }
    }
}