namespace TestPipe.Data.Seed
{
	using System;
	using System.Collections.Generic;
	using System.Xml.Linq;

	public class XmlSeedBase
	{
		//unit test this crap too
		public virtual void ProcessXml(XElement element)
		{
		}

		protected ICollection<string> ProcessList(XElement root, string tag, string itemTag)
		{
			ICollection<string> list = new List<string>();
			if (!this.IsValidProcessList(root, tag, itemTag))
			{
				return list;
			}

			IEnumerable<XElement> elements = root.Element(tag).Elements(itemTag);
			foreach (var element in elements)
			{
				list.Add(element.Value);
			}

			return list;
		}

		private bool IsValidProcessList(XElement root, string tag, string itemTag)
		{
			if (root == null)
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(tag))
			{
				return false;
			}

			if (string.IsNullOrWhiteSpace(itemTag))
			{
				return false;
			}

			return true;
		}
	}
}