using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ProyectoTest.Logica
{
    public class ArticuloLogica
    {
        private static ArticuloLogica _instancia = null;

        public ArticuloLogica()
        {

        }

        public static ArticuloLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new ArticuloLogica();
                }

                return _instancia;
            }
        }

        public List<Articulo> Listar()
        {

            List<Articulo> rptListaArticulo = new List<Articulo>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("sp_obtenerArticulo", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rptListaArticulo.Add(new Articulo()
                        {
                            IdArticulo = Convert.ToInt32(dr["IdArticulo"].ToString()),
                            Nombre = dr["Nombre"].ToString(),
                            Descripcion = dr["Descripcion"].ToString(),
                            oMarca = new Marca() { IdMarca = Convert.ToInt32(dr["IdMarca"].ToString()),Descripcion = dr["DescripcionMarca"].ToString() },
                            oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(dr["IdCategoria"].ToString()), Descripcion = dr["DescripcionCategoria"].ToString() },
                            Precio = Convert.ToDecimal(dr["Precio"].ToString(), new CultureInfo("es-PE")),
                            Stock = Convert.ToInt32(dr["Stock"].ToString()),
                            RutaImagen = dr["RutaImagen"].ToString(),
                            Activo = Convert.ToBoolean(dr["Activo"].ToString())
                        });
                    }
                    dr.Close();

                    return rptListaArticulo;

                }
                catch (Exception ex)
                {
                    rptListaArticulo = null;
                    return rptListaArticulo;
                }
            }
        }



        public int Registrar(Articulo oArticulo)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrarArticulo", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", oArticulo.Nombre );
                    cmd.Parameters.AddWithValue("Descripcion", oArticulo.Descripcion );
                    cmd.Parameters.AddWithValue("IdMarca", oArticulo.oMarca.IdMarca );
                    cmd.Parameters.AddWithValue("IdCategoria", oArticulo.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", oArticulo.Precio );
                    cmd.Parameters.AddWithValue("Stock", oArticulo.Stock );
                    cmd.Parameters.AddWithValue("RutaImagen", oArticulo.RutaImagen );
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
            return respuesta;
        }

        public bool Modificar(Articulo oArticulo)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_editarArticulo", oConexion);
                    cmd.Parameters.AddWithValue("IdArticulo", oArticulo.IdArticulo);
                    cmd.Parameters.AddWithValue("Nombre", oArticulo.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oArticulo.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", oArticulo.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", oArticulo.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Precio", oArticulo.Precio);
                    cmd.Parameters.AddWithValue("Stock", oArticulo.Stock);
                    cmd.Parameters.AddWithValue("Activo", oArticulo.Activo);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }
        

        public bool ActualizarRutaImagen(Articulo oArticulo)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizarRutaImagen", oConexion);
                    cmd.Parameters.AddWithValue("IdArticulo", oArticulo.IdArticulo);
                    cmd.Parameters.AddWithValue("RutaImagen", oArticulo.RutaImagen);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from ARTICULO where idarticulo = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }

    }
}