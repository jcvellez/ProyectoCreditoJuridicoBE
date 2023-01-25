using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.infrastructure.utils
{
    public static class ObfuscatorUtils
    {
		public static string? IdentificationNullableObfuscator(this string? identification)
		{
			if (identification == null)
			{
				return null;
			}
			return Regex.Replace(identification, @"(?<!^.?).(?!.?$)", "X");
		}
		public static string IdentificationObfuscator(this string identification)
		{
			return Regex.Replace(identification, @"(?<!^.?).(?!.?$)", "X");
		}
	}
}
