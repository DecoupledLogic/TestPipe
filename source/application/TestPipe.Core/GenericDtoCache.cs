namespace TestPipe.Core
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;
	using System.Xml.Linq;
	using TestPipe.Data;
	using TestPipe.Data.Transfer;

	public class GenericDtoCache
	{
		public static T Add<T>(XElement root, string key, bool seed = false) where T : BaseDto, new()
		{
			if (root == null)
			{
				throw new ArgumentNullException("root");
			}

			if (string.IsNullOrWhiteSpace(key))
			{
				throw new ArgumentException("key is null or white space.");
			}

			T entity = new T();

			if (!root.Name.ToString().Equals(entity.GetType().Name))
			{
				return entity;
			}

			entity = SetProperties(root, entity);

			TestSession.Cache.Add(key, entity);

			return entity;
		}

		public static T Seed<T>(T entity, string database) where T : class
		{
			GenericRepository<T> repo = new GenericRepository<T>(database);
			return repo.Insert(entity);
		}

		public static void Reseed<T>(T entity, string database) where T : BaseDto
		{
			GenericRepository<T> repo = new GenericRepository<T>(database);
			repo.Save(entity);
		}

		public static T SetProperties<T>(XElement root, T entity) where T : BaseDto
		{
			if (root == null)
			{
				throw new ArgumentNullException("root can't be a null value.");
			}

			if (entity == null)
			{
				throw new ArgumentNullException("entity can't be a null value.");
			}

			IEnumerable<XElement> elements = root.Elements();

			foreach (XElement element in elements)
			{
				SetProperty(element, entity);
			}

			return entity;
		}

		public static void SetProperty<T>(XElement element, T entity) where T : BaseDto
		{
			if (element == null)
			{
				return;
			}

			if (entity == null)
			{
				throw new ArgumentNullException("entity can't be a null value.");
			}

			var property = entity.GetType().GetProperty(element.Name.ToString());

			if (property == null || !property.CanWrite)
			{
				return;
			}

			Type type = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType;

			object safeValue = (element.Value == null || element.Value == string.Empty) 
														? null
														: Convert.ChangeType(element.Value, type);

			property.SetValue(entity, safeValue, null);
		}
	}
}