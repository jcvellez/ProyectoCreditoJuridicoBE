using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace bg.hd.banca.juridica.domain.entities.solicitud
{
    public class RegistrarSolicitudContactoResponse
    {
        public int CodigoRetorno { get; set; }
        public string Mensaje { get; set; }
        public int idSolicitudContacto { get; set; }
        [System.Text.Json.Serialization.JsonIgnore] public DetalleSolicitudContacto datosConsulta { get; set; }

        [System.Text.Json.Serialization.JsonIgnore] public Estado ESTATUS { get; set; } = new Estado();
        [System.Text.Json.Serialization.JsonIgnore] public DatosSolicitudContacto DataSet { get; set; } = new DatosSolicitudContacto();
    }

    public class DatosSolicitudContacto
    {
        public DetalleSolicitudContacto Table { get; set; } = new DetalleSolicitudContacto();
    }

    public class DetalleSolicitudContacto
    {
        public int idSolicitudContacto { get; set; }
        public string numeroIdentificacion { get; set; }
        public string razonSocial { get; set; }
        public string nombreContacto { get; set; }
        public string cargoContacto { get; set; }
        public string telefonoContacto { get; set; }
        public string correoContacto { get; set; }
        public string viaContacto { get; set; }
        public int idRangoVenta { get; set; }
        public int idProvincia { get; set; }
        public int idCiudad { get; set; }
        public string envioCorreo { get; set; }
        public string montoSolicitado { get; set; }
        public int idFormaPago { get; set; }
        public int idDestino { get; set; }
        public int idPlazo { get; set; }
        public int idPeriodicidad { get; set; }
        public int idTipoAmortizacion { get; set; }
        public string? esCliente { get; set; }
    }

    public class Estado
    {
        public int CODIGO { get; set; }
        public string? MENSAJE { get; set; }
    }
}
