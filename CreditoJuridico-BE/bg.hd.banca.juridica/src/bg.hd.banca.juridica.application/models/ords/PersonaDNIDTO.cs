using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.ords
{
    public class PersonaDNIDTO
    {
        public string? Identificacion { get; set; }
        public string? Nombre { get; set; }
        public bool? EsCliente { get; set; }
    }
}
