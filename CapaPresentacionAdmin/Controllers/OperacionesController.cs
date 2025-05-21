using CapaDatos;
using CapaEntidad;
using CapaNegocio;
using Newtonsoft.Json;
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

        //Compras
        public ActionResult Compras()
        {
            return View();
        }
        // --- Métodos para el Registro de Compra ---
        [HttpPost]
        public JsonResult RegistrarCompra(string objetoCompra)
        {
            string Mensaje = string.Empty;
            int idCompraGenerada = 0; 

            try
            {
                // Deserializar el objeto JSON de la compra que viene del frontend
                // Asegúrate de que la estructura JSON coincida con la clase Compra y sus DetalleCompra
                Compra oCompra = JsonConvert.DeserializeObject<Compra>(objetoCompra);

                

                // Llamar a la capa de negocio para registrar la compra
                idCompraGenerada = new CN_Compra().Registrar(oCompra, out Mensaje);

                // Determinar si la operación fue exitosa
                bool operacionExitosa = (idCompraGenerada != 0);

                return Json(new { operacionExitosa = operacionExitosa, idCompra = idCompraGenerada, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Capturar cualquier excepción inesperada y devolver un mensaje de error
                Mensaje = "Error inesperado al registrar la compra: " + ex.Message;
                return Json(new { operacionExitosa = false, idCompra = 0, mensaje = Mensaje }, JsonRequestBehavior.AllowGet);
            }
        }



        //clientes
        public ActionResult Clientes()
        {
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




        //proveedores
        public ActionResult Proveedores()
        {
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


        //Stock
        public ActionResult Stock()
        {
            return View();
        }

        [HttpGet]
        public JsonResult ListarStock()
        {
            // Usar la capa de datos (CD_Stock) para obtener la lista de stock
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

    }
}