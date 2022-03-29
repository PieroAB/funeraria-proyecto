using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Funeraria2020.Models
{
    public class DetallePlan
    {
        public string codPlan { get; set; }
        public string codItem { get; set; }
        public string nomItem { get; set; }
        public string desItem { get; set; }
        public int cantPlan { get; set; }
        public int cantidad { get; set; }
        public decimal precio { get; set; }
        public decimal subtotal { get { return cantPlan * precio; } }

        public decimal subtotalrecibido { get; set; }
    }
}