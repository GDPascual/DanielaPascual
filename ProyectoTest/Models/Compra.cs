using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoTest.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdCliente { get; set; }
        public string TotalArticulo { get; set; }
        public decimal Total { get; set; }
        public string Contacto { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string IdDistrito { get; set; }
        public string FechaTexto { get; set; }
        public List<DetalleCompra> oDetalleCompra { get; set; }

    }
}