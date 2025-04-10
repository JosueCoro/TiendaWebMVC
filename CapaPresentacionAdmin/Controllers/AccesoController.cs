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
                return RedirectToAction("Index", "Home");
                //ViewBag.Error = null;
                //return RedirectToAction("Index", "Home");
            }
        }
    }
}