namespace TestPipe.Core.Control
{
	using System;
	using System.Text.RegularExpressions;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Interfaces;

	public class Select : ISelect
	{
		private const string ERRNullEmptyAttributeValue = "Cannot find elements with a null or empty attribute value.";
		private string description = "TestPipe.Framework.Core.Select";

		public Select()
		{
		}

		public Select(FindByEnum findBy, string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			this.FindBy = findBy;
			this.EqualTo = equalTo;
			this.Timeout = timeoutSeconds;
			this.Displayed = displayed;
		}

		public bool Displayed
		{
			get;
			private set;
		}

		public string EqualTo
		{
			get;
			private set;
		}

		public FindByEnum FindBy
		{
			get;
			private set;
		}

		public uint Timeout
		{
			get;
			private set;
		}

		protected string Description
		{
			get
			{
				return this.description;
			}
			private set
			{
				this.description = value;
			}
		}

		public static Select ClassName(string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			ValidateAttributeValue(equalTo);

			if (new Regex(".*\\s+.*").IsMatch(equalTo))
			{
				throw new ArgumentException("Compound class names are not supported. Consider searching for one class name and filtering the results.");
			}

			return new Select()
			{
				FindBy = FindByEnum.ClassName,
				EqualTo = equalTo,
				Description = "Selector.ClassName[Contains]: " + equalTo,
				Timeout = timeoutSeconds,
				Displayed = displayed
			};
		}

		public static Select CssSelector(string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			ValidateAttributeValue(equalTo);

			return new Select()
			{
				FindBy = FindByEnum.CssSelector,
				EqualTo = equalTo,
				Description = "Selector.CssSelector: " + equalTo,
				Timeout = timeoutSeconds,
				Displayed = displayed
			};
		}

		public static Select Id(string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			ValidateAttributeValue(equalTo);

			return new Select()
			{
				FindBy = FindByEnum.Id,
				EqualTo = equalTo,
				Description = "Selector.Id: " + equalTo,
				Timeout = timeoutSeconds,
				Displayed = displayed
			};
		}

		public static Select LinkText(string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			ValidateAttributeValue(equalTo);

			return new Select()
			{
				FindBy = FindByEnum.LinkText,
				EqualTo = equalTo,
				Description = "Selector.LinkText: " + equalTo,
				Timeout = timeoutSeconds,
				Displayed = displayed
			};
		}

		public static Select Name(string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			ValidateAttributeValue(equalTo);

			return new Select()
			{
				FindBy = FindByEnum.Name,
				EqualTo = equalTo,
				Description = "Selector.Name: " + equalTo,
				Timeout = timeoutSeconds,
				Displayed = displayed
			};
		}

		public static bool operator !=(Select one, Select two)
		{
			return !(one == two);
		}

		public static bool operator ==(Select one, Select two)
		{
			if (object.ReferenceEquals((object)one, (object)two))
			{
				return true;
			}
			if (one == null || two == null)
			{
				return false;
			}
			else
			{
				return one.Equals((object)two);
			}
		}

		public static Select PartialLinkText(string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			ValidateAttributeValue(equalTo);

			return new Select()
			{
				FindBy = FindByEnum.PartialLinkText,
				EqualTo = equalTo,
				Description = "Selector.PartialLinkText: " + equalTo,
				Timeout = timeoutSeconds,
				Displayed = displayed
			};
		}

		public static Select TagName(string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			ValidateAttributeValue(equalTo);

			return new Select()
			{
				FindBy = FindByEnum.TagName,
				EqualTo = equalTo,
				Description = "Selector.TagName: " + equalTo,
				Timeout = timeoutSeconds,
				Displayed = displayed
			};
		}

		public static Select XPath(string equalTo, uint timeoutSeconds = 0, bool displayed = false)
		{
			ValidateAttributeValue(equalTo);

			return new Select()
			{
				FindBy = FindByEnum.XPath,
				EqualTo = equalTo,
				Description = "Selector.XPath: " + equalTo,
				Timeout = timeoutSeconds,
				Displayed = displayed
			};
		}

		public override bool Equals(object obj)
		{
			Select by = obj as Select;
			if (by != (Select)null)
				return this.description.Equals(by.description);
			else
				return false;
		}

		public override int GetHashCode()
		{
			return this.description.GetHashCode();
		}

		public override string ToString()
		{
			return this.description;
		}

		private static void ValidateAttributeValue(string equalTo)
		{
			if (string.IsNullOrWhiteSpace(equalTo))
			{
				throw new ArgumentNullException("equalTo", ERRNullEmptyAttributeValue);
			}
		}
	}
}