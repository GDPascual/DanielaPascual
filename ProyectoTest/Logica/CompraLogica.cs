using ProyectoTest.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace ProyectoTest.Logica
{
    public class CompraLogica
    {
        private static CompraLogica _instancia = null;

        public CompraLogica()
        {

        }

        public static CompraLogica Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new CompraLogica();
                }

                return _instancia;
            }
        }

        public bool Registrar(Compra oCompra)
        {

            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    StringBuilder query = new StringBuilder();
                    foreach (DetalleCompra dc in oCompra.oDetalleCompra) {
                        query.AppendLine("insert into detalle_compra(IdCompra,IdArticulo,Cantidad,Total) values (¡idcompra!," + dc.IdArticulo +","+dc.Cantidad+","+dc.Total+")");
                    }

                    SqlCommand cmd = new SqlCommand("sp_registrarCompra", oConexion);
                    cmd.Parameters.AddWithValue("IdCliente", oCompra.IdCliente);
                    cmd.Parameters.AddWithValue("TotalArticulo", oCompra.TotalArticulo);
                    cmd.Parameters.AddWithValue("Total", oCompra.Total);
                    cmd.Parameters.AddWithValue("Contacto", oCompra.Contacto);
                    cmd.Parameters.AddWithValue("Telefono", oCompra.Telefono);
                    cmd.Parameters.AddWithValue("Direccion", oCompra.Direccion);
                    cmd.Parameters.AddWithValue("IdDistrito", oCompra.IdDistrito);

                    cmd.Parameters.AddWithValue("QueryDetalleCompra", query.ToString());
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
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



    }
}