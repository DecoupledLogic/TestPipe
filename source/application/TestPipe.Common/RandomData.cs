namespace TestPipe
{
	using System;
	using System.Text;

	public class RandomData
	{
		private static Random random = new Random((int)DateTime.Now.Ticks);

		public static int Int(int minValue = int.MinValue, int maxValue = int.MaxValue)
		{
			return new Random().Next(minValue, maxValue);
		}

		public static string String(int size)
		{
			StringBuilder builder = new StringBuilder();
			char ch;
			for (int i = 0; i < size; i++)
			{
				ch = Convert.ToChar(Convert.ToInt32(Math.Floor((26 * random.NextDouble()) + 65)));
				builder.Append(ch);
			}

			return builder.ToString();
		}
	}
}