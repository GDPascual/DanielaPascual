using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;




namespace ProyectoTest.Logica
{
    public class ClienteLogica
    {
        private static ClienteLogica _instancia = null;

        public ClienteLogica()
        {

        }

        public static ClienteLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new ClienteLogica();
                }

                return _instancia;
            }
        }

        public Cliente Obtener(string _correo, string _contrasena)
        {
            Cliente objeto = null;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_obtenerCliente", oConexion);
                    cmd.Parameters.AddWithValue("Correo", _correo);
                    cmd.Parameters.AddWithValue("Contrasena", _contrasena);
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader()) {
                        while (dr.Read()) {
                            objeto = new Cliente()
                            {
                                IdCliente = Convert.ToInt32(dr["IdCliente"].ToString()),
                                Nombres = dr["Nombres"].ToString(),
                                Apellidos = dr["Apellidos"].ToString(),
                                Correo = dr["Correo"].ToString(),
                                Contrasena = dr["Contrasena"].ToString(),
                                EsAdministrador = Convert.ToBoolean(dr["EsAdministrador"].ToString())
                            };

                        }
                    }

                }
                catch (Exception ex)
                {
                    objeto = null;
                }
            }
            return objeto;
        }

        public int Registrar(Cliente oCliente)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_registrarCliente", oConexion);
                    cmd.Parameters.AddWithValue("Nombres", oCliente.Nombres);
                    cmd.Parameters.AddWithValue("Apellidos", oCliente.Apellidos);
                    cmd.Parameters.AddWithValue("Correo", oCliente.Correo);
                    cmd.Parameters.AddWithValue("Contrasena", oCliente.Contrasena);
                    cmd.Parameters.AddWithValue("EsAdministrador", oCliente.EsAdministrador);
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
    }
}