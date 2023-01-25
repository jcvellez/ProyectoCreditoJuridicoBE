using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.exceptions
{
    public class RegistrarSolicitudContactoException: BaseCustomException
    {
        public RegistrarSolicitudContactoException(string message = "RegistrarSolicitudContacto Exeption", string description = "", int statuscode = 500) : base(message, description, statuscode)
        {

        }
    }
}
