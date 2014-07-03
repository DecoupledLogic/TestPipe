namespace TestPipe.Core
{
	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;

	public class XmlTool
	{
		public const string DatabaseAttribute = "database";
		public const string DataTag = "Data";
		public const string DescriptionAttribute = "description";
		public const string FeatureTag = "Feature";
		public const string KeyAttribute = "key";
		public const string NameAttribute = "name";
		public const string NamespaceAttribute = "namespace";
		public const string ObjectTag = "Objects";
		public const string ParamTag = "Param";
		public const string RoleTag = "Role";
		public const string ScenarioTag = "Scenario";
		public const string SeedAttribute = "seed";
		public const string SetupTag = "Setup";
		public const string SuiteTag = "Suite";
		public const string TokenCategoryTag = "Token_Category";
		public const string TokenTag = "Token";
		public const string UserTag = "User";
		public const string VarListTag = "VarList";
		public const string VarTag = "Var";
		public const string VendorTag = "Vendor";

		public XmlTool()
		{
			this.Document = null;
		}

		public XElement Document { get; private set; }

		public void CacheFeature(string suiteKey, string featureKey)
		{
			string rootCacheKey = string.Format("{0}{1}", suiteKey, this.GetElementCacheKey(XmlTool.FeatureTag));

			this.ValidateElement(this.Document, FeatureTag);

			XElement featureObjects = this.Document.Element(ObjectTag);

			CacheHelper.CacheElements(this.Document, FeatureTag, string.Empty, rootCacheKey);
		}

		public void CacheScenario(string suiteKey, string scenarioKey)
		{
			string rootCacheKey = string.Format("{0}_{1}", suiteKey, this.GetElementCacheKey(XmlTool.FeatureTag));

			XElement element = this.GetElementByName(ScenarioTag, scenarioKey);

			string scenarioCacheKey = string.Format("{0}_{1}", rootCacheKey, this.GetElementCacheKey(string.Empty, element));

			CacheHelper.CacheElements(element, XmlTool.ScenarioTag, string.Empty, scenarioCacheKey);
		}

		public void CacheSuite()
		{
			string cacheKey = this.GetElementCacheKey(SuiteTag);

			CacheHelper.CacheElements(this.Document.Element(SetupTag), SetupTag, VarTag, cacheKey + "_Setup");
		}

		public XElement GetElementByName(string elementName, string name, XElement root = null)
		{
			if (root == null)
			{
				if (this.Document == null)
				{
					return null;
				}

				root = this.Document;
			}

			if (!this.IsValidGetElementByName(elementName, name))
			{
				return null;
			}

			IEnumerable<XElement> elements = root.Elements();

			foreach (XElement element in elements)
			{
				if (!this.IsMatchingElementByName(element, elementName, name))
				{
					continue;
				}

				return element;
			}

			return null;
		}

		public string GetElementCacheKey(string rootTag = "", XElement element = null)
		{
			if (element == null)
			{
				element = this.Document;
			}

			this.ValidateElement(element, rootTag);

			return string.Format("{0}_{1}", element.Name.ToString(), element.Attribute(XmlTool.NameAttribute).Value);
		}

		public void Load(string filePath)
		{
			if (string.IsNullOrWhiteSpace(filePath))
			{
				throw new ArgumentException("filePath can not be null or white space.");
			}

			this.Document = XElement.Load(filePath);
		}

		private bool IsMatchingElementByName(XElement element, string elementName, string name)
		{
			if (element == null)
			{
				return false;
			}

			if (!element.Name.ToString().Equals(elementName))
			{
				return false;
			}

			if (element.Attribute(NameAttribute) == null)
			{
				return false;
			}

			if (element.Attribute(NameAttribute).Value != name)
			{
				return false;
			}

			return true;
		}

		private bool IsValidGetElementByName(string elementName, string name)
		{
			if (string.IsNullOrWhiteSpace(elementName))
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(name))
			{
				return false;
			}

			return true;
		}

		private void ValidateElement(XElement element, string rootTag = "")
		{
			if (element == null)
			{
				throw new ArgumentNullException("XElement can't be null.");
			}

			if (element.Attribute(XmlTool.NameAttribute) == null)
			{
				throw new InvalidOperationException("XElement \"name\" Attribute is null.");
			}

			if (string.IsNullOrWhiteSpace(element.Attribute(XmlTool.NameAttribute).Value))
			{
				throw new InvalidOperationException("XElement \"name\" Attribute Value is null or white space.");
			}

			if (string.IsNullOrWhiteSpace(rootTag))
			{
				return;
			}

			if (!this.Document.Name.ToString().Equals(rootTag))
			{
				throw new ArgumentException("rootTag is invalid for this cache type");
			}
		}
	}
}