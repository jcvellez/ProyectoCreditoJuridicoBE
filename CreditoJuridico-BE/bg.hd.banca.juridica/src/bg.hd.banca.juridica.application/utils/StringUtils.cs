using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.utils
{
    public static class StringUtils
    {
		public static bool IsEmpty(this string? value) 
		{
			return string.IsNullOrEmpty(value);
		}

		public static bool IsNotEmpty(this string? value)
		{
			return !string.IsNullOrEmpty(value);
		}

        /*
		 * <p>Checks if a String is whitespace, empty ("") or null.</p>
		 *
		 * <pre>
		 * StringUtils.IsBlank(null)      = true
		 * StringUtils.IsBlank("")        = true
		 * StringUtils.IsBlank(" ")       = true
		 * StringUtils.IsBlank("bob")     = false
		 * StringUtils.IsBlank("  bob  ") = false
		 * </pre>
		 *
		 * @param str  the String to check, may be null
		 * @return <code>true</code> if the String is null, empty or whitespace
		 * @since 2.0
		 */
        public static bool IsBlank(this string? value)
		{
			return string.IsNullOrEmpty(value?.Trim());
		}
		public static bool IsNotBlank(this string? value)
		{
			return !string.IsNullOrEmpty(value?.Trim());
		}
	}	
}
