using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.persona
{
    public class ConsultaOficialResponse
    {
        public int CodigoRetorno = 0;
        public string Mensaje = string.Empty; 
        public string UsuarioOficial { get; set; } = string.Empty;
        public string OpidOficial { get; set; } = string.Empty;
        public string IdAgenciaOficial { get; set; } = string.Empty;
        public string UsuarioJefeAencia { get; set; } = string.Empty;
        public string OpidJefeAgencia { get; set; } = string.Empty;
        public bool OficialExiste { get; set; } = false;
    }
}
