using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.persona
{
    public class IdentificarClienteResponse
    {
        public int CodigoRetorno { get; set; } = 0;
        public string Mensaje { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = String.Empty;
        public bool EsCliente { get; set; } = false;

    }
}
