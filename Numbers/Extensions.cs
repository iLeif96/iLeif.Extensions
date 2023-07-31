
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Settings = System.Configuration.ConfigurationManager;

namespace iLeif.Extensions.Numbers
{
	public static class Extensions
	{
		public static double Tolerance { get; set; } = 0.01;

		static Extensions()
		{
			LoadTolerance();
		}

		public static double ToRad(this double degrees) => Math.PI / 180 * degrees;

		public static double ToDegrees(this double rads) => 180 / Math.PI * rads;

		public static bool IsLessTolerance(this double v) => v <= Tolerance;
		public static bool IsMoreTolerance(this double v) => v >= Tolerance;

		public static bool IsTouching(this double v1, double v2) => Math.Abs(v2 - v1) <= Tolerance;

		public static bool IsTouchingLeastOne(this double v1, params double[] values)
		{
			foreach (double value in values)
			{
				if (Math.Abs(value - v1) <= Tolerance)
				{
					return true;
				}
			}

			return false;
		}

		public static bool IsTouchingAll(this double v1, params double[] values)
		{
			foreach (double value in values)
			{
				if (Math.Abs(value - v1) > Tolerance)
				{
					return false;
				}
			}

			return true;
		}

		public static void SaveTolerance(double tolerance)
		{
			Settings.AppSettings.Set(nameof(Tolerance), tolerance.ToString());
		}

		public static double LoadTolerance()
		{
			//TODO: Replace it to separate class depends on (dynamic) Settings loading
			var tolerance = Settings.AppSettings.Get(nameof(Tolerance));
			double digitTolerance;
			if (double.TryParse(tolerance, out digitTolerance))
			{
				Tolerance = digitTolerance;
			}

			return Tolerance;
		}
	}
}
