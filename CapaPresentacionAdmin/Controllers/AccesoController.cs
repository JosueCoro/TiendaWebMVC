using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

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

        public ActionResult CerrarSesion()
        {
            //cerrar  session
            Session["Usuario"] = null;
            Session.Abandon();
            Session.Clear();
            FormsAuthentication.SignOut();
            //vista de login
            return RedirectToAction("Index", "Acceso");
            //return View();
        }

        public ActionResult CambiarContraseña()
        {
            //validar que id_user_activo no sea null osea que el usuario haya iniciado sesion para poder entrar aqui
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                //validar que el usuario tenga un rol asignado
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult CambiarContraseña(string correo, string contraseñaActual, string nuevaContraseña, string confirmarContraseña)
        {
            string mensaje = string.Empty;

            //validaciones
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

            User_activo oUserActivo = new User_activo();

            string contraseñaEncriptada = CN_Recursos.ConvertirSha256(contraseña);

            oUserActivo = new CN_Usuarios().ValidarUsuario(correo, contraseñaEncriptada);

            
            if (oUserActivo == null)
            {
                ViewBag.Error = "Correo o contraseña incorrectos, o el usuario se encuentra inactivo. Por favor, verifique sus datos e intente nuevamente.";
                return View();
            }
            else
            {
                Session["Usuario"] = oUserActivo;
                //Session["Usuario"] = oUsuario; guarda el objeto en la session para poder usarlo en otras vistas
                //validar que id_user_activo no sea null
                if (oUserActivo.id_user_activo == 0 && oUserActivo.id_tienda_user == 0)
                {
                    ViewBag.Error = "El usuario no tiene permisos para acceder al sistema.";
                    return View();
                }
                else
                {
                    FormsAuthentication.SetAuthCookie(oUserActivo.correo, false);
                    return RedirectToAction("Index", "Home");
                }

            }
        }
    }
}