using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using CapaEntidad;
using CapaNegocio;
using OfficeOpenXml; // Necesario para EPPlus
using OfficeOpenXml.Style; // Para estilos de celda
using System.Drawing; // Para colores en las celdas
using System.IO; // Para MemoryStream
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



        //HistorialVentas
        [HttpGet]
        public JsonResult ObtenerHistorialVentas(string fechaInicio, string fechaFin, string idVenta)
        {
            List<ReporteVentaDetalle> lista = new List<ReporteVentaDetalle>();
            CN_Reporte objCnReporte = new CN_Reporte();

            lista = objCnReporte.ObtenerHistorialVentas(fechaInicio, fechaFin, idVenta);

            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }

        //HistorialCompras
        [HttpGet]
        public JsonResult ObtenerHistorialCompras(string fechaInicio, string fechaFin, string idCompra)
        {
            List<ReporteCompraDetalle> lista = new List<ReporteCompraDetalle>();
            CN_Reporte objCnReporte = new CN_Reporte();
            lista = objCnReporte.ObtenerHistorialCompras(fechaInicio, fechaFin, idCompra);
            return Json(new { data = lista }, JsonRequestBehavior.AllowGet);
        }
        private CN_Reporte objCnReporte = new CN_Reporte();
        public ActionResult ExportarHistorialComprasExcel(string fechaInicio, string fechaFin, string idCompra)
        {
            List<ReporteCompraDetalle> listaCompras = objCnReporte.ObtenerHistorialCompras(fechaInicio, fechaFin, idCompra);

            ExcelPackage.License.SetNonCommercialPersonal("Josue Coro");


            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Historial de Compras");

                string[] headers = new string[] {
                "Fecha Compra", "Usuario", "Proveedor", "Tipo Pago", "Producto",
                "Cantidad", "Precio Unitario", "Subtotal Ítem", "Monto Total Compra", "Nº de Compra"
            };

                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = headers[i];
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#E0E6F0"));
                    worksheet.Cells[1, i + 1].Style.Font.Color.SetColor(ColorTranslator.FromHtml("#34495E"));
                }

                int row = 2;
                foreach (var compra in listaCompras)
                {
                    worksheet.Cells[row, 1].Value = compra.fechaCompra.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 2].Value = compra.nombreUsuario;
                    worksheet.Cells[row, 3].Value = compra.nombreProveedor;
                    worksheet.Cells[row, 4].Value = compra.tipoPago;
                    worksheet.Cells[row, 5].Value = compra.nombreProducto;
                    worksheet.Cells[row, 6].Value = compra.cantidad;
                    worksheet.Cells[row, 7].Value = compra.precioCompraUnitario;
                    worksheet.Cells[row, 8].Value = compra.subTotalItem;
                    worksheet.Cells[row, 9].Value = compra.montoTotalCompra;
                    worksheet.Cells[row, 10].Value = compra.idCompra;

                    row++;
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                MemoryStream stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Historial_Compras_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }



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
    }
}