﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGE.Entidades.Ventas
{
    public class LpServicio
    {
        public virtual int idLpServicio { get; set; }
        public virtual string descripcion { get; set; }
        public virtual bool activo { get; set; }
        public virtual List<LpServicioItem> items { get; set; }
        public virtual List<int> idsItems { get; set; }
    }
}
