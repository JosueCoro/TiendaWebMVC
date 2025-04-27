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
    public class CD_Proveedor
    {
        /*-- CRUD_PROVEEDOR
        CREATE PROCEDURE COMERCIAL.CRUD_PROVEEDOR
            @Operacion NVARCHAR(10),
            @IdProveedor INT = NULL,
            @Nombre VARCHAR(150) = NULL,
            @Telefono VARCHAR(20) = NULL,
            @Correo VARCHAR(50) = NULL,
            @NIT VARCHAR(50) = NULL,
            @Estado BIT = NULL,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        AS
        BEGIN
            SET @Resultado = 0;

            -- INSERTAR NUEVO PROVEEDOR
            IF @Operacion = 'INSERT'
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM COMERCIAL.PROVEEDOR WHERE nombre = @Nombre)
                BEGIN
                    INSERT INTO COMERCIAL.PROVEEDOR (nombre, telefono, correo, nit, estado)
                    VALUES (@Nombre, @Telefono, @Correo, @NIT, ISNULL(@Estado, 1));

                    SET @Resultado = 1;
                    SET @Mensaje = 'Proveedor registrado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe un proveedor con ese nombre.';
            END

            -- CONSULTAR PROVEEDORES
            ELSE IF @Operacion = 'SELECT'
            BEGIN
                SET @Resultado = 1;
                SELECT * FROM COMERCIAL.PROVEEDOR;
            END

            -- ACTUALIZAR PROVEEDOR
            ELSE IF @Operacion = 'UPDATE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 
                    FROM COMERCIAL.PROVEEDOR 
                    WHERE nombre = @Nombre AND id_proveedor != @IdProveedor
                )
                BEGIN
                    UPDATE COMERCIAL.PROVEEDOR
                    SET nombre = @Nombre,
                        telefono = @Telefono,
                        correo = @Correo,
                        nit = @NIT,
                        estado = @Estado
                    WHERE id_proveedor = @IdProveedor;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Proveedor actualizado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe otro proveedor con ese nombre.';
            END

            -- ELIMINAR PROVEEDOR
            ELSE IF @Operacion = 'DELETE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 
                    FROM COMERCIAL.COMPRA 
                    WHERE proveedor_id_proveedor = @IdProveedor
                )
                BEGIN
                    DELETE FROM COMERCIAL.PROVEEDOR
                    WHERE id_proveedor = @IdProveedor;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Proveedor eliminado correctamente.';
                END
                ELSE
                    SET @Mensaje = 'El proveedor está relacionado con una o más compras. No se puede eliminar.';
            END

            -- OPERACIÓN INVÁLIDA
            ELSE
            BEGIN
                SET @Mensaje = 'Operación no válida.';
            END
        END
        GO
        */
        public List<Proveedor> ListarProveedores()
        {
            List<Proveedor> lista = new List<Proveedor>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("COMERCIAL.CRUD_PROVEEDOR", conexion);
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
                        lista.Add(new Proveedor()
                        {
                            id_proveedor = Convert.ToInt32(lector["id_proveedor"]),
                            nombre = lector["nombre"].ToString(),
                            telefono = lector["telefono"].ToString(),
                            correo = lector["correo"].ToString(),
                            nit = lector["nit"].ToString(),
                            estado = Convert.ToBoolean(lector["estado"])
                        });
                    }
                }
            }
            catch
            {
                lista = new List<Proveedor>();
            }

            return lista;
        }

        public int Registrar(Proveedor proveedor, out string mensaje)
        {
            int resultadoInsert = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("COMERCIAL.CRUD_PROVEEDOR", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.nombre);
                    cmd.Parameters.AddWithValue("@Telefono", proveedor.telefono);
                    cmd.Parameters.AddWithValue("@Correo", proveedor.correo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NIT", proveedor.nit ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", proveedor.estado);

                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    resultadoInsert = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value) ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                resultadoInsert = 0;
                mensaje = ex.Message;
            }

            return resultadoInsert;
        }

        public bool Editar(Proveedor proveedor, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("COMERCIAL.CRUD_PROVEEDOR", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdProveedor", proveedor.id_proveedor);
                    cmd.Parameters.AddWithValue("@Nombre", proveedor.nombre);
                    cmd.Parameters.AddWithValue("@Telefono", proveedor.telefono);
                    cmd.Parameters.AddWithValue("@Correo", proveedor.correo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@NIT", proveedor.nit ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Estado", proveedor.estado);

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

        public bool Eliminar(int id_proveedor, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("COMERCIAL.CRUD_PROVEEDOR", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdProveedor", id_proveedor);

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
