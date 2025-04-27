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
    public class CD_Servicios
    {
        /*CREATE PROCEDURE TRANSACCIONES.CRUD_SERVICIO
            @Operacion NVARCHAR(10),
            @IdServicio INT = NULL,
            @Nombre VARCHAR(150) = NULL,
            @Descripcion VARCHAR(150) = NULL,
            @Precio DECIMAL(30,3) = NULL,
            @Estado BIT = NULL,
            @UsuarioId INT = NULL,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        AS
        BEGIN
            SET @Resultado = 0;

            -- INSERTAR SERVICIO
            IF @Operacion = 'INSERT'
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM TRANSACCIONES.SERVICIO WHERE nombre = @Nombre)
                BEGIN
                    INSERT INTO TRANSACCIONES.SERVICIO (nombre, descripcion, precio, estado, USUARIO_id_usuario)
                    VALUES (@Nombre, @Descripcion, @Precio, ISNULL(@Estado, 1), @UsuarioId);

                    SET @Resultado = 1;
                    SET @Mensaje = 'Servicio registrado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe un servicio con ese nombre.';
            END

            -- CONSULTAR SERVICIOS
            ELSE IF @Operacion = 'SELECT'
            BEGIN
                SELECT * FROM TRANSACCIONES.SERVICIO;
                SET @Resultado = 1;
            END

            -- ACTUALIZAR SERVICIO
            ELSE IF @Operacion = 'UPDATE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 
                    FROM TRANSACCIONES.SERVICIO 
                    WHERE nombre = @Nombre AND id_servicio != @IdServicio
                )
                BEGIN
                    UPDATE TRANSACCIONES.SERVICIO
                    SET nombre = @Nombre,
                        descripcion = @Descripcion,
                        precio = @Precio,
                        estado = @Estado,
                        USUARIO_id_usuario = @UsuarioId
                    WHERE id_servicio = @IdServicio;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Servicio actualizado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe otro servicio con ese nombre.';
            END

            -- ELIMINAR SERVICIO (solo si no está relacionado)
            ELSE IF @Operacion = 'DELETE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 
                    FROM TRANSACCIONES.DETALLE_SERVICIO 
                    WHERE SERVICIO_id_servicio = @IdServicio
                )
                BEGIN
                    DELETE FROM TRANSACCIONES.SERVICIO
                    WHERE id_servicio = @IdServicio;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Servicio eliminado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'El servicio está relacionado con una venta. No se puede eliminar.';
            END

            -- OPERACIÓN INVÁLIDA
            ELSE
            BEGIN
                SET @Mensaje = 'Operación no válida.';
            END
        END
        GO*/
        public List<Servicio> ListarServicios()
        {
            List<Servicio> lista = new List<Servicio>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.CRUD_SERVICIO", conexion);
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
                    SqlDataReader lector = cmd.ExecuteReader();

                    while (lector.Read())
                    {
                        lista.Add(new Servicio()
                        {
                            id_servicio = Convert.ToInt32(lector["id_servicio"]),
                            nombre = lector["nombre"].ToString(),
                            descripcion = lector["descripcion"].ToString(),
                            precio = Convert.ToDecimal(lector["precio"]),
                            estado = Convert.ToBoolean(lector["estado"]),
                            //USUARIO_id_usuario = Convert.ToInt32(lector["USUARIO_id_usuario"])
                        });
                    }
                }
            }
            catch
            {
                lista = new List<Servicio>();
            }

            return lista;
        }

        public int Registrar(Servicio obj, out string Mensaje)
        {
            int resultadoInsert = 0;
            Mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.CRUD_SERVICIO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("@Precio", obj.precio);
                    cmd.Parameters.AddWithValue("@Estado", obj.estado);
                    cmd.Parameters.AddWithValue("@UsuarioId", obj.USUARIO_id_usuario);


                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    resultadoInsert = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value) ? 1 : 0;

                }
            }
            catch (Exception ex)
            {
                resultadoInsert = 0;
                Mensaje = ex.Message;
            }
            return resultadoInsert;
        }

        public bool Editar(Servicio obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.CRUD_SERVICIO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdServicio", obj.id_servicio);
                    cmd.Parameters.AddWithValue("@Nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("@Precio", obj.precio);
                    cmd.Parameters.AddWithValue("@Estado", obj.estado);
                    cmd.Parameters.AddWithValue("@UsuarioId", obj.USUARIO_id_usuario);

                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;


                    conexion.Open();
                    cmd.ExecuteNonQuery();


                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);

                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.CRUD_SERVICIO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdServicio", id);
                    
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;


                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
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
