using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Proyecto_Funeraria2020.Models
{
    public class Transaccion
    {
        public string codBoleta { get; set; }
        public string codRepresentante { get; set; }

        [Required(ErrorMessage = "Ingrese una Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fechaEmisionBoleta { get; set; }
        public string dirSepelio { get; set; }
        public string nomCementerio { get; set; }
        [Required(ErrorMessage = "Ingrese una Fecha")]
        public DateTime fechaSepelio { get; set; }
        public decimal precioSinIGV { get; set; }
        public decimal igv { get; set; }
        public decimal Total { get; set; }

        public int estado { get; set; }
        public string nombreestado { get; set; }
    }
}