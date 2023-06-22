using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Agente
    {
        public int AgenteId { get; set; }
        public string NombreAgente { get; set; }
        public string Tlf { get; set; }
        public string Ubigeo { get; set; }
        public string Direccion { get; set; }
        public string latlng { get; set; }
        public string estado { get; set; }  
 
        //public string captcha { get; set; }
    }
}
