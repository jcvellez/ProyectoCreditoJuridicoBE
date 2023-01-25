using bg.hd.banca.juridica.application.models.exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bg.hd.banca.juridica.application.models.ms
{
    public class MSErrors 
    {
        public static BaseCustomException DOCUMENTO_DE_IDENTIDAD_NO_EXISTE = new ("0001", "Documento de identidad no existe", 400);
        public static BaseCustomException DOCUMENTO_SIN_FOTO_DE_PERSONA = new ("0002", "Foto del documento de identidad no existe", 400);
        public static BaseCustomException DOCUMENTO_SIN_FOTO_DE_FIRMA = new ("0003", "Firma del documento de identidad no existe", 400);
        public static BaseCustomException IDENTIFICACION_SIN_AFILIACIONES = new ("0004", "La identificación no posee afiliaciones", 400);
        public static BaseCustomException IDENTIFICACION_SIN_RUC = new ("0005", "La identificación no posee registros en SRI", 400);
        public static BaseCustomException RUC_NO_EXISTE = new ("0006", "No existe un contribuyente con el número en el SRI", 400);
        public static BaseCustomException IDENTIFICACION_NO_VALIDA = new ("0007", "La identificación enviada no es válida", 400);
        public static BaseCustomException USUARIO_CON_IDENTIFICACION_NO_EXISTE = new ("0008", "No existe usuario con la identificación", 400);
        public static BaseCustomException CIUDADANO_NO_EXISTE = new ("0009", "El ciudadano no existe", 400);
        public static BaseCustomException FECHA_INICIAL_INVALIDA = new ("0010", "La fecha inicial es inválida", 400);
        public static BaseCustomException FECHA_FINAL_INVALIDA = new ("0011", "La fecha final es inválida", 400);
        public static BaseCustomException PARAMETRO_NO_EXISTE = new ("0012", "El parámetro no existe", 400);
        public static BaseCustomException IMAGEN_NO_VALIDA = new ("0013", "La imagen no es válida", 400);
        public static BaseCustomException CLIENTE_NO_EXISTE = new ("0014", "Cliente no existe", 400);
        public static BaseCustomException AHORRO_META_NO_EXISTE = new ("0015", "Ahorro Meta no existe", 400);
        public static BaseCustomException EXCLUSION_CNB_NO_EXISTE = new ("0016", "Exclusión CNB no existe", 400);
        public static BaseCustomException ANTECEDENTE_PENAL_NO_EXISTE = new ("0017", "Antecedente Penal no encontrado.", 400);
        public static BaseCustomException CODIGO_DACTILAR_INVALIDO = new ("0018", "El código dactilar es inválido.", 400);
        public static BaseCustomException REGISTRO_SUSPENDIDO_NO_EXISTE = new ("0019", "El registro suspendido de RC no existe.", 400);
        public static BaseCustomException REGISTRO_SUSPENDIDO_YA_DESACTIVADO = new ("0020", "El registro suspendido ya ha sido desactivado.", 400);
        public static BaseCustomException ID_USUARIO_NO_VALIDO = new ("0021", "El idUsuario no es válido.", 400);
        public static BaseCustomException CUENTA_NO_VALIDA = new ("0022", "La cuenta enviada no es válida", 400);
        
        public static BaseCustomException HTTP_ORDS = new ("9000", "Error HTTP de ORDS: %s", 500);
        public static BaseCustomException SERVICIO_REGISTRO_CIVIL_NO_DISPONIBLE = new ("9001", "No se puede acceder al servicio web de Registro Civil", 500);
        public static BaseCustomException CONSULTA_NO_EXITOSA = new ("9002", "No se pudo realizar la consulta", 500);
        public static BaseCustomException ACTUALIZACION_NO_EXITOSA = new ("9003", "No se pudo realizar la actualización", 500);
        public static BaseCustomException NO_ESPERADO = new("9999", "Error no esperado", 500);
    }    
}
