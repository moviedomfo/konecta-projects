using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.SqlClient;
using CentralizedSecurity.webApi.Models;
using CentralizedSecurity.webApi.service;

namespace CentralizedSecurity.webApi.DAC
{
    /// <summary>
    /// 
    /// </summary>
    public class MeucciDAC
    {


        /// <summary>
        /// usp_OlvidoDeClave_DatosEmpleado Retorna dos tablas
        /// 1- Datos de Persona (1 fila)
        /// 2 - Datos de cada usuario de la persona (muchos)
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        public static EmpleadoBE VirifyUser_ForgotPassword(string dni)
        {
            EmpleadoBE item = null;
            var connectionString = Common.GetCnn(Common.CnnStringNameMeucci).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("Meucci2a.dbo.usp_OlvidoDeClave_DatosEmpleado", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                /// FACTURA_NRO
                cmd.Parameters.AddWithValue("@Dni", dni);


                using (IDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        item = new EmpleadoBE();

                        //cue_nombre sar_nombre car_nombre usuariowindows dominio
                        item.Emp_id = Convert.ToInt32(reader["Id"]);
                        item.ApeNom = reader["Nombre"].ToString();

                        if (reader["Tipo"] != DBNull.Value)
                            item.Tipo = reader["Tipo"].ToString();

                        if (reader["Email"] != DBNull.Value)
                            item.Email = reader["Email"].ToString();
                        


                    }
                    if (item != null)
                    {
                        if (reader.NextResult())
                        {
                            List<WindosUserBE> winUserList = new List<WindosUserBE>();
                            WindosUserBE winUser = null;
                            while (reader.Read())
                            {
                                winUser = new WindosUserBE();


                                winUser.Dominio = reader["dominio"].ToString();

                                winUser.WindowsUser = reader["usuariowindows"].ToString().ToLower();
                                winUser.dom_id = Convert.ToInt32(reader["dom_id"]);
                                

                                winUserList.Add(winUser);
                            }
                            item.WindosUserList = winUserList;
                        }
                    }


                }

                return item;

            }

        }

        /// <summary>
        /// verificará que el usuario esté habilitado para resetear o desbloquear un UW y que no registre un ausentismo para ese día.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domainId"></param>
        public static EmpleadoReseteoBE VirifyUser(string userName, int domainId)
        {
            EmpleadoReseteoBE item = null;
            var connectionString = Common.GetCnn(Common.CnnStringNameMeucci).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.usp_ReseteoWebVerificarUsuario", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                /// FACTURA_NRO
                cmd.Parameters.AddWithValue("@UW", userName);
                cmd.Parameters.AddWithValue("@dom_id", domainId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item = new EmpleadoReseteoBE();
                        try
                        {
                            //Estos no son empleados de la empresa pero no debe retornar error 
                            
                            if (reader["Legajo"] == DBNull.Value)
                                return null;
                        }
                        catch
                        {
                            return null;
                        }
                        

                        item.Emp_id = Convert.ToInt32(reader["Legajo"]);
                        item.Legajo = Convert.ToInt32(reader["Legajo"]);
                        item.CAIS = Convert.ToBoolean(reader["CAIS"]);

                        if (reader["Cuenta"] != DBNull.Value)
                            item.Cuenta = reader["Cuenta"].ToString();
                        else
                            item.Cuenta = string.Empty;
                        if (reader["Cargo"] != DBNull.Value)
                            item.Cargo = reader["Cargo"].ToString();
                        else
                            item.Cargo = string.Empty;

                        item.DomainId = domainId;
                        item.WindowsUser = userName;
                    }
                }

                return item;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName">Proviene de la tabla Domains URL y es el SiteDoamin</param>
        /// <returns></returns>
        internal static int GetDimainId(string domainName)
        {
            domainName = domainName.Replace("-", ".");
            var meucciDomains = RetriveDommains();
            var d = meucciDomains.Where(p => p.Domain.ToLower().Equals(domainName.ToLower())).FirstOrDefault();
            if (d == null)
                throw new Fwk.Exceptions.FunctionalException("No es posible encontrar informacion configurada sobre el dominio " + domainName.ToLower());
            return d.DomainId;

            //antes sacabamos del domain.json
            //var d = Common.Domains.Where(p => p.Domain.ToLower().Equals(domainName.ToLower())).FirstOrDefault();
            //if(d==null)
            //    throw new Fwk.Exceptions.FunctionalException("No es posible encontrar informacion configurada sobre el dominio " + domainName.ToLower());
            //return d.DomainId;
        }
       

        /// <summary>
        /// Retorna la cantidad de reseteos/desbloqueos realizados a un empleado en el presente día,   
        /// la acción puede ser "R" o "D"   
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domainId"></param>
        /// <param name="accion"></param>
        public static int ValidarteIntentos(string userName, int domainId, string accion)
        {
            int intentos = 0;

            var connectionString = Common.GetCnn(Common.CnnStringNameMeucci).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.usp_ReseteoWebValidarIntentos", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                
                cmd.Parameters.AddWithValue("@UW", userName);
                cmd.Parameters.AddWithValue("@dom_id", domainId);
                cmd.Parameters.AddWithValue("@accion", accion);

                
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        intentos = Convert.ToInt32(reader["Intentos"]);
                    }
                }

                return intentos;
            }

        }

        static EmpleadoBE RetriveDatosReseteoEmpleados_mok()
        {
            
            EmpleadoBE item = new EmpleadoBE();
            item.Emp_id = 666;
            item.Cargo = "Lider";
            item.Cuenta = "Movistar";
            item.Subarea = "Movistar Ventas";
            item.ApeNom = "Noelia Avila";
            item.Legajo = 99985425;
            //item.Aus_Id = 123;
            WindosUserBE winUser = new WindosUserBE();


            winUser.Dominio = "allus-ar";
            winUser.WindowsUser = "anoelia";
            winUser.dom_id = 3;
            item.WindosUserList = new List<WindosUserBE>();
            item.WindosUserList.Add(winUser);

            winUser = new WindosUserBE();


            winUser.Dominio = "allus-ar";
            winUser.WindowsUser = "suanoelia";
            winUser.dom_id = 3;

            item.WindosUserList.Add(winUser);

            winUser = new WindosUserBE();


            winUser.Dominio = "alcomovistar";
            winUser.WindowsUser = "suanoelia";
            winUser.dom_id = 2;

            item.WindosUserList.Add(winUser);

            return item;
        }

        /// <summary>
        /// Buscar
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domainId"></param>
        /// <param name="DNI"></param>
        public static EmpleadoBE RetriveDatosReseteoEmpleados(string userName, int domainId, string DNI)
        {
            //return RetriveDatosReseteoEmpleados_mok();

            EmpleadoBE item = null;

            
            var connectionString = Common.GetCnn(Common.CnnStringNameMeucci).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.usp_ReseteoWebDatosEmpleado", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@UW", userName);
                cmd.Parameters.AddWithValue("@dom_id", domainId);
                cmd.Parameters.AddWithValue("@DNI", DNI);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                   
                    while (reader.Read())
                    {

                        item = new EmpleadoBE();

                        //cue_nombre sar_nombre car_nombre usuariowindows dominio
                        item.Emp_id = Convert.ToInt32(reader["Emp_id"]);
                        item.Cargo = reader["car_nombre"].ToString();
                        item.Cuenta = reader["cue_nombre"].ToString();
                        item.Subarea = reader["sar_nombre"].ToString();
                        item.ApeNom = reader["emp_apenom"].ToString();
                        if(reader["aus_id"]!= DBNull.Value)
                            item.Aus_Id = Convert.ToInt32(reader["aus_id"]);

                        if (reader["Telefono"] != DBNull.Value)
                            item.Telefono = reader["Telefono"].ToString();
                        if (reader["Direccion"] != DBNull.Value)
                            item.Direccion = reader["Direccion"].ToString();
                        if (reader["Ciudad_Natal"] != DBNull.Value)
                            item.CiudadNatal = reader["Ciudad_Natal"].ToString();

                        if (reader["Fec_Nac"] != DBNull.Value)
                            item.FechaNacimiento = reader["Fec_Nac"].ToString();

                        
                    }
                    if (item != null)
                    {
                        if (reader.NextResult())
                        {
                            List<WindosUserBE> winUserList = new List<WindosUserBE>();
                            WindosUserBE winUser = null;
                            while (reader.Read())
                            {
                                winUser = new WindosUserBE();


                                winUser.Dominio = reader["dominio"].ToString();
                                winUser.WindowsUser = reader["usuariowindows"].ToString();
                                winUser.dom_id = Convert.ToInt32(reader["dom_id"]);

                                winUserList.Add(winUser);
                            }
                            item.WindosUserList = winUserList;
                        }
                    }
                  
                   
                }

                return item;

            }

        }


        /// <summary>
        /// Retorna dominios de la bd o cacheados
        /// </summary>
        /// <returns></returns>
        public static List<DomainsBE> RetriveDommains()
        {
            //si esta nulo o expiro refresca 
            if (Common.Domains == null || Common.Expired_ttl())
            {
                Common.Domains = RetriveDominiosFromDB();
            }

            return Common.Domains;
        }

        public static List<DomainsBE> RetriveDominiosFromDB()
        {
            DomainsBE item;
            List<DomainsBE> list = new List<DomainsBE>();
            var connectionString = Common.GetCnn(Common.CnnStringNameMeucci).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT d.DOM_NOMBRE,d.dom_id FROM dbo.dominio d WHERE d.anulado = 0 AND d.pai_id = 1", cnn)
            { CommandType = System.Data.CommandType.Text })
            {
                cnn.Open();



                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item = new DomainsBE();


                        //cue_nombre sar_nombre car_nombre usuariowindows dominio
                        item.DomainId = Convert.ToInt32(reader["dom_id"]);
                        item.Domain = reader["DOM_NOMBRE"].ToString();
                        list.Add(item);
                    }
                }

                return list;

            }


        }


        public static void ReseteoWeb_Log(int emp_id,string userName, int domainId, int resetUserId, string ticket ,string accion, string host)
        {
            if (string.IsNullOrEmpty(ticket))
            {
                ticket = "";
            }
            var connectionString = Common.GetCnn(Common.CnnStringNameMeucci).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.usp_ReseteoWeb_Log_i", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@usuariowindows", userName);
                cmd.Parameters.AddWithValue("@dom_id", domainId);
                
                cmd.Parameters.AddWithValue("@ticket", ticket);
                cmd.Parameters.AddWithValue("@user", resetUserId);
                if (String.IsNullOrEmpty(host))
                    host = "0";
                
                cmd.Parameters.AddWithValue("@host", host);

                cmd.Parameters.AddWithValue("@accion", accion);

                cmd.ExecuteNonQuery();

            }

        }

        public static void ReseteoWeb_EnviosMails(int emp_id, string userName, int domainId, int resetUserId, string accion, string host)
        {

            var connectionString = Common.GetCnn(Common.CnnStringNameMeucci).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.usp_ReseteoWeb_EnviosMails_i", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
    //            CREATE PROCEDURE[dbo].[usp_ReseteoWeb_EnviosMails_i]
    //    @emp_id int,  
    //@usuariowindows varchar(20),  
    //@dom_id int,  
    //@accion char (1),  
    //@user int,  
    //@host nvarchar(15)

                cmd.Parameters.AddWithValue("@emp_id", emp_id);
                cmd.Parameters.AddWithValue("@usuariowindows", userName);
                cmd.Parameters.AddWithValue("@dom_id", domainId);
                cmd.Parameters.AddWithValue("@user", resetUserId);
                cmd.Parameters.AddWithValue("@host", host);
                cmd.Parameters.AddWithValue("@accion", accion);
                cmd.ExecuteNonQuery();

            }

        }
        

    }
}