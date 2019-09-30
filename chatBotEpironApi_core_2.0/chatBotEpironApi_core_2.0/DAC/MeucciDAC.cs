using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using chatBotEpironApi.webApi.Models;
using chatBotEpironApi.webApi.common;
using chatBotEpironApi.webApi.models;

namespace chatBotEpironApi.webApi.DAC
{
    public class EpironDAC
    {

       

        /// <summary>
        /// verificará que el usuario esté habilitado para resetear o desbloquear un UW y que no registre un ausentismo para ese día.
        /// </summary>
        /// <param name="userName"></param>
        public static UserAPiBE VirifyUser(string userName)
        {
            UserAPiBE item = null;
            var connectionString =  Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.usp_ReseteoWebVerificarUsuario", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                /// FACTURA_NRO
                cmd.Parameters.AddWithValue("@UW", userName);
                //cmd.Parameters.AddWithValue("@dom_id", domainId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item = new UserAPiBE();
                        
                        item.Legajo = Convert.ToInt32(reader["Legajo"]);
                        item.CAIS = Convert.ToBoolean(reader["CAIS"]);
                        item.Cuenta = reader["Cuenta"].ToString();
                        item.Cargo = reader["Cargo"].ToString();
                        item.Emp_id = Convert.ToInt32(reader["Legajo"]);
                        //item.DomainId = domainId;
                        item.WindowsUser = userName;
                    }
                }

                return item;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static BotBE GetBot(int id)
        {
            BotBE item = null;
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.botApi_g", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                
                cmd.Parameters.AddWithValue("@id", id);
                //cmd.Parameters.AddWithValue("@dom_id", domainId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item = new BotBE();

                        item.AccountUnique = Guid.Parse(reader["BotAccountUnique"].ToString());
                        item.CaseId = Convert.ToInt32(reader["BotCaseId"]);
                        item.CaseCommentTextSent = reader["BotCaseCommentTextSent"].ToString();
                        item.AccountUnique = Guid.Parse(reader["BotAccountUnique"].ToString());
                        item.CaseCommentGUID = Guid.Parse(reader["BotCaseCommentGUID"].ToString());
                        item.SCInternalCode = Convert.ToInt32(reader["BotSCInternalCode"]); 
                        item.ElementTypePublic = Convert.ToBoolean(reader["BotElementTypePublic"]);

                        item.Action = reader["BotAction"].ToString();
                        item.Status = reader["BotStatus"].ToString();

                        item.CaseCommentTextReceived = reader["BotCaseCommentTextReceived"].ToString();
                        
                    }
                }

                return item;

            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseCommentGUID"></param>
        /// <param name="action"></param>
        /// <param name="caseCommentTextReceived"></param>

        public static void Bot_update_response(Guid caseCommentGUID, string action,string caseCommentTextReceived)
        {
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.botApi_u_response", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@caseCommentGUID", caseCommentGUID);
                cmd.Parameters.AddWithValue("@action", action);
                cmd.Parameters.AddWithValue("@caseCommentTextReceived", caseCommentTextReceived);
             

                cmd.ExecuteNonQuery();
            }
        }

        public static void Bot_update_sendStatus(Guid caseCommentGUID, string status, string error)
        {
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.botApi_u_sendStatus", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@caseCommentGUID", caseCommentGUID);
                cmd.Parameters.AddWithValue("@Error", error);
                cmd.Parameters.AddWithValue("@status", status);

                cmd.ExecuteNonQuery();
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="domainName">Proviene de la tabla Domains URL y es el SiteDoamin</param>
        /// <returns></returns>
        //internal static int GetDimainId(string domainName)
        //{
        //    domainName = domainName.Replace("-", ".");
        //    var epironDomains = RetriveDommains();
        //    var d = epironDomains.Where(p => p.Domain.ToLower().Equals(domainName.ToLower())).FirstOrDefault();
        //    if (d == null)
        //        throw new Fwk.Exceptions.FunctionalException("No es posible encontrar informacion configurada sobre el dominio " + domainName.ToLower());
        //    return d.DomainId;

        //    //antes sacabamos del domain.json
        //    //var d = Common.Domains.Where(p => p.Domain.ToLower().Equals(domainName.ToLower())).FirstOrDefault();
        //    //if(d==null)
        //    //    throw new Fwk.Exceptions.FunctionalException("No es posible encontrar informacion configurada sobre el dominio " + domainName.ToLower());
        //    //return d.DomainId;
        //}

        //private static object RetriveDommains()
        //{
        //    throw new NotImplementedException();
        //}


        /// <summary>
        /// Buscar
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="domainId"></param>
        /// <param name="DNI"></param>
        public static UserAPiBE RetriveDatosUserAPi(string userName, int domainId, string DNI)
        {
            //return RetriveDatosReseteoEmpleados_mok();

            UserAPiBE item = null;

            
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("dbo.UserAPi", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@UW", userName);
                cmd.Parameters.AddWithValue("@dom_id", domainId);
                cmd.Parameters.AddWithValue("@DNI", DNI);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                   
                    while (reader.Read())
                    {

                        item = new UserAPiBE();

                        //cue_nombre sar_nombre car_nombre usuariowindows dominio
                        item.Emp_id = Convert.ToInt32(reader["Emp_id"]);
                        item.Cargo = reader["car_nombre"].ToString();
                        item.Cuenta = reader["cue_nombre"].ToString();
                        item.Subarea = reader["sar_nombre"].ToString();
                        item.ApeNom = reader["emp_apenom"].ToString();
                        if (reader["aus_id"] != DBNull.Value)
                            item.Aus_Id = Convert.ToInt32(reader["aus_id"]);

                 

                    }
                    
                  
                   
                }

                return item;

            }

        }


      



    }
}