using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Proyecto_Funeraria2020.Models
{
    public class RepresentanteDifunto
    {
        public string codRepresentante { get; set; }
        public string nombreDifunto { get; set; }
        public string apellidoDifunto { get; set; }
        public string codDifunto { get; set; }
        public int codParentesco { get; set; }
    }
}