using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objUsuario = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objUsuario.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            if (string.IsNullOrEmpty(obj.nombres)|| string.IsNullOrWhiteSpace(obj.nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.apellidos) || string.IsNullOrWhiteSpace(obj.apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.correo) || string.IsNullOrWhiteSpace(obj.correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.contraseña) || string.IsNullOrWhiteSpace(obj.contraseña))
            {
                Mensaje = "La contraseña del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El telefono del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                string clave = "13540503";
                obj.contraseña = CN_Recursos.ConvertirSha256(clave);

                return objUsuario.Registrar(obj, out Mensaje);
            }
            else
            {
                return 0;
            }

        }
        public bool Editar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;
            if (string.IsNullOrEmpty(obj.nombres) || string.IsNullOrWhiteSpace(obj.nombres))
            {
                Mensaje = "El nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.apellidos) || string.IsNullOrWhiteSpace(obj.apellidos))
            {
                Mensaje = "El apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.correo) || string.IsNullOrWhiteSpace(obj.correo))
            {
                Mensaje = "El correo del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.contraseña) || string.IsNullOrWhiteSpace(obj.contraseña))
            {
                Mensaje = "La contraseña del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El telefono del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {
                return objUsuario.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objUsuario.Eliminar(id, out Mensaje);
        }


    }
}
