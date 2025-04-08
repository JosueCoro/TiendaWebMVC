using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        //USUARIO
        public ActionResult Usuarios()
        {
            return View();
        }
        public JsonResult ListarUsuarios()
        {
            List<Usuario> olista = new List<Usuario>();
            olista = new CN_Usuarios().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }








        //ROLES
        public ActionResult Roles()
        {
            return View();
        }











        //PERMISOS
        public ActionResult Permisos()
        {
            return View();
        }

    }
}