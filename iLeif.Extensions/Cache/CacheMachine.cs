using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Cache
{
	public class CacheMachine<TIdentificator, TCachedItem> where TCachedItem : ICachedItem<TCachedItem> where TIdentificator : notnull
	{
		public bool IsUseDefaultComparsion { get; set; }

		private Dictionary<TIdentificator, TCachedItem> _cachedItems { get; } = new();

		private Func<TCachedItem, TCachedItem, bool>? _defaultComparsionFunc;

		public CacheMachine()
		{

		}

		public CacheMachine(Func<TCachedItem, TCachedItem, bool> defaultComparsionFunc)
		{
			_defaultComparsionFunc = defaultComparsionFunc;
		}

		public bool CheckForNeedsUpdate(TIdentificator identificator, TCachedItem item)
		{
			bool result = true;

			if (_cachedItems.TryGetValue(identificator, out TCachedItem? cached))
			{
				if (cached is null)
				{
					return result;
				}

				if (_defaultComparsionFunc != null)
				{
					result = _defaultComparsionFunc(cached, item);
				}

				result = result && cached.CheckForNeedsUpdate(item);
			}

			return result;
		}

		public void AddToCache(TIdentificator identificator, TCachedItem input)
		{
			_cachedItems[identificator] = input;
		}

		public bool TryToCacheItem(TIdentificator identificator, TCachedItem input)
		{
			if (CheckForNeedsUpdate(identificator, input))
			{
				_cachedItems[identificator] = input;
				return true;
			}

			return false;
		}

		public TCachedItem GetCachedItem(TIdentificator identificator)
		{
			return _cachedItems[identificator];
		}

		public void Remove(TIdentificator identificator)
		{
			_cachedItems.Remove(identificator);
		}

		public void ClearCache()
		{
			_cachedItems.Clear();
		}
	}
}
