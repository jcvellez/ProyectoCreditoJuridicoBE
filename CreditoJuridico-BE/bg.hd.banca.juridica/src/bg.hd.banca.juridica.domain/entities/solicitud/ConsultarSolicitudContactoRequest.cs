using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.solicitud
{
    public class ConsultarSolicitudContactoRequest
    {
        public int Opcion { get; set; } = 0;
        public int IdSolicitudContacto { get; set; } = 0;
		public string? numeroIdentificacion { get; set; } = string.Empty;
		public string? razonSocial { get; set; } = string.Empty;
		public string? nombreContacto { get; set; } = string.Empty;
		public string? cargoContacto { get; set; } = string.Empty;
		public string? telefonoContacto { get; set; } = string.Empty;
		public string? correoContacto { get; set; } = string.Empty;
		public string? viaContacto { get; set; } = string.Empty;
		public int? idRangoVenta { get; set; } = 0;
		public int? idProvincia { get; set; } = 0;
		public int? idCiudad { get; set; } = 0;
		public string? envioCorreo { get; set; } = string.Empty;
		public string? montoSolicitado { get; set; } = "0";
		public int? idFormaPago { get; set; } = 0;
		public int? idDestino { get; set; } = 0;
		public int? idPlazo { get; set; } = 0;
		public int? idPeriodicidad { get; set; } = 0;
		public int? idTipoAmortizacion { get; set; } = 0;
		public int idClteHost { get; set; } = 0;
		public string oficial { get; set; } = string.Empty;


	}
}
