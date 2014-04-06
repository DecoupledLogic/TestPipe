namespace TestPipe.Runner
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class ValidationErrors
	{
		private List<ValidationError> errors;

		public ValidationErrors()
		{
			this.errors = new List<ValidationError>();
		}

		public IList<ValidationError> Items
		{
			get { return this.errors; }
		}

		public void Add(string propertyName)
		{
			this.errors.Add(new ValidationError()
			{
				PropertyName = propertyName, 
				Message = propertyName + " is required."
			});
		}

		public void Add(string propertyName, string errorMessage)
		{
			this.errors.Add(new ValidationError()
			{
				PropertyName = propertyName,
				Message = errorMessage
			});
		}

		public void Add(ValidationError error)
		{
			this.errors.Add(error);
		}

		public void AddRange(List<ValidationError> errors)
		{
			this.errors.AddRange(errors);
		}

		internal void Clear()
		{
			this.errors.Clear();
		}
	}
}