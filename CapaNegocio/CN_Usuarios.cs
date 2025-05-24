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
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El telefono del usuario no puede ser vacio";
            }
            else if (obj.id_rol == 0)
            {
                Mensaje = "El rol del usuario no puede ser vacio";
            }
            else if (obj.id_tienda == 0)
            {
                Mensaje = "La tienda del usuario no puede ser vacio";
            }

            if (string.IsNullOrEmpty(Mensaje))
            {


                string clave = CN_Recursos.GenerarClave();

                string asunto = "Creacion de Cuenta";
                string mensaje = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave!</p>";
                mensaje = mensaje.Replace("!clave!", clave);

                bool respuesta = CN_Recursos.EnviarCorreo(obj.correo, asunto, mensaje);

                if (respuesta)
                {
                    obj.contraseña = CN_Recursos.ConvertirSha256(clave);
                    return objUsuario.Registrar(obj, out Mensaje);
                }else {
                    Mensaje = "Error al enviar el correo, por favor verifique su correo electronico";
                    return 0;
                }            
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
            else if (string.IsNullOrEmpty(obj.telefono) || string.IsNullOrWhiteSpace(obj.telefono))
            {
                Mensaje = "El telefono del usuario no puede ser vacio";
            }
            else if (obj.id_rol == 0)
            {
                Mensaje = "El rol del usuario no puede ser vacio";
            }
            else if (obj.id_tienda == 0)
            {
                Mensaje = "La tienda del usuario no puede ser vacio";
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

        //validar usuario
        public User_activo ValidarUsuario(string correo, string contraseña)
        {
            //contraseña = CN_Recursos.ConvertirSha256(contraseña);

            return objUsuario.ValidarUsuario(correo, contraseña);
        }


        // Cambiar contraseña por correo
        public bool CambiarContraseña(string correo, string contraseñaActual, string nuevaContraseña, out string Mensaje)
        {
            Mensaje = string.Empty;

            //validaciones
            if (string.IsNullOrWhiteSpace(correo))
            {
                Mensaje = "El correo del usuario no puede estar vacío.";
            }
            else if (string.IsNullOrWhiteSpace(contraseñaActual))
            {
                Mensaje = "La contraseña actual no puede estar vacía.";
            }
            else if (string.IsNullOrWhiteSpace(nuevaContraseña))
            {
                Mensaje = "La nueva contraseña no puede estar vacía.";
            }

            if (!string.IsNullOrEmpty(Mensaje))
                return false;

            //encriptar contraseñas
            string contraseñaActualHash = CN_Recursos.ConvertirSha256(contraseñaActual);
            string nuevaContraseñaHash = CN_Recursos.ConvertirSha256(nuevaContraseña);

            //capa de datos
            bool resultado = objUsuario.CambiarContraseña(correo, contraseñaActualHash, nuevaContraseñaHash, out Mensaje);

            //enviar correo
            if (resultado)
            {
                string asunto = "Cambio de contraseña"; 
                string mensaje = "<h3>Su contraseña fue cambiada correctamente</h3><br/><p>Su nueva contraseña es: <strong>" + nuevaContraseña + "</strong></p>";

                bool envioCorreo = CN_Recursos.EnviarCorreo(correo, asunto, mensaje);

                if (!envioCorreo)
                {
                    Mensaje = "La contraseña fue actualizada, pero hubo un error al enviar el correo.";
                }
            }

            return resultado;
        }
        //Actualizar contraseña
        public bool ActualizarContraseñaUsuario(int idUsuario, string correo, string nuevaContraseña, out string Mensaje)
        {
            Mensaje = string.Empty;

            // Validaciones
            if (idUsuario <= 0)
            {
                Mensaje = "El ID de usuario no es válido.";
            }
            else if (string.IsNullOrWhiteSpace(correo))
            {
                Mensaje = "El correo del usuario no puede estar vacío.";
            }
            else if (string.IsNullOrWhiteSpace(nuevaContraseña))
            {
                Mensaje = "La nueva contraseña no puede estar vacía.";
            }

            if (!string.IsNullOrEmpty(Mensaje))
                return false;

            //encriptar la nueva contraseña
            string nuevaContraseñaHash = CN_Recursos.ConvertirSha256(nuevaContraseña);

            //capa de datos
            bool resultado = objUsuario.ActualizarContraseñaUsuario(idUsuario, correo, nuevaContraseñaHash, out Mensaje);

            //enviar correo
            if (resultado)
            {
                string asunto = "Actualización de contraseña";
                string mensaje = "<h3>Su contraseña fue actualizada por un administrador</h3><br/><p>Su nueva contraseña es: <strong>" + nuevaContraseña + "</strong></p>";

                bool envioCorreo = CN_Recursos.EnviarCorreo(correo, asunto, mensaje);

                if (!envioCorreo)
                {
                    Mensaje = "La contraseña fue actualizada, pero no se pudo enviar el correo de notificación.";
                }
            }

            return resultado;
        }






    }
}
