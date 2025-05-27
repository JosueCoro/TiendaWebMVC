using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacionAdmin.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
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


        #region USUARIO
        //USUARIO
        public ActionResult Usuarios()
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

        [HttpGet]
        public JsonResult ListarUsuarios()
        {
            List<Usuario> olista = new List<Usuario>();
            olista = new CN_Usuarios().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GuardarUsuario(Usuario objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if(objeto.id_usuario == 0)
            {
                resultado = new CN_Usuarios().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Usuarios().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult EliminarUsuario(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;

            respuesta = new CN_Usuarios().Eliminar(id, out Mensaje);

            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        //Actualizar contraseña
        [HttpPost]
        public JsonResult ActualizarContraseña(Usuario objeto)
        {
            bool resultado = false;
            string Mensaje = string.Empty;
            resultado = new CN_Usuarios().ActualizarContraseñaUsuario(objeto.id_usuario, objeto.correo, objeto.contraseña, out Mensaje);
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion




        #region ROL
        //ROLES
        public ActionResult Roles()
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

        [HttpGet]
        public JsonResult ListarRoles()
        {
            List<Roles> olista = new List<Roles>();
            olista = new CN_Roles().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult GuardarRol(Roles objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if (objeto.id_rol == 0)
            {
                resultado = new CN_Roles().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Roles().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarRol(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta = new CN_Roles().Eliminar(id, out Mensaje);
            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion










        //PERMISOS
        public ActionResult Permisos()
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


        //DASHBOARD
        //DashboardResumen
        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            // Instancia de la capa de negocio de reportes
            CN_Reporte objCnReporte = new CN_Reporte();

            // Obtener los datos resumidos del dashboard
            DashboardResumen oResumen = objCnReporte.ObtenerDatosDashboardResumen();

            // Devolver los datos como JSON
            // Se usa JsonRequestBehavior.AllowGet para permitir solicitudes GET
            return Json(new { resultado = oResumen }, JsonRequestBehavior.AllowGet);
        }

        // Nuevo método para obtener el historial de ventas para el DataTable
        [HttpGet]
        public JsonResult ObtenerHistorialVentas(string fechaInicio, string fechaFin, string idVenta)
        {
            List<ReporteVentaDetalle> lista = new List<ReporteVentaDetalle>();
            CN_Reporte objCnReporte = new CN_Reporte();

            lista = objCnReporte.ObtenerHistorialVentas(fechaInicio, fechaFin, idVenta);

            // DataTables espera un objeto con una propiedad 'data' que sea un array
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        //PrivacyPolicy
        public ActionResult PrivacyPolicy()
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

        //TermsAndConditions
        public ActionResult TermsAndConditions()
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
    }
}