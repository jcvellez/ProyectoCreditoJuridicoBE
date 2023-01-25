using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.ldp
{
    public class ContratoDTO
    {                
        public string NumReferencia { get; set; }
        public string? Descripcion { get; set; }
        public byte[]? ArchivoContenidoBinario { get; set; }
        public string? ArchivoNombre { get; set; }
        public string? ArchivoContenidoTipo { get; set; }
        public int? MesesValidez { get; set; }        
    }
}
