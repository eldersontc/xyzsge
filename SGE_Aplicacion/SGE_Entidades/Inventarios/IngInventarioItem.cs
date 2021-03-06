﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Entidades.Inventarios
{
    public class IngInventarioItem
    {
        public virtual int idIngInventarioItem { get; set; }
        public virtual int idIngInventario { get; set; }
        public virtual Material material { get; set; }
        public virtual Almacen almacen { get; set; }
        public virtual Unidad unidad { get; set; }
        public virtual int factor { get; set; }
        public virtual decimal cantidad { get; set; }
        public virtual decimal precio { get; set; }
        public virtual decimal total { get; set; }
        public virtual decimal tipoCambio { get; set; }
    }
}
