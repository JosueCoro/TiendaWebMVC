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
    public class CD_Marca
    {
        /*--CRUD_MARCA
        CREATE PROCEDURE INVENTARIO.CRUD_MARCA
            @Operacion NVARCHAR(10),
            @IdMarca INT = NULL,
            @Descripcion VARCHAR(100) = NULL,
            @Estado BIT = NULL,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        AS
        BEGIN
            SET @Resultado = 0;

            -- INSERTAR NUEVA MARCA
            IF @Operacion = 'INSERT'
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM INVENTARIO.MARCA WHERE descripcion = @Descripcion)
                BEGIN
                    INSERT INTO INVENTARIO.MARCA (descripcion, estado)
                    VALUES (@Descripcion, ISNULL(@Estado, 1));

                    SET @Resultado = 1;
                    SET @Mensaje = 'Marca registrada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe una marca con esa descripción.';
            END

            -- CONSULTAR MARCAS
            ELSE IF @Operacion = 'SELECT'
            BEGIN
                SET @Resultado = 1;
                SELECT * FROM INVENTARIO.MARCA;
            END

            -- ACTUALIZAR MARCA
            ELSE IF @Operacion = 'UPDATE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 
                    FROM INVENTARIO.MARCA 
                    WHERE descripcion = @Descripcion AND id_marca != @IdMarca
                )
                BEGIN
                    UPDATE INVENTARIO.MARCA
                    SET descripcion = @Descripcion,
                        estado = @Estado
                    WHERE id_marca = @IdMarca;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Marca actualizada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe otra marca con esa descripción.';
            END

            -- ELIMINAR MARCA
            ELSE IF @Operacion = 'DELETE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 
                    FROM INVENTARIO.PRODUCTO 
                    WHERE MARCA_id_marca = @IdMarca
                )
                BEGIN
                    DELETE FROM INVENTARIO.MARCA
                    WHERE id_marca = @IdMarca;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Marca eliminada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'La marca está relacionada con uno o más productos. No se puede eliminar.';
            END

            -- OPERACIÓN INVÁLIDA
            ELSE
            BEGIN
                SET @Mensaje = 'Operación no válida.';
            END
        END
        GO*/
        public List<Marca> ListarMarcas()
        {
            List<Marca> lista = new List<Marca>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_MARCA", conexion);
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
                        lista.Add(new Marca()
                        {
                            id_marca = Convert.ToInt32(lector["id_marca"]),
                            descripcion = lector["descripcion"].ToString(),
                            estado = Convert.ToBoolean(lector["estado"])
                        });
                    }
                }
            }
            catch
            {
                lista = new List<Marca>();
            }

            return lista;
        }

        public int Registrar(Marca marca, out string mensaje)
        {
            int resultadoInsert = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_MARCA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Descripcion", marca.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", marca.estado);

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

        public bool Editar(Marca marca, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_MARCA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdMarca", marca.id_marca);
                    cmd.Parameters.AddWithValue("@Descripcion", marca.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", marca.estado);

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

        public bool Eliminar(int id_marca, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_MARCA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdMarca", id_marca);

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
