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
    public class CD_Tienda
    {
        public List<Tienda> ListarTiendas()
        {
            List<Tienda> lista = new List<Tienda>();
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn)) 
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_TIENDA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "SELECT");

                    SqlParameter paramMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500);
                    paramMensaje.Direction = ParameterDirection.Output;
                    SqlParameter paramResultado = new SqlParameter("@Resultado", SqlDbType.Bit);
                    paramResultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramMensaje);
                    cmd.Parameters.Add(paramResultado);

                    conexion.Open();

                    SqlDataReader lectura = cmd.ExecuteReader();
                    while (lectura.Read())
                    {
                        lista.Add(new Tienda() 
                        {
                            id_tienda = Convert.ToInt32(lectura["id_tienda"]),
                            nombre = lectura["nombre"].ToString(),
                            dirrecion = lectura["dirrecion"].ToString(),
                            telefono = lectura["telefono"].ToString(),
                            estado = Convert.ToBoolean(lectura["estado"])
                        });
                    }
                    conexion.Close(); //cerrar la conexion
                }
            }
            catch (Exception ex)
            {
                lista = new List<Tienda>();
            }
            return lista;
        }

        public int Registrar(Tienda tienda, out string mensaje)
        {
            int id_autogenerado = 0;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn)) //Usas la clase Conexion
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_TIENDA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Nombre", tienda.nombre);
                    cmd.Parameters.AddWithValue("@Direccion", tienda.dirrecion);
                    cmd.Parameters.AddWithValue("@Telefono", tienda.telefono);
                    cmd.Parameters.AddWithValue("@Estado", tienda.estado);
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    id_autogenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    conexion.Close(); 
                }
            }
            catch (Exception ex)
            {
                id_autogenerado = 0;
                mensaje = ex.Message;
            }
            return id_autogenerado;
        }

        public bool Editar(Tienda tienda, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn)) //Usas la clase Conexion
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_TIENDA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdTienda", tienda.id_tienda);
                    cmd.Parameters.AddWithValue("@Nombre", tienda.nombre);
                    cmd.Parameters.AddWithValue("@Direccion", tienda.dirrecion);
                    cmd.Parameters.AddWithValue("@Telefono", tienda.telefono);
                    cmd.Parameters.AddWithValue("@Estado", tienda.estado);
                    SqlParameter paramMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500);
                    paramMensaje.Direction = ParameterDirection.Output;
                    SqlParameter paramResultado = new SqlParameter("@Resultado", SqlDbType.Bit);
                    paramResultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramMensaje);
                    cmd.Parameters.Add(paramResultado);

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    mensaje = paramMensaje.Value.ToString();
                    resultado = Convert.ToBoolean(paramResultado.Value);
                    conexion.Close(); 
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                mensaje = ex.Message;
            }
            return resultado;
        }

        public bool Eliminar(int id_tienda, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;
            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn)) 
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.CRUD_TIENDA", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdTienda", id_tienda);

                    SqlParameter paramMensaje = new SqlParameter("@Mensaje", SqlDbType.VarChar, 500);
                    paramMensaje.Direction = ParameterDirection.Output;
                    SqlParameter paramResultado = new SqlParameter("@Resultado", SqlDbType.Bit);
                    paramResultado.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(paramMensaje);
                    cmd.Parameters.Add(paramResultado);

                    conexion.Open();
                    cmd.ExecuteNonQuery();

                    mensaje = paramMensaje.Value.ToString();
                    resultado = Convert.ToBoolean(paramResultado.Value);
                    conexion.Close(); 
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
