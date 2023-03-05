using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Events
{
	public class EventArgs<TObject> : EventArgs
	{
		public TObject Args { get; init; }

		public EventArgs(TObject args)
		{
			Args = args;
		}
	}
}
