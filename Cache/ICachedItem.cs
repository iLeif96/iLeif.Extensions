using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Cache
{
	public interface ICachedItem<TCachedItem>
	{
		bool CheckForNeedsUpdate(TCachedItem newest);
	}
}
