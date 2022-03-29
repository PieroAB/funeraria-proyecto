using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Funeraria2020.Models
{
    public class DetalleTransaccion
    {
        public string codBoleta { get; set; }
        public string codItemPlan { get; set;}
        public string nombrePlan { get; set; }
        public int cantidad { get; set; }
        public decimal importe { get; set; }
        public decimal subtotal { get { return cantidad * importe; } }
    }
}