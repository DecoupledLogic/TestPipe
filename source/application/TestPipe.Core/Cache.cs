namespace TestPipe.Core
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Runtime.Serialization;

	[Serializable]
	public class Cache<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
	{
		public Cache()
			: base()
		{
		}

		protected Cache(SerializationInfo info, StreamingContext context)
			: base()
		{
		}

		public new TValue this[TKey key]
		{
			get
			{
				return this.GetKeyValue(key);
			}

			set
			{
				this.SetKey(key, value);
			}
		}

		public void Add(TKey key, TValue value)
		{
			this.SetKey(key, value);
		}

		public void Clear(string keyPrefix)
		{
			foreach (TKey key in this.Keys)
			{
				if (((string)Convert.ChangeType(key, typeof(string))).StartsWith(keyPrefix))
				{
					this.Remove(key);
				}
			}
		}

		public bool ContainsKey(string key)
		{
			return base.ContainsKey(GetKey(key));
		}

		protected TValue GetKeyValue(TKey key)
		{
			if (this.ContainsKey(key))
			{
				return base[key];
			}

			if (this.AliasContainsKey(key))
			{
				return base[(TKey)Convert.ChangeType(this.AliasValue()[(key as string)], typeof(TKey))];
			}

			throw new KeyNotFoundException(string.Format("The alias: \"{0}\" could not be found. Please check that an appropriate element exists in the configuration file(s) with the former alias.", key));
		}

		protected void Remove(TKey key)
		{
			TValue value;
			this.TryRemove(key, out value);
		}

		protected void SetKey(TKey key, TValue value)
		{
           if (!this.TryAdd(key, value))
           {
               throw new ArgumentException(string.Format("Duplicate key: \"{0}\" with value: \"{1}\" provided during Dictionary Merge.", key.ToString(), value.ToString()));
           }		
		}

		private static TKey GetKey(string key)
		{
			return (TKey)Convert.ChangeType(key, typeof(TKey));
		}

		private bool AliasContainsKey(TKey key)
		{
			return this.ContainsKey("Aliases") && this.AliasValue().ContainsKey(key as string);
		}

		private Dictionary<string, string> AliasValue()
		{
			return base[GetKey("Aliases")] as Dictionary<string, string>;
		}
	}
}