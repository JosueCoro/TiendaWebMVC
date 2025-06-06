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
    public class CD_RolPermiso
    {
        /// <summary>
        /// Obtiene una lista de IDs de permisos asignados a un rol específico.
        /// </summary>
        /// <param name="idRol">El ID del rol para el cual se quieren obtener los permisos.</param>
        /// <returns>Una lista de enteros con los IDs de permisos.</returns>
        public List<int> ObtenerPermisosPorRol(int idRol)
        {
            List<int> listaIdPermisos = new List<int>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    oconexion.Open();
                    SqlCommand cmd = new SqlCommand("SELECT PERMISOS_id_permiso FROM SEGURIDAD.ROLES_PERMISOS WHERE ROLES_id_rol = @IdRol", oconexion);
                    cmd.Parameters.AddWithValue("@IdRol", idRol);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            listaIdPermisos.Add(Convert.ToInt32(dr["PERMISOS_id_permiso"]));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener permisos por rol: " + ex.Message);
                listaIdPermisos = new List<int>();
            }

            return listaIdPermisos;
        }

        /// <summary>
        /// Asigna un nuevo conjunto de permisos a un rol, eliminando los anteriores.
        /// Esta operación se realiza dentro de una transacción para asegurar la integridad.
        /// </summary>
        /// <param name="idRol">El ID del rol al que se asignarán los permisos.</param>
        /// <param name="idsPermisos">Una lista de IDs de permisos que deben ser asignados.</param>
        /// <returns>Verdadero si la operación fue exitosa, falso en caso contrario.</returns>
        public bool AsignarPermisosARol(int idRol, List<int> idsPermisos)
        {
            bool resultado = false;

            // Usamos Conexion.cn directamente aquí
            using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
            {
                oconexion.Open();
                SqlTransaction objTransaccion = null;
                try
                {
                    objTransaccion = oconexion.BeginTransaction();

                    SqlCommand cmdDelete = new SqlCommand("DELETE FROM SEGURIDAD.ROLES_PERMISOS WHERE ROLES_id_rol = @IdRol", oconexion, objTransaccion);
                    cmdDelete.Parameters.AddWithValue("@IdRol", idRol);
                    cmdDelete.CommandType = CommandType.Text;
                    cmdDelete.ExecuteNonQuery();

                    foreach (int idPermiso in idsPermisos)
                    {
                        SqlCommand cmdInsert = new SqlCommand("INSERT INTO SEGURIDAD.ROLES_PERMISOS (ROLES_id_rol, PERMISOS_id_permiso) VALUES (@IdRol, @IdPermiso)", oconexion, objTransaccion);
                        cmdInsert.Parameters.AddWithValue("@IdRol", idRol);
                        cmdInsert.Parameters.AddWithValue("@IdPermiso", idPermiso);
                        cmdInsert.CommandType = CommandType.Text;
                        cmdInsert.ExecuteNonQuery();
                    }

                    objTransaccion.Commit();
                    resultado = true;
                }
                catch (Exception ex)
                {
                    if (objTransaccion != null)
                    {
                        objTransaccion.Rollback();
                    }
                    Console.WriteLine("Error en la asignación de permisos a rol: " + ex.Message);
                    resultado = false;
                }
            }
            return resultado;
        }
    }
}
