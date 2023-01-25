using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.juridica.domain.entities.solicitud
{
    public class RegistrarSolicitudContactoRequestMicro
    {
		[JsonIgnore]
		public int? opcion { get; set; }
		public int? idSolicitudContacto { get; set; }
		public string? numeroIdentificacion { get; set; }
		public string? razonSocial { get; set; }
		public string? nombreContacto { get; set; }
		public string? cargoContacto { get; set; }
		public string? telefonoContacto { get; set; }
		public string? correoContacto { get; set; }
		public string? viaContacto { get; set; }
		public int? idRangoVenta { get; set; }
		public int? idProvincia { get; set; }
		public int? idCiudad { get; set; }
		public string? oficial { get; set; }
		public string? envioCorreo { get; set; }
		public string? montoSolicitado { get; set; }
		public int? idFormaPago { get; set; }
		public int? idDestino { get; set; }
		public int? idPlazo { get; set; }
		public int? idPeriodicidad { get; set; }
		public int? idTipoAmortizacion { get; set; }
		/// <example>0</example>
		/// 
		public int? idClteHost { get; set; }		
	}
}
