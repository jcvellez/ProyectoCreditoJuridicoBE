using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.juridica.domain.entities.cotizador
{
    public class ConsultaTasaInteresRequest
    {
        public int idCanal { get; set; } = 0;
        public string Producto { get; set; } = "";
        public int PeriodicidadDias { get; set; } = 0;
        [JsonIgnore]
        public int idProducto { get; set; } = 0;

    }
}
