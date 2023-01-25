﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.dtos
{
    public class MsDtoError
    { 
        /// <summary>
        /// Código http.
        /// </summary>
        /// <example>400</example>
        public int code { get; set; }

        /// <summary>
        /// Mensaje de error.
        /// </summary>
        /// <example>Error Aplicativo</example>
        public string message { get; set; }
    }
}
