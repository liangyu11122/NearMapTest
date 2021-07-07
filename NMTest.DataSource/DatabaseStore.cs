using System;
using System.Collections.Generic;
using System.Threading;

namespace NMTest.DataSource
{
	public class DatabaseStore : IDatabaseStore
	{
		private readonly Dictionary<string, string> _values = new Dictionary<string, string>();

		public Dictionary<string, string> LoadFromDatabase()
		{
			Thread.Sleep(500);
			for (var i = 0; i < 10; i++)
			{
				var key = "key" + i;
				var value = "value" + i;
				_values.Add(key, value);
			}
			return _values;
		}

		public string GetValueFromDatabase(string key)
		{
			Thread.Sleep(500);
			if (_values.TryGetValue(key, out var result))
			{
				return result;
			}
			else if (_values.Count == 0)
			{
				LoadFromDatabase();
				_values.TryGetValue(key, out result);
				return result;
			}
			return null;
		}
	}
}
