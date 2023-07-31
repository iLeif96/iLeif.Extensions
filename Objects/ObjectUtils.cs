using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iLeif.Extensions
{
	public static class ObjectUtils
	{
		public static bool IsAnyNull([NotNullWhen(false)] params object?[] objects)
		{
			foreach (object? obj in objects)
			{
				if (obj is null)
				{
					return true;
				}
			}

			return false;
		}
	}
}
