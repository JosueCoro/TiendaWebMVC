using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CapaPresentacionAdmin.Controllers
{
    public class MantenedorController : Controller
    {
        // GET: Mantenedor
        public ActionResult Categorias()
        {
            return View();
        }

        public ActionResult Marcas() {
            return View();

        }
        public ActionResult Productos()
        {
            return View();
        }

        public ActionResult Servicios()
        {
            return View();
        }
        public ActionResult TipoPago()
        {
            return View();
        }
        public ActionResult Tiendas()
        {
            return View();
        }
    }
}