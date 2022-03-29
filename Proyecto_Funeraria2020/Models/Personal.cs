using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Funeraria2020.Models
{
    public class Personal
    {
        [Required(ErrorMessage = "Ingrese DNI")][StringLength(8)]
        [RegularExpression(@"^[0-9]{8}$", ErrorMessage = "Ingrese un DNI correcto")]
        public string dniPersonal { get; set; }

        [Required(ErrorMessage = "Ingrese Nombre")][StringLength(50)] 
        public string nomPersonal { get; set; }

        [Required(ErrorMessage = "Ingrese Apellidos")][StringLength(50)]
        public string apePersonal { get; set; }
        public string codGenero { get; set; }

        [Required(ErrorMessage = "Ingrese una Dirección")][StringLength(80)]
        public string dirPersonal { get; set; }

        public int codDistrito { get; set; }

        [Required(ErrorMessage = "Ingrese una Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecNacPersonal { get; set; }
        [Required(ErrorMessage = "Ingrese una Numero Telefonico")]
        [StringLength(13)] 
        public string telPersonal { get; set; }
        [StringLength(60)] public string emailPersonal { get; set; }
        public int codCargo { get; set; }
        public Decimal sueldoPersonal { get; set; }
        [StringLength(15)] public string usuPersonal { get; set; }
        [StringLength(15)] public string conPersonal { get; set; }
        public int codEstadoPersonal { get; set; }
        [StringLength(80)] public string imgPersonal { get; set; }
        public string asigPersonal { get; set; }

    }
}