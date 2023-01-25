using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.ords
{
    public class PersonaDTO
    {
        public string? guid_persona { get; set; }
        public string? id_persona { get; set; }
        public long? id_clte { get; set; }
        public string? nombre { get; set; }
        public string? genero { get; set; }
        public string? ciudadania { get; set; }
        public string? rg_estado_civil { get; set; }        
    }
}
