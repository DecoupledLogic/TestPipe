namespace TestPipe.Specs
{
	using System;
	using System.Xml.Linq;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core;
	using TestPipe.Data.Dto.TestPipeRepo;

	[TestClass]
	[DeploymentItem(@"Data\XmlTool_TestSuite.xml")]
	[DeploymentItem(@"Data\XmlTool_Login_feature.xml")]
	[DeploymentItem(@"Data\NoRootNameAttribute.xml")]
	[DeploymentItem(@"Data\EmptyRootNameAttribute.xml")]
	public class XmlToolSpec
	{
		private XmlTool sut;
		private string suitePath;
		private string featurePath;
		private string noRootNameAttributePath;
		private string emptyRootNameAttributePath;
		private XElement testElement;

		[TestInitialize]
		public void TestSetup()
		{
			this.sut = new XmlTool();
			this.suitePath = "XmlTool_TestSuite.xml";
			this.featurePath = "XmlTool_Login_feature.xml";
			this.noRootNameAttributePath = "NoRootNameAttribute.xml";
			this.emptyRootNameAttributePath = "EmptyRootNameAttribute.xml";
			TestSession.Cache.Clear();
		}

		[TestMethod]
		public void CacheSuite_Given_Browser_Then_Cache_Suite_Browser()
		{
			string key = "Suite_TestPipe_Setup_Browser";
			string expected = "IE";
			this.ValidateCacheSuite(key, expected);
		}

		public void ValidateCacheSuite(string key, string expected)
		{
			this.sut.Load(this.suitePath);

			this.sut.CacheSuite();
			string actual = TestSession.Cache[key] as string;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "doc can't be null.")]
		public void CacheSuite_Given_Null_Document_Then_Throw_Exception()
		{
			this.sut.CacheSuite();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "doc \"name\" Attribute is null.")]
		public void CacheSuite_Given_A_Document_Without_Root_Element_Name_Attribute_Then_Throw_Exception()
		{
			this.sut.Load(this.noRootNameAttributePath);
			this.sut.CacheSuite();
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "doc \"name\" Attribute Value is null or white space.")]
		public void CacheSuite_Given_A_Document_With_Empty_Root_Element_Name_Attribute_Then_Throw_Exception()
		{
			this.sut.Load(this.emptyRootNameAttributePath);
			this.sut.CacheSuite();
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "rootTag is invalid for this cache type")]
		public void CacheSuite_Given_A_Feature_Document_Then_Throw_Exception()
		{
			this.sut.Load(this.featurePath);
			this.sut.CacheSuite();
		}

		[TestMethod]
		public void CacheScenario_Given_Scenario_With_User_Then_User_Cached()
		{
			string suiteKey = "Suite_TestPipe";
			string scenarioKey = "1";
			string key = "Suite_TestPipe_Feature_Login_Scenario_1_testuser1";
			string expected = "testuser1";

			this.sut.Load(this.featurePath);

			this.sut.CacheScenario(suiteKey, scenarioKey);
			User user = TestSession.Cache[key] as User;
			string actual = user.User_Name;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "doc can't be null.")]
		public void CacheScenario_Given_Null_Document_Then_Throw_Exception()
		{
			string suiteKey = "Suite_TestPipe";
			string scenarioKey = "1";
			this.sut.CacheScenario(suiteKey, scenarioKey);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "doc \"name\" Attribute is null.")]
		public void CacheScenario_Given_A_Document_Without_Root_Element_Name_Attribute_Then_Throw_Exception()
		{
			string suiteKey = "Suite_TestPipe";
			string scenarioKey = "1";
			this.sut.Load(this.noRootNameAttributePath);
			this.sut.CacheScenario(suiteKey, scenarioKey);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "doc \"name\" Attribute Value is null or white space.")]
		public void CacheScenario_Given_A_Document_With_Empty_Root_Element_Name_Attribute_Then_Throw_Exception()
		{
			string suiteKey = "Suite_TestPipe";
			string scenarioKey = "1";
			this.sut.Load(this.emptyRootNameAttributePath);
			this.sut.CacheScenario(suiteKey, scenarioKey);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "rootTag is invalid for this cache type")]
		public void CacheScenario_Given_A_Feature_Document_Then_Throw_Exception()
		{
			string suiteKey = "Suite_TestPipe";
			string scenarioKey = "1";
			this.sut.Load(this.suitePath);
			this.sut.CacheScenario(suiteKey, scenarioKey);
		}

		[TestMethod]
		public void GetElementByName_Then_Return_Element()
		{ 
			string elementName = XmlTool.ScenarioTag;
			string expected = "1";
			this.testElement = XElement.Load(this.featurePath);

			XElement element = this.sut.GetElementByName(elementName, expected, this.testElement);
			string actual = element.Attribute(XmlTool.NameAttribute).Value;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetElementByName_Given_Loaded_Feature_Then_Return_Element()
		{
			string elementName = XmlTool.ScenarioTag;
			string expected = "1";
			this.sut.Load(this.featurePath);

			XElement element = this.sut.GetElementByName(elementName, expected);
			string actual = element.Attribute(XmlTool.NameAttribute).Value;

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetElementByName_Given_Empty_Element_Name_Then_Return_Null()
		{
			string elementName = string.Empty;
			string name = "1";
			string expected = null;
			this.testElement = XElement.Load(this.featurePath);

			XElement actual = this.sut.GetElementByName(elementName, name, this.testElement);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetElementByName_Given_Empty_Name_Then_Return_Null()
		{
			string elementName = XmlTool.ScenarioTag;
			string name = string.Empty;
			string expected = null;
			this.testElement = XElement.Load(this.featurePath);

			XElement actual = this.sut.GetElementByName(elementName, name, this.testElement);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetElementByName_Given_Null_Document_And_Null_Element_Then_Return_Null()
		{
			string elementName = string.Empty;
			string name = "1";
			string expected = null;

			XElement actual = this.sut.GetElementByName(elementName, name);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void GetElementCacheKey_Given_A_Suite_Root_Then_Return_Suite_Key()
		{
			string expected = "Suite_TestPipe";
			this.sut.Load(this.suitePath);

			string actual = this.sut.GetElementCacheKey(XmlTool.SuiteTag);

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException), "doc can't be null.")]
		public void GetElementCacheKey_Given_A_Null_Document_Then_Throw_Exception()
		{
			this.sut.GetElementCacheKey(XmlTool.SuiteTag);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "doc \"name\" Attribute is null.")]
		public void GetElementCacheKey_Given_A_Document_Without_Root_Element_Name_Attribute_Then_Throw_Exception()
		{
			this.sut.Load(this.noRootNameAttributePath);
			this.sut.GetElementCacheKey(XmlTool.SuiteTag);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException), "doc \"name\" Attribute Value is null or white space.")]
		public void GetElementCacheKey_Given_A_Document_With_Empty_Root_Element_Name_Attribute_Then_Throw_Exception()
		{
			this.sut.Load(this.emptyRootNameAttributePath);
			this.sut.GetElementCacheKey(XmlTool.SuiteTag);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException), "rootTag is invalid for this cache type")]
		public void GetElementCacheKey_Given_A_Feature_Document_Then_Throw_Exception()
		{
			this.sut.Load(this.featurePath);
			this.sut.GetElementCacheKey(XmlTool.SuiteTag);
		}

		[TestMethod]
		public void Load_Then_Load_Document()
		{
			this.sut.Load(this.suitePath);
			Assert.IsTrue(this.sut.Document != null);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void Load_Given_Empty_File_Path_Then_Load_Document()
		{
			this.sut.Load(string.Empty);
			Assert.IsTrue(this.sut.Document != null);
		}
	}
}
