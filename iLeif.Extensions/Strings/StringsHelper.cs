using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iLeif.Extensions.Strings
{
	public static class StringsHelper
	{
		public static string AddOrReplaceBracketsMark(this string name, string textToBrackets)
		{
			string newName = name;
			Regex regex = new Regex(@"(?<=\()\d+(?=\)$)");

			var match = regex.Match(name);

			if (match.Success)
			{
				//Remove brackets from original name
				newName = name.Remove(match.Index - 1, match.Index + match.Value.Length + 1);
			}

			newName = $"{newName} ({textToBrackets})";
			return newName;
		}

		/// <summary>
		/// Find (6) in filename and compare it with given names set. (7)
		/// Substitute or replace new name with (8) in the end.
		/// Find brackets with digits before file end of line
		/// </summary>
		/// <param name="name"></param>
		/// <param name="namesForCompare"></param>
		/// <returns></returns>
		public static string GetUniqueName(this string name, IEnumerable<string> namesForCompare)
		{
			if (namesForCompare == null || !namesForCompare.Contains(name))
			{
				return name;
			}

			Regex regex = new Regex(@"(?<=\()\d+(?=\)$)");

			var match = regex.Match(name);
			int nameIndex = 1;

			if (match.Success && int.TryParse(match.Value, out int parsedIndex))
			{
				nameIndex = parsedIndex;

				//Remove brackets from original name
				name = name.Remove(match.Index - 1, match.Index + match.Value.Length + 1);
			}

			string newName = $"{name} ({nameIndex})";

			while (namesForCompare.Contains(newName))
			{
				nameIndex++;
				newName = $"{name} ({nameIndex})";
			}

			return newName;
		}

		/// <summary>
		/// Find (6) in filename and compare it with given names set. (7)
		/// Substitute or replace new name with (8) in the end.
		/// Find brackets with digits before file extension
		/// </summary>
		/// <param name="name"></param>
		/// <param name="namesForCompare"></param>
		/// <returns></returns>
		public static string GetUniqueNameWithExtension(this string name, IEnumerable<string> namesForCompare)
		{
			//new Regex(@"(?<=\\()\\d+(?=\\)\\.)")

			var extension = Path.GetExtension(name);
			var nameWithoutExtension = Path.GetFileNameWithoutExtension(name);
			var namesWithoutExtensions = namesForCompare
				.Where(n => n.EndsWith(extension))
				.Select(n => Path.GetFileNameWithoutExtension(n));

			var resultName = GetUniqueName(nameWithoutExtension, namesWithoutExtensions);
			resultName += extension;

			return resultName;
		}
	}
}
