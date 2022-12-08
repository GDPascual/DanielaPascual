using ProyectoTest.Logica;
using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoTest.Controllers
{
    public class TiendaController : Controller
    {
        private static Cliente oCliente;
        //VISTA
        public ActionResult Index()
        {
            if (Session["Cliente"] == null)
                return RedirectToAction("Index", "Login");
            else
                oCliente = (Cliente)Session["Cliente"];

            return View();
        }

        //VISTA
        public ActionResult Articulo(int idarticulo = 0)
        {
            if (Session["Cliente"] == null)
                return RedirectToAction("Index", "Login");
            else
                oCliente = (Cliente)Session["Cliente"];

            Articulo oArticulo = new Articulo();
            List<Articulo> oLista = new List<Articulo>();

            oLista = ArticuloLogica.Instancia.Listar();
            oArticulo = (from o in oLista
                      where o.IdArticulo == idarticulo
                      select new Articulo()
                      {
                          IdArticulo = o.IdArticulo,
                          Nombre = o.Nombre,
                          Descripcion = o.Descripcion,
                          oMarca = o.oMarca,
                          oCategoria = o.oCategoria,
                          Precio = o.Precio,
                          Stock = o.Stock,
                          RutaImagen = o.RutaImagen,
                          base64 = utilidades.convertirBase64(Server.MapPath(o.RutaImagen)),
                          extension = Path.GetExtension(o.RutaImagen).Replace(".", ""),
                          Activo = o.Activo
                      }).FirstOrDefault();

            return View(oArticulo);
        }

        //VISTA
        public ActionResult Carrito()
        {
            if (Session["Cliente"] == null)
                return RedirectToAction("Index", "Login");
            else
                oCliente = (Cliente)Session["Cliente"];

            return View();
        }

        //VISTA
        public ActionResult Compras()
        {
            if (Session["Cliente"] == null)
                return RedirectToAction("Index", "Login");
            else
                oCliente = (Cliente)Session["Cliente"];

            return View();
        }






        [HttpPost]
        public JsonResult ListarArticulo(int idcategoria = 0)
        {
            List<Articulo> oLista = new List<Articulo>();

            oLista = ArticuloLogica.Instancia.Listar();
            oLista = (from o in oLista
                      select new Articulo()
                      {
                          IdArticulo = o.IdArticulo,
                          Nombre = o.Nombre,
                          Descripcion = o.Descripcion,
                          oMarca = o.oMarca,
                          oCategoria = o.oCategoria,
                          Precio = o.Precio,
                          Stock = o.Stock,
                          RutaImagen = o.RutaImagen,
                          base64 = utilidades.convertirBase64(Server.MapPath(o.RutaImagen)),
                          extension = Path.GetExtension(o.RutaImagen).Replace(".", ""),
                          Activo = o.Activo
                      }).ToList();

            if (idcategoria != 0){
                oLista = oLista.Where(x => x.oCategoria.IdCategoria == idcategoria).ToList() ;
            }

            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = CategoriaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertarCarrito(Carrito oCarrito)
        {
            oCarrito.oCliente = new Cliente() { IdCliente = oCliente.IdCliente };
            int _respuesta = 0;
            _respuesta = CarritoLogica.Instancia.Registrar(oCarrito) ;
            return Json(new { respuesta = _respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult CantidadCarrito()
        {
            int _respuesta = 0;
            _respuesta = CarritoLogica.Instancia.Cantidad(oCliente.IdCliente);
            return Json(new { respuesta = _respuesta }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ObtenerCarrito()
        {
            List<Carrito> oLista = new List<Carrito>();
            oLista = CarritoLogica.Instancia.Obtener(oCliente.IdCliente);

            if (oLista.Count != 0) {
                oLista = (from d in oLista
                          select new Carrito()
                          {
                              IdCarrito = d.IdCarrito,
                              oArticulo = new Articulo()
                              {
                                  IdArticulo = d.oArticulo.IdArticulo,
                                  Nombre = d.oArticulo.Nombre,
                                  oMarca = new Marca() { Descripcion = d.oArticulo.oMarca.Descripcion },
                                  Precio = d.oArticulo.Precio,
                                  RutaImagen = d.oArticulo.RutaImagen,
                                  base64 = utilidades.convertirBase64(Server.MapPath(d.oArticulo.RutaImagen)),
                                  extension = Path.GetExtension(d.oArticulo.RutaImagen).Replace(".", ""),
                              }
                          }).ToList();
            }


            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarCarrito(string IdCarrito, string IdArticulo)
        {
            bool respuesta = false;
            respuesta = CarritoLogica.Instancia.Eliminar(IdCarrito, IdArticulo);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CerrarSesion() {
            FormsAuthentication.SignOut();
            Session["Usuario"] = null;
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public JsonResult ObtenerDepartamento()
        {
            List<Departamento> oLista = new List<Departamento>();
            oLista = UbigeoLogica.Instancia.ObtenerDepartamento();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerProvincia(string _IdDepartamento)
        {
            List<Provincia> oLista = new List<Provincia>();
            oLista = UbigeoLogica.Instancia.ObtenerProvincia(_IdDepartamento);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ObtenerDistrito(string _IdProvincia,string _IdDepartamento)
        {
            List<Distrito> oLista = new List<Distrito>();
            oLista = UbigeoLogica.Instancia.ObtenerDistrito(_IdProvincia,_IdDepartamento);
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RegistrarCompra(Compra oCompra)
        {
            bool respuesta = false;

            oCompra.IdCliente = oCliente.IdCliente;
            respuesta = CompraLogica.Instancia.Registrar(oCompra);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        //
        [HttpGet]
        public JsonResult ObtenerCompra()
        {
            List<Compra> oLista = new List<Compra>();

            oLista = CarritoLogica.Instancia.ObtenerCompra(oCliente.IdCliente);

            oLista = (from c in oLista
                      select new Compra()
                      {
                          Total = c.Total,
                          FechaTexto = c.FechaTexto,
                          oDetalleCompra = (from dc in c.oDetalleCompra
                                            select new DetalleCompra() {
                                                oArticulo = new Articulo() {
                                                    oMarca = new Marca() {Descripcion = dc.oArticulo.oMarca.Descripcion },
                                                    Nombre = dc.oArticulo.Nombre,
                                                    RutaImagen = dc.oArticulo.RutaImagen,
                                                    base64 = utilidades.convertirBase64(Server.MapPath(dc.oArticulo.RutaImagen)),
                                                    extension = Path.GetExtension(dc.oArticulo.RutaImagen).Replace(".", ""),
                                                },
                                                Total = dc.Total,
                                                Cantidad = dc.Cantidad
                                            }).ToList()
                      }).ToList();
            return Json(new { lista = oLista }, JsonRequestBehavior.AllowGet);
        }
    }
}