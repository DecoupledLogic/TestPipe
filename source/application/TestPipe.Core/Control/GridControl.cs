namespace TestPipe.Core.Control
{
	using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
	using System.Text;
	using TestPipe.Core.Enums;
	using TestPipe.Core.Exceptions;
	using TestPipe.Core.Interfaces;

	public class GridControl : BaseControl
	{
		private IList<IControl> columns;

		private IList<IControl> rows;

        public GridControl(IBrowser browser)
			: base(browser, null, null)
		{
		}

		public GridControl(IBrowser browser, ISelect selector = null, string id = null, uint timeoutInSeconds = 0, bool displayed = false)
			: base(browser, selector, id, timeoutInSeconds, displayed)
		{
			if (this.Selector == null && string.IsNullOrWhiteSpace(id))
			{
				throw new ArgumentException("A selector or id string must be provided to create grid controls.");
			}

			if (this.Selector.FindBy != FindByEnum.Id)
			{
				throw new ArgumentException("If an id string is not provided, the selector find by argument must be FindByEnum.Id to create grid controls.");
			}

			this.SetColumns();
			this.SetRows();
		}

		public int ColumnCount { get; set; }

		public int RowCount { get; set; }

        private ReadOnlyCollection<IControl> ColumnHeaders
		{
			get
			{
				if (this.columns == null)
				{
					return null;
				}

				return new ReadOnlyCollection<IControl>(this.columns);
			}
		}

        public IControl GetCell(int row, int column, ControlTypeEnum type = ControlTypeEnum.Unknown)
		{
			Select selector = this.SelectByCell(row, column);
			selector = this.AppendControlTypeXpath(selector, type);
			IControl control = this.Find(selector);
			return control;
		}

		public IControl GetCellByColumnName(int row, string columnName, ControlTypeEnum type = ControlTypeEnum.Unknown)
		{
			int column = this.GetColumnNumber(columnName);
			return this.GetCell(row, column, type);
		}

		public IControl GetCellByColumnNameAndCellText(string columnName, string text, ControlTypeEnum type = ControlTypeEnum.Unknown)
		{
			int row = this.GetRowNumberByColumnNameAndCellText(columnName, text);
			return this.GetCellByColumnName(row, columnName, type);
		}

		public IControl GetColumnHeader(int column, ControlTypeEnum type = ControlTypeEnum.Unknown)
		{
			Select selector = this.SelectByHeader(1, column);
			selector = this.AppendControlTypeXpath(selector, type);
			IControl control = this.Find(selector);
			return control;
		}

		public IControl GetColumnHeaderByColumnName(string columnName, ControlTypeEnum type = ControlTypeEnum.Unknown)
		{
			int column = this.GetColumnNumber(columnName);
			return this.GetColumnHeader(column, type);
		}

		public int GetColumnNumber(string column)
		{
			for (int col = 1; col <= this.ColumnCount; col++)
			{
				Select selectHeader = this.SelectHeaderClass(col);
				IControl testHeading = this.Find(selectHeader);

				if (testHeading.Text.Contains(column))
				{
					//Console.WriteLine("\nThe Column Name: \"" + column + "\" was found to be column # " + col);
					return col;
				}
			}
			throw new ElementNotFoundException("A column with the header \"" + column + "\" could not be found in the grid \"" + this.GetAttribute("id") + "\" .");
		}

		public ICollection<string> GetColumnText(int column)
		{
			ICollection<string> columnText = new List<string>();

			for (int i = 1; i <= this.RowCount; i++)
			{
				IControl control = this.GetCell(i, column);

				if (!this.IsValidControl(control))
				{
					continue;
				}

				columnText.Add(control.Text);
			}

			return columnText;
		}

		public ICollection<string> GetColumnTextByColumnName(string columnName)
		{
			int columnNumber = this.GetColumnNumber(columnName);

			return this.GetColumnText(columnNumber);
		}

		public int GetRowNumberByCellText(int column, string text)
		{
			for (int row = 1; row <= this.RowCount; row++)
			{
				IControl control = this.GetCell(row, column);

				if (!this.IsValidControl(control))
				{
					continue;
				}

				if (control.Text.Equals(text))
				{
					return row;
				}
			}
			throw new ElementNotFoundException("A row with the value \"" + text + "\" in the column \"" + column + "\"could not be found in the grid \"" + this.GetAttribute("id") + "\" .");
		}

		public int GetRowNumberByColumnNameAndCellText(string columnName, string text)
		{
			for (int row = 1; row <= this.RowCount; row++)
			{
				IControl control = this.GetCellByColumnName(row, columnName);

				if (!this.IsValidControl(control))
				{
					continue;
				}

				if (control.Text.Equals(text))
				{
					return row;
				}
			}
			throw new ElementNotFoundException("A row with the value \"" + text + "\" in the column \"" + columnName + "\"could not be found in the grid \"" + this.GetAttribute("id") + "\" .");
		}

		public ICollection<string> GetRowText(int row)
		{
			ICollection<string> rowText = new List<string>();

			for (int i = 1; i <= this.ColumnCount; i++)
			{
				IControl control = this.GetCell(row, i);

				if (!this.IsValidControl(control))
				{
					continue;
				}

				rowText.Add(control.Text);
			}

			return rowText;
		}

        public List<IControl> GetSelectedColumn(int columnNumber)
        {
            ReadOnlyCollection<IControl> columnsControl;
            List<IControl> columnControl = new List<IControl>();
            columnsControl = this.ColumnHeaders;
            int numOfRows = this.RowCount;
            int numOfColums = this.ColumnCount / this.RowCount;
            columnsControl.ToArray<IControl>();
            for (int i = 0; i < numOfRows; i++)
            {
                columnControl.Add(columnsControl[(i * numOfColums) + columnNumber]);
            }
            return columnControl;
        }

		private static string ColumnXPath(string id)
		{
			return string.Format("//*[@id='{0}']/tbody/tr/th", id);
		}

		private Select AppendControlTypeXpath(Select selector, ControlTypeEnum type = ControlTypeEnum.Unknown)
		{
			if (type != ControlTypeEnum.Unknown)
			{
				string selectorString = selector.EqualTo + this.GetControlTypeXpath(type);
				selector = new Select(FindByEnum.XPath, selectorString);
			}

			return selector;
		}

		private Select GetColumnsSelector()
		{
			Select selector = new Select(FindByEnum.XPath, ColumnXPath(this.SelectId));
			return selector;
		}

		private string GetControlTypeXpath(ControlTypeEnum type)
		{
			switch (type)
			{
				case ControlTypeEnum.Anchor:
					return "/a";
			}

			return string.Empty;
		}

		private Select GetRowsSelector()
		{
			string xpath = string.Format("//*[@id='{0}']/tbody/tr", this.SelectId);
			Select selector = new Select(FindByEnum.XPath, xpath);
			return selector;
		}

		private bool IsValidControl(IControl control)
		{
			if (control == null)
			{
				return false;
			}

			if (!control.Exists())
			{
				return false;
			}

			return true;
		}

		private Select SelectByCell(int row, int column)
		{
			StringBuilder xpathCell = new StringBuilder();
			xpathCell.Append("//*[@id='");
			xpathCell.Append(this.SelectId.ToString());
			xpathCell.Append("']/tbody/tr[");
			xpathCell.Append(row);
			xpathCell.Append("]/td[");
			xpathCell.Append(column);
			xpathCell.Append("]");
			Select selectCell = new Select(FindByEnum.XPath, xpathCell.ToString());
			return selectCell;
		}

		private Select SelectByHeader(int row, int column)
		{
			StringBuilder xpathHeader = new StringBuilder();
			xpathHeader.Append("//*[@id='");
			xpathHeader.Append(this.SelectId.ToString());
			xpathHeader.Append("']/tbody/tr[");
			xpathHeader.Append(row);
			xpathHeader.Append("]/th[");
			xpathHeader.Append(column);
			xpathHeader.Append("]");

			Select selectHeader = new Select(FindByEnum.XPath, xpathHeader.ToString());
			return selectHeader;
		}

		private Select SelectHeaderClass(int column)
		{
			StringBuilder xpathHeader = new StringBuilder();
			xpathHeader.Append(ColumnXPath(this.SelectId));
			xpathHeader.Append("[");
			xpathHeader.Append(column);
			xpathHeader.Append("]");
			Select selectHeader = new Select(FindByEnum.XPath, xpathHeader.ToString());
			return selectHeader;
		}

        /* Brief : if default XPath for columns fail, this function will be called
         * Parameters : void 
         * Return Value : IList<IControl> : All the columns in the table
        */

        private IList<IControl> GetAnotherColumnFromat()
        {
            IList<IControl> columns;
            IControl control = new BaseControl(this.Browser, null, null);
            Select selector = new Select(FindByEnum.XPath, string.Format("//*[@id='{0}']/tbody/tr/td", this.SelectId));
            columns = control.FindAll(selector);
            return columns;
        }

		private void SetColumns()
		{
			IControl control = new BaseControl(this.Browser, null, null);
			Select selector = this.GetColumnsSelector();
            int countColumnFormatFindingAttempts = 0;
			this.columns = control.FindAll(selector);
            
            while (this.columns == null)
            {
                if (countColumnFormatFindingAttempts == 1)
                {
                    break;
                }
                this.columns = this.GetAnotherColumnFromat();
                countColumnFormatFindingAttempts++;
            }

			this.ColumnCount = this.columns.Count;
		}

		private void SetRows()
		{
			IControl control = new BaseControl(this.Browser, null, null);
			Select selector = this.GetRowsSelector();
			this.rows = control.FindAll(selector);
			this.RowCount = this.rows.Count;
		}
	}
}