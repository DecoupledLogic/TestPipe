namespace TestPipe.Build
{
	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;

	public class Nuspec
	{
		public ICollection<Package> GetProjectPackages(string projectPackagesPath)
		{
			ICollection<Package> packages = new List<Package>();

			XDocument xDocument = XDocument.Load(projectPackagesPath);
			IEnumerable<XElement> elements = xDocument.Root.Elements();

			foreach (var element in elements)
			{
				Package package = new Package();
				package.Name = GetElementAttributeValue(element, "id");
				package.Version = GetElementAttributeValue(element, "version");
				package.Framework = GetElementAttributeValue(element, "targetFramework");
				bool developmentDependency = false;
				bool.TryParse(GetElementAttributeValue(element, "developmentDependency"), out developmentDependency);
				package.DevelopmentDependency = developmentDependency;
				packages.Add(package);
			}

			return packages;
		}

		public XDocument UpdateNuspec(string nuspecPath, string nuspecTemplatePath)
		{
			XDocument nuspecTemplate = XDocument.Load(nuspecTemplatePath);
			XDocument nuspec = XDocument.Load(nuspecPath);

			XNamespace ns = "http://schemas.microsoft.com/packaging/2011/08/nuspec.xsd";

			XElement templateMetadata = nuspecTemplate.Root.Element(ns + "metadata");
			XElement nuspecMetadata = nuspec.Root.Element(ns + "metadata");

			nuspecMetadata.Element(ns + "version").Value = GetElementValue(templateMetadata, "version", ns);

			nuspecMetadata.Element(ns + "authors").Value = GetElementValue(templateMetadata, "authors", ns);

			nuspecMetadata.Element(ns + "owners").Value = GetElementValue(templateMetadata, "owners", ns);

			nuspecMetadata.Element(ns + "language").Value = GetElementValue(templateMetadata, "language", ns);

			nuspecMetadata.Element(ns + "projectUrl").Value = GetElementValue(templateMetadata, "projectUrl", ns);

			nuspecMetadata.Element(ns + "iconUrl").Value = GetElementValue(templateMetadata, "iconUrl", ns);

			nuspecMetadata.Element(ns + "licenseUrl").Value = GetElementValue(templateMetadata, "licenseUrl", ns);

			nuspecMetadata.Element(ns + "copyright").Value = GetElementValue(templateMetadata, "copyright", ns);

			nuspecMetadata.Element(ns + "requireLicenseAcceptance").Value = GetElementValue(templateMetadata, "requireLicenseAcceptance", ns);

			nuspecMetadata.Element(ns + "tags").Value = GetElementValue(templateMetadata, "tags", ns);

			nuspecMetadata = this.UpdateDependencies(nuspecMetadata, templateMetadata.Element(ns + "version").Value, ns);

			nuspec.Save(nuspecPath);

			return nuspec;
		}

		private XElement UpdateDependencies(XElement root, string version, XNamespace ns)
		{
			IEnumerable<XElement> elements = root.Element(ns + "dependencies").Elements();

			foreach (var element in elements)
			{
				string id = GetElementAttributeValue(element, "id");

				if (id.StartsWith("TestPipe."))
				{
					SetElementAttributeValue(element, "version", version);
				}
			}

			return root;
		}

		private string GetElementValue(XElement root, string elementName, XNamespace ns)
		{
			string value = string.Empty;

			if (root.Element(ns + elementName) != null)
			{
				value = root.Element(ns + elementName).Value;
			}

			return value;
		}

		private string GetElementAttributeValue(XElement element, string attribute)
		{ 
			return element.Attribute(attribute) != null ? element.Attribute(attribute).Value : string.Empty;
		}

		private void SetElementAttributeValue(XElement element, string attribute, string value)
		{
			if (element.Attribute(attribute) != null)
			{
				element.Attribute(attribute).Value = value;
			}
		}
	}
}