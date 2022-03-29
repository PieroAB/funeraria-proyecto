using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Funeraria2020.Models
{
    public class Planes
    {
        [StringLength(7)] public string codPlan { get; set; }

        [Required(ErrorMessage = "Ingrese un nombre para el plan")] [StringLength(50)] public string nombreplan { get; set; }
        [Required(ErrorMessage = "Ingrese un Precio")] public decimal precioPlan { get; set; }
        [StringLength(80)] public string imgPlan { get; set; }

        public int estPlan { get; set; }
        public string cuentaplanes{get;set;}
        
        public string existplanes { get; set; }
    }
}