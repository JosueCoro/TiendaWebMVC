using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using CapaEntidad;
/*CREATE TABLE SEGURIDAD.USUARIO 
    (
     id_usuario INTEGER NOT NULL IDENTITY(1,1), 
     nombres VARCHAR (150) NOT NULL , 
     apellidos VARCHAR (150) NOT NULL , 
     correo VARCHAR (50) NOT NULL , 
     contraseña VARCHAR (150) NOT NULL , 
     telefono VARCHAR (20) NOT NULL , 
     estado BIT DEFAULT 1 NOT NULL , 
     ROLES_id_rol INTEGER NOT NULL 
    )
GO*/
namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
           
            try
            {
                using (SqlConnection oconexion= new SqlConnection(Conexion.cn)) 
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.LISTAR_USUARIOS", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure; 

                    oconexion.Open();
                    SqlDataReader lectura = cmd.ExecuteReader();

                    while (lectura.Read())
                    {
                        lista.Add(new Usuario()
                        {
                            id_usuario = Convert.ToInt32(lectura["id_usuario"]),
                            nombres = lectura["nombres"].ToString(),
                            apellidos = lectura["apellidos"].ToString(),
                            correo = lectura["correo"].ToString(),
                            contraseña = lectura["contraseña"].ToString(),
                            telefono = lectura["telefono"].ToString(),
                            estado = Convert.ToBoolean(lectura["estado"]),
                            id_rol = Convert.ToInt32(lectura["ROLES_id_rol"]),
                            rol = lectura["nombre_rol"] != DBNull.Value ? lectura["nombre_rol"].ToString() : "Sin Rol"
                            //rol = lectura["nombre_rol"].ToString() 
                        });
                    }
                }               
            }
            catch 
            {
                lista = new List<Usuario>();
            }

            return lista;
        }


        public int Registrar(Usuario obj, out string Mensaje)
        {
            int id_autogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("@Nombres", obj.nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.apellidos);
                    cmd.Parameters.AddWithValue("@Correo", obj.correo);
                    cmd.Parameters.AddWithValue("@Contraseña", obj.contraseña);
                    cmd.Parameters.AddWithValue("@Telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("@Estado", obj.estado);
                    cmd.Parameters.AddWithValue("@RolId", obj.id_rol);
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();
                    id_autogenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                id_autogenerado = 0;
                Mensaje = ex.Message;
            }
            return id_autogenerado;
        }

        /*CREATE PROCEDURE SEGURIDAD.EditarUsuario
        (
            @IdUsuario INT,
            @Nombres VARCHAR(150),
            @Apellidos VARCHAR(150),
            @Correo VARCHAR(50),
            @Contraseña VARCHAR(150),
            @Telefono VARCHAR(20),
            @Estado BIT,
            @RolId INT,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        )
        AS
        BEGIN
            SET @Resultado = 0;

            -- Validar si el correo ya está en uso por otro usuario
            IF NOT EXISTS (
                SELECT 1 
                FROM SEGURIDAD.USUARIO 
                WHERE correo = @Correo AND id_usuario != @IdUsuario
            )
            BEGIN
                UPDATE SEGURIDAD.USUARIO
                SET nombres = @Nombres,
                    apellidos = @Apellidos,
                    correo = @Correo,
                    contraseña = @Contraseña,
                    telefono = @Telefono,
                    estado = @Estado,
                    ROLES_id_rol = @RolId
                WHERE id_usuario = @IdUsuario;

                SET @Resultado = 1;
            END
            ELSE
            BEGIN
                SET @Mensaje = 'El correo del usuario ya existe';
            END
        END
        GO*/
        public bool Editar(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.EditarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", obj.id_usuario);
                    cmd.Parameters.AddWithValue("@Nombres", obj.nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", obj.apellidos);
                    cmd.Parameters.AddWithValue("@Correo", obj.correo);
                    //cmd.Parameters.AddWithValue("@Contraseña", obj.contraseña);
                    cmd.Parameters.AddWithValue("@Telefono", obj.telefono);
                    cmd.Parameters.AddWithValue("@Estado", obj.estado);
                    cmd.Parameters.AddWithValue("@RolId", obj.id_rol);
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        /*CREATE PROCEDURE SEGURIDAD.EliminarUsuario
        (
            @IdUsuario INT,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        )
        AS
        BEGIN
            SET @Resultado = 0;

            IF EXISTS (SELECT * FROM SEGURIDAD.USUARIO WHERE id_usuario = @IdUsuario)
            BEGIN
                UPDATE SEGURIDAD.USUARIO
                SET estado = 0
                WHERE id_usuario = @IdUsuario;

                SET @Resultado = 1;
                SET @Mensaje = 'Usuario eliminado correctamente (estado = 0).';
            END
            ELSE
            BEGIN
                SET @Mensaje = 'El usuario no existe.';
            END
        END
        GO*/

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.EliminarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", id);
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        /*CREATE PROCEDURE SEGURIDAD.PA_ValidarLogin
        (
            @Correo VARCHAR(50),
            @Contraseña VARCHAR(150)
        )
        AS
        BEGIN
            -- Declarar variables para almacenar el ID del usuario y el resultado
            DECLARE @ID_Usuario INT;
            DECLARE @Resultado INT;

            -- Comprobar si el usuario existe con el correo y la contraseña proporcionados
            SELECT @ID_Usuario = id_usuario
            FROM SEGURIDAD.USUARIO
            WHERE correo = @Correo
              AND contraseña = @Contraseña
              AND estado = 1;  -- Solo usuarios activos

            -- Si el usuario fue encontrado y está activo
            IF @ID_Usuario IS NOT NULL
            BEGIN
                -- Se asigna el resultado 1 (usuario existe y está activo)
                SET @Resultado = 1;
            END
            ELSE
            BEGIN
                -- Si no se encuentra el usuario o está inactivo, el resultado es 0
                SET @Resultado = 0;
                SET @ID_Usuario = NULL; -- Se asegura de que no se devuelva un ID si no se encuentra el usuario
            END

            -- Retornar los valores de ID_Usuario y Resultado
            SELECT @ID_Usuario AS ID_Usuario, @Resultado AS Resultado;
        END;
        GO
        */
        public Usuario ValidarUsuario(string correo, string contraseña)
        {
            Usuario oUsuario = null;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.PA_ValidarLogin", oconexion);
                    cmd.Parameters.Add("@Correo", SqlDbType.VarChar, 50).Value = correo;
                    cmd.Parameters.Add("@Contraseña", SqlDbType.VarChar, 150).Value = contraseña;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    SqlDataReader lectura = cmd.ExecuteReader();

                    if (lectura.Read())
                    {
                        int idUsuario = Convert.ToInt32(lectura["ID_Usuario"]);
                        int resultado = Convert.ToInt32(lectura["Resultado"]); 

                        if (resultado == 1)
                        {
                            oUsuario = new Usuario()
                            {
                                id_usuario = idUsuario
                            };
                        }
                        else
                        {
                            oUsuario = null;
                        }
                    }
                    else
                    {
                        oUsuario = null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al validar usuario: {ex.Message}");
                oUsuario = null;
            }

            return oUsuario;
        }

        /*--Procedimiento almacenado para que el usuario cambie su contraseña
         * COMO PARAMETROS LE PASAMOS EL CORREO, LA CONTRASEÑA ACTUAL Y LA NUEVA CONTRASEÑA(CORREO VA FUNCIONAR COMO ID YA QUE EL CORREO ES UNICO EN CADA USUARIO)
         CREATE PROCEDURE SEGURIDAD.CambiarContraseña
        (
            @Correo VARCHAR(50),
            @ContraseñaActual VARCHAR(150),
            @NuevaContraseña VARCHAR(150),
            @ID_Usuario INT OUTPUT,
            @Resultado INT OUTPUT,
            @Mensaje VARCHAR(200) OUTPUT
        )
        AS
        BEGIN
            -- Inicializar valores
            SET @ID_Usuario = NULL;
            SET @Resultado = 0;
            SET @Mensaje = '';

            -- Buscar usuario con correo y contraseña actual
            SELECT @ID_Usuario = id_usuario
            FROM SEGURIDAD.USUARIO
            WHERE correo = @Correo
              AND contraseña = @ContraseñaActual
              AND estado = 1;

            -- Validar existencia y estado del usuario
            IF @ID_Usuario IS NOT NULL
            BEGIN
                -- Actualizar contraseña
                UPDATE SEGURIDAD.USUARIO
                SET contraseña = @NuevaContraseña
                WHERE id_usuario = @ID_Usuario;

                SET @Resultado = 1;
                SET @Mensaje = 'Contraseña actualizada correctamente.';
            END
            ELSE
            BEGIN
                SET @Mensaje = 'Correo o contraseña actual incorrectos, o el usuario está inactivo.';
            END
        END;
        GO
         
         */
        public bool CambiarContraseña(string correo, string contraseñaActual, string nuevaContraseña, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CambiarContraseña", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetros de entrada
                    cmd.Parameters.Add("@Correo", SqlDbType.VarChar, 50).Value = correo;
                    cmd.Parameters.Add("@ContraseñaActual", SqlDbType.VarChar, 150).Value = contraseñaActual;
                    cmd.Parameters.Add("@NuevaContraseña", SqlDbType.VarChar, 150).Value = nuevaContraseña;

                    // Parámetros de salida
                    cmd.Parameters.Add("@ID_Usuario", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    // Captura de resultados
                    resultado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value) == 1;
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }

            return resultado;
        }
        /*Procedimiento almacenado para un administrador pueda actualizar contraseña de un usuario(osea que va ir con id, correo,contraseña y nueva como parametros)
         CREATE PROCEDURE SEGURIDAD.EditarUsuario
        (
            @IdUsuario INT,
            @Nombres VARCHAR(150),
            @Apellidos VARCHAR(150),
            @Correo VARCHAR(50),
            @Telefono VARCHAR(20),
            @Estado BIT,
            @RolId INT,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        )
        AS
        BEGIN
            SET @Resultado = 0;

            -- Validar si el correo ya está en uso por otro usuario
            IF NOT EXISTS (
                SELECT * 
                FROM SEGURIDAD.USUARIO 
                WHERE correo = @Correo AND id_usuario != @IdUsuario
            )
            BEGIN
                UPDATE SEGURIDAD.USUARIO
                SET nombres = @Nombres,
                    apellidos = @Apellidos,
                    correo = @Correo,
                    telefono = @Telefono,
                    estado = @Estado,
                    ROLES_id_rol = @RolId
                WHERE id_usuario = @IdUsuario;

                SET @Resultado = 1;
            END
            ELSE
            BEGIN
                SET @Mensaje = 'El correo del usuario ya existe';
            END
        END
        GO

         */
        public bool ActualizarContraseñaUsuario(int idUsuario, string correo, string nuevaContraseña, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.ActualizarContraseñaUsuario", oConexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Correo", correo);
                    cmd.Parameters.AddWithValue("@NuevaContraseña", nuevaContraseña);

                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 200).Direction = ParameterDirection.Output;

                    oConexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }

            return resultado;
        }


    }
}
