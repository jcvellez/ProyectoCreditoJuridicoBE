using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.cotizador
{
    public class SimularCreditoRequest
    {
        /// <example>15000</example>
        public int monto { get; set; } = 0;
        /// <example>capitalTrabajo</example>
        public string? destinoFinanciero { get; set; }
        /// <example>alVencimiento</example>
        public string? tipoProducto { get; set; }
        /// <example>2</example>
        public int plazo { get; set; } = 0;
        /// <example>2123</example>
        public string? TipoAmortizacion { get; set; }
        /// <example>77498 </example>
        public string RangoVentas { get; set; } = "";
        /// <example>77492</example>
        public string PeriodoCuota { get; set; } = "";
        /// <example>alVencimiento</example>
        public string tipoProductoStr { get; set; } = "";
    }
}
