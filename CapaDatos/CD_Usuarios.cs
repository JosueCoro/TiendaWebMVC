using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using CapaEntidad;
//CREATE TABLE SEGURIDAD.USUARIO 
//(
//id_usuario INTEGER NOT NULL IDENTITY(1,1), 
//nombres VARCHAR(150) NOT NULL,
//apellidos VARCHAR(150) NOT NULL,
//correo VARCHAR(20) NOT NULL,
//contraseña VARCHAR(150) NOT NULL,
//telefono VARCHAR(20) NOT NULL,
//estado BIT DEFAULT 1 NOT NULL,
//ROLES_id_rol INTEGER NOT NULL 
//)
//GO 
namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
           
            try
            {
                using (SqlConnection oconexion= new SqlConnection(Conexion.cn)) 
                {
                    SqlCommand cmd = new SqlCommand("SEGURIDAD.LISTAR_USUARIOS", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure; 

                    oconexion.Open();
                    SqlDataReader lectura = cmd.ExecuteReader();

                    while (lectura.Read())
                    {
                        lista.Add(new Usuario()
                        {
                            id_usuario = Convert.ToInt32(lectura["id_usuario"]),
                            nombres = lectura["nombres"].ToString(),
                            apellidos = lectura["apellidos"].ToString(),
                            correo = lectura["correo"].ToString(),
                            contraseña = lectura["contraseña"].ToString(),
                            telefono = lectura["telefono"].ToString(),
                            estado = Convert.ToBoolean(lectura["estado"]),
                            id_rol = Convert.ToInt32(lectura["ROLES_id_rol"]),
                            rol = lectura["nombre_rol"].ToString() 
                        });
                    }

                }
                
            }
            catch 
            {
                lista = new List<Usuario>();
            }

            return lista;
        }
    }
}
