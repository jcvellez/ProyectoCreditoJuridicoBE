using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;


namespace bg.hd.banca.juridica.domain.entities.solicitud
{
    public class RegistrarSolicitudContactoRequest
    {
		[JsonIgnore]
		public int? opcion { get; set; }		
		/// <example>0</example>
		public int? SolicitudContacto { get; set; }
		/// <example>0999999999</example>
		public string? numeroIdentificacion { get; set; } = "";
		/// <example>KaizenDevs</example>
		public string? razonSocial { get; set; }
		/// <example>dev001</example>
		public string? nombreContacto { get; set; }
		/// <example>DevBack</example>
		public string? cargoContacto { get; set; }
		/// <example>0999800800</example>
		public string? telefonoContacto { get; set; }
		/// <example>dev001@bancoguayaquil.com</example>
		public string? correoContacto { get; set; }
		/// <example>T</example>
		public string? viaContacto { get; set; }
		/// <example>77498</example>
		public int? RangoVenta { get; set; } = 0;
		/// <example>18</example>
		public int? Provincia { get; set; } = 0;
		/// <example>56</example>
		public int? Ciudad { get; set; } = 0;
		/// <example>MailOficial</example>
		public string? Oficial { get; set; } = "";
		/// <example>0</example>
		[JsonIgnore] public string? envioCorreo { get; set; }
		/// <example>0</example>
		public int? montoSolicitado { get; set; } = 0;
		/// <example>1</example>
		public string? FormaPago { get; set; } = "0";
		/// <example>1</example>
		public string? Destino { get; set; } = "0";
		/// <example>1</example>
		public string? Plazo { get; set; } = "0";
		/// <example>1</example>
		public string? Periodicidad { get; set; } = "0";
		/// <example>1</example>
		public string? TipoAmortizacion { get; set; } = "0";		
	}
}
