using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class AccesoController : Controller
    {
        // GET: Acceso
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CambiarContraseña()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CambiarContraseña(string correo, string contraseñaActual, string nuevaContraseña, string confirmarContraseña)
        {
            string mensaje = string.Empty;

            // Validaciones
            if (string.IsNullOrWhiteSpace(correo) ||
                string.IsNullOrWhiteSpace(contraseñaActual) ||
                string.IsNullOrWhiteSpace(nuevaContraseña) ||
                string.IsNullOrWhiteSpace(confirmarContraseña))
            {
                ViewBag.Mensaje = "Todos los campos son obligatorios.";
                return View();
            }

            if (nuevaContraseña != confirmarContraseña)
            {
                ViewBag.Mensaje = "La nueva contraseña y su confirmación no coinciden.";
                return View();
            }

            bool resultado = new CN_Usuarios().CambiarContraseña(correo, contraseñaActual, nuevaContraseña, out mensaje);

            if (resultado)
            {
                ViewBag.MensajeExito = "Contraseña actualizada correctamente. Revise su correo.";
            }
            else
            {
                ViewBag.Mensaje = mensaje;  // mensaje puede venir de la capa de negocio
            }

            return View();
        }







        //Login
        [HttpPost]
        public ActionResult Index(string correo, string contraseña)
        {
            Usuario oUsuario = new Usuario();

            string contraseñaEncriptada = CN_Recursos.ConvertirSha256(contraseña);

            oUsuario = new CN_Usuarios().ValidarUsuario(correo, contraseñaEncriptada);

            if (oUsuario == null)
            {
                ViewBag.Error = "Correo o contraseña no corecta";
                return View();
            }
            else
            {
                Session["Usuario"] = oUsuario;
                //Session["id_usuario"] = oUsuario.id_usuario;
                return RedirectToAction("Index", "Home");
                //MOSTRAR EL ID DEL USUARIO QUE INICIO SESION
                
                //ViewBag.Error = null;
                //return RedirectToAction("Index", "Home");
            }
        }
    }
}