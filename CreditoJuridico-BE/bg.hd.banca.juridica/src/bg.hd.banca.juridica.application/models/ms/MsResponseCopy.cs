using bg.hd.banca.juridica.application.models.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.ms
{
    public class MsResponseCopy<T>
    {
        public MsResponseCopy(T data, string traceId)
        {
            this.traceid = traceId;
            this.data = data;
            success = true;
            collection = false;
            count = 1;
            error = null;
        }
        public MsResponseCopy(Exception exception, string traceId, int status)
        {
            this.traceid = traceId;
            success = false;           
            error = new MsError 
            {
                errorCode="999",
                userMessage=exception.Message,
                traceId=traceId,
                status=status
            };
            collection = false;
            count=0;
            data = default(T);
        }
        public MsResponseCopy(BaseCustomException exception, string traceId)
        {
            this.traceid = traceId;
            success = false;
            error = new MsError
            {
                errorCode = exception.Message,
                userMessage = exception.StackTrace.ToUpper().Contains(", CONTENT:")? exception.StackTrace.Substring(0, exception.StackTrace.IndexOf(", Content:") +1) : exception.StackTrace,
                traceId = traceId,
                status= exception.Code
            };
            collection = false;
            count = 0;
            data = default(T);
        }
        public string traceid { get; set; }
        public bool? success { get; set; }
        public bool? collection { get; set; }
        public int? count { get; set; }
        public T? data { get; set; }
        public MsError? error { get; set; }
    }
}
