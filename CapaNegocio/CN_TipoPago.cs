using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_TipoPago
    {
        private CD_TipoPago objTipoPago = new CD_TipoPago();

        public List<TipoPago> Listar()
        {
            return objTipoPago.Listar();
        }

        public int Registrar(TipoPago obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción del tipo de pago no puede ser vacía.";
                return 0;
            }

            return objTipoPago.Registrar(obj, out Mensaje);
        }

        public bool Editar(TipoPago obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrWhiteSpace(obj.descripcion))
            {
                Mensaje = "La descripción del tipo de pago no puede ser vacía.";
                return false;
            }

            return objTipoPago.Editar(obj, out Mensaje);
        }

        public bool Eliminar(int id_tipo_pago, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (objTipoPago.Eliminar(id_tipo_pago, out Mensaje) == false)
            {
                Mensaje = "El tipo de pago se encuentra relacionado con una venta o compra. No se puede eliminar.";
                return false;
            }

            return true;
        }
    }
}
