using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.persona
{
    public class PersonaDatosDNI
    {
        public int Id { get; set; }
        public string? Identificacion { get; set; }
        public string? Nombres { get; set; }
        public string? Genero { get; set; }
        public string? Tipo { get; set; }
        public string? FechaNacimiento { get; set; }
        public string? LugarNacimiento { get; set; }
        public string? CodigoDactilar { get; set; }
        public string? LugarExpedicion { get; set; }
        public string? FechaExpedicion { get; set; }
        public string? FechaExpiracion { get; set; }
        public string? Nacionalidad { get; set; }
        public string? EstadoCivil { get; set; }
        public string? NivelEducacion { get; set; }
        public string? Profesion { get; set; }
        public string? FechaDefuncion { get; set; }
        public string? LugarDefuncion { get; set; }
        public string? LugarDomicilio { get; set; }
        public string? DireccionDomicilio { get; set; }
        public string? IdentificacionConyuge { get; set; }
        public bool? Discapacitado { get; set; }
        public bool? Fallecido { get; set; }
        public string? FechaUltimaAct { get; set; }
    }
}
