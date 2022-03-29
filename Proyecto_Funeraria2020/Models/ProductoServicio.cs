using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Funeraria2020.Models
{
    public class ProductoServicio
    {
        [StringLength(7)] public string codItem { get; set; }
        [Required(ErrorMessage = "Ingrese Nombre")] [StringLength(50)] public string nomItem { get; set; }
        [Required(ErrorMessage = "Ingrese Descripción")] [StringLength(50)] public string desItem { get; set; }
        public string codColor { get; set; }
        public string codMaterial { get; set; }
        public int stockItem { get; set; }
        public Decimal precioItem { get; set; }
        [StringLength(80)] public string imgItem { get; set; }
        public int codCategoria { get; set; }
        public int codEstado { get; set; }

        public string existEnPlan { get; set; }
    }
}