namespace TestPipe.Specs
{
	using System;
	using System.Linq;
	using System.Xml.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core;
	using TestPipe.Data.Dto.TestPipeRepo;

	[TestClass]
	[DeploymentItem(@"Data\CacheHellper_TestSuite.xml")]
	[DeploymentItem(@"Data\CacheHellper_Login_feature.xml")]
	[DeploymentItem(@"Data\NoScenarioChildElements.xml")]
	public class CacheHelperSpec
	{
		private XElement featureElement;
		private XElement scenarioElement;
		private XElement suiteElement;

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CacheAddGivenElementDoesNotHaveANameAttributeThenThrowException()
		{
			string key = "test";
			XElement testElement = this.suiteElement.Element(XmlTool.SetupTag);

			CacheHelper.CacheAdd(key, testElement);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CacheAddGivenElementInvalidThenThrowException()
		{
			string key = "test";
			XElement testElement = null;

			CacheHelper.CacheAdd(key, testElement);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void CacheAddGivenKeyInvalidThenThrowException()
		{
			string key = null;
			XElement testElement = this.suiteElement.Element(XmlTool.FeatureTag);

			CacheHelper.CacheAdd(key, testElement);
		}

		[TestMethod]
		public void CacheAddGivenNameIsBaseURLThenAddElementValueToBaseUrl()
		{
			string key = "test";
			string expected = "http://localhost/testpipe.testsite/";
			XElement testElement = this.suiteElement.Element(XmlTool.SetupTag).Elements(XmlTool.VarTag).Where(x => x.Attribute(XmlTool.NameAttribute).Value == "BaseURL").FirstOrDefault();

			CacheHelper.CacheAdd(key, testElement);
			string actual = TestSession.BaseUrl;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CacheAddThenCacheElementValue()
		{
			string key = "test";
			string expected = "Login_feature.xml";
			XElement testElement = this.suiteElement.Element(XmlTool.FeatureTag);

			CacheHelper.CacheAdd(key, testElement);
			string actual = (string)TestSession.Cache[key];

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CacheElementGivenCacheKeyAlreadyInCacheThenNotAddElementToCache()
		{
			string key = string.Empty;
			string rootName = XmlTool.FeatureTag;
			XElement testElement = this.suiteElement.Element(XmlTool.FeatureTag);

			CacheHelper.CacheElement(key, testElement, rootName);
			int expected = TestSession.Cache.Count;
			CacheHelper.CacheElement(key, testElement, rootName);
			int actual = TestSession.Cache.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void CacheElementGivenElementDoesNotHaveANameAttributeThenThrowException()
		{
			string key = string.Empty;
			string rootName = XmlTool.FeatureTag;
			XElement testElement = this.suiteElement.Element(XmlTool.SetupTag);

			CacheHelper.CacheElement(key, testElement, rootName);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CacheElementGivenElementInvalidThenThrowException()
		{
			string key = string.Empty;
			string rootName = XmlTool.FeatureTag;
			XElement testElement = null;

			CacheHelper.CacheElement(key, testElement, rootName);
		}

		[TestMethod]
		public void CacheElementGivenElementNameEqualsScenarioAndElementIsUserThenCacheUserObject()
		{
			string key = string.Empty;
			string cacheKey = "testuser1";
			string rootName = XmlTool.ScenarioTag;
			string expected = "testuser1";
			XElement testElement = this.featureElement.Element(XmlTool.ScenarioTag).Element(XmlTool.ObjectTag).Element(XmlTool.UserTag);

			CacheHelper.CacheElement(key, testElement, rootName);

			User user = TestSession.Cache[cacheKey] as User;
			string actual = user.User_Name;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CacheElementGivenKeyHasValueThenAppendKeyToCacheKey()
		{
			string key = "test";
			string cacheKey = "1";
			string expected = "Login_feature.xml";
			string rootName = XmlTool.FeatureTag;
			XElement testElement = this.suiteElement.Element(XmlTool.FeatureTag);

			CacheHelper.CacheElement(key, testElement, rootName);
			string actual = (string)TestSession.Cache[key + "_" + cacheKey];

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CacheElementsGivenChildThenCacheElements()
		{
			string key = string.Empty;
			XElement testElement = this.suiteElement;
			int expected = 1;

			CacheHelper.CacheElements(testElement, XmlTool.SuiteTag, XmlTool.FeatureTag, key);

			int actual = TestSession.Cache.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CacheElementsGivenElementInvalidThenThrowException()
		{
			string key = string.Empty;
			XElement testElement = null;

			CacheHelper.CacheElements(testElement, XmlTool.SuiteTag, XmlTool.FeatureTag, key);
		}

		[TestMethod]
		public void CacheElementsGivenEmptyChildCacheElements()
		{
			string key = string.Empty;
			XElement testElement = this.scenarioElement;
			int expected = 1;

			CacheHelper.CacheElements(testElement, XmlTool.ScenarioTag, null, key);

			int actual = TestSession.Cache.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "The root element does not contain elements.")]
		public void CacheElementsGivenRootElementContainsNoChildElementsThenThrowException()
		{
			string key = string.Empty;
			XElement testElement = XElement.Load("NoScenarioChildElements.xml");

			CacheHelper.CacheElements(testElement, XmlTool.ScenarioTag, string.Empty, key);
		}

		[TestMethod]
		public void CacheElementsGivenRootNameDoesNotEqualParentThenNotCacheElements()
		{
			string key = string.Empty;
			XElement testElement = this.suiteElement;
			int expected = 0;

			CacheHelper.CacheElements(testElement, XmlTool.FeatureTag, XmlTool.FeatureTag, key);

			int actual = TestSession.Cache.Count;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CacheElementThenCacheElementValue()
		{
			string key = string.Empty;
			string cacheKey = "1";
			string expected = "Login_feature.xml";
			string rootName = XmlTool.FeatureTag;
			XElement testElement = this.suiteElement.Element(XmlTool.FeatureTag);

			CacheHelper.CacheElement(key, testElement, rootName);
			string actual = (string)TestSession.Cache[cacheKey];

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetKeyGivenHasNoPrefixAndHasKeyNameThenReturnKeyName()
		{
			string prefix = null;
			string keyName = "World";
			string expected = "World";

			string actual = CacheHelper.GetKey(prefix, keyName);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetKeyGivenHasNoPrefixAndHasNoKeyNameThenReturnEmptyString()
		{
			string prefix = null;
			string keyName = null;
			string expected = string.Empty;

			string actual = CacheHelper.GetKey(prefix, keyName);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetKeyGivenHasPrefixAndHasKeyNameThenReturnConcatenated()
		{
			string prefix = "Hello";
			string keyName = "World";
			string expected = "Hello_World";

			string actual = CacheHelper.GetKey(prefix, keyName);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetKeyGivenHasPrefixAndHasNoKeyNameThenReturnPrefix()
		{
			string prefix = "Hello";
			string keyName = null;
			string expected = "Hello";

			string actual = CacheHelper.GetKey(prefix, keyName);

			Assert.AreEqual(expected, actual);
		}

		[TestInitialize]
		public void TestSetup()
		{
			TestSession.Cache.Clear();
			this.suiteElement = XElement.Load("CacheHellper_TestSuite.xml");
			this.featureElement = XElement.Load("CacheHellper_Login_feature.xml");
			this.scenarioElement = this.featureElement.Element(XmlTool.ScenarioTag);
		}
	}
}