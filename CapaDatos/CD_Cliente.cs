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
    public class CD_Cliente
    {
        /*CREATE PROCEDURE COMERCIAL.CRUD_CLIENTE
            @Operacion NVARCHAR(10),
            @IdCliente INT = NULL,
            @Nombres VARCHAR(150) = NULL,
            @Apellidos VARCHAR(150) = NULL,
            @Telefono VARCHAR(20) = NULL,
            @Correo VARCHAR(50) = NULL,
            @Estado BIT = NULL,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        AS
        BEGIN
            SET @Resultado = 0;

            -- INSERTAR CLIENTE
            IF @Operacion = 'INSERT'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM COMERCIAL.CLIENTE 
                    WHERE nombres = @Nombres AND apellidos = @Apellidos AND correo = @Correo
                )
                BEGIN
                    INSERT INTO COMERCIAL.CLIENTE (nombres, apellidos, telefono, correo, estado)
                    VALUES (@Nombres, @Apellidos, @Telefono, @Correo, ISNULL(@Estado, 1));
                    SET @Resultado = 1;
                    SET @Mensaje = 'Cliente registrado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe un cliente con esos datos.';
            END

            -- CONSULTAR CLIENTES
            ELSE IF @Operacion = 'SELECT'
            BEGIN
                SELECT * FROM COMERCIAL.CLIENTE;
                SET @Resultado = 1;
            END

            -- ACTUALIZAR CLIENTE
            ELSE IF @Operacion = 'UPDATE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM COMERCIAL.CLIENTE 
                    WHERE nombres = @Nombres AND apellidos = @Apellidos AND correo = @Correo AND id_cliente != @IdCliente
                )
                BEGIN
                    UPDATE COMERCIAL.CLIENTE
                    SET nombres = @Nombres,
                        apellidos = @Apellidos,
                        telefono = @Telefono,
                        correo = @Correo,
                        estado = @Estado
                    WHERE id_cliente = @IdCliente;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Cliente actualizado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe otro cliente con esos datos.';
            END

            -- ELIMINAR CLIENTE (solo si no está relacionado con una venta)
            ELSE IF @Operacion = 'DELETE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM TRANSACCIONES.VENTA WHERE CLIENTE_id_cliente = @IdCliente
                )
                BEGIN
                    DELETE FROM COMERCIAL.CLIENTE WHERE id_cliente = @IdCliente;
                    SET @Resultado = 1;
                    SET @Mensaje = 'Cliente eliminado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'El cliente está relacionado con una o más ventas. No se puede eliminar.';
            END

            -- OPERACIÓN NO VÁLIDA
            ELSE
            BEGIN
                SET @Mensaje = 'Operación no válida.';
            END
        END
        GO
        */
        public List<Cliente> ListarClientes()
        {
            List<Cliente> lista = new List<Cliente>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("COMERCIAL.CRUD_CLIENTE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "SELECT");

                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    SqlDataReader lector = cmd.ExecuteReader();

                    while (lector.Read())
                    {
                        lista.Add(new Cliente()
                        {
                            id_cliente = Convert.ToInt32(lector["id_cliente"]),
                            nombres = lector["nombres"].ToString(),
                            apellidos = lector["apellidos"]?.ToString(),
                            telefono = lector["telefono"]?.ToString(),
                            correo = lector["correo"]?.ToString(),
                            estado = Convert.ToBoolean(lector["estado"])
                        });
                    }
                }
            }
            catch
            {
                lista = new List<Cliente>();
            }

            return lista;
        }

        public int Registrar(Cliente cliente, out string mensaje)
        {
            int resultado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("COMERCIAL.CRUD_CLIENTE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Nombres", cliente.nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", cliente.apellidos ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Telefono", cliente.telefono ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Correo", cliente.correo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", cliente.estado);

                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value) ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(Cliente cliente, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("COMERCIAL.CRUD_CLIENTE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdCliente", cliente.id_cliente);
                    cmd.Parameters.AddWithValue("@Nombres", cliente.nombres);
                    cmd.Parameters.AddWithValue("@Apellidos", cliente.apellidos ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Telefono", cliente.telefono ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Correo", cliente.correo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", cliente.estado);

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

        public bool Eliminar(int id_cliente, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("COMERCIAL.CRUD_CLIENTE", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdCliente", id_cliente);
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
    }
}
