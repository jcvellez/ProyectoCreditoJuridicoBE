using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.dtos
{
    public class MsDtoResponseError
    {
        /// <summary>
        /// Código http.
        /// </summary>
        /// <example>400</example>
        public int code { get; set; }

        /// <summary>
        /// Identificador de trazabilidad.
        /// </summary>
        /// <example>6ee1b7a7bcd2c7c9</example>
        public string traceid { get; set; }

        /// <summary>
        /// Mensaje de error.
        /// </summary>
        /// <example>Error Aplicativo</example>
        public string message { get; set; }

        public List<MsDtoError> errors { get; set; }

    }
}
