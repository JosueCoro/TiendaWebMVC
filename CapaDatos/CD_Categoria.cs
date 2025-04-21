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
    public class CD_Categoria
    {
        /*CREATE PROCEDURE INVENTARIO.CRUD_CATEGORIA
            @Operacion NVARCHAR(10),
            @IdCategoria INT = NULL,
            @Descripcion VARCHAR(100) = NULL,
            @Estado BIT = NULL,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        AS
        BEGIN
            SET @Resultado = 0;

            -- INSERTAR CATEGORÍA
            IF @Operacion = 'INSERT'
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM INVENTARIO.CATEGORIA WHERE descripcion = @Descripcion)
                BEGIN
                    INSERT INTO INVENTARIO.CATEGORIA (descripcion, estado)
                    VALUES (@Descripcion, ISNULL(@Estado, 1));

                    SET @Resultado = 1;
                    SET @Mensaje = 'Categoría registrada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe una categoría con esa descripción.';
            END

            -- CONSULTAR CATEGORÍAS
            ELSE IF @Operacion = 'SELECT'
            BEGIN
                SELECT * FROM INVENTARIO.CATEGORIA;
                SET @Resultado = 1;
            END

            -- ACTUALIZAR CATEGORÍA
            ELSE IF @Operacion = 'UPDATE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM INVENTARIO.CATEGORIA 
                    WHERE descripcion = @Descripcion AND id_categoria != @IdCategoria
                )
                BEGIN
                    UPDATE INVENTARIO.CATEGORIA
                    SET descripcion = @Descripcion,
                        estado = @Estado
                    WHERE id_categoria = @IdCategoria;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Categoría actualizada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe otra categoría con esa descripción.';
            END

            -- ELIMINAR CATEGORÍA (si no está relacionada)
            ELSE IF @Operacion = 'DELETE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM INVENTARIO.PRODUCTO WHERE CATEGORIA_id_categoria = @IdCategoria
                )
                BEGIN
                    DELETE FROM INVENTARIO.CATEGORIA
                    WHERE id_categoria = @IdCategoria;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Categoría eliminada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'La categoría está relacionada con uno o más productos. No se puede eliminar.';
            END

            -- OPERACIÓN INVÁLIDA
            ELSE
            BEGIN
                SET @Mensaje = 'Operación no válida.';
            END
        END
        GO
        */
        public List<Categoria> ListarCategorias()
        {
            List<Categoria> lista = new List<Categoria>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_CATEGORIA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "SELECT");

                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    SqlDataReader lector = cmd.ExecuteReader();

                    while (lector.Read())
                    {
                        lista.Add(new Categoria()
                        {
                            id_categoria = Convert.ToInt32(lector["id_categoria"]),
                            descripcion = lector["descripcion"].ToString(),
                            estado = Convert.ToBoolean(lector["estado"])
                        });
                    }
                }
            }
            catch
            {
                lista = new List<Categoria>();
            }

            return lista;
        }

        public int Registrar(Categoria categoria, out string mensaje)
        {
            int resultado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_CATEGORIA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Descripcion", categoria.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", categoria.estado);
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

        public bool Editar(Categoria categoria, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_CATEGORIA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdCategoria", categoria.id_categoria);
                    cmd.Parameters.AddWithValue("@Descripcion", categoria.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", categoria.estado);
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

        public bool Eliminar(int id_categoria, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_CATEGORIA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdCategoria", id_categoria);
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
