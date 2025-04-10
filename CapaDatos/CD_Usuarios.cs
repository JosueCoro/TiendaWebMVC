using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using CapaEntidad;
//CREATE TABLE SEGURIDAD.USUARIO 
//(
//id_usuario INTEGER NOT NULL IDENTITY(1,1), 
//nombres VARCHAR(150) NOT NULL,
//apellidos VARCHAR(150) NOT NULL,
//correo VARCHAR(20) NOT NULL,
//contraseña VARCHAR(150) NOT NULL,
//telefono VARCHAR(20) NOT NULL,
//estado BIT DEFAULT 1 NOT NULL,
//ROLES_id_rol INTEGER NOT NULL 
//)
//GO 
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
                            rol = lectura["nombre_rol"].ToString() 
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


    }
}
