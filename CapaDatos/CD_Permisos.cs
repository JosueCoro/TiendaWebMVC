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
    public class CD_Permisos
    {
        public List<Permisos> ListarPermisos()
        {
            List<Permisos> lista = new List<Permisos>();

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CRUD_PERMISOS", conexion);
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
                        lista.Add(new Permisos()
                        {
                            id_permiso = Convert.ToInt32(lector["id_permiso"]),
                            nombre = lector["nombre"].ToString(),
                            descripcion = lector["descripcion"].ToString(),
                            estado = Convert.ToBoolean(lector["estado"])
                        });
                    }
                    lector.Close(); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al listar permisos: " + ex.Message); 
                lista = new List<Permisos>(); 
            }

            return lista;
        }

        public int Registrar(Permisos permiso, out string mensaje)
        {
            int idGenerado = 0; 
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CRUD_PERMISOS", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "INSERT");
                    cmd.Parameters.AddWithValue("@Nombre", permiso.nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", permiso.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", permiso.estado);

                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                    conexion.Open();
                    cmd.ExecuteNonQuery();
                    mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                    bool resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);

                    if (resultado)
                    {
                        
                        idGenerado = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                idGenerado = 0; 
                mensaje = "Error en la capa de datos: " + ex.Message; 
            }

            return idGenerado; 
        }

        //metodo para editar un permiso existente
        public bool Editar(Permisos permiso, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CRUD_PERMISOS", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "UPDATE");
                    cmd.Parameters.AddWithValue("@IdPermiso", permiso.id_permiso);
                    cmd.Parameters.AddWithValue("@Nombre", permiso.nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", permiso.descripcion);
                    cmd.Parameters.AddWithValue("@Estado", permiso.estado);

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
                mensaje = "Error en la capa de datos: " + ex.Message;
            }

            return resultado;
        }

        //metodo para eliminar un permiso
        public bool Eliminar(int id_permiso, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            try
            {
                using (SqlConnection conexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.CRUD_PERMISOS", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Operacion", "DELETE");
                    cmd.Parameters.AddWithValue("@IdPermiso", id_permiso);

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
                mensaje = "Error en la capa de datos: " + ex.Message;
            }

            return resultado;
        }
    }
}
