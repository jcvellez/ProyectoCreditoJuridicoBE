using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.utils
{
    public static class DateTimeExtension
    {
        public static DateTime NowEC(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime().AddHours(-5);
        }
    }
}
