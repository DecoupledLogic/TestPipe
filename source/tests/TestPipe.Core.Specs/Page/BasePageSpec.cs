﻿namespace TestPipe.Specs.Page
{
	using System;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using Moq;
	using TestPipe.Core;
	using TestPipe.Core.Interfaces;
	using TestPipe.Core.Page;

	[TestClass]
	public class BasePageSpec
	{
		private BasePage sut;

		[TestMethod]
		public void IsOpenGivenEmptyTitleAndValidUrlShouldReturnTrue()
		{
			string title = string.Empty;
			string url = "default.aspx";
			this.InitializeSut(title, url);
			bool expected = true;

			bool actual = this.sut.IsOpen();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsOpenGivenValidTitleAndValidUrlShouldReturnTrue()
		{
			string title = "Hello";
			string url = "default.aspx";
			this.InitializeSut(title, url);
			bool expected = true;

			bool actual = this.sut.IsOpen();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsOpenGivenValidTitleAndEmptyUrlShouldReturnTrue()
		{
			string title = "Hello";
			string url = string.Empty;
			this.InitializeSut(title, url);
			bool expected = false;

			bool actual = this.sut.IsOpen();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void IsOpenGivenEmptyTitleAndEmptyUrlShouldReturnFalse()
		{
			string title = string.Empty;
			string url = string.Empty;
			this.InitializeSut(title, url);
			bool expected = false;

			bool actual = this.sut.IsOpen();

			Assert.AreEqual(expected, actual);
		}

		private static IBrowser GetBrowser(string title, string url, TestEnvironment environment)
		{
			var browser = new Mock<IBrowser>();
			browser.Setup(x => x.Url).Returns(url);
			browser.Setup(x => x.HasUrl(environment.BaseUrl + url)).Returns(true);
			browser.Setup(x => x.Title).Returns(title);
			return browser.Object;
		}

		private static TestEnvironment GetEnvironment()
		{
			TestEnvironment environment = new TestEnvironment();
			environment.BaseUrl = "http://localhost/";
			return environment;
		}

		private void InitializeSut(string title, string url)
		{
			TestEnvironment environment = GetEnvironment();
			var browser = GetBrowser(title, url, environment);

			this.sut = new BasePage(browser, environment);
			this.sut.PageRelativeUrl = url;
		}
	}
}