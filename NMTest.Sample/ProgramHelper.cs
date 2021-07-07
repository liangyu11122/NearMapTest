using System;
using System.Text.RegularExpressions;
using NMTest.DataSource;

namespace NMTest.Sample
{
	public static class ProgramHelper
	{
		public static string GenerateRandomKey()
		{
			var random = new Random();
			return FormatKey(random.Next(0, 9));
		}

		private static string FormatKey(int key)
		{
			return Resources.PrefixKey + key;
		}

		public static bool IsValidValue(string value)
		{
			if (!string.IsNullOrEmpty(value))
			{
				var match = Regex.Match(value, @"value\d");
				return match.Success;
			}
			return false;
		}
	}
}