namespace TestPipe.Specs.Control
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using TestPipe.Common;
    using TestPipe.Core.Control;
    using TestPipe.Core.Enums;
    using TestPipe.Core.Interfaces;
    using TestPipe.Selenium.Browsers;

    [TestClass]
    public class CheckBoxControlSpec
    {
        private IBrowser browserInstance;
        private CheckBox sut;

        private string RootPath
        {
            get
            {
                return ConfigurationManager.AppSettings["htmlsite"];
            }
        }

        private string FormsTestPage
        {
            get
            {
                return this.RootPath + "formPage.html";
            }
        }

        [TestInitialize]
        public void Setup()
        {
            sut = null;

            this.browserInstance.Open(this.FormsTestPage);

            ISelect selector = new Select(FindByEnum.Id, "checky");
            this.sut = new CheckBox(this.browserInstance, selector);

            Assert.IsNotNull(this.sut);
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void CheckShouldCheckCheckBox()
        {
            sut.Uncheck();

            Assert.IsFalse(sut.IsSelected());

            sut.Check();

            Assert.IsTrue(sut.IsSelected());
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void UncheckShouldUncheckCheckBox()
        {
            sut.Check();

            Assert.IsTrue(sut.IsSelected());

            sut.Uncheck();

            Assert.IsFalse(sut.IsSelected());
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void IsCheckedShouldReturnFalse()
        {
            sut.Uncheck();

            Assert.IsFalse(sut.IsChecked());
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void IsCheckedShouldReturnTrue()
        {
            sut.Check();

            Assert.IsTrue(sut.IsChecked());
        }
    }
}