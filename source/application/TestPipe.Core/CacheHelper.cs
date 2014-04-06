namespace TestPipe.Core
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml.Linq;

	public class CacheHelper
	{
		public static void CacheAdd(string key, XElement element)
		{
			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentException("key");
			}

			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			if (element.Attribute(XmlTool.NameAttribute) == null)
			{
				throw new InvalidOperationException("element does not contain an Atttribute named \"" + XmlTool.NameAttribute + "\"");
			}

			switch (element.Attribute(XmlTool.NameAttribute).Value)
			{
				case "BaseURL":
					TestSession.BaseUrl = element.Value;
					break;

				case "LogoutURL":
					TestSession.LogoutUrl = element.Value;
					break;

				default:
					TestSession.Cache.Add(key, element.Value);
					break;
			}
		}

		public static void CacheElement(string key, XElement element, string rootName)
		{
			if (element == null)
			{
				throw new ArgumentNullException("element");
			}

			if (element.Attribute(XmlTool.NameAttribute) == null)
			{
				throw new InvalidOperationException("element does not contain an Atttribute named \"" + XmlTool.NameAttribute + "\"");
			}

			string cacheKey = GetKey(key, element.Attribute(XmlTool.NameAttribute).Value);

			if (TestSession.Cache.ContainsKey(cacheKey))
			{
				return;
			}

			if ((rootName.Equals(XmlTool.ScenarioTag) || rootName.Equals(XmlTool.FeatureTag)) && CacheObject(cacheKey, element))
			{
				return;
			}

			CacheAdd(cacheKey, element);
		}

		public static void CacheElements(XElement root, string parent, string child, string key)
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}

			if (!root.Name.ToString().Equals(parent))
			{
				return;
			}

			IEnumerable<XElement> elements;

			if (string.IsNullOrWhiteSpace(child))
			{
				elements = root.Elements();
			}
			else
			{
				elements = root.Elements(child);
			}

			if (elements.Count() < 1)
			{
				throw new InvalidOperationException("The root element does not contain elements.");
			}

			if (elements.FirstOrDefault().Name == XmlTool.ObjectTag || elements.FirstOrDefault().Name == XmlTool.DataTag)
			{
				elements = elements.FirstOrDefault().Elements();
			}

			string rootName = root.Name.ToString();

			foreach (XElement element in elements)
			{
				CacheElement(key, element, rootName);
			}
		}

		public static string GetKey(string prefix, string keyName)
		{
			if (string.IsNullOrWhiteSpace(keyName))
			{
				if (string.IsNullOrWhiteSpace(prefix))
				{
					return string.Empty;
				}

				return prefix;
			}

			if (string.IsNullOrWhiteSpace(prefix))
			{
				return keyName;
			}

			return string.Format("{0}_{1}", prefix, keyName);
		}

		private static bool CacheObject(string key, XElement element)
		{
			if (element.Parent == null)
			{
				return false;
			}

			if (element.Parent.Name == null)
			{
				return false;
			}

			if (!element.Parent.Name.ToString().Equals(XmlTool.ObjectTag))
			{
				return false;
			}

			Cache(element, key); 
			return true;
		}

		private static void Cache(XElement element, string key)
		{
			bool seed = false;

			if (element.Attribute(XmlTool.SeedAttribute) != null)
			{
				bool.TryParse(element.Attribute(XmlTool.SeedAttribute).Value, out seed);
			}

			if (element.Name == null || string.IsNullOrWhiteSpace(element.Name.ToString()))
			{
				return;
			}

			if (element.Attribute(XmlTool.NamespaceAttribute) == null || string.IsNullOrWhiteSpace(element.Attribute(XmlTool.NamespaceAttribute).Value))
			{
				return;
			}

			string className = element.Name.ToString();
			string classNamespace = element.Attribute(XmlTool.NamespaceAttribute).Value;

			Type type = FindType(string.Format("{0}.{1}", classNamespace, className));

			if (type == null)
			{
				return;
			}

			dynamic entity = Activator.CreateInstance(type);

			if (seed)
			{
				if (element.Attribute(XmlTool.DatabaseAttribute) == null || string.IsNullOrWhiteSpace(element.Attribute(XmlTool.DatabaseAttribute).Value))
				{
					throw new InvalidOperationException("Database attribute was not specified. To seed the database with data you must provide a Database attribute");
				}

				string database = element.Attribute(XmlTool.DatabaseAttribute).Value;

				entity = GenericDtoCache.Seed(entity, database);
			}

			typeof(GenericDtoCache).GetMethod("Add").MakeGenericMethod(type).Invoke(entity, new object[] { element, key, seed });
		}

		private static Type FindType(string fullName)
		{
			return
					AppDomain.CurrentDomain.GetAssemblies()
							.Where(a => !a.IsDynamic)
							.SelectMany(a => a.GetTypes())
							.FirstOrDefault(t => t.FullName.Equals(fullName));
		}
	}
}