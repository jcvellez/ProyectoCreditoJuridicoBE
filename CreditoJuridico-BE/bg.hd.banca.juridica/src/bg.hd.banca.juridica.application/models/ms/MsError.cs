using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.ms
{
    public class MsError
    {
        //public MsError() { }
        //public MsError(int status, string errorCode, string userMessage) { }
        public int? status { set; get; }
        public string? errorCode { set; get; }
        public string? userMessage { set; get; }
        public string? traceId { set; get; }
    }
}
