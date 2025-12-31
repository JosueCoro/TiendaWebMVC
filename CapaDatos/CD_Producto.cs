using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using CapaEntidad;
using System.Globalization;

namespace CapaDatos
{
    public class CD_Producto
    {
        private User_activo user_Activo = new User_activo();
        public List<Producto> Listar()
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT P.id_producto, P.nombre, P.descripcion, P.codigo_barra,"); // Se agrega codigo_barra
                    sb.AppendLine("S.id_stock, S.cantidad AS stock_actual,");
                    sb.AppendLine("T.id_tienda, T.nombre AS nombre_tienda,");
                    sb.AppendLine("C.id_categoria, C.descripcion AS DesCategoria,");
                    sb.AppendLine("M.id_marca, M.descripcion AS DesMarca,");
                    sb.AppendLine("U.id_unidad_medida, U.descripcion AS DesUnidadMed,");
                    sb.AppendLine("P.precio, P.ruta_imagen, P.nombre_imagen, P.estado");
                    sb.AppendLine("FROM INVENTARIO.PRODUCTO P");
                    sb.AppendLine("LEFT JOIN INVENTARIO.STOCK S ON S.PRODUCTO_id_producto = P.id_producto");
                    sb.AppendLine("LEFT JOIN INVENTARIO.TIENDA T ON T.id_tienda = S.TIENDA_id_tienda");
                    sb.AppendLine("INNER JOIN INVENTARIO.CATEGORIA C ON C.id_categoria = P.CATEGORIA_id_categoria");
                    sb.AppendLine("INNER JOIN INVENTARIO.MARCA M ON M.id_marca = P.MARCA_id_marca");
                    sb.AppendLine("INNER JOIN INVENTARIO.UNIDAD_MEDIDA U ON U.id_unidad_medida = P.UNIDAD_MEDIDA_id_unidad_medida");

                    SqlCommand cmd = new SqlCommand(sb.ToString(), oconexion);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                id_producto = Convert.ToInt32(dr["id_producto"]),
                                nombre = dr["nombre"].ToString(),
                                descripcion = dr["descripcion"].ToString(),
                                codigo_barra = dr["codigo_barra"].ToString(), // Mapeo de la nueva columna
                                oStock = new Stock()
                                {
                                    id_stock = dr["id_stock"] != DBNull.Value ? Convert.ToInt32(dr["id_stock"]) : 0,
                                    cantidad = dr["stock_actual"] != DBNull.Value ? Convert.ToInt32(dr["stock_actual"]) : 0
                                },
                                oTienda = new Tienda()
                                {
                                    id_tienda = dr["id_tienda"] != DBNull.Value ? Convert.ToInt32(dr["id_tienda"]) : 0,
                                    nombre = dr["nombre_tienda"] != DBNull.Value ? dr["nombre_tienda"].ToString() : "No asignada"
                                },
                                oMarca = new Marca()
                                {
                                    id_marca = Convert.ToInt32(dr["id_marca"]),
                                    descripcion = dr["DesMarca"].ToString(),
                                },
                                oCategoria = new Categoria()
                                {
                                    id_categoria = Convert.ToInt32(dr["id_categoria"]),
                                    descripcion = dr["DesCategoria"].ToString(),
                                },
                                oUnidadMedida = new UnidadMedida()
                                {
                                    id_unidad_medida = Convert.ToInt32(dr["id_unidad_medida"]),
                                    descripcion = dr["DesUnidadMed"].ToString(),
                                },
                                precio = Convert.ToDecimal(dr["precio"], new CultureInfo("es-PE")),
                                ruta_imagen = dr["ruta_imagen"].ToString(),
                                nombre_imagen = dr["nombre_imagen"].ToString(),
                                estado = Convert.ToBoolean(dr["estado"])
                            });
                        }
                    }
                }
            }
            catch { lista = new List<Producto>(); }
            return lista;
        }





        public int Registrar(Producto obj, out string Mensaje)
        {
            int id_autogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.sp_RegistrarProducto", oconexion);
                    cmd.Parameters.AddWithValue("@nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("@Precio", obj.precio);
                    cmd.Parameters.AddWithValue("@ruta_imagen", obj.ruta_imagen);
                    cmd.Parameters.AddWithValue("@nombre_imagen", obj.nombre_imagen);
                    cmd.Parameters.AddWithValue("@estado", obj.estado);
                    cmd.Parameters.AddWithValue("@MARCA_id_marca", obj.MARCA_id_marca);
                    cmd.Parameters.AddWithValue("@CATEGORIA_id_categoria", obj.CATEGORIA_id_categoria);
                    cmd.Parameters.AddWithValue("@UNIDAD_MEDIDA_id_unidad_medida", obj.UNIDAD_MEDIDA_id_unidad_medida);
                    cmd.Parameters.AddWithValue("@codigo_barra", obj.codigo_barra); // Nuevo parámetro enviado al SP
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    id_autogenerado = Convert.ToInt32(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                id_autogenerado = 0;
                Mensaje = ex.Message;
            }
            return id_autogenerado;
        }

        
        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.sp_EditarProducto", oconexion);
                    cmd.Parameters.AddWithValue("@id_producto", obj.id_producto);
                    cmd.Parameters.AddWithValue("@nombre", obj.nombre);
                    cmd.Parameters.AddWithValue("@descripcion", obj.descripcion);
                    cmd.Parameters.AddWithValue("@precio", obj.precio);
                    cmd.Parameters.AddWithValue("@ruta_imagen", obj.ruta_imagen);
                    cmd.Parameters.AddWithValue("@nombre_imagen", obj.nombre_imagen);
                    cmd.Parameters.AddWithValue("@estado", obj.estado);
                    cmd.Parameters.AddWithValue("@MARCA_id_marca", obj.MARCA_id_marca);
                    cmd.Parameters.AddWithValue("@CATEGORIA_id_categoria", obj.CATEGORIA_id_categoria);
                    cmd.Parameters.AddWithValue("@UNIDAD_MEDIDA_id_unidad_medida", obj.UNIDAD_MEDIDA_id_unidad_medida);
                    cmd.Parameters.AddWithValue("@codigo_barra", obj.codigo_barra); // Nuevo parámetro enviado al SP
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();
                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }


        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "UPDATE INVENTARIO.PRODUCTO SET ruta_imagen = @ruta_imagen, nombre_imagen = @nombre_imagen WHERE id_producto = @id_producto";


                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@ruta_imagen", oProducto.ruta_imagen);
                    cmd.Parameters.AddWithValue("@nombre_imagen", oProducto.nombre_imagen);
                    cmd.Parameters.AddWithValue("@id_producto", oProducto.id_producto);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar imagen.";
                    }
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;

        }



        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.sp_EliminarProducto", oconexion);
                    cmd.Parameters.AddWithValue("@id_producto", id);
                    cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    resultado = Convert.ToBoolean(cmd.Parameters["@Resultado"].Value);
                    Mensaje = cmd.Parameters["@Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }


        //Lista de productos para el catalogo
        public List<Producto> ListarProductosFiltro(int idMarca = 0, int idCategoria = 0)
        {
            List<Producto> lista = new List<Producto>();

            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.SP_LISTAR_PRODUCTOS_FILTROS", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdMarca", idMarca == 0 ? (object)DBNull.Value : idMarca);
                    cmd.Parameters.AddWithValue("@IdCategoria", idCategoria == 0 ? (object)DBNull.Value : idCategoria);

                    oconexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                id_producto = Convert.ToInt32(dr["id_producto"]),
                                nombre = dr["nombre"].ToString(),
                                descripcion = dr["DescripcionProducto"].ToString(), 
                                precio = Convert.ToDecimal(dr["precio"], new CultureInfo("es-PE")),
                                ruta_imagen = dr["ruta_imagen"].ToString(),
                                nombre_imagen = dr["nombre_imagen"].ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),

                                oMarca = new Marca()
                                {
                                    id_marca = Convert.ToInt32(dr["id_marca"]),
                                    descripcion = dr["DescripcionMarca"].ToString(),
                                },
                                oCategoria = new Categoria()
                                {
                                    id_categoria = Convert.ToInt32(dr["id_categoria"]),
                                    descripcion = dr["DescripcionCategoria"].ToString(), 
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Producto>();
                Console.WriteLine("Error al listar productos con filtros: " + ex.Message); 
            }

            return lista;
        }

        public List<Producto> ObtenerProductoPorId(int id_producto = 0) 
        {
            List<Producto> lista = new List<Producto>();
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("INVENTARIO.LISTAR_PRODUCTO_ID", oconexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_producto", id_producto);
                    oconexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Producto()
                            {
                                id_producto = Convert.ToInt32(dr["id_producto"]),
                                nombre = dr["nombre"].ToString(),
                                descripcion = dr["DescripcionProducto"].ToString(),
                                precio = Convert.ToDecimal(dr["precio"], new CultureInfo("es-PE")),
                                ruta_imagen = dr["ruta_imagen"].ToString(),
                                nombre_imagen = dr["nombre_imagen"].ToString(),
                                estado = Convert.ToBoolean(dr["estado"]),
                                oMarca = new Marca()
                                {
                                    id_marca = Convert.ToInt32(dr["id_marca"]),
                                    descripcion = dr["descripcion_marca"].ToString(),
                                },
                                oCategoria = new Categoria()
                                {
                                    id_categoria = Convert.ToInt32(dr["id_categoria"]),
                                    descripcion = dr["descripcion_categoria"].ToString(),
                                }
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lista = new List<Producto>();
                Console.WriteLine("Error al obtener producto por ID: " + ex.Message);
            }
            return lista;
        }



    }
}
