using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoTest.Models
{
    public class DetalleCompra
    {
        public int IdDetalleCompra { get; set; }
        public int IdCompra { get; set; }
        public int IdArticulo { get; set; }
        public Articulo oArticulo { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
    }
}