using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMTest.DataSource;

namespace NMTest.Test
{
	[TestClass]
	public class DatabaseStoreTests
	{
		[TestMethod]
		public void TestLoadFromDatabase()
		{
			var data = new DatabaseStore().LoadFromDatabase();
			var expected = new Dictionary<string, string>()
			{
				{"key0", "value0"},
				{"key1", "value1"},
				{"key2", "value2"},
				{"key3", "value3"},
				{"key4", "value4"},
				{"key5", "value5"},
				{"key6", "value6"},
				{"key7", "value7"},
				{"key8", "value8"},
				{"key9", "value9"}
			};
			CollectionAssert.AreEquivalent(expected, data);
		}

		[TestMethod]
		public void TestGetValueFromDatabase()
		{
			var result = new DatabaseStore().GetValueFromDatabase("key1");
			var result1 = new DatabaseStore().GetValueFromDatabase("invalid");
			Assert.AreEqual("value1", result);
			Assert.AreEqual(null, result1);
		}
	}
}
