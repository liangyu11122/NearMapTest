using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Moq;
using NMTest.DataSource;
using NMTest.Sample;
using NUnit.Framework;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NMTest.Test
{
	[TestFixture]
	public class ProgramTests
	{
		private Mock<IDistributedCacheStore> CacheMock;
		private Mock<IDatabaseStore> DbMock;
		private Dictionary<string, string> MockData;

		[SetUp]
		public void Initialize()
		{
			CacheMock = new Mock<IDistributedCacheStore>();
			DbMock = new Mock<IDatabaseStore>();
			MockData = new Dictionary<string, string>()
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
		}

		[Test]
		public void TestProcessSuccess()
		{
			CacheMock.Setup(x => x.GetObjectFromCache<Dictionary<string, string>>(Resources.CacheKeyName)).Returns(MockData);
			var failedTimes = 0;
			Program.Process(CacheMock.Object, DbMock.Object, failedTimes);

			Assert.AreEqual(0, failedTimes);
		}
		[Test]
		public void TestProcessSingleThreadSuccess()
		{
			CacheMock.Setup(x => x.GetObjectFromCache<Dictionary<string, string>>(Resources.CacheKeyName)).Returns(MockData);
			var failedTimes = 0;
			var result = Program.ProcessSingleThread(CacheMock.Object, DbMock.Object, ref failedTimes);
			var match = Regex.Match(result.ToString(), @"\[\d+\]\sRequest\s\'key\d\',\sresponse\s\'value\d\',\stime:\s\d+\.\d\d\sms$");

			Assert.IsTrue(match.Success);
			Assert.AreEqual(0, failedTimes);
		}

		[Test]
		public void TestProcessSingleThreadFailFromCacheandDb()
		{
			CacheMock.Setup(x => x.GetObjectFromCache<Dictionary<string, string>>(Resources.CacheKeyName)).Throws(new Exception("Text Exception"));

			var failedTimes = 0;
			var result = Program.ProcessSingleThread(CacheMock.Object, DbMock.Object, ref failedTimes);

			Assert.IsTrue(result.ToString().Contains("end with exception"));
			Assert.IsTrue(result.ToString().Contains("Text Exception"));
			Assert.AreEqual(1, failedTimes);
		}

		[Test]
		public void TestProcessSingleThreadFailedToFindFromCache()
		{
			CacheMock.Setup(x => x.GetObjectFromCache<Dictionary<string, string>>(Resources.CacheKeyName)).Returns(new Dictionary<string, string>() { { "key10", "value10" } });
			DbMock.Setup(x => x.GetValueFromDatabase(It.IsAny<string>())).Returns("databaseValue");

			var failedTimes = 0;
			var result = Program.ProcessSingleThread(CacheMock.Object, DbMock.Object, ref failedTimes);
			Assert.IsTrue(result.ToString().Contains("Failed Getting Form Cache, reading from Database with Value"));
			Assert.IsTrue(result.ToString().Contains("databaseValue"));
			Assert.AreEqual(1, failedTimes);
		}


	}
}
