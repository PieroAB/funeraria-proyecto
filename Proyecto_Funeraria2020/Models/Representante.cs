using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Proyecto_Funeraria2020.Models
{
    public class Representante
    {
        [Required(ErrorMessage = "Ingrese Nro de Identidad")] [StringLength(11)] public string codRepresentante { get; set; }
        public int codTipoDocumento { get; set; }
        public int codTipoPersona { get; set; }

        public string combonombres { get; set; }

        [Required(ErrorMessage = "Ingrese Nombre")] [StringLength(50)] public string nomRepresentante { get; set; }
        [Required(ErrorMessage = "Ingrese Apellido")] [StringLength(50)] public string apeRepresentante { get; set; }
        public string codGenero { get; set; }
        [Required(ErrorMessage = "Ingrese Direccion")] [StringLength(80)] public string dirRepresentante { get; set; }
        public int codDistrito { get; set; }
        [Required(ErrorMessage = "Ingrese Telefono")] [StringLength(11)] public string telRepresentante { get; set; }
    }
}