using System;
using System.Collections.Generic;
using System.Text;

namespace bg.hd.banca.juridica.domain.entities.catalogos
{
    public class ConsultarCatalogoRequestMicroServ
    {
        public int opcion { get; set; }        
        public string idCatalogo { get; set; }
        public string idCatalogoPadre { get; set; }
        public string Filtro { get; set; }
        public string valorFiltro { get; set; }
    }
}
