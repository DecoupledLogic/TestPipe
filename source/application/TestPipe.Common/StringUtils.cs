namespace TestPipe
{
	using System;
	using System.ComponentModel;

	public class StringUtils
	{
		public static T To<T>(string input) where T : struct
		{
			T result = default(T);

			if (string.IsNullOrWhiteSpace(input))
			{
				return default(T);
			}

			try
			{
				result = (T)Convert.ChangeType(input, typeof(T));
			}
			catch
			{
			}

			return result;
		}

		public static T? ToNullable<T>(string input) where T : struct
		{
			T? result = new T?();

			try
			{
				if (string.IsNullOrWhiteSpace(input))
				{
					return result;
				}

				TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
				result = (T)converter.ConvertFrom(input);
			}
			catch
			{
			}

			return result;
		}
	}
}