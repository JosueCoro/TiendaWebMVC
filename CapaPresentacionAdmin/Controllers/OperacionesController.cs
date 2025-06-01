using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuestPDF.Fluent; //libreria QuestPDF
using QuestPDF.Helpers; //colores para el pdf
using QuestPDF.Infrastructure; 
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace CapaPresentacionAdmin.Controllers
{
    //[Authorize]
    public class OperacionesController : Controller
    {
        // GET: Operaciones

        #region Ventas
        //Ventas
        public ActionResult Ventas()
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
        public JsonResult RegistrarVenta(string objetoVenta)
        {
            string Mensaje = string.Empty;
            int idVentaGenerada = 0;

            User_activo usuarioActivo = Session["Usuario"] as User_activo;

            if (usuarioActivo == null || usuarioActivo.id_user_activo == 0)
            {
                Mensaje = "No se ha encontrado el ID del usuario logeado. Por favor, inicie sesión nuevamente.";
                return Json(new { resultado = 0, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }

            int idUsuarioLogeado = usuarioActivo.id_user_activo;
            int idTienda = usuarioActivo.id_tienda_user;

            try
            {
                Venta oVenta = JsonConvert.DeserializeObject<Venta>(objetoVenta);


                idVentaGenerada = new CN_Venta().Registrar(oVenta,  idUsuarioLogeado,  idTienda,out Mensaje);

                bool operacionExitosa = (idVentaGenerada != 0);

                return Json(new { operacionExitosa = operacionExitosa, idVenta = idVentaGenerada, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Mensaje = "Error inesperado al registrar la venta: " + ex.Message;
                return Json(new { operacionExitosa = false, idVenta = 0, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }


        // Método existente para el historial de ventas
        [HttpGet]
        public PartialViewResult ObtenerDetalleVenta(int idVenta)
        {
            string mensaje = string.Empty;
            Venta oVenta = new CN_Venta().ObtenerVenta(idVenta, out mensaje);

            if (oVenta == null)
            {
                ViewBag.ErrorMessage = mensaje; 
                return PartialView("_DetalleVentaPartial", null); 
            }

            return PartialView("_DetalleVentaPartial", oVenta);

        }


        [HttpGet]
        public ActionResult GenerarFacturaFormal(int idVenta)
        {
            string mensaje = string.Empty;
            Venta oVenta = new CN_Venta().ObtenerVenta(idVenta, out mensaje);

            if (oVenta == null)
            {
                return Content($"<script>alert('Error al obtener los datos de la venta: {mensaje}'); window.close();</script>", "text/html");
            }
            byte[] logoBytes = null;
            string logoPath = Server.MapPath("~/Content/Imagenes/Logo_Factura.png"); 

            if (System.IO.File.Exists(logoPath))
            {
                logoBytes = System.IO.File.ReadAllBytes(logoPath); 
            }

            //generar el  PDF
            byte[] pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.Letter);
                    page.Margin(40); 
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header()
                        .PaddingBottom(10)
                        .Column(column =>
                        {
                            if (logoBytes != null)
                            {
                                column.Item()
                                    .AlignCenter()
                                    .Width(200)
                                    .Height(100)
                                    .Image(logoBytes) 
                                    .FitArea();
                            }
                            else
                            {
                                column.Item().AlignCenter().Text("LOGO NO DISPONIBLE").FontSize(8).FontColor(Colors.Red.Medium);
                            }
                            column.Item().AlignCenter().Text("AUTOPARTES DOS HERMANOS").FontSize(16).Bold();
                            column.Item().AlignCenter().Text("NIT: 000000000").FontSize(10);
                            column.Item().AlignCenter().Text("Dirección: Av. Principal El Bisito").FontSize(10);
                            column.Item().AlignCenter().Text("Teléfono: 71015570").FontSize(10);
                            column.Item().LineHorizontal(1);
                        });

                    page.Content()
                        .Column(column =>
                        {
                            column.Item().PaddingBottom(10).Row(row =>
                            {
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text($"Factura Nro: {oVenta.id_venta}").Bold();
                                    col.Item().Text($"Fecha: {oVenta.fecha.ToString("dd/MM/yyyy")}");
                                    col.Item().Text($"Hora: {DateTime.Now.ToString("HH:mm")}");
                                    col.Item().Text($"Tipo de Pago: {oVenta.oTipoPago?.descripcion ?? "N/A"}");
                                });
                                row.RelativeItem().Column(col =>
                                {
                                    col.Item().Text("Datos del Cliente:").Bold();
                                    col.Item().Text($"Nombre: {oVenta.oCliente?.nombres ?? "N/A"} {oVenta.oCliente?.apellidos ?? ""}");
                                    col.Item().Text($"Teléfono: {oVenta.oCliente?.telefono ?? "N/A"}");
                                    col.Item().Text($"Correo: {oVenta.oCliente?.correo ?? "N/A"}");
                                });
                            });

                            column.Item().PaddingVertical(10).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(1); 
                                    columns.RelativeColumn(3); 
                                    columns.RelativeColumn(1); 
                                    columns.RelativeColumn(1.5f); 
                                    columns.RelativeColumn(1.5f);
                                });

                                table.Header(header =>
                                {
                                    header.Cell().BorderBottom(1).Padding(5).Text("Tipo").Bold();
                                    header.Cell().BorderBottom(1).Padding(5).Text("Ítem").Bold();
                                    header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Cantidad").Bold();
                                    header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("P. Unitario").Bold();
                                    header.Cell().BorderBottom(1).Padding(5).AlignRight().Text("Subtotal").Bold();
                                });

                                foreach (var detalle in oVenta.oDetalleVenta)
                                {
                                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(detalle.tipo_item);
                                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).Text(detalle.tipo_item == "PRODUCTO" ? detalle.oProducto?.nombre ?? "Producto Desconocido" : detalle.oServicio?.nombre ?? "Servicio Desconocido");
                                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text(detalle.cantidad.ToString());
                                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text($"Bs./ {detalle.precio.ToString("N2")}");
                                    table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5).AlignRight().Text($"Bs./ {detalle.sub_total.ToString("N2")}");
                                }
                            });

                            column.Item().AlignRight().Text($"Total Venta: Bs./ {oVenta.monto_total.ToString("N2")}").FontSize(12).Bold();
                        });

                    page.Footer()
                        .PaddingTop(10)
                        .Column(column =>
                        {
                            column.Item().LineHorizontal(1);
                            column.Item().AlignCenter().Text($"Atendido por: {oVenta.oUsuario?.nombres ?? "N/A"} {oVenta.oUsuario?.apellidos ?? ""}").FontSize(8);
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.Span("Página ").FontSize(8);
                                text.CurrentPageNumber().FontSize(8);
                                text.Span(" de ").FontSize(8);
                                text.TotalPages().FontSize(8);
                            });
                        });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Factura_Venta_{oVenta.id_venta}.pdf");
        }

        [HttpGet]
        public ActionResult GenerarReciboTermico(int idVenta)
        {
            string mensaje = string.Empty;
            Venta oVenta = new CN_Venta().ObtenerVenta(idVenta, out mensaje);

            if (oVenta == null)
            {
                return Content($"<script>alert('Error al obtener los datos de la venta: {mensaje}'); window.close();</script>", "text/html");
            }

            const float mmToPoints = 2.83465f;
            const float width80mm = 80 * mmToPoints; 
            const float heightLarge = 1000 * mmToPoints;

            //documento para la imprresora termica
            byte[] pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(new PageSize(width80mm, heightLarge));
                    page.Margin(10); 
                    page.DefaultTextStyle(x => x.FontSize(8)); 

                    page.Header()
                        .PaddingBottom(5)
                        .Column(column =>
                        {
                            column.Item().AlignCenter().Text("AUTOPARTES DOS HERMANOS").FontSize(10).Bold();
                            column.Item().AlignCenter().Text("RECIBO DE VENTA").FontSize(9).Bold();
                            column.Item().AlignCenter().Text($"Nro: {oVenta.id_venta}").FontSize(9);
                            column.Item().AlignCenter().Text($"Fecha: {oVenta.fecha.ToString("dd/MM/yyyy")}").FontSize(8);
                            column.Item().AlignCenter().Text($"Hora: {DateTime.Now.ToString("HH:mm")}");
                            column.Item().LineHorizontal(0.5f);
                        });

                    page.Content()
                        .Column(column =>
                        {
                            column.Item().PaddingBottom(5).Text($"Cliente: {oVenta.oCliente?.nombres ?? "N/A"} {oVenta.oCliente?.apellidos ?? ""}").FontSize(8);
                            column.Item().PaddingBottom(5).Text($"Tipo Pago: {oVenta.oTipoPago?.descripcion ?? "N/A"}").FontSize(8);
                            column.Item().LineHorizontal(0.5f);

                            column.Item().PaddingVertical(5).Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(4); 
                                    columns.RelativeColumn(2); 
                                });

                                table.Header(header =>
                                {
                                    header.Cell().Padding(2).Text("Ítem").Bold();
                                    header.Cell().Padding(2).AlignRight().Text("Subtotal").Bold();
                                });

                                foreach (var detalle in oVenta.oDetalleVenta)
                                {
                                    string itemName = detalle.tipo_item == "PRODUCTO" ? detalle.oProducto?.nombre ?? "Producto Desconocido" : detalle.oServicio?.nombre ?? "Servicio Desconocido";
                                    table.Cell().Padding(2).Text($"{detalle.cantidad} x {itemName} (Bs./ {detalle.precio.ToString("N2")})");
                                    table.Cell().Padding(2).AlignRight().Text($"Bs./ {detalle.sub_total.ToString("N2")}");
                                }
                            });

                            column.Item().LineHorizontal(0.5f);
                            column.Item().AlignRight().Text($"TOTAL: Bs./ {oVenta.monto_total.ToString("N2")}").FontSize(10).Bold();
                            column.Item().LineHorizontal(0.5f);

                            column.Item().PaddingTop(10).AlignCenter().Text("¡Gracias por su compra!").FontSize(8);
                            column.Item().AlignCenter().Text($"Atendido por: {oVenta.oUsuario?.nombres ?? "N/A"}").FontSize(7);
                            column.Item().AlignCenter().Text(text =>
                            {
                                text.Span("Página ").FontSize(7);
                                text.CurrentPageNumber().FontSize(7);
                                text.Span(" de ").FontSize(7);
                                text.TotalPages().FontSize(7);
                            });
                        });
                });
            }).GeneratePdf();

            return File(pdfBytes, "application/pdf", $"Recibo_Venta_{oVenta.id_venta}.pdf");
        }

        private CN_Reporte objCnReporte = new CN_Reporte();

        public ActionResult ExportarHistorialVentasExcel(string fechaInicio, string fechaFin, string idVenta)
        {
            //usar el método de ObtenerVenta del public class CN_Venta en capa de negocio, unicamente pasando el idVenta, 
            List<ReporteVentaDetalle> listaVentas = objCnReporte.ObtenerHistorialVentas(fechaInicio, fechaFin, idVenta);
            ExcelPackage.License.SetNonCommercialPersonal("Josue Coro");

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reporte de Registro de Ventas");
                string[] headers = new string[] {
                "Fecha Venta", "Cliente", "Tipo Item", "Nombre Item", "Precio Unitario",
                "Cantidad", "SubTotal Ítem", "Nº de Venta"
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
                foreach (var venta in listaVentas)
                {
                    worksheet.Cells[row, 1].Value = venta.fechaVenta.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 2].Value = venta.nombreCliente;
                    worksheet.Cells[row, 3].Value = venta.tipoItem;
                    worksheet.Cells[row, 4].Value = venta.nombreItem;
                    worksheet.Cells[row, 5].Value = venta.precioUnitario;
                    worksheet.Cells[row, 6].Value = venta.cantidad;
                    worksheet.Cells[row, 7].Value = venta.subTotal;
                    worksheet.Cells[row, 8].Value = venta.idVenta;
                    row++;
                }
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                MemoryStream stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                string excelName = $"Reporte_Ventas_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }

        }
        #endregion


        #region Compras
        //Compras
        public ActionResult Compras()
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
        public JsonResult RegistrarCompra(string objetoCompra)
        {
            string Mensaje = string.Empty;
            int idCompraGenerada = 0;

           

            User_activo usuarioActivo = Session["Usuario"] as User_activo;

            if (usuarioActivo == null || usuarioActivo.id_user_activo == 0)
            {
                Mensaje = "No se ha encontrado el ID del usuario logeado. Por favor, inicie sesión nuevamente.";
                return Json(new { resultado = 0, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }

            int idUsuarioLogeado = usuarioActivo.id_user_activo;
            int idTienda = usuarioActivo.id_tienda_user;

            try
            {
                Compra oCompra = JsonConvert.DeserializeObject<Compra>(objetoCompra);

                

                idCompraGenerada = new CN_Compra().Registrar(oCompra, idUsuarioLogeado, idTienda, out Mensaje);

                bool operacionExitosa = (idCompraGenerada != 0);

                return Json(new { operacionExitosa = operacionExitosa, idCompra = idCompraGenerada, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Mensaje = "Error inesperado al registrar la compra: " + ex.Message;
                return Json(new { operacionExitosa = false, idCompra = 0, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }
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
        
        public ActionResult ExportarHistorialComprasExcel(string fechaInicio, string fechaFin, string idCompra)
        {
            List<ReporteCompraDetalle> listaCompras = objCnReporte.ObtenerHistorialCompras(fechaInicio, fechaFin, idCompra);

            ExcelPackage.License.SetNonCommercialPersonal("Josue Coro");


            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Reporte de Registro de Compras");

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
        #endregion



        #region Clientes 
        //clientes
        public ActionResult Clientes()
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
        public JsonResult ListarClientes()
        {
            List<Cliente> olista = new List<Cliente>();
            olista = new CN_Clientes().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult GuardarCliente(Cliente objeto)
        {
            object resultado;
            string Mensaje = string.Empty;

            if (objeto.id_cliente == 0)
            {
                resultado = new CN_Clientes().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Clientes().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult EliminarCliente(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta = new CN_Clientes().Eliminar(id, out Mensaje);
            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Proveedores
        //proveedores
        public ActionResult Proveedores()
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
        public JsonResult ListarProveedores()
        {
            List<Proveedor> olista = new List<Proveedor>();
            olista = new CN_Proveedores().Listar();
            return Json(new { data = olista }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarProveedor(Proveedor objeto)
        {
            object resultado;
            string Mensaje = string.Empty;
            if (objeto.id_proveedor == 0)
            {
                resultado = new CN_Proveedores().Registrar(objeto, out Mensaje);
            }
            else
            {
                resultado = new CN_Proveedores().Editar(objeto, out Mensaje);
            }
            return Json(new { resultado = resultado, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarProveedor(int id)
        {
            bool respuesta = false;
            string Mensaje = string.Empty;
            respuesta = new CN_Proveedores().Eliminar(id, out Mensaje);
            return Json(new { resultado = respuesta, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Stock
        //Stock
        public ActionResult Stock()
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
        public JsonResult ListarStock()
        {
            List<Stock> olista = new CD_Stock().ListarStock();

            // Formatear la respuesta para que coincida con lo que espera tu vista/javascript
            var resultado = olista.Select(s => new
            {
                s.id_stock,
                s.cantidad,
                s.stock_minimo,
                //s.fecha_actualizacion,
                fecha_actualizacion = s.fecha_actualizacion.ToString("yyyy-MM-dd"),
                PRODUCTO_id_producto = s.oProducto.id_producto,
                TIENDA_id_tienda = s.oTienda.id_tienda,
                nombreProducto = s.oProducto.nombre,
                nombreTienda = s.oTienda.nombre,

            });

            return Json(new { data = resultado }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarStock(string objeto)
        {
            string Mensaje = string.Empty;
            bool operacion_exitosa = true;


            Stock ostock = JsonConvert.DeserializeObject<Stock>(objeto);


            if (ostock.id_stock == 0)
            {
                int idStockGenerado = new CN_Stocks().Registrar(ostock, out Mensaje);
                if (idStockGenerado == 0)
                {
                    operacion_exitosa = false;
                }
            }
            else
            {
                operacion_exitosa = new CN_Stocks().Editar(ostock, out Mensaje);
            }

            return Json(new { operacionExitosa = operacion_exitosa, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Reportes
        //Reporte de Ventas
        public ActionResult ReporteVentas()
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

        

        //Reporte de Compras
        public ActionResult ReporteCompras()
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
        #endregion

    }
}