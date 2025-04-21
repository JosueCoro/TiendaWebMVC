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
    public class CD_TipoPago
    {
        public List<TipoPago> Listar()
        {
            List<TipoPago> lista = new List<TipoPago>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.CRUD_TIPO_PAGO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "SELECT");
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        lista.Add(new TipoPago()
                        {
                            id_tipo_pago = Convert.ToInt32(reader["id_tipo_pago"]),
                            descripcion = reader["descripcion"].ToString(),
                            estado = Convert.ToBoolean(reader["estado"])
                        });
                    }
                }
            }
            catch
            {
                lista = new List<TipoPago>();
            }

            return lista;
        }

        public int Registrar(TipoPago tipoPago, out string mensaje)
        {
            int resultado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.CRUD_TIPO_PAGO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Descripcion", tipoPago.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", tipoPago.estado);
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
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Editar(TipoPago tipoPago, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.CRUD_TIPO_PAGO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdTipoPago", tipoPago.id_tipo_pago);
                    cmd.Parameters.AddWithValue("@Descripcion", tipoPago.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", tipoPago.estado);
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
                mensaje = ex.Message;
            }

            return resultado;
        }

        public bool Eliminar(int idTipoPago, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("TRANSACCIONES.CRUD_TIPO_PAGO", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdTipoPago", idTipoPago);
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
                mensaje = ex.Message;
            }

            return resultado;
        }
    }
}
