[module: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1201:ElementsMustAppearInTheCorrectOrder", Justification = "Reviewed. Suppression is OK here.")]
namespace TestPipe.Core.Control
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Drawing;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Interfaces;

	public class BaseControl : IControl
	{
		//Constructor
		public BaseControl()
		{
		}

		public BaseControl(IBrowser browser, IElement element)
		{
			this.Browser = browser;
			this.Element = element;
		}

		public BaseControl(IBrowser browser, string id, uint timeoutInSeconds = 0, bool displayed = false)
			: this(browser, null, id, timeoutInSeconds, displayed)
		{
			if (string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("Parameter id must not be null or white space.");
			}
		}

		public BaseControl(IBrowser browser, ISelect selector = null, string id = "", uint timeoutInSeconds = 0, bool displayed = false)
		{
			Initialize(browser, selector, id, timeoutInSeconds, displayed);
		}

		public void Initialize(IBrowser browser, ISelect selector = null, string id = "", uint timeoutInSeconds = 0, bool displayed = false)
		{
			if (timeoutInSeconds == 0)
			{
				timeoutInSeconds = (uint)TestSession.Timeout.Seconds;
			}

			this.Browser = browser;

			if (!string.IsNullOrWhiteSpace(id))
			{
				this.SelectId = id;
				this.Selector = new Select(FindByEnum.Id, id);
				this.Find(this.Selector, timeoutInSeconds, displayed);
			}

			this.Selector = selector;

			if (this.Selector != null)
			{
				if (this.Selector.FindBy == FindByEnum.Id)
				{
					this.SelectId = this.Selector.EqualTo;
				}

				this.Find(this.Selector, timeoutInSeconds, displayed);
				return;
			}
		}

		public IBrowser Browser { get; private set; }

		public string Id
		{
			get
			{
				return this.Element.GetAttribute("id");
			}
		}

		public string SelectId { get; private set; }

		#region IControl

		public string Class
		{
			get
			{
				return this.Element.GetAttribute("class");
			}
		}

		public ControlTypeEnum ControlType
		{
			get
			{
				switch (this.TagName)
				{
					case "input":
						switch (this.GetAttribute("type").ToLower())
						{
							case "text":
								return ControlTypeEnum.Text;

							case "password":
								return ControlTypeEnum.Password;

							case "hidden":
								return ControlTypeEnum.Hidden;
						}

						break;

					case "textarea":
						return ControlTypeEnum.TextArea;

					case "select":
						return ControlTypeEnum.Select;

					case "div":
						return ControlTypeEnum.Div;

					case "a":
						return ControlTypeEnum.Anchor;
				}

				return ControlTypeEnum.Unknown;
			}
		}

		public IElement Element
		{
			get;
			set;
		}

		public Point Location
		{
			get
			{
				return this.Element.Location;
			}
		}

		public ISelect Selector
		{
			get;
			internal set;
		}

		public Size Size
		{
			get
			{
				return this.Element.Size;
			}
		}

		public string TagName
		{
			get
			{
				return this.Element.TagName.ToLower();
			}
		}

		public string Text
		{
			get
			{
				return this.Element.Text;
			}
		}

		public string GetAttribute(string attributeName)
		{
			return this.Element.GetAttribute(attributeName);
		}

		public string GetCssValue(string propertyName)
		{
			return this.Element.GetCssValue(propertyName);
		}

		#endregion IControl

		#region IControlAssert

		public bool Displayed
		{
			get
			{
				return this.Element.Displayed;
			}
		}

		public bool Enabled
		{
			get
			{
				return this.Element.Enabled;
			}
		}

		public bool Selected
		{
			get
			{
				return this.Element.Selected;
			}
		}

		public bool CanFind(ISelect by)
		{
			this.Find(by);
			return this.Exists();
		}

		public bool CanFindById(string id)
		{
			this.FindById(id);
			return this.Exists();
		}

		public bool CanFindByText(string text)
		{
			this.FindByText(text);
			return this.Exists();
		}

		public bool Exists()
		{
			return this.Element != null;
		}

		public bool HasFocus()
		{
			IElement active = this.Browser.BrowserSearchContext.GetActiveElement();
			return this.Element.Equals(active);
		}

		#endregion IControlAssert

		#region IControlContext

		public IControl Find()
		{
			if (this.Selector == null)
			{
				throw new ArgumentException("Selector can not be null.");
			}

			return this.Find(this.Selector);
		}

		public IControl Find(ISelect by, uint timeoutInSeconds = 0, bool displayed = false)
		{
			try
			{
				this.Element = this.Browser.BrowserSearchContext.FindElement(by, timeoutInSeconds, displayed);
			}
			catch (ElementNotFoundException)
			{
				this.Element = null;
			}

			return this;
		}

		public IControl FindChild(ISelect by, uint timeoutInSeconds = 0, bool displayed = false)
		{
			try
			{
				this.Element = this.Element.FindElement(by, timeoutInSeconds, displayed);
			}
			catch (ElementNotFoundException)
			{
				this.Element = null;
			}

			return this;
		}

		public ReadOnlyCollection<IControl> FindAll(ISelect by, uint timeoutInSeconds = 0)
		{
			ReadOnlyCollection<IElement> elements = this.Browser.BrowserSearchContext.FindElements(by, timeoutInSeconds);

			if (elements == null)
			{
				return null;
			}

			if (elements.Count < 1)
			{
				return null;
			}

			List<IControl> controls = new List<IControl>();

			foreach (var element in elements)
			{
				IControl control = new BaseControl(this.Browser, element);
				controls.Add(control);
			}

			ReadOnlyCollection<IControl> results = new ReadOnlyCollection<IControl>(controls);
			return results;
		}

		public ReadOnlyCollection<IControl> FindAllChildren(ISelect by, uint timeoutInSeconds = 0)
		{
			ReadOnlyCollection<IElement> elements = this.Element.FindElements(by, timeoutInSeconds);

			if (elements == null)
			{
				return null;
			}

			if (elements.Count < 1)
			{
				return null;
			}

			List<IControl> controls = new List<IControl>();

			foreach (var element in elements)
			{
				IControl control = new BaseControl(this.Browser, element);
				controls.Add(control);
			}

			ReadOnlyCollection<IControl> results = new ReadOnlyCollection<IControl>(controls);
			return results;
		}

		public IControl FindById(string id, uint timeoutInSeconds = 0, bool displayed = false)
		{
			ISelect by = new Select(FindByEnum.Id, id);
			return this.Find(by, timeoutInSeconds, displayed);
		}

		public IControl FindByText(string text, uint timeoutInSeconds = 0, bool displayed = false)
		{
			ISelect by = new Select(FindByEnum.LinkText, text);
			return this.Find(by, timeoutInSeconds, displayed);
		}

		#endregion IControlContext

		#region IControlAction

		public void AppendText(Func<IControl> element, string text)
		{
		}

		public void AppendTextWithoutEvents(Func<IControl> element, string text)
		{
		}

		public void Clear()
		{
			this.Element.Clear();
		}

		public void Click(uint timeoutInSeconds = 0, string pageTitle = "")
		{
			if (timeoutInSeconds == 0)
			{
				timeoutInSeconds = (uint)TestSession.Timeout.TotalSeconds;
			}

			this.Element.Click();

			if (timeoutInSeconds > 0)
			{
				this.Browser.WaitForPageLoad(timeoutInSeconds, pageTitle);
			}
		}

		public void Click(int x, int y)
		{
		}

		public void Click(Func<IControl> element, int x, int y)
		{
		}

		public void Click(Func<IControl> element)
		{
		}

		public void DoubleClick(int x, int y)
		{
		}

		public void DoubleClick(Func<IControl> element, int x, int y)
		{
		}

		public void DoubleClick(Func<IControl> element)
		{
		}

		public void DragAndDrop(int sourceX, int sourceY, int destinationX, int destinationY)
		{
		}

		public void DragAndDrop(Func<IControl> source, Func<IControl> target)
		{
		}

		public void EnterText(Func<IControl> element, string text)
		{
		}

		public void EnterTextWithoutEvents(Func<IControl> element, string text)
		{
		}

		public void Focus()
		{
			this.Browser.MoveToElement(this.Element);
		}

		public void Focus(Func<IControl> element)
		{ 
		}

		public void Hover(int x, int y)
		{
		}

		public void Hover(Func<IControl> element, int x, int y)
		{
		}

		public void Hover(Func<IControl> element)
		{
		}

		public void MultiSelectIndex(Func<IControl> element, int[] optionIndices)
		{
		}

		public void MultiSelectText(Func<IControl> element, string[] optionTextCollection)
		{
		}

		public void MultiSelectValue(Func<IControl> element, string[] optionValues)
		{
		}

		public void Press(string keys)
		{
		}

		public void RightClick(Func<IControl> element)
		{
		}

		public void SelectIndex(Func<IControl> element, int optionIndex)
		{
			throw new NotImplementedException();
		}

		public void SelectText(Func<IControl> element, string optionText)
		{
		}

		public void SelectValue(Func<IControl> element, string optionValue)
		{
		}

		public void Submit()
		{
			this.Element.Submit();
		}

		public void TypeText(string text)
		{
			this.Element.SendKeys(text);
		}

		public void UploadFile(Func<IControl> element, int x, int y, string fileName)
		{
		}

		public void Wait()
		{
		}

		public void Wait(int seconds)
		{
		}

		public void Wait(TimeSpan timeSpan)
		{
		}

		public void WaitUntil(System.Linq.Expressions.Expression<Func<bool>> conditionFunc)
		{
		}

		public void WaitUntil(System.Linq.Expressions.Expression<Func<bool>> conditionFunc, TimeSpan timeout)
		{
		}

		public void WaitUntil(System.Linq.Expressions.Expression<Action> conditionAction)
		{
		}

		public void WaitUntil(System.Linq.Expressions.Expression<Action> conditionAction, TimeSpan timeout)
		{
		}

		#endregion IControlAction
	}
}