using System;
using Microsoft.Extensions.DependencyInjection;
using NMTest.DataSource;

namespace NMTest.Sample
{
	public static class StartUp
	{
		public static IServiceProvider Setup()
		{

			var serviceProvider = new ServiceCollection()
				.AddSingleton<IDistributedCacheStore, DistributedCacheStore>()
				.AddSingleton<IDatabaseStore, DatabaseStore>()
				.BuildServiceProvider();
			serviceProvider.GetService<IDistributedCacheStore>().SetCachedObject(Resources.CacheKeyName, 20, serviceProvider.GetService<IDatabaseStore>().LoadFromDatabase);
			Console.WriteLine("DataLoaded from DataStore successfully, Cache setup successfully");

			return serviceProvider;
		}
	}
}