using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using CapaDatos;

namespace CapaNegocio
{
    public class CN_Clientes
    {
        private CD_Cliente objCliente = new CD_Cliente();

        public List<Cliente> Listar()
        {
            return objCliente.ListarClientes();
        }

        public int Registrar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombres) || string.IsNullOrWhiteSpace(obj.nombres))
            {
                Mensaje = "El nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.apellidos) || string.IsNullOrWhiteSpace(obj.apellidos))
            {
                Mensaje = "El apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.correo) || string.IsNullOrWhiteSpace(obj.correo))
            {
                Mensaje = "El correo del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El telefono del cliente no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCliente.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }

        }
        public bool Editar(Cliente obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombres) || string.IsNullOrWhiteSpace(obj.nombres))
            {
                Mensaje = "El nombre del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.apellidos) || string.IsNullOrWhiteSpace(obj.apellidos))
            {
                Mensaje = "El apellido del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.correo) || string.IsNullOrWhiteSpace(obj.correo))
            {
                Mensaje = "El correo del cliente no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El telefono del cliente no puede ser vacio";
            }
            if (string.IsNullOrEmpty(Mensaje))
            {
                return objCliente.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id_cliente, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (objCliente.Eliminar(id_cliente, out Mensaje) == false)
            {
                Mensaje = "El cliente se encuentra relacionado con una o más ventas. No se puede eliminar.";
                return false;
            }
            return true;
        }
    }
}
