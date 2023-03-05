using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions.Objects
{
	public static class Universal
	{
		public static (T left, T right) ReverseTuple<T>((T left, T right) input)
		{
			return (input.right, input.left);
		}
	}
}
