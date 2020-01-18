using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace EpironAPI.classes
{
    public class MetaDatos
    {
        public static SqlCommand CrearComando()
        {
            string cadenaConexion = Configuracion.Conn;
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            conexion.ConnectionString = cadenaConexion;
            comando = conexion.CreateCommand();
            comando.CommandType = CommandType.Text;
            return comando;
        }

        public static SqlCommand CrearComandoProc(string nproc)// nproc es el nombre del proc. alm.
        {
            string cadenaConexion = Configuracion.Conn;
            SqlConnection conexion = new SqlConnection(cadenaConexion);
            SqlCommand comando = new SqlCommand(nproc, conexion);
            comando.CommandType = CommandType.StoredProcedure;

            return comando;
        }

        public static int EjecutarComando(SqlCommand comando)
        {
            try
            {
                comando.Connection.Open();
                return comando.ExecuteNonQuery();
            }
            catch { throw; }
            finally
            {
                comando.Connection.Dispose();
                comando.Connection.Close();
            }
        }

        public static DataTable EjecutarComandoSelect(SqlCommand comando)
        {
            DataTable tabla = new DataTable();
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter();
                comando.Connection.Open();
                adaptador.SelectCommand = comando;
                adaptador.Fill(tabla);
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { comando.Connection.Close(); }
            return tabla;
        }

        //Sentencia para cuando la consulta devuelve mas de una tabla
        public static DataSet EjecutarComandoSelectDs(SqlCommand comando)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlDataAdapter adaptador = new SqlDataAdapter();
                comando.Connection.Open();
                adaptador.SelectCommand = comando;
                adaptador.Fill(ds);
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { comando.Connection.Close(); }
            return ds;
        }
    }



    public class Configuracion
    {

        static string conn = ConfigurationManager.AppSettings["conn"];//.ConnectionStrings["SecurityConnectionString"].ConnectionString;


        public static string Conn
        {
            get
            {
                if (conn == null)
                {
                    conn = ConfigurationManager.ConnectionStrings["dbSeguridad"].ConnectionString;
                }
                return conn;
            }
        }
    }
}