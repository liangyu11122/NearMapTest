using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMTest.Sample;

namespace NMTest.Test
{
	[TestClass]
	public class ProgramHelperTests
	{
		[TestMethod]
		public void TestGenerateRandomKey()
		{
			var result = ProgramHelper.GenerateRandomKey();
			var collection = new List<string>()
			{
				"key0","key1","key2","key3","key4","key5","key6","key7","key8","key9",
			};
			Assert.IsTrue(collection.Contains(result));
		}

		[TestMethod]
		public void TestIsValidValue()
		{
			Assert.IsTrue(ProgramHelper.IsValidValue("value0"));
			Assert.IsTrue(ProgramHelper.IsValidValue("value1"));
			Assert.IsFalse(ProgramHelper.IsValidValue("key1"));
			Assert.IsFalse(ProgramHelper.IsValidValue("anything"));
		}
	}
}
