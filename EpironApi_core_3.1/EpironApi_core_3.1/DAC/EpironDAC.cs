using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using epironApi.webApi.Models;
using epironApi.webApi.common;
using epironApi.webApi.models;
using EpironApi.webApi.BE;

namespace epironApi.webApi.DAC
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
        /// <param name="caseId"></param>
        public static void Bot_update_delivery(Guid caseCommentGUID, int caseId)
        {
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[MessageBot_u_BotCreatedDateDelivery]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@CaseCommentGUID", caseCommentGUID);
                cmd.Parameters.AddWithValue("@CaseId", caseId);
                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        public static int MessageBot_u_BotCreatedDateObtained(EnqueueCommentBotReq req)
        {
            int modifiedRecords = 0;
            var json = Fwk.HelperFunctions.SerializationFunctions.SerializeObjectToJson<EnqueueCommentBotReq>(req);
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            try
            {
                using (SqlConnection cnn = new SqlConnection(connectionString))
                using (SqlCommand cmd = new SqlCommand("[Api].[MessageBot_u_BotCreatedDateObtained]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
                {
                    cnn.Open();
                    SqlParameter outParam = new SqlParameter("@modifiedRecords", SqlDbType.Int);
                    outParam.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(outParam);
                    cmd.Parameters.AddWithValue("@CaseCommentGUID", req.CaseCommentGUID);
                    cmd.Parameters.AddWithValue("@CaseId", req.CaseId);
                    cmd.Parameters.AddWithValue("@BotJson", json);
                    cmd.ExecuteNonQuery();

                    modifiedRecords = (System.Int32)outParam.Value;
                }
            }
            catch (Exception ex)
            {
                throw ex;
                }
            return modifiedRecords;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        //public static void Bot_insert_sendStatus(EnqueueCommentBotReq req)
        //{
        //    var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
        //    using (SqlConnection cnn = new SqlConnection(connectionString))
        //    using (SqlCommand cmd = new SqlCommand("dbo.MessageBot_i", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
        //    {
        //        cnn.Open();

        //        cmd.Parameters.AddWithValue("@CaseCommentTextSent", req.CaseCommentTextSent);
        //        cmd.Parameters.AddWithValue("@AccountUnique ", req.AccountUnique);
        //        cmd.Parameters.AddWithValue("@CaseId", req.CaseId);
        //        cmd.Parameters.AddWithValue("@CaseCommentGUID", req.CaseCommentGUID);
        //        cmd.Parameters.AddWithValue("@CreationDateLog", DateTime.Now);
        //        cmd.Parameters.AddWithValue("@SCInternalCode", req.SCInternalCode);
        //        cmd.Parameters.AddWithValue("@ElementTypePublic", req.ElementTypePublic);
                


        //        cmd.ExecuteNonQuery();
        //    }
        //}

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




        #region Luego de recibido la respuesta desde bot

        /// <summary>
        /// Se actualiza el registro con la respuesta enviada desde la APi Bot
        /// </summary>
        /// <param name="caseCommentGUID"></param>
        /// <param name="caseId"></param>
        /// <param name="action"></param>
        /// <param name="caseCommentTextReceived"></param>
        public static void Bot_update_response(Guid caseCommentGUID, int caseId, int action, string caseCommentTextReceived)
        {
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[MessageBot_u_Response]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@CaseCommentGUID", caseCommentGUID);
                cmd.Parameters.AddWithValue("@CaseCommentTextToPublication", caseCommentTextReceived);
                cmd.Parameters.AddWithValue("@ActionInternalCode", action);
                cmd.Parameters.AddWithValue("@CaseId", caseId);
                cmd.ExecuteNonQuery();
            }
        }



        /// <summary>
        /// Obtener tipo de elemento de salida:
        /// </summary>
        /// <param name="caseCommentGUID"> identificador del comentario entrante. </param>
        /// <param name="caseId">identificador del caso.  </param>
        public static List<AccountOutputChannelTypeBE> ElementTypeOutput(Guid caseCommentGUID, int caseId )
        {
            AccountOutputChannelTypeBE item;
            List<AccountOutputChannelTypeBE> list = new List<AccountOutputChannelTypeBE>();
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[ElementTypeOutput_s_ByParams]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@CaseCommentGUID", caseCommentGUID);

                cmd.Parameters.AddWithValue("@CaseId", caseId);
                using (IDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {

                        item = new AccountOutputChannelTypeBE();

                        
                        item.ElementTypeId = Convert.ToInt32(reader["ElementTypeId"]);
                        item.InputPublic = Convert.ToBoolean(reader["InputPublic"]);
                        item.ElementTypeOutputId = Convert.ToInt32(reader["ElementTypeOutputId"]);
                        item.OutputPublic = Convert.ToBoolean(reader["OutputPublic"]);
                        item.PChannelType = Convert.ToInt32(reader["PChannelType"]);

                        list.Add(item);

                    }

                    return list;

                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseCommentGUID"></param>
        /// <returns></returns>
        public static CaseCommentBE CaseComment(Guid caseCommentGUID)
        {
            CaseCommentBE item = null;
            //List<AccountOutputChannelTypeBE> list = new List<AccountOutputChannelTypeBE>();
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[CaseComment_s_Publication]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@CaseCommentGUID", caseCommentGUID);
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item = new CaseCommentBE();
                        item.AccountId = Convert.ToInt32(reader["AccountId"]);
                        item.CaseCommentPublicationTo = reader["CaseCommentPublicationTo"].ToString();
                        item.CaseCommentText = reader["CaseCommentText"].ToString();
                        item.UCUserName = reader["UCUserName"].ToString();

                        item.UserChannelId = Convert.ToInt32(reader["UserChannelId"]);
                        
                        

                        if(reader["AttentionQueueId"] != DBNull.Value)
                            item.AttentionQueueId = Convert.ToInt32(reader["AttentionQueueId"]);
                        if (reader["AccountDetailUniqueOutput"] != DBNull.Value)
                            item.AccountDetailUniqueOutput = Guid.Parse(reader["AccountDetailUniqueOutput"].ToString());

                        if (reader["AccountDetailIdOutput"] != DBNull.Value)
                            item.AccountDetailIdOutput = Convert.ToInt32(reader["AccountDetailIdOutput"]);

                        if (reader["ElementTypeId"] != DBNull.Value)
                            item.ElementTypeId = Convert.ToInt32(reader["ElementTypeId"]);
                        item.UCPublicationTo = reader["UCPublicationTo"].ToString();
                        item.CaseCommentText = reader["CaseCommentText"].ToString();
                        item.AccountEmailBodyTemplate = reader["AccountEmailBodyTemplate"].ToString();
                        item.Subject = reader["Subject"].ToString();

                    }

                    return item;

                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseComment"></param>
        /// <returns></returns>
        public static void Insert_CaseComment(CaseCommentCreationBE caseComment)
        {
            
            //List<AccountOutputChannelTypeBE> list = new List<AccountOutputChannelTypeBE>();
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[CaseComment_i]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                //output @CaseCommentId
                SqlParameter param = new SqlParameter("@CaseCommentId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                param = new SqlParameter("@CaseCommentGUID", SqlDbType.UniqueIdentifier);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                
                //compos que agrega flor por chat 11/08/2020 12:16
                cmd.Parameters.AddWithValue("@CaseCommentAutomatic", 1);
                cmd.Parameters.AddWithValue("@ActionTypeCode", "A0000105");

                cmd.Parameters.AddWithValue("@CaseId", caseComment.CaseId);
                cmd.Parameters.AddWithValue("@CaseCommentText", caseComment.CaseCommentText);

                if(!string.IsNullOrEmpty(caseComment.CaseCommentPublicationTo))
                    cmd.Parameters.AddWithValue("@CaseCommentPublicationTo", caseComment.CaseCommentPublicationTo);

                
               cmd.Parameters.AddWithValue("@AccountDetailId", caseComment.AccountDetailId);
                cmd.Parameters.AddWithValue("@UserChannelId", caseComment.UserChannelId);


                // @CaseConfigurationId: NULL
                //@ProcessDetailsId: NULL
                //@ElementId: NULL
                //cmd.Parameters.AddWithValue("@CaseConfigurationId", caseComment.CaseConfigurationId);
                //cmd.Parameters.AddWithValue("@ProcessDetailsId", caseComment.ProcessDetailsId);
                //cmd.Parameters.AddWithValue("@ElementId", caseComment.ElementId);


                cmd.Parameters.AddWithValue("@ElementTypeId", caseComment.ElementTypeId);

            

                if (caseComment.CaseCommentModifiedByUserId.HasValue)
                    cmd.Parameters.AddWithValue("@CaseCommentModifiedByUserId", caseComment.CaseCommentModifiedByUserId);

                //@StateValidationId: Identificador del estado de validación.Se pasa NULL.
                
                if (caseComment.AttentionQueueId.HasValue)
                    cmd.Parameters.AddWithValue("@AttentionQueueId", caseComment.AttentionQueueId);


                //ReplyToCaseCommentId = null
                cmd.ExecuteNonQuery();
                caseComment.CaseCommentId = (int)cmd.Parameters["@CaseCommentId"].Value;
                caseComment.CaseCommentGUID = Guid.Parse(cmd.Parameters["@CaseCommentGUID"].Value.ToString());

            }
        }


        /// <summary>
        /// solo  PChannelType=315
        /// </summary>
        /// <param name="detailCaseCommentBE"></param>
        public static void Insert_DetailCaseComment(DetailCaseCommentBE detailCaseCommentBE)
        {
            
            //List<AccountOutputChannelTypeBE> list = new List<AccountOutputChannelTypeBE>();
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[DetailCaseComment_i]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();

                cmd.Parameters.AddWithValue("@CaseCommentId", detailCaseCommentBE.CaseCommentId);
                cmd.Parameters.AddWithValue("@DCCSubject", detailCaseCommentBE.DCCSubject);
                cmd.Parameters.AddWithValue("@DCCTextEmailBody", detailCaseCommentBE.DCCTextEmailBody);
                cmd.Parameters.AddWithValue("@DCCPermlinkRoot", detailCaseCommentBE.DCCPermlinkRoot);
                cmd.Parameters.AddWithValue("@DCCDescription", detailCaseCommentBE.DCCDescription);
                cmd.ExecuteNonQuery();

            }
        }

        /// <summary>
        /// solo  PChannelType=315
        /// </summary>
        /// <param name="publication"></param>
        public static void Insert_Publication(PublicationBE publication)
        {

            //List<AccountOutputChannelTypeBE> list = new List<AccountOutputChannelTypeBE>();
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[Publication_i]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                //output @CaseCommentId
                SqlParameter param = new SqlParameter("@PublicationId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);


                cmd.Parameters.AddWithValue("@PComment", publication.PComment);
                cmd.Parameters.AddWithValue("@PublicationTo", publication.PublicationTo);
                cmd.Parameters.AddWithValue("@AccountDetailUnique", publication.AccountDetailUnique);

                cmd.Parameters.AddWithValue("@PublicationDate", null);

                cmd.Parameters.AddWithValue("@PChannelType", publication.PChannelType);

                cmd.Parameters.AddWithValue("@PublicationErrorId", null);
                cmd.Parameters.AddWithValue("@PRetriesQuantity", 0);

                
                cmd.Parameters.AddWithValue("@PModifiedByUserId", publication.PModifiedByUserId);

                cmd.Parameters.AddWithValue("@ProcessDetailId", null);

                cmd.Parameters.AddWithValue("@PActiveFlag", 1);

                cmd.Parameters.AddWithValue("@PAwaitingUserConfirmation", 0);


                //if (!string.IsNullOrEmpty(detailCaseCommentBE.CaseCommentPublicationTo))
                cmd.Parameters.AddWithValue("@PSourcePublicationId", null);
                cmd.Parameters.AddWithValue("@CaseCommentGUID", publication.CaseCommentGUID);
                
                cmd.ExecuteNonQuery();
            
                publication.PublicationId = (int)cmd.Parameters["@PublicationId"].Value;

            }
        }


        /// <summary>
        /// calls Publication_i
        /// </summary>
        /// <param name="publicationDetail"></param>
        public static void Insert_PublicationDetail(PublicationDetailBE publicationDetail)
        {

            //List<AccountOutputChannelTypeBE> list = new List<AccountOutputChannelTypeBE>();
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[Publication_i]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
               

                cmd.Parameters.AddWithValue("@PublicationId", publicationDetail.PublicationId);
                cmd.Parameters.AddWithValue("@PDCommentAux1", publicationDetail.PDCommentAux1);
                
                cmd.ExecuteNonQuery();


            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public static void Realease_CaseComment(int caseId)
        {
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[Realease_u_ByCaseId]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                SqlParameter param = new SqlParameter("@CaseLogId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                cmd.Parameters.AddWithValue("@CaseId", caseId);
                
                cmd.ExecuteNonQuery();
              }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="caseId"></param>
        public static void Close_CaseComment(int caseId)
        {

            
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[Close_u_ByCaseId]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                SqlParameter param = new SqlParameter("@CaseLogId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(param);
                cmd.Parameters.AddWithValue("@CaseId", caseId);

                cmd.ExecuteNonQuery();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="settingId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public static ApplicationSettingsBE ApplicationSettings(int  settingId, int? accountId)
        {
            ApplicationSettingsBE item=null;
            
            var connectionString = Common.GetCnn(Common.CnnStringNameepiron).ConnectionString;
            using (SqlConnection cnn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand("[Api].[ApplicationSettings_s_BySettingId]", cnn) { CommandType = System.Data.CommandType.StoredProcedure })
            {
                cnn.Open();
                cmd.Parameters.AddWithValue("@SettingId", settingId);
                cmd.Parameters.AddWithValue("@AccountId", accountId);

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        item = new ApplicationSettingsBE();

                        item.SettingId = Convert.ToInt32(reader["SettingId"]);
                        item.Value = reader["Value"].ToString();
                        item.Description = reader["Description"].ToString();
    
                    }

                    return item;

                }
            }
        }

        #endregion

    }
}