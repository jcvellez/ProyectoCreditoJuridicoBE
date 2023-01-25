using bg.hd.banca.juridica.domain.entities;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;


namespace bg.hd.banca.juridica.infrastructure.utils
{
    public class PrimitiveDataUtils
    {
        public static string objectToString(Object data)
        {
            string dataRetorno = String.Empty;
            foreach (PropertyDescriptor desc in TypeDescriptor.GetProperties(data))
            {
                if (desc.Name.ToLower() == "identificacion" || desc.Name.ToLower() == "cedula" || desc.Name.ToLower() == "numidentificacion")
                {
                    string identificacion = (string)desc.GetValue(data);
                    string identificacionFuscada = ObfuscateType(identificacion, typeTag.Identificacion);
                    dataRetorno += desc.Name + ": " + identificacionFuscada + "\n";
                }
                else
                    dataRetorno += desc.Name + ": " + desc.GetValue(data) + "\n";
            }
            return dataRetorno;

        }

        public static string ObfuscateType(string value, typeTag type)
        {
            if (string.IsNullOrEmpty(value))
                return value;
            switch (type)
            {
                case typeTag.Correo:
                    if (value.Contains("@"))
                    {
                        int idx = 0;

                        StringBuilder tempValueEmail = new StringBuilder();
                        string[] splitEmail = value.Split('@');
                        int limitemail = splitEmail[0].Length - 1;
                        foreach (var tag in splitEmail[0].ToArray())
                        {
                            if (idx >= 2 && idx < limitemail)
                            {
                                tempValueEmail.Append('X');
                            }
                            else
                            {
                                tempValueEmail.Append(tag);
                            }
                            idx++;
                        }
                        value = string.Format("{0}@{1}", tempValueEmail, splitEmail[1]);
                    }
                    break;
                case typeTag.Identificacion:
                    int i = 0;
                    int limit = value.Length - 3;
                    StringBuilder tempValue = new StringBuilder();
                    foreach (var tag in value.ToArray())
                    {
                        if (i >= 3 && i < limit)
                        {
                            tempValue.Append('X');
                        }
                        else
                        {
                            tempValue.Append(tag);
                        }
                        i++;
                    }
                    value = tempValue.ToString();
                    break;
                case typeTag.numeroCuenta:
                    int y = 0;
                    int limite = value.Length - 3;
                    StringBuilder tempValor = new StringBuilder();
                    foreach (var tag in value.ToArray())
                    {
                        if (y >= 3 && y < limite)
                        {
                            tempValor.Append('X');
                        }
                        else
                        {
                            tempValor.Append(tag);
                        }
                        y++;
                    }
                    value = tempValor.ToString();
                    break;
            }
            return value;
        }

        public enum typeTag
        {
            Telefono,
            Nombre,
            Correo,
            Identificacion,
            numeroCuenta
        }

        public static string objectToString(Object data, List<string> elementosIncluidos)
        {
            string dataRetorno = String.Empty;
            if (elementosIncluidos is null)
            {
                foreach (PropertyDescriptor desc in TypeDescriptor.GetProperties(data))
                {
                    dataRetorno += desc.Name + ": " + desc.GetValue(data) + "\n";
                }
            }
            else
            {
                foreach (PropertyDescriptor desc in TypeDescriptor.GetProperties(data))
                {
                    if (elementosIncluidos.Contains(desc.Name))
                    {
                        dataRetorno += desc.Name + ": " + desc.GetValue(data) + "\n";

                    }
                }
            }
            return dataRetorno;

        }

        public static DataTable ConvertXmlElementToDataTable(XmlElement? xmlElement, string tagName)
        {
            XmlNodeList xmlNodeList = xmlElement.GetElementsByTagName(tagName);

            DataTable dt = new DataTable(tagName);
            if (xmlNodeList.Count > 0)
            {
                int TempColumn = 0;
                foreach (XmlNode node in xmlNodeList.Item(0).ChildNodes)
                {
                    TempColumn++;
                    DataColumn dc = new DataColumn(node.Name, Type.GetType("System.String"));
                    if (dt.Columns.Contains(node.Name))
                    {
                        dt.Columns.Add(dc.ColumnName = dc.ColumnName + TempColumn.ToString());
                    }
                    else
                    {
                        dt.Columns.Add(dc);
                    }
                }
                int ColumnsCount = dt.Columns.Count;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < ColumnsCount; j++)
                    {
                        if (xmlNodeList.Item(i).ChildNodes[j] != null)
                            dr[j] = xmlNodeList.Item(i).ChildNodes[j].InnerText;
                        else
                            dr[j] = "";
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static string GetDataTableStringColum(DataSet src, string table, string column)
        {
            return src.Tables[table] != null &&
                   src.Tables[table].Columns.Contains(column) ?
                   src.Tables[table].Rows[0][column].ToString() : string.Empty;
        }

        public static string GetDataStringXmlNode(XmlElement src, string node)
        {
            // return src.GetElementsByTagName(node) != null ? src.GetElementsByTagName(node)[0].InnerText : "";
            // Cambia validacion de XmlNodeList nula por cantidad de sus elementos mayor a 0 (TS Juan Lung, 23-Dic-2021)
            return src.GetElementsByTagName(node).Count > 0 ? src.GetElementsByTagName(node)[0].InnerText : "";
        }

        public static Producto obtenerProducto(string nombreProducto, IConfiguration _configuration)
        {
            Producto productoSelect = new Producto();
            Producto[] producto = _configuration.GetSection("ProductoConfig").Get<Producto[]>();
            foreach (Producto productoAUX in producto)
            {
                if (productoAUX.codigoProducto == nombreProducto)
                {
                    productoSelect = productoAUX;
                }
            }

            return productoSelect;
        }

        public static Utilitarios obtenerCatalogosPermitidos(IConfiguration _configuration)
        {
            Utilitarios utilSelect = new Utilitarios();
            Utilitarios[] utilitarios = _configuration.GetSection("Utilitarios").Get<Utilitarios[]>();
            foreach (Utilitarios utilAUX in utilitarios)
            {
                utilSelect = utilAUX;
            }

            return utilSelect;
        }


        public static void saveLogsInformation(string resourceRequestPath, string identificador, Object requestDataO, Object responseDataO)
        {
            string requestData = objectToString(requestDataO);
            string responseData = objectToString(responseDataO);
            string indetificacionFuscate = ObfuscateType(identificador, typeTag.Identificacion);
            Log.Information("{ResourceRequestPath} {identificador} {requestData} {ResponseData}", resourceRequestPath, indetificacionFuscate, requestData, responseData);
        }

        public static bool ValidarEstructuraEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {

                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));


                string DomainMapper(Match match)
                {

                    var idn = new IdnMapping();


                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
