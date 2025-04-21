using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Roles
    {
        /*--CRUD_PA_ROLES
        CREATE PROCEDURE SEGURIDAD.CRUD_ROLES
            @Operacion NVARCHAR(10),
            @IdRol INT = NULL,
            @Nombre VARCHAR(150) = NULL,
            @Descripcion VARCHAR(150)= NULL,
            @Estado BIT = NULL,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        AS
        BEGIN
            SET @Resultado = 0;

            -- INSERTAR NUEVO ROL
            IF @Operacion = 'INSERT'
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM SEGURIDAD.ROLES WHERE nombre = @Nombre)
                BEGIN
                    INSERT INTO SEGURIDAD.ROLES (nombre, descripcion, estado)
                    VALUES (@Nombre, @Descripcion, ISNULL(@Estado, 1));

                    SET @Resultado = 1;
                    SET @Mensaje = 'Rol registrado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe un rol con ese nombre.';
            END

            -- CONSULTAR ROLES
            ELSE IF @Operacion = 'SELECT'
            BEGIN
                SELECT * FROM SEGURIDAD.ROLES;
                SET @Resultado = 1;
            END

            -- EDITAR ROL
            ELSE IF @Operacion = 'UPDATE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 
                    FROM SEGURIDAD.ROLES 
                    WHERE nombre = @Nombre AND id_rol != @IdRol
                )
                BEGIN
                    UPDATE SEGURIDAD.ROLES
                    SET nombre = @Nombre,
                        descripcion = @Descripcion,
                        estado = @Estado
                    WHERE id_rol = @IdRol;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Rol actualizado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe otro rol con ese nombre.';
            END

            -- ELIMINACIÓN LÓGICA
            ELSE IF @Operacion = 'DELETE'
            BEGIN
                IF EXISTS (SELECT 1 FROM SEGURIDAD.ROLES WHERE id_rol = @IdRol)
                BEGIN
                    UPDATE SEGURIDAD.ROLES
                    SET estado = 0
                    WHERE id_rol = @IdRol;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Rol eliminado lógicamente (estado = 0).';
                END
                ELSE
                    SET @Mensaje = 'El rol no existe.';
            END

            -- OPERACIÓN INVÁLIDA
            ELSE
            BEGIN
                SET @Mensaje = 'Operación no válida.';
            END
        END
        GO*/
        public List<Roles> ListarRoles()
        {
            List<Roles> lista = new List<Roles>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CRUD_ROLES", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "SELECT");

                    SqlParameter paramMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter paramResultado = new SqlParameter("@Resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(paramMensaje);
                    cmd.Parameters.Add(paramResultado);

                    conexion.Open();


                    SqlDataReader lectura = cmd.ExecuteReader();
                    while (lectura.Read())
                    {
                        lista.Add(new Roles()
                        {
                            id_rol = Convert.ToInt32(lectura["id_rol"]),
                            nombre = lectura["nombre"].ToString(),
                            descripcion = lectura["descripcion"].ToString(),
                            estado = Convert.ToBoolean(lectura["estado"])
                        });
                    }
                }
                

            }
            catch 
            {
                lista = new List<Roles>();
            }
            return lista;
        }

        public int Registrar(Roles rol, out string mensaje)
        {
            int id_autogenerado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CRUD_ROLES", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");

                    cmd.Parameters.AddWithValue("@Nombre", rol.nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", rol.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", rol.estado);
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();

                    cmd.ExecuteNonQuery();
                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    id_autogenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                id_autogenerado = 0;
                mensaje = ex.Message;
            }
            return id_autogenerado;
        }


        public bool Editar(Roles rol, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CRUD_ROLES", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdRol", rol.id_rol);
                    cmd.Parameters.AddWithValue("@Nombre", rol.nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", rol.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", rol.estado);
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }



        public bool Eliminar(int id_rol, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CRUD_ROLES", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@Nombre", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Descripcion", DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", DBNull.Value);
                    cmd.Parameters.AddWithValue("@IdRol", id_rol);

                    SqlParameter pMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter pResultado = new SqlParameter("@Resultado", SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };

                    cmd.Parameters.Add(pMensaje);
                    cmd.Parameters.Add(pResultado);

                    conexion.Open();

                    cmd.ExecuteNonQuery();

                    //mostrar mensaje
                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
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
