namespace TestPipe.Specs
{
	using System;
	using System.Diagnostics;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.VisualStudio.TestTools.UnitTesting;
	using TestPipe.Core;

	[TestClass]
	public class CacheSpecs
	{
		private static Cache<int, int> cache = new Cache<int, int>();
		private static int count = 40;

		public static void Read()
		{
			foreach (var item in cache.Values)
			{
				int current = item;
				Thread.Sleep(20);
				Debugger.Break();
			}
		}

		public static void Write()
		{
			for (int i = count - 1; i >= 0; i--)
			{
				Thread.Sleep(10);

				int removedValue;
				if (!cache.TryRemove(i, out removedValue))
					Assert.Fail(string.Format("Did not remove item {0}", i));

				Debugger.Break();

				int newValue = 50 + (i * 2);
				cache[newValue] = newValue;

				Thread.Sleep(10);
				newValue++;
				if (!cache.TryAdd(newValue, newValue))
					Assert.Fail(string.Format("Did not add item {0}", newValue));

				Debugger.Break();
			}
		}

		[TestMethod]
        [TestCategory("Slow")]
		public void Cache_Should_Allow_Concurrent_Read_And_Write_From_Separate_Threads()
		{
			for (int i = 0; i < count; i++)
			{
				cache.TryAdd(i, i);
			}

			Task t1 = new Task(Read);
			Task t2 = new Task(Write);

			t1.Start();
			t2.Start();

			Task.WaitAny(t1, t2);
		}
	}
}