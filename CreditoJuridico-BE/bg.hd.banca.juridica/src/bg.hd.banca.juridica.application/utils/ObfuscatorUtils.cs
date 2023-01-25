using System.Text.RegularExpressions;

namespace bg.hd.banca.juridica.application.utils
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
