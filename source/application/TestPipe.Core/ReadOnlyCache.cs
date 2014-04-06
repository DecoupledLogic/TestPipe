namespace TestPipe.Core
{
	using System;
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[Serializable]
	public class ReadOnlyCache<TKey, TValue> : Cache<TKey, TValue>
	{
		public ReadOnlyCache()
			: base()
		{
		}

		protected ReadOnlyCache(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		public new TValue this[TKey key]
		{
			get
			{
				return this.GetKeyValue(key);
			}
		}
	}
}