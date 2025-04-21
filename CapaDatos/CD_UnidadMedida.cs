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
    public class CD_UnidadMedida
    {
        /*CREATE PROCEDURE INVENTARIO.CRUD_UNIDAD_MEDIDA
            @Operacion NVARCHAR(10),
            @IdUnidadMedida INT = NULL,
            @Descripcion VARCHAR(50) = NULL,
            @Estado BIT = NULL,
            @Mensaje VARCHAR(500) OUTPUT,
            @Resultado BIT OUTPUT
        AS
        BEGIN
            SET @Resultado = 0;

            -- INSERTAR UNIDAD DE MEDIDA
            IF @Operacion = 'INSERT'
            BEGIN
                IF NOT EXISTS (SELECT 1 FROM INVENTARIO.UNIDAD_MEDIDA WHERE descripcion = @Descripcion)
                BEGIN
                    INSERT INTO INVENTARIO.UNIDAD_MEDIDA (descripcion, estado)
                    VALUES (@Descripcion, ISNULL(@Estado, 1));

                    SET @Resultado = 1;
                    SET @Mensaje = 'Unidad de medida registrada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe una unidad de medida con esa descripción.';
            END

            -- CONSULTAR UNIDADES DE MEDIDA
            ELSE IF @Operacion = 'SELECT'
            BEGIN
                SELECT * FROM INVENTARIO.UNIDAD_MEDIDA;
                SET @Resultado = 1;
            END

            -- ACTUALIZAR UNIDAD DE MEDIDA
            ELSE IF @Operacion = 'UPDATE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM INVENTARIO.UNIDAD_MEDIDA
                    WHERE descripcion = @Descripcion AND id_unidad_medida != @IdUnidadMedida
                )
                BEGIN
                    UPDATE INVENTARIO.UNIDAD_MEDIDA
                    SET descripcion = @Descripcion,
                        estado = @Estado
                    WHERE id_unidad_medida = @IdUnidadMedida;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Unidad de medida actualizada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'Ya existe otra unidad de medida con esa descripción.';
            END

            -- ELIMINAR UNIDAD DE MEDIDA (si no está relacionada)
            ELSE IF @Operacion = 'DELETE'
            BEGIN
                IF NOT EXISTS (
                    SELECT 1 FROM INVENTARIO.PRODUCTO WHERE UNIDAD_MEDIDA_id_unidad_medida = @IdUnidadMedida
                )
                BEGIN
                    DELETE FROM INVENTARIO.UNIDAD_MEDIDA
                    WHERE id_unidad_medida = @IdUnidadMedida;

                    SET @Resultado = 1;
                    SET @Mensaje = 'Unidad de medida eliminada correctamente.';
                END
                ELSE
                    SET @Mensaje = 'La unidad de medida está relacionada con uno o más productos. No se puede eliminar.';
            END

            -- OPERACIÓN NO VÁLIDA
            ELSE
            BEGIN
                SET @Mensaje = 'Operación no válida.';
            END
        END
        GO
        */
        public List<UnidadMedida> Listar()
        {
            List<UnidadMedida> lista = new List<UnidadMedida>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_UNIDAD_MEDIDA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "SELECT");

                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new UnidadMedida()
                        {
                            id_unidad_medida = Convert.ToInt32(dr["id_unidad_medida"]),
                            descripcion = dr["descripcion"].ToString(),
                            estado = Convert.ToBoolean(dr["estado"])
                        });
                    }
                }
            }
            catch
            {
                lista = new List<UnidadMedida>();
            }

            return lista;
        }

        public int Registrar(UnidadMedida unidad, out string mensaje)
        {
            int resultado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_UNIDAD_MEDIDA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Descripcion", unidad.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", unidad.estado);
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

        public bool Editar(UnidadMedida unidad, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_UNIDAD_MEDIDA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdUnidadMedida", unidad.id_unidad_medida);
                    cmd.Parameters.AddWithValue("@Descripcion", unidad.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", unidad.estado);
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

        public bool Eliminar(int idUnidad, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_UNIDAD_MEDIDA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdUnidadMedida", idUnidad);
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
