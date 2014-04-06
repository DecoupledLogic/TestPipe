namespace TestPipe.Runner
{
	using System;

	public class Entity
	{
		private readonly ValidationErrors validationErrors;

		public Entity()
		{
			this.validationErrors = new ValidationErrors();
		}

		public string Id { get; set; }

		public virtual bool IsValid
		{
			get
			{
				return ValidationErrors.Items.Count == 0;
			}
		}

		public string Name { get; set; }

		public virtual ValidationErrors ValidationErrors
		{
			get { return this.validationErrors; }
		}
	}
}