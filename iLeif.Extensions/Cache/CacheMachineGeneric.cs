using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Cache
{
    public class CacheMachineGeneric<TCachedItem>
    {
        public Func<TCachedItem, TCachedItem, bool>? IdentityChecker { get; set; }
        public bool IsUseDefaultComparsion { get; set; }

		private Dictionary<string, TCachedItem> _cachedItems { get; }


		public CacheMachineGeneric(bool isUseDeafaultComparsion, Func<TCachedItem, TCachedItem, bool>? identityChecker)
        {
			_cachedItems = new Dictionary<string, TCachedItem>();

            IdentityChecker = identityChecker;
            IsUseDefaultComparsion = isUseDeafaultComparsion;
        }

		public bool CheckForNeedsUpdate(string identificator, TCachedItem item)
		{
			bool result = true;

			if (_cachedItems.TryGetValue(identificator, out TCachedItem? cached))
			{
				if (cached is null || IsUseDefaultComparsion == false && IdentityChecker == null)
				{
					return result;
				}

				if (IsUseDefaultComparsion)
				{
					result = item.Equals(cached);
				}

				if (IdentityChecker != null)
				{
					result = result && IdentityChecker(cached, item);
				}
			}

			return result;
		}

		public void AddToCache(string identificator, TCachedItem input)
		{
			_cachedItems[identificator] = input;
		}

		public bool TryToCacheItem(string identificator, TCachedItem input)
		{
			if (CheckForNeedsUpdate(identificator, input))
			{
				_cachedItems[identificator] = input;
				return true;
			}

			return false;
		}

		public TCachedItem GetCachedItem(string identificator)
		{
			return _cachedItems[identificator];
		}

		public void Remove(string identificator)
		{
			_cachedItems.Remove(identificator);
		}

		public void ClearCache()
		{
			_cachedItems.Clear();
		}
	}
}
