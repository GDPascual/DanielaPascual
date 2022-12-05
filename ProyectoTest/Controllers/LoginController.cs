using ProyectoTest.Logica;
using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProyectoTest.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string NCorreo, string NContrasena)
        {
            
            Cliente oCliente = new Cliente();

            oCliente = ClienteLogica.Instancia.Obtener(NCorreo, NContrasena);

            if (oCliente == null)
            {
                ViewBag.Error = "Correo o contraseña no correcta";
                return View();
            }

            FormsAuthentication.SetAuthCookie(oCliente.Correo, false);
            Session["Cliente"] = oCliente;

            if (oCliente.EsAdministrador == true)
            {
                return RedirectToAction("Index", "Home");
            }
            else {
                return RedirectToAction("Index", "Tienda");
            }

            
        }

        // GET: Login
        public ActionResult Registrarse()
        {
            return View(new Cliente() { Nombres= "",Apellidos= "",Correo="",Contrasena="",ConfirmarContrasena="" });
        }

        [HttpPost]
        public ActionResult Registrarse(string NNombres, string NApellidos, string NCorreo, string NContrasena, string NConfirmarContrasena)
        {
            Cliente oCliente = new Cliente()
            {
                Nombres = NNombres,
                Apellidos = NApellidos,
                Correo = NCorreo,
                Contrasena = NContrasena,
                ConfirmarContrasena = NConfirmarContrasena,
                EsAdministrador = false
            };

            if (NContrasena != NConfirmarContrasena)
            {
                ViewBag.Error = "Las contraseñas no coinciden";
                return View(oCliente);
            }
            else {
                

                int idcliente_respuesta = ClienteLogica.Instancia.Registrar(oCliente);

                if (idcliente_respuesta == 0)
                {
                    ViewBag.Error = "Error al registrar";
                    return View();

                }
                else {
                    return RedirectToAction("Index", "Login");
                }
            }
        }

    }

}