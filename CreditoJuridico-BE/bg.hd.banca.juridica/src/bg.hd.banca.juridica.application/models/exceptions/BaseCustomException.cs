﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.exceptions
{
    public class BaseCustomException : Exception
    {
        public int Code { get; }
        public override string StackTrace { get; }

        public BaseCustomException(string message, string stackTrace, int code) : base(message)
        {
            Code = code;
            StackTrace = stackTrace;
        }
    }
}
