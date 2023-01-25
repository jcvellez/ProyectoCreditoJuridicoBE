using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.utils
{
    public static class RegistroUtils
    {
        private const string _dactilar = @"[aievxAIEVX0][0-9xX][0-9xX][0-9xX][0-9xX][aievxAIEVX0].*";
        private static Regex _dactilarPattern = new Regex(_dactilar);
        
        public static bool DactilarInvalido(string? dactilar)
        {
            return string.IsNullOrEmpty(dactilar?.Trim()) || !_dactilarPattern.Matches(dactilar).Any();
        }
        public static bool DactilarValido(string? dactilar)
        {
            return !string.IsNullOrEmpty(dactilar?.Trim()) && _dactilarPattern.Matches(dactilar).Any();
        }

        public static string? MejorarDactilar(string? dactilar)
        {
            if (dactilar.IsBlank())
            {
                return null;
            }
            if (!string.IsNullOrEmpty(dactilar) && dactilar.Length >= 6)
            {
                dactilar = dactilar.Substring(0, 6);
            }
            return dactilar?.ToUpper();
        }

        public static string? MejorarDactilar(string? dactilar1, string? dactilar2)
        {
            if (dactilar1.IsNotBlank())
            {
                return MejorarDactilar(dactilar1);
            }
            else 
            {
                return MejorarDactilar(dactilar2);
            }
        }
    }
}
