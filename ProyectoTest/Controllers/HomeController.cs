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

        public ActionResult Categoria()
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

        public ActionResult Tienda()
        {
            if (Session["Usuario"] == null)
                return RedirectToAction("Index", "Login");

            return View();
        }


        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = CategoriaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
    
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.IdCategoria == 0) ? CategoriaLogica.Instancia.Registrar(objeto) : CategoriaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            respuesta = CategoriaLogica.Instancia.Eliminar(id);
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
                          oCategoria = o.oCategoria,
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
                string physicalPath = Server.MapPath("~/Imagenes/Articulos");

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