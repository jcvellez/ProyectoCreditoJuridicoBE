using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.exceptions
{
    public class AuthenticationServiceException : BaseCustomException
    {
        public AuthenticationServiceException(string? message, string? description, int statuscode = 500) : base(message ?? "Authentication Exception", description ?? "", statuscode)
        {
        }
    }
}
