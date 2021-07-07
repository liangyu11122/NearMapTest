using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMTest.DataSource;

namespace NMTest.Test
{
	[TestClass]
	public class DistributedCacheStoreTests
	{
		[TestMethod]
		public void TestSetObjectFromCache()
		{
			var caching = new DistributedCacheStore();
			var database = new DatabaseStore();
			caching.SetCachedObject(TestCacheKey, 5, GenerateMockDatabase);
			caching.SetCachedObject(TestCacheKey2, 5, GenerateTestList);
			caching.SetCachedObject(TestCacheKey3, 5, database.LoadFromDatabase);
			caching.SetCachedObject(TestCacheKey4, 5, GenerateEmptyDictionary);
			var result = caching.Get(TestCacheKey);
			var result2 = caching.Get(TestCacheKey2);
			var result3 = caching.Get(TestCacheKey3);
			var result4 = caching.Get(TestCacheKey4);
			var expected = new Dictionary<string, string>()
			{
				{"key0","value0"},
				{"key1","value1"},
				{"key2","value2"},
				{"key3","value3"},
				{"key4","value4"},
				{"key5","value5"},
				{"key6","value6"},
				{"key7","value7"},
				{"key8","value8"},
				{"key9","value9"}
			};
			var expected2 = new List<string>() { "1", "2", "3" };

			CollectionAssert.AreEquivalent(expected, (Dictionary<string, string>)result);
			CollectionAssert.AreEquivalent(expected2, (List<string>)result2);
			CollectionAssert.AreEquivalent(expected, (Dictionary<string, string>)result3);
			CollectionAssert.AreEquivalent(null, (Dictionary<string, string>)result4);

		}

		[TestMethod]
		public void TestExpiredCache()
		{
			var caching = new DistributedCacheStore();
			caching.SetCachedObject(TestCacheKey, 0, GenerateTestList);
			var result = caching.Get(TestCacheKey);
			Assert.AreEqual(null, result);
		}

		[TestMethod]
		public void TestRemoveCache()
		{
			var caching = new DistributedCacheStore();
			var list = GenerateTestList();
			caching.Set(TestCacheKey, 5, list);
			caching.Remove(TestCacheKey);
			var result = caching.Get(TestCacheKey);
			Assert.AreEqual(null, result);
		}

		[TestMethod]
		public void TestGetObjectFromCacheInvalidCastException()
		{
			var caching = new DistributedCacheStore();
			var mockDb = GenerateMockDatabase();
			caching.Set(TestCacheKey, 5, mockDb);
			try
			{
				var result = caching.GetObjectFromCache<List<string>>(TestCacheKey);
				Assert.Fail("Should throw invalid format exception");
			}
			catch (Exception e)
			{
				Assert.AreEqual(typeof(InvalidCastException), e.GetType());
			}
		}

		[TestMethod]
		public void TestGetObjectFromCacheEmptyValue()
		{
			var caching = new DistributedCacheStore();
			var mockDb = GenerateMockDatabase();
			caching.Set(TestCacheKey, 5, mockDb);
			var result = caching.GetObjectFromCache<Dictionary<string, string>>(TestCacheKey2);
			Assert.AreEqual(null, result);
		}

		[TestMethod]
		public void TestGetObjectFromCache()
		{
			var caching = new DistributedCacheStore();
			var mockDb = GenerateMockDatabase();
			caching.Set(TestCacheKey, 5, mockDb);
			var result = caching.GetObjectFromCache<Dictionary<string, string>>(TestCacheKey);
			var expected = new Dictionary<string, string>()
			{
				{"key0","value0"},
				{"key1","value1"},
				{"key2","value2"},
				{"key3","value3"},
				{"key4","value4"},
				{"key5","value5"},
				{"key6","value6"},
				{"key7","value7"},
				{"key8","value8"},
				{"key9","value9"}
			};
			CollectionAssert.AreEquivalent(expected, result);
		}

		#region Implementation

		private List<string> GenerateTestList()
		{
			return new List<string>() { "1", "2", "3" };
		}
		private Dictionary<string, string> GenerateEmptyDictionary()
		{
			return new Dictionary<string, string>();
		}

		private Dictionary<string, string> GenerateMockDatabase()
		{
			var data = new Dictionary<string, string>();
			for (var i = 0; i < 10; i++)
			{
				var key = "key" + i;
				var value = "value" + i;
				data.Add(key, value);
			}
			return data;
		}
		public static readonly string TestCacheKey = "TestCacheKey";
		public static readonly string TestCacheKey2 = "TestCacheKey2";
		public static readonly string TestCacheKey3 = "TestCacheKey3";
		public static readonly string TestCacheKey4 = "TestCacheKey4";

		#endregion

	}
}
