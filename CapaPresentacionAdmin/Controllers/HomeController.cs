using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CapaEntidad;
using CapaNegocio;
using OfficeOpenXml; 
using OfficeOpenXml.Style; 
using System.Drawing; 
using System.IO;

using CapaPresentacionAdmin.ViewModels;

namespace CapaPresentacionAdmin.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        #region Dashboard
        //Dashboard
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
        //DashboardResumen
        [HttpGet]
        public JsonResult VistaDashBoard()
        {
            CN_Reporte objCnReporte = new CN_Reporte();

            DashboardResumen oResumen = objCnReporte.ObtenerDatosDashboardResumen();

            return Json(new { resultado = oResumen }, JsonRequestBehavior.AllowGet);
        }



        //HistorialVentas para el DASHBOARD
        [HttpGet]
        public JsonResult ObtenerHistorialVentas(string fechaInicio, string fechaFin, string idVenta)
        {
            List<ReporteVentaDetalle> lista = new List<ReporteVentaDetalle>();
            CN_Reporte objCnReporte = new CN_Reporte();

            lista = objCnReporte.ObtenerHistorialVentas(fechaInicio, fechaFin, idVenta);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        #endregion



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



        #region PERMISOS
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
        [HttpGet]
        public JsonResult ListarPermisos()
        {
            List<Permisos> olista = new List<Permisos>();
            olista = new CN_Permisos().Listar(); 
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarPermiso(Permisos objeto)
        {
            object resultado; 
            string Mensaje = string.Empty; 

            if (objeto.id_permiso == 0)
            {
                resultado = new CN_Permisos().Registrar(objeto, out Mensaje);
            }
            else 
            {
                resultado = new CN_Permisos().Editar(objeto, out Mensaje);
            }

            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarPermiso(int id)
        {
            bool respuesta = false; 
            string Mensaje = string.Empty;

            respuesta = new CN_Permisos().Eliminar(id, out Mensaje); 

            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        #endregion



        public ActionResult RolesPorPermisos()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                // Aquí podrías validar un permiso específico, e.g., si tiene permiso para "Administrar Roles"
                // if (!oUserActivo.oRol.Permisos.Any(p => p.Nombre == "Administrar Roles")) { return RedirectToAction("SinAcceso", "Home"); }
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View(); 
        }

        [HttpGet]
        public JsonResult ListarRolesPermisos()
        {
            List<Roles> olista = new List<Roles>();
            olista = new CN_Roles().Listar(); 

            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ObtenerPermisosPorRol(int idRol)
        {
            CN_Permisos cnPermisos = new CN_Permisos();
            CN_RolesPermisos cnRolesPermisos = new CN_RolesPermisos();

            List<Permisos> todosPermisos = cnPermisos.Listar();

            List<int> idsPermisosAsignados = cnRolesPermisos.ObtenerPermisosPorRol(idRol);

            List<PermisoAsignacionViewModel> listaPermisosAsignacion = new List<PermisoAsignacionViewModel>();

            foreach (var permiso in todosPermisos)
            {
                listaPermisosAsignacion.Add(new PermisoAsignacionViewModel
                {
                    IdPermiso = permiso.id_permiso, 
                    Nombre = permiso.nombre,         
                    Descripcion = permiso.descripcion, 
                    Asignado = idsPermisosAsignados.Contains(permiso.id_permiso) 
                });
            }

            return Json(new { listaPermisos = listaPermisosAsignacion }, JsonRequestBehavior.AllowGet);
        }

        // Método AJAX para guardar las asignaciones de permisos a un rol
        [HttpPost]
        public JsonResult GuardarPermisosRol(int idRol, List<int> idsPermisos)
        {
            string mensaje = string.Empty;
            bool resultado = false;

            CN_RolesPermisos cnRolesPermisos = new CN_RolesPermisos();

            resultado = cnRolesPermisos.AsignarPermisosARol(idRol, idsPermisos, out mensaje);

            return Json(new { resultado = resultado, mensaje = mensaje }, JsonRequestBehavior.AllowGet);
        }

        #region PrivacyPolicy
        //PrivacyPolicy
        public ActionResult PrivacyPolicy()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }
        #endregion



        #region TermsAndConditions
        // TermsAndConditions
        public ActionResult TermsAndConditions()
        {
            if (Session["Usuario"] == null)
            {
                return RedirectToAction("Index", "Acceso");
            }
            else
            {
                User_activo oUserActivo = (User_activo)Session["Usuario"];
                if (oUserActivo.id_user_activo == 0)
                {
                    return RedirectToAction("Index", "Acceso");
                }
            }
            return View();
        }
        #endregion
    }
}