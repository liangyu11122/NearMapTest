using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using NMTest.DataSource;

namespace NMTest.Sample
{
	public static class Program
	{
		public static void Main(string[] args)
		{
			// your code goes here
			var service = StartUp.Setup();
			var caching = service.GetService<IDistributedCacheStore>();
			var database = service.GetService<IDatabaseStore>();
			var failedTimes = 0;
			var summaryReport = new StringBuilder();
			failedTimes = Process(caching, database, failedTimes);
			summaryReport.AppendFormat("{0} failed to get from Cache, ", failedTimes);
			Console.WriteLine(summaryReport);
		}

		public static int Process(IDistributedCacheStore caching, IDatabaseStore database, int failedTimes)
		{
			for (var i = 0; i < 10; i++)
			{
				var thread = new Thread(() =>
				{
					var counter = 0;
					while (counter < 50)
					{
						var report = ProcessSingleThread(caching, database, ref failedTimes);
						counter++;
						Console.WriteLine(report);
					}
				});

				thread.Start();
				thread.Join();
			}

			return failedTimes;
		}

		public static StringBuilder ProcessSingleThread(IDistributedCacheStore caching, IDatabaseStore database, ref int failedTimes)
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();
			var report = new StringBuilder();
			var key = ProgramHelper.GenerateRandomKey();
			var value = string.Empty;
			try
			{
				var cacheDictionary = caching.GetObjectFromCache<Dictionary<string, string>>(Resources.CacheKeyName);
				cacheDictionary.TryGetValue(key, out value);
				if (!ProgramHelper.IsValidValue(value))
				{
					failedTimes++;
					value = database.GetValueFromDatabase(key);
					report.AppendFormat("Failed Getting Form Cache, reading from Database with Value {0}", value);
				}
				stopWatch.Stop();
				return report.AppendFormat("[{0}] Request '{1}', response '{2}', time: {3:0.##} ms",
					Thread.CurrentThread.ManagedThreadId, key, value, stopWatch.Elapsed.TotalMilliseconds);
			}
			catch (Exception e)
			{
				failedTimes++;
				stopWatch.Stop();
				return report.AppendFormat("[{0}] Request '{1}', response '{2}', time: {3:0.##} ms end with exception {4}",
					Thread.CurrentThread.ManagedThreadId, key, value, stopWatch.Elapsed.TotalMilliseconds, e);
			}
		}
	}
}