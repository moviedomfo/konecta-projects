﻿using EpironAPI.BE;
using EpironAPI.classes;
using EpironAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace EpironAPI.Controllers
{
    public class SecurityController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }




        public void ValidarAplicacion(ValidarAplicacionReq req)
        {
        }

        [Route("execute")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage UserAutenticacion(UserAutenticacionReq req)
        {
            Error errorResponse;
            LoginResponseBE loginResponseBE = new LoginResponseBE();

            var jsonReq = Fwk.HelperFunctions.SerializationFunctions.SerializeObjectToJson_Newtonsoft(req);
            int reintentos, idType;
            try
            {
                Guid guidSession = Guid.NewGuid();
                if (req.AppInstanceGUID == Guid.Empty | req.LoginHost == string.Empty | req.LoginIP == string.Empty | req.guidintercambio == Guid.Empty | req.AutTypeGUID == Guid.Empty | req.UserName == string.Empty | req.UserKey == string.Empty)
                {
                    //Error
                    errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(2);
                    return apiHelper.fromObject<ApiOkResponse>(new ApiOkResponse(errorResponse));

                }

                var dtTag = AccesoDatos.Event_s_ByTag(req.Event_Tag);
                int eventIDUserAutenticacion = dtTag.EventId; //<--- EventId para USER-AUTENTIC
                DateTime auditTrailLoginEndDateUserAuthentic = DateTime.Now.AddSeconds(dtTag.EventDurationTime);

                //Pregunto si existen los datos del evento
                if (dtTag != null)
                {

                    //Consulto si el guid de negociacion es valido
                    var dtValido = AccesoDatos.AuditTrailLogin_s_ByAuditTrailLoginGUID_Valid(req.AppInstanceGUID, req.guidintercambio);
                    reintentos = CalcularReintentos(req.Event_Tag, req.AppInstanceGUID, req.guidintercambio, dtValido);
                    //si el dtvalido tiene mas de una columna es valido, si tiene solo una columna significa que me devuelve un codigo de error
                    if (dtValido.Rows.Count > 1)//No hay error
                    {

                        if ((reintentos != 999) && (reintentos != 22))
                        {
                            //Verificar que el usuario sea valido
                            UserBE dtUser = AccesoDatos.User_s_ByUserName_Valid(req.UserName);

                            if (dtUser != null)
                            {
                                //idUser = Convert.ToInt32(dtUser.Rows[0][0].ToString());

                                var dtType = AccesoDatos.AuthenticationType_s_ByGUID(req.AutTypeGUID);

                                //Pregunto si la consulta me devuelve resultados para el tipo de autenticacion seleccionado
                                //va a depender entre otras cosas del activeflag
                                if (dtType != null)
                                {
                                    ////AuthenticationTypeTag = dtType.Rows[0][8].ToString();

                                    ///idType = int.Parse(dtType.Rows[0][0].ToString());

                                    var dtEvent = AccesoDatos.Event_s_ByTag("USER-OK");
                                    //Da
                                    if (dtEvent != null)
                                    {

                                        int eventIDUserOk = Convert.ToInt32(dtEvent.EventId); //<---obtengo el id para el evento USER-OK
                                        //a la fecha y hora actual se le suman los segundos definidos para el evento req.Event_Tag
                                        DateTime auditTrailLoginEndDate = DateTime.Now.AddSeconds(Convert.ToInt32(dtEvent.EventDurationTime));
                                        bool mustChangePassword;
                                        //En caso de que no sea autenticacion de AD
                                        if (req.DomainGUID == Guid.Empty)
                                        {
                                            //Consulta si el usuario tiene acceso a la instncia de la app
                                            //con el tipo de autenticacion seleccionado
                                            var dtInstanceUser = AccesoDatos.AuthenticationTypeApplicationInstanceUser_s_Valid(req.UserName, req.AppInstanceGUID, req.AutTypeGUID);

                                            if (dtInstanceUser != null)
                                            {
                                                //validar previamente si el usuario para el tipo de autenticacion tiene asignado la validacion
                                                //mediante membership
                                                var dtMembership = AccesoDatos.AuthenticationTypeUser_s_MembershipUserID(dtUser.UserId, dtType.AuthenticationTypeId);
                                                //Valido si el usuario tiene activo la opcion de validar mediante membership
                                                if (dtMembership != null)
                                                {
                                                    //Valida si el usuario existe en la aplicación mediante Membership
                                                    if (Membership.ValidateUser(req.UserName, req.UserKey))
                                                    {
                                                        //DataSet dsLogDevolucion = new DataSet();
                                                        //dsLogDevolucion.Tables.Add(Log(Guid.Empty, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq).Copy());

                                                        ////Validaciones objValidaciones = new Validaciones();
                                                        ////string eventResponseText = objValidaciones.ToStringAsXml(dsLogDevolucion);
                                                        ////UpdateResponse(dsLogDevolucion.Tables[0], eventResponseText);

                                                        //Guid GuidIntercambio = (Guid)dsLogDevolucion.Tables[0].Rows[0][0];

                                                        //Transacción!!
                                                        var dtSession = AccesoDatos.TransaccionSession(eventIDUserAutenticacion, dtUser.UserId, dtType.AuthenticationTypeId, string.Empty, auditTrailLoginEndDate, 0, req.guidintercambio, req.AppInstanceGUID, req.LoginHost, req.LoginIP, string.Empty, jsonReq, eventIDUserOk, auditTrailLoginEndDateUserAuthentic);
                                                        guidSession = dtSession.GUID;
                                                        mustChangePassword = dtInstanceUser.AuthenticationTypeUserMustChangePassword;

                                                        XMLResponse = ResponseUserAuthenticXML(mustChangePassword, guidSession);
                                                        UpdateResponseSession(dtSession, XMLResponse);
                                                        ///dtSession.Columns.Remove("AuditTrailLoginId");

                                                        ///dtSession.Columns.Add("UserGuid");
                                                        loginResponseBE.UserGuid = dtInstanceUser.UserGUID;

                                                        ////Se agregan los datos de la persona y el usuario
                                                        ////dtSession.Columns.Add("UserName");
                                                        loginResponseBE.UserName = dtUser.UserName;

                                                        ///dtSession.Columns.Add("PersonFirstName");
                                                        loginResponseBE.PersonFirstName = dtUser.PersonFirstName;

                                                        ///dtSession.Columns.Add("PersonLastName");
                                                        loginResponseBE.PersonLastName = dtUser.PersonLastName;

                                                        ////dtSession.Columns.Add("PersonDocNumber");
                                                        ////dtSession.Rows[0]["PersonDocNumber"] = dtUser.PersonDocNumber;
                                                        loginResponseBE.PersonDocNumber = dtUser.PersonDocNumber;


                                                        ////dtSession.Columns.Add("UserPlaceGUID");
                                                        ////dtSession.Rows[0]["UserPlaceGUID"] = dtUser.UserPlaceGuid;
                                                        loginResponseBE.UserPlaceGuid = dtUser.UserPlaceGuid;

                                                        ////dtSession.Columns.Add("UserPlaceName");
                                                        ////dtSession.Rows[0]["UserPlaceName"] = dtUser.UserPlaceName;
                                                        loginResponseBE.UserPlaceName = dtUser.UserPlaceName;

                                                        //dtSession.Columns.Add("UserPlaceDescript");
                                                        loginResponseBE.UserPlaceDescript = dtUser.UserPlaceDescript;

                                                        ////if (dtUser.Rows[0]["UserPlaceDescript"] != DBNull.Value)
                                                        ////    dtSession.Rows[0]["UserPlaceDescript"] = dtUser.Rows[0]["UserPlaceDescript"].ToString();

                                                        ////dtSession.Columns.Add("PersonGUID");
                                                        ////dtSession.Rows[0]["PersonGUID"] = dtUser.Rows[0]["PersonGUID"].ToString();
                                                        loginResponseBE.PersonGUID = dtUser.PersonGUID;

                                                        ////dtSession.Columns.Add("PersonModifiedDate");
                                                        ////dtSession.Rows[0]["PersonModifiedDate"] = ((DateTime)dtUser.Rows[0]["PersonModifiedDate"]).ToString("o");

                                                        loginResponseBE.PersonModifiedDate = dtUser.PersonModifiedDate;
                                                        /////////////////////////////////////////////

                                                        ///dsSession.Tables.Add(dtSession);

                                                        //GuidIntercambio = new Guid(dtSession.Rows[0][0].ToString());
                                                    }
                                                    else
                                                    {
                                                        //row = dtCampos.NewRow();
                                                        errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(15);
                                                        errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);
                                                        //row["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                                        //row["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                        //row["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                                        //row["Guid"] = dtLog.Rows[0][0].ToString();

                                                        dtCampos.Rows.Add(row);
                                                        XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                                        UpdateResponse(dtLog, XMLResponse);

                                                        dsSession.Tables.Add(dtCampos);

                                                    }
                                                }
                                                else
                                                {
                                                    //Usuario no posee relacion con estructura de MemberShip (Autenticacion Propia)
                                                    //row = dtCampos.NewRow();
                                                    errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(42);
                                                    errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);
                                                    //row["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                                    //row["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                    //row["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                                    //row["Guid"] = dtLog.Rows[0][0].ToString();

                                                    ////dtCampos.Rows.Add(row);
                                                    ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                                    ////UpdateResponse(dtLog, XMLResponse);
                                                    ////dsSession.Tables.Add(dtCampos);
                                                    //GuidIntercambio = new Guid(dtLog.Rows[0][0].ToString());
                                                }
                                            }
                                            else
                                            {
                                                //El usuario no posee acceso a la instancia de aplicación
                                                //con el tipo de autenticacion seleccionado   7 (si NO ES Windows)
                                                ///row = dtCampos.NewRow();
                                                errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(7);
                                                errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);
                                                ////row["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                                ////row["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                ////row["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                                ////row["Guid"] = dtLog.Rows[0][0].ToString();

                                                ////dtCampos.Rows.Add(row);
                                                ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                                ////UpdateResponse(dtLog, XMLResponse);
                                                ////dsSession.Tables.Add(dtCampos);
                                                //GuidIntercambio = new Guid(dtLog.Rows[0][0].ToString());

                                            }
                                        }
                                        else //Si autentica por Active Directory
                                        {
                                            //Consulta si el dominio de Active Directory existe
                                            DomainBE dtDominio = AccesoDatos.Domain_s_ByGUID(req.DomainGUID);

                                            if (dtDominio != null)
                                            {
                                                //Verifica si el usuario tiene acceso a la instancia de la app 
                                                //con el tipo de autenticacion seleccionado
                                                //y dominio seleccionado
                                                var dtDominioUser = AccesoDatos.AuthenticationTypeApplicationInstanceUser_s_Valid_Domain(req.UserName, req.AppInstanceGUID, req.AutTypeGUID, req.DomainGUID);
                                                if (dtDominioUser != null)
                                                {
                                                    ActiveDirectory objAD = new ActiveDirectory();

                                                    //Pregunto si se puede conectar al controlador de dominio
                                                    if (objAD.IsAuthenticated(dtDominio.LDAPPath, dtDominio.DomainUsr.ToString(), Encriptador.desencriptar(dtDominio.DomainPwd.ToString())) == true)
                                                    {
                                                        //Pregunto si el usuario tiene las credenciales necesarias en active directory para dicho dominio
                                                        if (objAD.IsAuthenticated(dtDominio.LDAPPath, req.UserName, req.UserKey) == true)
                                                        {

                                                            //DataSet dsLogDevolucion = new DataSet();
                                                            //dsLogDevolucion.Tables.Add(Log(Guid.Empty, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq).Copy());

                                                            ////Validaciones objValidaciones = new Validaciones();
                                                            ////string eventResponseText = objValidaciones.ToStringAsXml(dsLogDevolucion);
                                                            ////UpdateResponse(dsLogDevolucion.Tables[0], eventResponseText);

                                                            //Guid GuidIntercambio = (Guid)dsLogDevolucion.Tables[0].Rows[0][0];

                                                            //Transacción!!}
                                                            ///DataTable dtTransaccion = new DataTable();
                                                            var dtTransaccion = AccesoDatos.TransaccionSession(eventIDUserAutenticacion, dtUser.UserId, dtType.AuthenticationTypeId, string.Empty, auditTrailLoginEndDate, dtDominio.DomainId, req.guidintercambio, req.AppInstanceGUID, req.LoginHost, req.LoginIP, string.Empty, jsonReq, eventIDUserOk, req.AuditTrailLoginEndDateUserAuthentic);
                                                            ///guidSession = dtTransaccion.GUID;
                                                            ///mustChangePassword = bool.Parse(dtDominioUser.Rows[0][1].ToString());
                                                            XMLResponse = ResponseUserAuthenticXML(dtDominioUser.AuthenticationTypeUserMustChangePassword, dtTransaccion.GUID);
                                                            UpdateResponseSession(dtTransaccion, XMLResponse);

                                                            ///dtTransaccion.Columns.Remove("AuditTrailLoginId");

                                                            ///dtTransaccion.Columns.Add("UserGuid");
                                                            loginResponseBE.UserGuid = dtDominioUser.UserGUID;

                                                            //Se agregan los datos de la persona y el usuario.
                                                            //dtTransaccion.Columns.Add("UserName");
                                                            loginResponseBE.UserName = dtUser.UserName.ToString();

                                                            ////dtTransaccion.Columns.Add("PersonFirstName");
                                                            loginResponseBE.PersonFirstName = dtUser.PersonFirstName;

                                                            ////dtTransaccion.Columns.Add("PersonLastName");
                                                            loginResponseBE.PersonLastName = dtUser.PersonLastName;

                                                            ////dtTransaccion.Columns.Add("PersonDocNumber");
                                                            loginResponseBE.PersonDocNumber = dtUser.PersonDocNumber;
                                                            ////dtTransaccion.Columns.Add("UserPlaceGUID");
                                                            loginResponseBE.UserPlaceGuid = dtUser.UserPlaceGuid;

                                                            ////dtTransaccion.Columns.Add("UserPlaceName");
                                                            loginResponseBE.UserPlaceName = dtUser.UserPlaceName;

                                                            ///dtTransaccion.Columns.Add("UserPlaceDescript");
                                                            ////if (dtUser.Rows[0]["UserPlaceDescript"] != DBNull.Value)
                                                            loginResponseBE.UserPlaceDescript = dtUser.UserPlaceDescript;

                                                            ////dtTransaccion.Columns.Add("PersonGUID");
                                                            loginResponseBE.PersonGUID = dtUser.PersonGUID;


                                                            ///dtTr/*/*a*/*/nsaccion.Columns.Add("PersonModifiedDate");
                                                            loginResponseBE.PersonModifiedDate = dtUser.PersonModifiedDate;

                                                            ////dsSession.Tables.Add(dtTransaccion);
                                                            //GuidIntercambio = new Guid(dtTransaccion.Rows[0][0].ToString());
                                                        }
                                                        else
                                                        {
                                                            ///row = dtCampos.NewRow();
                                                            //Error 8
                                                            errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(8);
                                                            errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);
                                                            ////row["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                                            ////row["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                            ////row["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                                            ////row["Guid"] = dtLog.Rows[0][0].ToString();

                                                            ////dtCampos.Rows.Add(row);

                                                            ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                                            ////UpdateResponse(dtLog, XMLResponse);
                                                            ////dsSession.Tables.Add(dtCampos);
                                                            //GuidIntercambio = new Guid(dtLog.Rows[0][0].ToString());
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //Error 24 - EL usuario no pudo autenticarse en el controlador de active directory
                                                        ////row = dtCampos.NewRow();
                                                        errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(24);
                                                        errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);
                                                        ////row["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                                        ////row["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                        ////row["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                                        ////row["Guid"] = dtLog.Rows[0][0].ToString();

                                                        ////dtCampos.Rows.Add(row);
                                                        ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                                        ////UpdateResponse(dtLog, XMLResponse);
                                                        ////dsSession.Tables.Add(dtCampos);
                                                        //GuidIntercambio = new Guid(dtLog.Rows[0][0].ToString());
                                                    }

                                                }
                                                else
                                                {
                                                    //El usuario no posee acceso a la aplicación con el tipo de autenticación seleccionado
                                                    //en el dominio seleccionado (si es windows)
                                                    //Anexo 10 no devuelve resultados (AuthenticationTypeApplicationInstanceUser_s_Valid)
                                                    //21 (si ES Windows)
                                                    ////row = dtCampos.NewRow();
                                                    errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(21);
                                                    errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);
                                                    ////row["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                                    ////row["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                    ////row["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                                    ////row["Guid"] = dtLog.Rows[0][0].ToString();

                                                    ////dtCampos.Rows.Add(row);
                                                    ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                                    ////UpdateResponse(dtLog, XMLResponse);
                                                    ////dsSession.Tables.Add(dtCampos);
                                                    //GuidIntercambio = new Guid(dtLog.Rows[0][0].ToString());

                                                }

                                            }
                                            else
                                            {
                                                ////row = dtCampos.NewRow();
                                                //El dominio de AD no existe
                                                errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(23);
                                                errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);
                                                ////row["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                                ////row["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                ////row["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                                ////row["Guid"] = dtLog.Rows[0][0].ToString();

                                                ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                                ////UpdateResponse(dtLog, XMLResponse);
                                                ////dtCampos.Rows.Add(row);
                                                ////dsSession.Tables.Add(dtCampos);
                                                //GuidIntercambio = new Guid(dtLog.Rows[0][0].ToString());
                                            }
                                        }
                                    }

                                    else
                                    {
                                        //No se encuentran los datos del evento
                                        errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(22);
                                        ///dsSession.Tables.Add(dtError);
                                    }
                                }
                                else
                                {
                                    //No hoy datos del tipo de autenticacion

                                    ///row = dtCampos.NewRow();

                                    errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(41);
                                    errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);
                                    ////row["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                    ////row["EventResponseText"] = dtError.Rows[0][1].ToString();
                                    ////row["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                    ////row["Guid"] = dtLog.Rows[0][0].ToString();



                                    ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                    ////UpdateResponse(dtLog, XMLResponse);
                                    ////dtCampos.Rows.Add(row);
                                    ////dsSession.Tables.Add(dtCampos);
                                    //GuidIntercambio = new Guid(dtLog.Rows[0][0].ToString());
                                }

                            }
                            else
                            {
                                //DataRow drow;
                                ///drow = dtCampos.NewRow();
                                //El usuario no es válido
                                errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(6);
                                errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);

                                ////drow["EventResponseId"] = Convert.ToInt32(dtError.Rows[0][0].ToString());
                                ////drow["EventResponseText"] = dtError.Rows[0][1].ToString();
                                ////drow["EventResponseInternalCode"] = Convert.ToInt32(dtError.Rows[0][2].ToString());
                                ////drow["Guid"] = dtLog.Rows[0][0].ToString();

                                ///dtCampos.Rows.Add(drow);

                                //Actualizo el campo Response de AuditTrailLogin
                                ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                                ////UpdateResponse(dtLog, XMLResponse);

                                ////dsSession.Tables.Add(dtCampos);
                                //GuidIntercambio = new Guid(dtLog.Rows[0][0].ToString());
                            }
                        }
                        if (reintentos == 999)
                        {
                            //Supero la cantidad de reintentos, loguear..
                            //El sistema detecta que se alcanzó el tope de reintentos
                            errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(18);
                            errorResponse.Guid = Log(req.guidintercambio, req.AppInstanceGUID, req.Event_Tag, req.LoginHost, req.LoginIP, jsonReq);

                            ////dsSession.Tables.Add(dtError);
                            ////XMLResponse = ResponseErrorXML(dtError.Rows[0][1].ToString(), Guid.Parse(dtLog.Rows[0][0].ToString()), "AuditTrailLoguinGUID");
                            ////UpdateResponse(dtLog, XMLResponse);

                        }
                        if (reintentos == 22)
                        {
                            //No se encuentran los datos del evento
                            errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(22);
                            ////dsSession.Tables.Add(dtError);
                        }

                    }
                    else //Error
                    {
                        errorResponse = new Error();
                        int codigoError = int.Parse(dtValido.Rows[0][0].ToString());
                        //reintentos = CalcularReintentos(req.Event_Tag, req.AppInstanceGUID, req.guidintercambio, dtValido);
                    }
                }
                else //Error
                {

                    errorResponse = AccesoDatos.EventResponse_s_ByInternalCode(22);
                    return apiHelper.fromObject<ApiOkResponse>(new ApiOkResponse(errorResponse));

                }


                return apiHelper.fromObject<ApiOkResponse>(new ApiOkResponse(loginResponseBE));
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }

        public int CalcularReintentos(string Event_Tag, Guid AppInstanceGUID, Guid RequestLoginGUID, DataTable dtValido)
        {
            int contador;//, reintentosEvento;
            //DataTable dtEvent, dtEventoFiltrado = new DataTable();
            //AccesoDatos AccesoDatos = new AccesoDatos();
            //int EventId;

            ////Obtengo los datos del evento
            //dtEvent = AccesoDatos.Event_s_ByTag(req.Event_Tag);
            //if (dtEvent.Rows.Count > 0)
            //{
            //    EventId = Convert.ToInt32(dtEvent.Rows[0][0].ToString());

            //    DataView dvValido = dtValido.DefaultView;

            //    dvValido.RowFilter = "EventId=" + EventId;
            //    dtEventoFiltrado = dvValido.ToTable();

            //    //Pregunto si tiene mas de un registro
            //    if (dtEventoFiltrado.Rows.Count > 0)
            //    {
            //        contador = dtEventoFiltrado.Rows.Count;

            //        reintentosEvento = Convert.ToInt32(dtEvent.Rows[0][10].ToString());
            //        //Consultar si el total de intentos es el que esta definido en la tabla Event
            //        if (contador < reintentosEvento)
            //        {
            //            contador += 1;
            //        }
            //        else
            //        {
            //            contador = 999;
            //        }
            //    }
            //    else
            //    {
            //        contador = 0;
            //    }
            //}
            //else
            //{
            //    //EventResponseInternalCode = El evento no existe
            //    contador = 22;
            //}
            contador = 0;
            return contador;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditTrailLoginParentGUID"></param>
        /// <param name="auditTrailLoginAppInstanceGUID"></param>
        /// <param name="event_Tag"></param>
        /// <param name="auditTrailLoginHost"></param>
        /// <param name="auditTrailLoginIP"></param>
        /// <param name="auditTrailLoginRequest"></param>
        /// <returns>string con el guid </returns>
        public Guid Log(Guid auditTrailLoginParentGUID, Guid auditTrailLoginAppInstanceGUID, string event_Tag, string auditTrailLoginHost, string auditTrailLoginIP, string auditTrailLoginRequest)
        {
            
            var dtEvent = AccesoDatos.Event_s_ByTag(event_Tag);
            //a la fecha y hora actual se le suman los segundos definidos para el evento req.Event_Tag
            DateTime auditTrailLoginEndDate = DateTime.Now.AddSeconds(dtEvent.EventDurationTime);

            var dtLog = AccesoDatos.AuditTrailLogin_i(auditTrailLoginParentGUID, auditTrailLoginAppInstanceGUID, dtEvent.EventId, auditTrailLoginEndDate, auditTrailLoginHost, auditTrailLoginIP, "", auditTrailLoginRequest);
            return dtLog.GUID;
        }
    }
}
