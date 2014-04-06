namespace TestPipe
{
	using System;
	using System.IO;
	using System.Security.Cryptography;

	public class Hash
	{
		public static string GeneratePassword()
		{
			string generatedPassword = Path.GetRandomFileName();
			generatedPassword = generatedPassword.Replace(".", string.Empty).Substring(0, 8);
			Random rnd = new Random();
			int randomIndex = rnd.Next(0, generatedPassword.Length);
			while (char.IsDigit(generatedPassword[randomIndex]))
				randomIndex = rnd.Next(0, generatedPassword.Length);

			generatedPassword = ReplaceByIndex(generatedPassword, randomIndex, char.ToUpper(generatedPassword[randomIndex]));

			if (!ContainsNumbers(generatedPassword))
			{
				int anotherRandom = rnd.Next(0, generatedPassword.Length - 1);
				while (randomIndex == anotherRandom)
					anotherRandom = rnd.Next(0, generatedPassword.Length - 1);
				generatedPassword = ReplaceByIndex(generatedPassword, anotherRandom, rnd.Next(0, 9).ToString().ToCharArray()[0]);
			}
			return generatedPassword;
		}

		public static byte[] HashToBuffer(string input)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(input);
			writer.Flush();
			stream.Position = 0;
			HashAlgorithm sha = new SHA1CryptoServiceProvider();
			return sha.ComputeHash(stream);
		}

		public static byte[] HashToBuffer(byte[] input)
		{
			HashAlgorithm sha = new SHA1CryptoServiceProvider();
			return sha.ComputeHash(input);
		}

		/// <summary>
		/// Supports 512 bit hash algorithm
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static byte[] HashToBuffer512(string input)
		{
			MemoryStream stream = new MemoryStream();
			StreamWriter writer = new StreamWriter(stream);
			writer.Write(input);
			writer.Flush();
			stream.Position = 0;
			HashAlgorithm sha = new SHA512CryptoServiceProvider();
			return sha.ComputeHash(stream);
		}

		/// <summary>
		/// Supports 512 bit hash algorithm
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static byte[] HashToBuffer512(byte[] input)
		{
			HashAlgorithm sha = new SHA512CryptoServiceProvider();
			return sha.ComputeHash(input);
		}

		public static string HashToString(string input)
		{
			return Formater.ByteToHexString(HashToBuffer(input));
		}

		public static string HashToString(byte[] input)
		{
			return Formater.ByteToHexString(HashToBuffer(input));
		}

		/// <summary>
		/// Supports 512 bit hash algorithm
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string HashToString512(string input)
		{
			return Formater.ByteToHexString(HashToBuffer512(input));
		}

		/// <summary>
		/// Supports 512 bit hash algorithm
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string HashToString512(byte[] input)
		{
			return Formater.ByteToHexString(HashToBuffer512(input));
		}

		private static bool ContainsNumbers(string value)
		{
			System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"[0-9]", System.Text.RegularExpressions.RegexOptions.Singleline);
			return regEx.IsMatch(value);
		}

		private static string ReplaceByIndex(string str, int index, char newChar)
		{
			str = str.Remove(index, 1);
			str = str.Insert(index, newChar.ToString());
			return str;
		}
	}
}