using ProyectoTest.Models;
using ProyectoTest.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace ProyectoTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Session["Cliente"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult Tienda()
        {
            if (Session["Cliente"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult Marca()
        {
            if (Session["Cliente"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        public ActionResult Articulo()
        {
            if (Session["Cliente"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }

        //public ActionResult Tienda()
        //{
        //    if (Session["Usuario"] == null)
        //        return RedirectToAction("Index", "Login");

        //    return View();
        //}


        [HttpGet]
        public JsonResult ListarTienda() {
            List<Tienda> oLista = new List<Tienda>();
            oLista = TiendaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarTienda(Tienda objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.IdTienda == 0) ? TiendaLogica.Instancia.Registrar(objeto) : TiendaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarTienda(int id)
        {
            bool respuesta = false;
            respuesta = TiendaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarMarca()
        {
            List<Marca> oLista = new List<Marca>();
            oLista = MarcaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarMarca(Marca objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.IdMarca == 0) ? MarcaLogica.Instancia.Registrar(objeto) : MarcaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarMarca(int id)
        {
            bool respuesta = false;
            respuesta = MarcaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarArticulo()
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
                          oTienda = o.oTienda,
                          Precio = o.Precio,
                          Stock = o.Stock,
                          RutaImagen = o.RutaImagen,
                          base64 = utilidades.convertirBase64(Server.MapPath(o.RutaImagen)),
                          extension = Path.GetExtension(o.RutaImagen).Replace(".", ""),
                          Activo = o.Activo
                      }).ToList();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarArticulo(string objeto, HttpPostedFileBase imagenArchivo)
        {

            Response oresponse = new Response() { resultado = true, mensaje = "" };

            try
            {
                Articulo oArticulo = new Articulo();
                oArticulo = JsonConvert.DeserializeObject<Articulo>(objeto);

                string GuardarEnRuta = "~/Imagenes/Articulos/";
                string physicalPath = Server.MapPath("~/Imagenes/Articulo");

                if (!Directory.Exists(physicalPath))
                    Directory.CreateDirectory(physicalPath);

                if (oArticulo.IdArticulo == 0)
                {
                    int id = ArticuloLogica.Instancia.Registrar(oArticulo);
                    oArticulo.IdArticulo = id;
                    oresponse.resultado = oArticulo.IdArticulo == 0 ? false : true;

                }
                else
                {
                    oresponse.resultado = ArticuloLogica.Instancia.Modificar(oArticulo);
                }

                
                if (imagenArchivo != null && oArticulo.IdArticulo != 0)
                {
                    string extension = Path.GetExtension(imagenArchivo.FileName);
                    GuardarEnRuta = GuardarEnRuta + oArticulo.IdArticulo.ToString() + extension;
                    oArticulo.RutaImagen = GuardarEnRuta;

                    imagenArchivo.SaveAs(physicalPath + "/" + oArticulo.IdArticulo.ToString() + extension );

                    oresponse.resultado = ArticuloLogica.Instancia.ActualizarRutaImagen(oArticulo);
                }

            }
            catch (Exception e)
            {
                oresponse.resultado = false;
                oresponse.mensaje = e.Message;
            }

            return Json(oresponse, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarArticulo(int id)
        {
            bool respuesta = false;
            respuesta = ArticuloLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }

    public class Response {

        public bool resultado { get; set; }
        public string mensaje { get; set; }
    }
}