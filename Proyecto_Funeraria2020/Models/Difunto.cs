using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Funeraria2020.Models
{
    public class Difunto
    {
        [Required(ErrorMessage = "Ingrese Nro de documento")] [StringLength(8)] public string codDifunto { get; set; }
        public int codTipoDocumento { get; set; }
        [Required(ErrorMessage = "Ingrese Nombre")] [StringLength(50)] public string nomDifunto { get; set; }
        [Required(ErrorMessage = "Ingrese Apellido")] [StringLength(50)] public string apeDifunto { get; set; }
        public string codGenero { get; set; }
        public string codEstadoCivil { get; set; }

        [Required(ErrorMessage = "Ingrese una Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecNacDifunto { get; set; }

        [Required(ErrorMessage = "Ingrese una Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime fecFallDifunto { get; set; }
        [Required(ErrorMessage = "Ingrese Datos del Lugar")] [StringLength(80)] public string lugFallDifunto { get; set; }
        [Required(ErrorMessage = "Ingrese Imagen De Acta de Defuncion")] [StringLength(50)] public string imgActaDifunto { get; set; }
        public string asignaDifunto { get; set; }
    }
}