using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.juridica.domain.entities.catalogos
{
    public class ConsultarCatalogoRequest
    {
        /// <summary>
        /// IdCatalogo 
        /// </summary>
        /// <example>cuotaMensual</example>
        [JsonIgnore]
        public string? producto { get; set; }
        /// <summary>
        /// Opción 
        /// </summary>
        /// <example>1</example>
        [Required] public int opcion { get; set; }
        /// <summary>
        /// IdCatalogo 
        /// </summary>
        /// <example>345</example>
        [Required] public string idCatalogo { get; set; }
        /// <summary>
        /// IdCatalogoPadre
        /// </summary>
        /// <example>0</example>
        public string? idCatalogoPadre { get; set; }
        [JsonIgnore]
        public string? Filtro { get; set; }
        [JsonIgnore]
        /// <example>0</example>
        public string? valorFiltro { get; set; }

    }
}
