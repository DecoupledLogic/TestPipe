namespace TestPipe.Core.Helpers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public static class TextHelper
	{
		//http://stackoverflow.com/questions/141045/how-do-i-replace-the-first-instance-of-a-string-in-net
		public static string ReplaceFirst(string text, string search, string replace)
		{
			int pos = text.IndexOf(search);

			if (pos < 0)
			{
				return text;
			}

			return text.Substring(0, pos) + replace + text.Substring(pos + search.Length);
		}
	}
}
