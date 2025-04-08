using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class OperacionesController : Controller
    {
        // GET: Operaciones
        public ActionResult Ventas()
        {
            return View();
        }
        public ActionResult Compras()
        {
            return View();
        }
        public ActionResult Clientes()
        {
            return View();
        }
        public ActionResult Proveedores()
        {
            return View();
        }
        public ActionResult Stock()
        {
            return View();
        }

    }
}