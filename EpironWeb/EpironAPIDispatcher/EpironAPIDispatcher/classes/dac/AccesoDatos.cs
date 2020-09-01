using EpironAPI.BE;
using EpironAPI.classes.BE;
using EpironAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EpironAPI.classes
{
    public class AccesoDatos
    {



        /// <summary>
        /// Anexo 2 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Valida la instancia de la aplicación
        /// </summary>
        /// <param name="Par_AppInstanceGUID">GUID de la instancia de aplicación</param>
        /// <returns>
        /// Los datos de la instancia de la aplicación
        /// </returns>
        public static ApplicationInstanceBE ApplicationInstance_s_ByGUID_Valid(Guid Par_AppInstanceGUID)
        {
            ApplicationInstanceBE item = null;
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.ApplicationInstance_s_ByGUID_Valid");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_AppInstanceGUID);
            var dtt = MetaDatos.EjecutarComandoSelect(comando);
            if (dtt.Rows.Count > 0)
            {
                item = new ApplicationInstanceBE();
                item.ControlEntity = Convert.ToBoolean( dtt.Rows[0]["ControlEntity"]);
                item.ApplicationName = dtt.Rows[0]["ApplicationName"].ToString();
                item.ApplicationInstanceName = dtt.Rows[0]["ApplicationInstanceName"].ToString();

            }

            return item;
        }

        /// <summary>
        /// Anexo 3 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Busca datos del evento
        /// </summary>
        /// <param name="Par_Event_Tag">TAG del evento</param>
        /// <returns>
        /// Los datos del evento
        /// </returns>
        public static EventBE Event_s_ByTag(string Par_EventTag)
        {
            EventBE item = null;
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.Event_s_ByTag");
            comando.Parameters.AddWithValue("@EventTag", Par_EventTag);
            var dtt = MetaDatos.EjecutarComandoSelect(comando);

            if (dtt.Rows.Count > 0)
            {
                item = new EventBE();
                item.EventId = Convert.ToInt32(dtt.Rows[0]["EventId"].ToString());
                item.EventGUID = Guid.Parse(dtt.Rows[0]["EventGUID"].ToString());
                item.EventDurationTime = Convert.ToInt32(dtt.Rows[0]["EventDurationTime"].ToString());
                item.EventName = dtt.Rows[0]["EventName"].ToString();
                item.EventRetriesQuantity = Convert.ToInt32(dtt.Rows[0]["EventDurationTime"].ToString());
                item.EventTag = dtt.Rows[0]["EventTag"].ToString();
            }

            return item;

        }

        /// <summary>
        /// Anexo 4 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Registra la pista de auditoría de login en cada intento de acceso a la aplicación
        /// </summary>
        /// <param name="Par_AuditTrailLoginParentGUID">Valida que un GUID no pueda ser utilizado mas de una vez</param>
        /// <param name="Par_AuditTrailLoginAppInstanceGUID">GUID de la instancia de aplicación</param>
        /// <param name="Par_EventId">ID del evento a registrar</param>
        /// <param name="Par_AuditTrailLoginEndDate">Fecha y hora en la que caducará el AuditTrailLoginId</param>
        /// <param name="Par_AuditTrailLoginHost">Nombre de la maquina que intenta comunicarse</param>
        /// <param name="Par_AuditTrailLoginIP">IP de la maquina que intenta comunicarse</param>
        /// <param name="Par_AuditTrailLoginResponse">Resultado + AuditTrailLginGUID</param>
        /// <param name="Par_AuditTrailLoginRequest">Event tag + AutirTrailLoginId</param>
        /// <returns>
        /// Inserta en la tabla AuditTrailLogin y devuelve el GUID
        ///</returns>
        public static TransaccionSessionBE AuditTrailLogin_i(Guid Par_AuditTrailLoginParentGUID, Guid Par_AuditTrailLoginAppInstanceGUID, int Par_EventId, DateTime Par_AuditTrailLoginEndDate, string Par_AuditTrailLoginHost, string Par_AuditTrailLoginIP, string Par_AuditTrailLoginResponse, string Par_AuditTrailLoginRequest)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuditTrailLogin_i");
            TransaccionSessionBE item = new TransaccionSessionBE();
            if (Par_AuditTrailLoginParentGUID == Guid.Empty)
            {
                comando.Parameters.AddWithValue("AuditTrailLoginParentGUID", DBNull.Value);
            }
            else
            {
                comando.Parameters.AddWithValue("@AuditTrailLoginParentGUID", Par_AuditTrailLoginParentGUID);
            }
            comando.Parameters.AddWithValue("@AuditTrailLoginAppInstanceGUID", Par_AuditTrailLoginAppInstanceGUID);
            comando.Parameters.AddWithValue("@EventId", Par_EventId);
            comando.Parameters.AddWithValue("@AuditTrailLoginEndDate", Par_AuditTrailLoginEndDate);
            comando.Parameters.AddWithValue("@AuditTrailLoginHost", Par_AuditTrailLoginHost);
            comando.Parameters.AddWithValue("@AuditTrailLoginIP", Par_AuditTrailLoginIP);
            comando.Parameters.AddWithValue("@AuditTrailLoginResponse", Par_AuditTrailLoginResponse);
            comando.Parameters.AddWithValue("@AuditTrailLoginRequest", Par_AuditTrailLoginRequest);

            SqlParameter RetornoGUID = new SqlParameter("@AuditTrailLoginGUID", SqlDbType.UniqueIdentifier);
            SqlParameter RetornoID = new SqlParameter("@AuditTrailLoginId", SqlDbType.Int);

            RetornoGUID.Direction = ParameterDirection.Output;
            RetornoID.Direction = ParameterDirection.Output;
            comando.Parameters.Add(RetornoGUID);
            comando.Parameters.Add(RetornoID);

            MetaDatos.EjecutarComando(comando);

            ////Guid Par_AuditTrailLoginGUID = new Guid(RetornoGUID.Value.ToString());
            ////int AuditTrailLoginId = Convert.ToInt32(RetornoID.Value.ToString());
            item.AuditTrailSessionGUID = new Guid(RetornoGUID.Value.ToString());
            item.AuditTrailLoginId = Convert.ToInt32(RetornoID.Value.ToString());


            //DataTable dtRetorno = new DataTable("NewDataTable");
            //dtRetorno.Columns.Add("GUID", typeof(Guid));
            //dtRetorno.Columns.Add("AuditTrailLoginId", typeof(int));
            //DataRow NewRow = dtRetorno.NewRow();



            ///dtRetorno.Rows.Add(NewRow);
            return item;
        }


        /// <summary>
        /// Anexo 4 CU005 SistemaDeSeguridad_Aplicación_CU005_GestionarSolicitudesDeAplicacion
        /// Validar y Devolver los datos de un elemento de menu 
        /// </summary>
        /// <param name="Par_MenuGUID">GUID del elemento de menu</param>
        /// <returns></returns>
        public static DataTable Menu_s_ByMenuGUID_Valid(Guid Par_MenuGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.Menu_s_ByMenuGUID_Valid");
            comando.Parameters.AddWithValue("@MenuGUID", Par_MenuGUID);
            return MetaDatos.EjecutarComandoSelect(comando);
        }


        /// <summary>
        /// Anexo 5 CU005 SistemaDeSeguridad_Aplicación_CU005_GestionarSolicitudesDeAplicacion
        /// Devolver los permisos sobre los controles a un usuario en una
        /// instancia de aplicacion, buscando por el GUID de la instancia, del usuario y del menú
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID de la instancia de aplicación</param>
        /// <param name="Par_UserGUID">GUID del usuario</param>
        /// <param name="Par_MenuGUID">GUID del elemento de menú</param>
        /// <returns></returns>
        public static DataTable ControlUserPermission_s(Guid Par_ApplicationInstanceGUID, Guid Par_UserGUID, Guid Par_MenuGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.ControlUserPermission_s");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);
            comando.Parameters.AddWithValue("@UserGUID", Par_UserGUID);
            comando.Parameters.AddWithValue("@MenuGUID", Par_MenuGUID);
            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="auditTrailSessionId"></param>
        /// <param name="authenticationTypeUserMustChangePassword"> permite generar el json auditTrailSessionResponse</param>
        /// <param name="auditTrailSessionGUID">permite generar el json auditTrailSessionResponse</param>
        public static void UpdateResponseSession(
            int auditTrailSessionId, 
            
            Boolean authenticationTypeUserMustChangePassword,
            Guid auditTrailSessionGUID)
        {
            //AccesoDatos objAccesoDatos = new AccesoDatos();

            //int AuditTralSessionId = int.Parse(dtLog.Rows[0][1].ToString());
            UserAuthentic userAuthentic = new UserAuthentic();
            userAuthentic.AuditTrailSessionGUID = auditTrailSessionGUID;
            userAuthentic.AuthenticationTypeUserMustChangePassword = authenticationTypeUserMustChangePassword;

            var json = JsonConvert.SerializeObject(userAuthentic);
            AccesoDatos.AuditTrailSession_u_SessionResponse(auditTrailSessionId, json);
        }

        /// <summary>
        /// Busca todos los controles que tiene configurada la aplicación para ese menu
        /// </summary>
        /// <param name="Par_MenuGUID">GUID del elemento de menú</param>
        /// <returns></returns>
        public static DataTable ControlUserPermission_s_byMenuGuid(Guid Par_MenuGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("[Security].[Controls_s_ByMenuGUID]");
            comando.Parameters.AddWithValue("@MenuGUID", Par_MenuGUID);
            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// Anexo 5 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Completa la respuesta a la aplicación
        /// </summary>
        /// <param name="Par_AuditTrailLoginId">id del registro actual de la tabla AuditTrailLogin</param>
        /// <param name="Par_AuditTrailLoginResponse">respuesta a enviar a la aplicación</param>
        /// <returns>
        /// Actualiza el campo AuditTrailLoginResponse
        /// </returns>
        public int AuditTrailLogin_u_LoginResponse(int Par_AuditTrailLoginId, string Par_AuditTrailLoginResponse)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuditTrailLogin_u_LoginResponse");
            comando.Parameters.AddWithValue("@AuditTrailLoginId", Par_AuditTrailLoginId);
            comando.Parameters.AddWithValue("@AuditTrailLoginResponse", Par_AuditTrailLoginResponse);
            return MetaDatos.EjecutarComando(comando);
        }

        /// <summary>
        /// Anexo 6 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// El sistema detecta que el GUID recibido no es válido
        ///     El sistema busca la respuesta a enviar a la aplicación en función del error detectado
        /// </summary>
        /// <param name="Par_EventResponseInternalCode">Código que permite identificar una respuesta a evento particular</param>
        /// <returns>
        /// Devuelve todos los datos de la respuesta a evento, realizando la busqueda por el codigo interno 
        /// </returns>
        public static Error EventResponse_s_ByInternalCode(int eventResponseInternalCode)
        {
            Error errorResponse = new Error();
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.EventResponse_s_ByInternalCode");
            comando.Parameters.AddWithValue("@EventResponseInternalCode", eventResponseInternalCode);

            var dtError = MetaDatos.EjecutarComandoSelect(comando);

            errorResponse.EventResponseId = Convert.ToInt32(dtError.Rows[0]["EventResponseId"].ToString());
            errorResponse.EventResponseText = dtError.Rows[0]["EventResponseText"].ToString();
            errorResponse.EventResponseInternalCode = Convert.ToInt32(dtError.Rows[0]["EventResponseInternalCode"].ToString());
            errorResponse.Guid = Guid.Empty;

            return errorResponse;
        }

        /// <summary>
        /// Anexo 7 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Validar y devolver los datos de un registro de AuditTrailLogin
        /// realizando la busqueda por el GUID del registro. Si no es valido se devuelve 
        /// el codigo interno de error
        /// </summary>
        /// <param name="Par_AppInstanceGUID">GUID de instancia de la aplicación</param>
        /// <param name="Par_AuditTrailLoginGUID">GUID recibido de la aplicación</param>
        /// <returns>
        /// Devuelve los datos de un registro de AuditTrailLogin
        /// </returns>
        public static AuditTrailLoginBE AuditTrailLogin_s_ByAuditTrailLoginGUID_Valid(Guid Par_AppInstanceGUID, Guid Par_AuditTrailLoginGUID)
        {
            AuditTrailLoginBE item = null;
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuditTrailLogin_s_ByAuditTrailLoginGUID_Valid");
            comando.Parameters.AddWithValue("@AppInstanceGUID", Par_AppInstanceGUID);
            comando.Parameters.AddWithValue("@AuditTrailLoginGUID", Par_AuditTrailLoginGUID);

            var dtt = MetaDatos.EjecutarComandoSelect(comando);

            if (dtt.Rows.Count > 0)
            {
                item = new AuditTrailLoginBE();

                if (dtt.Columns["EventResponseInternalCode"] == null){
                    
                    item.EventId = Convert.ToInt32(dtt.Rows[0]["EventId"].ToString());
                    item.AuditTrailLoginAppInstanceGUID = Guid.Parse(dtt.Rows[0]["AuditTrailLoginAppInstanceGUID"].ToString());
                    item.AuditTrailLoginGUID = Guid.Parse(dtt.Rows[0]["AuditTrailLoginGUID"].ToString());
                    item.EventName = dtt.Rows[0]["EventName"].ToString();

                    item.AuditTrailLoginIP = dtt.Rows[0]["AuditTrailLoginIP"].ToString();
                    item.AuditTrailLoginHost = dtt.Rows[0]["AuditTrailLoginHost"].ToString();
                    item.AuditTrailLoginEndDate = Convert.ToDateTime(dtt.Rows[0]["AuditTrailLoginEndDate"].ToString());
                    item.AuditTrailLoginRequest = dtt.Rows[0]["AuditTrailLoginRequest"].ToString();
                    item.AuditTrailLoginResponse = dtt.Rows[0]["AuditTrailLoginResponse"].ToString();
                }
                else
                {
                    item.EventResponseInternalCode= Convert.ToInt32(dtt.Rows[0]["EventResponseInternalCode"].ToString());
                }

                
            }
            return item;
        }

        /// <summary>
        /// Anexo 8 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Devolver todos los tipos de autenticacion de una instancia de aplicacion realizando la busqueda por el GUID de la instancia
        /// </summary>
        /// <param name="Par_AppInstanceGUID">GUID de instancia de la aplicación</param>
        /// <returns>
        /// Devuelve los tipos de autenticacion de una instancia de aplicacion
        /// </returns>s
        public static List<AuthenticationTypeBE> AuthenticationType_s_ByApplicationInstanceGUID(Guid Par_AppInstanceGUID)
        {
            AuthenticationTypeBE item = null;
            List<AuthenticationTypeBE> list = new List<AuthenticationTypeBE>();
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuthenticationType_s_ByApplicationInstanceGUID");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_AppInstanceGUID);

            var dtt =  MetaDatos.EjecutarComandoSelect(comando);
            for (int i = 0; i <= dtt.Rows.Count - 1; i++)
            {
                item = new AuthenticationTypeBE();
                item.AuthenticationTypeName = dtt.Rows[i]["AuthenticationTypeName"].ToString();
                item.AuthenticationTypeTag = dtt.Rows[i]["AuthenticationTypeTag"].ToString();
                item.AuthenticationTypeGUID = Guid.Parse(dtt.Rows[i]["AuthenticationTypeGUID"].ToString());
                list.Add(item);
            }

            return list;
        }

        /// <summary>
        /// Anexo 8 CU005 SistemaDeSeguridad_Aplicación_CU005_GestionarSolicitudesDeAplicacion
        /// Validar y Devolver los datos de un usuario particular realizando la busqueda por el GUID de sesion 
        /// </summary>
        /// <param name="Par_AuditTrailSessionGUID">Guid de sesión</param>
        /// <returns>Los datos del usuario</returns>
        public static DataTable User_s_BySessionGUID_Valid(Guid Par_AuditTrailSessionGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.User_s_BySessionGUID_Valid");
            comando.Parameters.AddWithValue("@AuditTrailSessionGUID", Par_AuditTrailSessionGUID);

            return MetaDatos.EjecutarComandoSelect(comando);
        }


        /// <summary>
        /// Anexo 9 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Devolver todos los dominios de una instancia de aplicacion realizando la busqueda por el GUID de la instancia teniendo en cuenta
        /// un tipo de autenticacion
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID de la instancia de aplicación</param>
        /// <param name="Par_AuthenticationTypeTag"> Tag del tipo de autenticación</param>
        /// <returns>
        /// Devuelve los dominios de una instancia de aplicacion
        /// </returns>
        public static List<DomainBE> Domain_s_ByApplicationInstanceGUID(Guid Par_ApplicationInstanceGUID, string Par_AuthenticationTypeTag)
        {
            List<DomainBE> list = new List<DomainBE>();
               SqlCommand comando = MetaDatos.CrearComandoProc("Security.Domain_s_ByApplicationInstanceGUID");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);
            comando.Parameters.AddWithValue("@AuthenticationTypeTag", Par_AuthenticationTypeTag);
            
            var dtt = MetaDatos.EjecutarComandoSelect(comando);
            DomainBE item;
            foreach (DataRow dr in dtt.Rows)
            {
                item = new DomainBE();

                item.DomainName = dr["DomainName"].ToString();
                item.DomainGuid = (Guid)dr["DomainGUID"];

                list.Add(item);
            }

            return list; 
        }


        /// <summary>
        /// Anexo 10 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Valida si un usuario posee acceso a una instancia de aplicacion con un tipo de autenticacion
        /// SIN considerar dominio (para tipos de autenticacion <> Windows)
        /// </summary>
        /// <param name="Par_UserName">Nombre de usuario</param>
        /// <param name="Par_AppInstanceGUID">GUID de instancia de aplicación</param>
        /// <param name="Par_AutTypeGUID">GUID del tipo de autenticación seleccionado</param>
        /// <returns></returns>
        public static AuthenticationTypeApplicationInstanceUserBE AuthenticationTypeApplicationInstanceUser_s_Valid(string Par_UserName, Guid Par_AppInstanceGUID, Guid Par_AutTypeGUID)
        {
            AuthenticationTypeApplicationInstanceUserBE item = null;
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuthenticationTypeApplicationInstanceUser_s_Valid");
            comando.Parameters.AddWithValue("@UserName", Par_UserName);
            comando.Parameters.AddWithValue("@AppInstanceGUID", Par_AppInstanceGUID);
            comando.Parameters.AddWithValue("@AutenticacionTypeGUID", Par_AutTypeGUID);
            var dtt = MetaDatos.EjecutarComandoSelect(comando);

            if (dtt.Rows.Count > 0)
            {
                item = new AuthenticationTypeApplicationInstanceUserBE();
                item.ATAUGUID = Guid.Parse(dtt.Rows[0][0].ToString());
                item.AuthenticationTypeUserMustChangePassword = Convert.ToBoolean(dtt.Rows[0]["AuthenticationTypeUserMustChangePassword"].ToString());
                item.UserGUID = Guid.Parse(dtt.Rows[0]["UserGUID"].ToString());

            }

            return item;
        }


        /// <summary>
        /// Anexo 10 CU005 SistemaDeSeguridad_Aplicación_CU005_GestionarSolicitudesDeAplicacion
        /// Devolver los usuarios habilitados de una instancia de aplicacion buscando por el GUID de la instancia
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">Guid de la instancia de aplicación</param>
        /// <returns></returns>
        public static DataTable User_s_ByApplicationInstanceGUID(Guid Par_ApplicationInstanceGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.User_s_ByApplicationInstanceGUID");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);

            return MetaDatos.EjecutarComandoSelect(comando);

        }


        /// <summary>
        /// Anexo 10 CU005 SistemaDeSeguridad_Aplicación_CU005_GestionarSolicitudesDeAplicacion
        /// Devolver los usuarios habilitados de una instancia de aplicacion buscando por el GUID de la instancia
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">Guid de la instancia de aplicación</param>
        /// <returns></returns>
        public static DataTable Application_s_byPersonDocument(string Par_ApplicationInstanceGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.ApplicationInstance_s_ByPersonDocNumber");
            comando.Parameters.AddWithValue("@PersonDocNumber", Par_ApplicationInstanceGUID);

            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// Anexo 11 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Validar y Devolver los datos de un usuario particular realizando la busqueda por el nombre de usuario 
        /// </summary>
        /// <param name="Par_UserName">Nombre de usuario</param>
        /// <returns>
        /// Devuelve los datos de un usuario particular
        /// </returns>
        public static UserBE User_s_ByUserName_Valid(string Par_UserName)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.User_s_ByUserName_Valid");
            comando.Parameters.AddWithValue("@UserName", Par_UserName);

            var dtUser = MetaDatos.EjecutarComandoSelect(comando);
            UserBE userBE = null;

            if (dtUser.Rows.Count > 0)
            {
                userBE = new UserBE();
                userBE.UserId = Convert.ToInt32(dtUser.Rows[0][0].ToString());


                userBE.UserName = dtUser.Rows[0]["UserName"].ToString();
                userBE.PersonFirstName = dtUser.Rows[0]["PersonFirstName"].ToString();
                userBE.PersonLastName = dtUser.Rows[0]["PersonLastName"].ToString();
                userBE.PersonDocNumber = dtUser.Rows[0]["PersonDocNumber"].ToString();


                if(dtUser.Rows[0]["UserPlaceGuid"] != DBNull.Value  )
                    userBE.UserPlaceGuid = Guid.Parse(dtUser.Rows[0]["UserPlaceGuid"].ToString());

                userBE.UserPlaceName = dtUser.Rows[0]["UserPlaceName"].ToString();
                if (dtUser.Rows[0]["UserPlaceDescript"] != DBNull.Value)
                    userBE.UserPlaceDescript = dtUser.Rows[0]["UserPlaceDescript"].ToString();
                if (dtUser.Rows[0]["PersonGUID"] != DBNull.Value)
                    userBE.PersonGUID = Guid.Parse(dtUser.Rows[0]["PersonGUID"].ToString());
                userBE.PersonModifiedDate = Convert.ToDateTime(dtUser.Rows[0]["PersonModifiedDate"].ToString());



            }
            return userBE;
        }

        /// <summary>
        /// Anexo 12 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Actualiza el campo AuditTrailSessionResponse
        /// </summary>
        /// <param name="Par_AuditTrailSessionId">ID de la tabla AuditTrailSession</param>
        /// <param name="Par_AuditTrailSessionResponse">Respuesta a enviar a la aplicación</param>
        /// <returns></returns>
        public static int AuditTrailSession_u_SessionResponse(int Par_AuditTrailSessionId, string Par_AuditTrailSessionResponse)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuditTrailSession_u_SessionResponse");
            comando.Parameters.AddWithValue("@AuditTrailSessionId", Par_AuditTrailSessionId);
            comando.Parameters.AddWithValue("@AuditTrailSessionResponse", Par_AuditTrailSessionResponse);

            return MetaDatos.EjecutarComando(comando);
        }


        /// <summary>
        /// Anexo 13 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Valida si un usuario posee acceso a una instancia de aplicacion con un tipo de autenticacion
        ///en un determinado dominio 
        /// </summary>
        /// <param name="Par_UserName">Nombre de usuario</param>
        /// <param name="Par_AppInstanceGUID">GUID de instancia de aplicación</param>
        /// <param name="Par_AutTypeGUID">GUID del tipo de autenticación seleccionado</param>
        /// <param name="Par_DomainGUID">GUID del dominio seleccionado</param>
        /// <returns></returns>
        public static AuthenticationTypeApplicationInstanceUserBE AuthenticationTypeApplicationInstanceUser_s_Valid_Domain(string Par_UserName, Guid Par_AppInstanceGUID, Guid Par_AutTypeGUID, Guid Par_DomainGUID)
        {
            AuthenticationTypeApplicationInstanceUserBE item = null;
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuthenticationTypeApplicationInstanceUser_s_Valid_Domain");
            comando.Parameters.AddWithValue("@UserName", Par_UserName);
            comando.Parameters.AddWithValue("@AppInstanceGUID", Par_AppInstanceGUID);
            comando.Parameters.AddWithValue("@AutenticacionTypeGUID", Par_AutTypeGUID);
            comando.Parameters.AddWithValue("@DomainGUID", Par_DomainGUID);
            var dtt = MetaDatos.EjecutarComandoSelect(comando);

            if (dtt.Rows.Count > 0)
            {
                item = new AuthenticationTypeApplicationInstanceUserBE();
                item.ATAUGUID = Guid.Parse(dtt.Rows[0][0].ToString());
                item.AuthenticationTypeUserMustChangePassword = Convert.ToBoolean(dtt.Rows[0]["AuthenticationTypeUserMustChangePassword"].ToString());
                item.UserGUID = Guid.Parse(dtt.Rows[0]["UserGUID"].ToString());

            }
            return item;
        }


        /// <summary>
        /// Anexo 13 CU005 SistemaDeSeguridad_Aplicación_CU005_GestionarSolicitudesDeAplicacion
        /// Devolver los permisos sobre controles de UI de un elemento de menu, buscando por el GUID del grupo 
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID de la instancia de aplicación </param>
        /// <param name="Par_GroupGUID">GUID del grupo</param>
        /// <param name="Par_MenuGUID">GUID del elemento de menú</param>
        /// <param name="Par_DomainGUID">GUID del dominio</param>
        /// <returns></returns>
        public static DataTable ControlPermission_s_ByGroupGUID(Guid Par_ApplicationInstanceGUID, Guid Par_GroupGUID, Guid Par_MenuGUID, Guid Par_DomainGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.ControlPermission_s_ByGroupGUID");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);
            comando.Parameters.AddWithValue("@GroupGUID", Par_GroupGUID);
            comando.Parameters.AddWithValue("@MenuGUID", Par_MenuGUID);
            comando.Parameters.AddWithValue("@DomainGUID", Par_DomainGUID);
            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// Anexo 14 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// El sistema verifica que el evento es “Autenticar Usuario”
        ///     El sistema completa la respuesta a la aplicación, agregando el GUID del nuevo registro generado
        /// </summary>
        /// <param name="Par_AuditTrailLoginId">ID del registro en la pista de auditoría de Login</param>
        /// <param name="Par_EventId">ID del evento ‘User-Ok’</param>
        /// <param name="Par_UserId">ID del usuario que inicia sesión en la instancia de aplicación</param>
        /// <param name="Par_AuthenticationTypeId">ID del tipo de autenticación con que el usuario inicia sesión</param>
        /// <param name="Par_AuditTrailSessionResponse"></param>
        /// <param name="Par_AuditTrailSessionEndDate">fecha y hora de caducidad del registro</param>
        /// <param name="Par_DomainId">ID del dominio seleccionado por el usuario, en caso de autenticación “Windows”</param>
        /// <returns></returns>
        public static DataTable AuditTrailSession_i(int Par_AuditTrailLoginId, int Par_EventId, int Par_UserId, int Par_AuthenticationTypeId, string Par_AuditTrailSessionResponse, DateTime Par_AuditTrailSessionEndDate, int Par_DomainId)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuditTrailSession_i");
            DataSet dtRetorno = new DataSet();

            dtRetorno.Tables.Add("mitabla");

            dtRetorno.Tables["mitabla"].Columns.Add("GUID", typeof(Guid));
            dtRetorno.Tables["mitabla"].Columns.Add("ID", typeof(int));

            comando.Parameters.AddWithValue("@AuditTrailLoginId", Par_AuditTrailLoginId);
            comando.Parameters.AddWithValue("@EventId", Par_EventId);
            comando.Parameters.AddWithValue("@UserId", Par_UserId);
            comando.Parameters.AddWithValue("@AuthenticationTypeId", Par_AuthenticationTypeId);
            comando.Parameters.AddWithValue("@AuditTrailSessionResponse", Par_AuditTrailSessionResponse);
            comando.Parameters.AddWithValue("@AuditTrailSessionEndDate", Par_AuditTrailSessionEndDate);
            comando.Parameters.AddWithValue("@DomainId", Par_DomainId);

            SqlParameter RetornoGUID = new SqlParameter("@AuditTrailSessionGUID", SqlDbType.UniqueIdentifier);
            SqlParameter RetornoID = new SqlParameter("@AuditTrailSessionId", SqlDbType.Int);

            RetornoGUID.Direction = ParameterDirection.Output;
            RetornoID.Direction = ParameterDirection.Output;
            comando.Parameters.Add(RetornoGUID);
            comando.Parameters.Add(RetornoID);

            MetaDatos.EjecutarComando(comando);
            Guid AuditTrailSessionGUID = new Guid(RetornoGUID.Value.ToString());
            int AuditTrailSessionId = Convert.ToInt32(RetornoID.Value.ToString());

            DataRow row = dtRetorno.Tables["mitabla"].NewRow();
            row["GUID"] = AuditTrailSessionGUID;
            row["ID"] = AuditTrailSessionId;
            dtRetorno.Tables["mitabla"].Rows.Add(row);

            return dtRetorno.Tables["mitabla"];
        }


        /// <summary>
        /// Anexo 15 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Verifica que el GUID de sesión sea válido
        /// </summary>
        /// <param name="Par_AuditTrailSessionGUID">GUID de sesión</param>
        /// <returns></returns>
        public static DataSet AuditTrailSession_s_ByAuditTrailSessionGUID_Valid(Guid Par_AuditTrailSessionGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuditTrailSession_s_ByAuditTrailSessionGUID_Valid");
            comando.Parameters.AddWithValue("@AuditTrailSessionGUID", Par_AuditTrailSessionGUID);

            return MetaDatos.EjecutarComandoSelectDs(comando);
        }


        /// <summary>
        /// Anexo 15 CU005 SistemaDeSeguridad_Aplicación_CU005_GestionarSolicitudesDeAplicacion
        /// Validar si un usuario tiene permiso sobre un menu de una
        /// instancia de aplicacion, buscando por el GUID de la instancia, del usuario y del menu
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID  de la instancia de aplicación</param>
        /// <param name="Par_UserGUID">GUID del usuario</param>
        /// <param name="Par_MenuGUID">GUID del elemento de menu</param>
        /// <returns></returns>
        public static DataTable MenuUserPermission_s_ByMenuGUID_Valid(Guid Par_ApplicationInstanceGUID, Guid Par_UserGUID, Guid Par_MenuGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.MenuUserPermission_s_ByMenuGUID_Valid");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);
            comando.Parameters.AddWithValue("@UserGUID", Par_UserGUID);
            comando.Parameters.AddWithValue("@MenuGUID", Par_MenuGUID);
            return MetaDatos.EjecutarComandoSelect(comando);
        }


        /// <summary>
        /// Anexo 16 CU005 SistemaDeSeguridad_Aplicación_CU005_GestionarSolicitudesDeAplicacion
        /// Validar si existe el permiso sobre un menu de una
        /// instancia de aplicacion, buscando por el GUID de la instancia, del menu y del grupo AD
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID  de la instancia de aplicación</param>
        /// <param name="Par_GroupGUID">GUID del grupo AD</param>
        /// <param name="Par_DomainGUID">GUID del dominio</param>
        /// <param name="Par_MenuGUID">GUID del elemento de menú</param>
        /// <returns></returns>
        public static DataTable MenuPermission_s_ByMenuGUID(Guid Par_ApplicationInstanceGUID, Guid Par_GroupGUID, Guid Par_DomainGUID, Guid Par_MenuGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.MenuPermission_s_ByMenuGUID");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);
            comando.Parameters.AddWithValue("@GroupGUID", Par_GroupGUID);
            comando.Parameters.AddWithValue("@DomainGUID", Par_DomainGUID);
            comando.Parameters.AddWithValue("@MenuGUID", Par_MenuGUID);
            return MetaDatos.EjecutarComandoSelect(comando);
        }


        /// <summary>
        /// Anexo 16 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// El sistema busca los permisos del usuario sobre los elementos de menú de la aplicación, 
        /// teniendo en cuenta los perfiles asignados y los permisos concedidos individualmente como usuario, y encuentra al menos uno.
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID de instancia de la aplicación</param>
        /// <param name="Par_UserGUID">GUID de usuario</param>
        /// <returns></returns>
        public static DataTable MenuUserPermission_s(Guid Par_ApplicationInstanceGUID, Guid Par_UserGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.MenuUserPermission_s");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);
            comando.Parameters.AddWithValue("@UserGUID", Par_UserGUID);

            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// Anexo 17 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Inserta en la tabla AuditTrailSessionDet
        /// </summary>
        /// <param name="Par_AuditTrailSessionId">ID del registro en la tabla encabezado Security.AuditTrailSession </param>
        /// <param name="Par_EventId">ID del evento</param>
        /// <param name="Par_AuditTrailSessionDetResponse">Respuesta a enviar a la aplicación</param>
        /// <param name="Par_AuditTrailSessionDetRequest">parámetros enviados por la aplicación en su solicitud</param>
        /// <returns>El ID del nuevo registro</returns>
        public static DataTable AuditTrailSessionDet_i(int Par_AuditTrailSessionId, int Par_EventId, string Par_AuditTrailSessionDetResponse, string Par_AuditTrailSessionDetRequest)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuditTrailSessionDet_i");
            DataSet dtRetorno = new DataSet();

            dtRetorno.Tables.Add("mitabla");
            dtRetorno.Tables["mitabla"].Columns.Add("ID", typeof(int));

            comando.Parameters.AddWithValue("@AuditTrailSessionId", Par_AuditTrailSessionId);
            comando.Parameters.AddWithValue("@EventId", Par_EventId);
            if (Par_AuditTrailSessionDetResponse != string.Empty)
                comando.Parameters.AddWithValue("@AuditTrailSessionDetResponse", Par_AuditTrailSessionDetResponse);
            else
                comando.Parameters.AddWithValue("@AuditTrailSessionDetResponse", DBNull.Value);
            comando.Parameters.AddWithValue("@AuditTrailSessionDetRequest", Par_AuditTrailSessionDetRequest);

            SqlParameter RetornoID = new SqlParameter("@AuditTrailSessionDetId", SqlDbType.Int);

            RetornoID.Direction = ParameterDirection.Output;
            comando.Parameters.Add(RetornoID);

            MetaDatos.EjecutarComando(comando);
            int AuditTrailSessionDetId = Convert.ToInt32(RetornoID.Value.ToString());

            DataRow row = dtRetorno.Tables["mitabla"].NewRow();
            row["ID"] = AuditTrailSessionDetId;
            dtRetorno.Tables["mitabla"].Rows.Add(row);

            return dtRetorno.Tables["mitabla"];
        }


        /// <summary>
        /// Anexo 18 
        /// Devolver los permisos sobre menu de una instancia de aplicacion, buscando por el GUID del grupo 
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID de la instancia de aplicación</param>
        /// <param name="Par_GroupGUID">GUID del Grupo</param>
        /// <param name="Par_DomainGUID">GUID del dominio</param>
        /// <returns>los permisos sobre menu de una instancia de aplicacion</returns>  
        public static DataTable MenuPermission_s_ByGroupGUID(Guid Par_ApplicationInstanceGUID, Guid Par_GroupGUID, Guid Par_DomainGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.MenuPermission_s_ByGroupGUID");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);
            comando.Parameters.AddWithValue("@GroupGUID", Par_GroupGUID);
            comando.Parameters.AddWithValue("@DomainGUID", Par_DomainGUID);

            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// Anexo 19 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Devolver los datos de un grupo, buscando por el nombre del grupo realizando la busqueda por el nombre de grupo
        /// </summary>
        /// <param name="Par_GroupName">Nombre del grupo</param>
        /// <returns>Los datos del grupo</returns>
        public static DataTable Group_s_ByName(string Par_GroupName)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.Group_s_ByName");
            comando.Parameters.AddWithValue("@GroupName", Par_GroupName);

            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// Anexo 22 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// Obtiene los datos del tipo de autenticación seleccionado
        /// </summary>
        /// <param name="autTypeGUID">GUID del tipo de autenticación</param>
        /// <returns></returns>
        public static AuthenticationTypeBE AuthenticationType_s_ByGUID(Guid autTypeGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuthenticationType_s_ByGUID");
            comando.Parameters.AddWithValue("@AuthenticationTypeGUID", autTypeGUID);

            var dtt = MetaDatos.EjecutarComandoSelect(comando);
            AuthenticationTypeBE authenticationType = null;

            if (dtt.Rows.Count > 0)
            {
                authenticationType = new AuthenticationTypeBE();
                authenticationType.AuthenticationTypeGUID = autTypeGUID;
                authenticationType.AuthenticationTypeTag = dtt.Rows[0]["AuthenticationTypeTag"].ToString();
                authenticationType.AuthenticationTypeName = dtt.Rows[0]["AuthenticationTypeName"].ToString();
                

                authenticationType.AuthenticationTypeId = Convert.ToInt32(dtt.Rows[0]["AuthenticationTypeId"]);
            }
                
            return authenticationType;
        }

        /// <summary>
        /// Anexo 23 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// El sistema verifica si el dominio de Active Directory recibido existe
        /// </summary>
        /// <param name="Par_DomainGUID">GUID del dominio</param>
        /// <returns></returns>
        public static DomainBE Domain_s_ByGUID(Guid Par_DomainGUID)
        {
            DomainBE domain = null;
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.Domain_s_ByGUID");
            comando.Parameters.AddWithValue("@DomainGUID", Par_DomainGUID);

            var dtDominio = MetaDatos.EjecutarComandoSelect(comando);
            if (dtDominio.Rows.Count > 0)
            {
                domain = new DomainBE();
                domain.DomainName = dtDominio.Rows[0]["DomainName"].ToString();
                domain.DomainUsr = dtDominio.Rows[0]["DomainUsr"].ToString();
                domain.LDAPPath = dtDominio.Rows[0]["LDAPPath"].ToString();
                domain.DomainSiteName = dtDominio.Rows[0]["DomainSiteName"].ToString();
                domain.DomainPwd = dtDominio.Rows[0]["DomainPwd"].ToString();
            }

            return domain;
        }


        /// <summary>
        /// 
        /// El sistema verifica si el dominio de Active Directory recibido existe
        /// </summary>
        /// <param name="Par_DomainName">Nombre del dominio</param>
        /// <returns></returns>
        public static DataTable Domain_s_Name(string Par_DomainName)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("[Security].[Domain_s_DomainName]");
            comando.Parameters.AddWithValue("@DomainName", Par_DomainName);
            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// cu005 anexo 22
        /// </summary>
        /// <param name="pApplicationId"></param>
        /// <returns></returns>
        public static List<EntityBE> Entity_s_ApplicationId(int pApplicationId)
        {
            var rv = new List<EntityBE>();
            SqlCommand comando = MetaDatos.CrearComandoProc("[Security].[Entity_s_ApplicationId]");
            comando.Parameters.AddWithValue("@ApplicationId", pApplicationId);
            var select = MetaDatos.EjecutarComandoSelect(comando);

            foreach (DataRow r in select.Rows)
            {
                var cur = new EntityBE();
                cur.EntityGUID = (Guid)r["EntityGUID"];
                cur.EntiyId = (int)r["EntityId"];
                cur.EntityName = (string)r["EntityName"];
                rv.Add(cur);
            }

            return rv;
        }

        /// <summary>
        /// cu005 anexo 23
        /// </summary>
        /// <param name="pEntityId"></param>
        /// <param name="pUserId"></param>
        /// <param name="pApplicationInstanceId"></param>
        /// <returns></returns>
        public static List<EntityRegisterUserPermissionBE> EntityRegisterUserPermission_s(int pEntityId, int pUserId, int pApplicationInstanceId)
        {
            var rv = new List<EntityRegisterUserPermissionBE>();
            SqlCommand comando = MetaDatos.CrearComandoProc("[Security].[EntityRegisterUserPermission_s]");
            comando.Parameters.AddWithValue("@EntityId", pEntityId);
            comando.Parameters.AddWithValue("@UserId", pUserId);
            comando.Parameters.AddWithValue("@ApplicationInstanceId", pApplicationInstanceId);
            var select = MetaDatos.EjecutarComandoSelect(comando);

            foreach (DataRow r in select.Rows)
            {
                var cur = new EntityRegisterUserPermissionBE();
                cur.EntityRegisterId = (int)r["EntityRegisterId"];
                cur.EntityRegisterName = (string)r["EntityRegisterName"];
                cur.EntityRegisterGUID = (Guid)r["EntityRegisterGUID"];
                cur.EntityRegisterPk = (string)r["EntityRegisterPk"];
                cur.EntityRegisterElementGUID = (Guid)r["EntityRegisterElementGUID"];
                if (r["EntityRegisterParentGUID"] != DBNull.Value)
                    cur.EntityRegisterParentGUID = (Guid)r["EntityRegisterParentGUID"];
                cur.Edicion = (bool)r["Edicion"];
                cur.Eliminacion = (bool)r["Eliminacion"];
                cur.Seleccion = (bool)r["Seleccion"];
                rv.Add(cur);

            }

            return rv;
        }

        /// <summary>
        /// Anexo 24 CU004 SistemaDeSeguridad_Aplicación_CU004_LoginDeAplicaciones
        /// ?
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID de instancia de la aplicación</param>
        /// <returns></returns>
        public static DataTable Application_s_ByApplicationInstanceGUID(Guid Par_ApplicationInstanceGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.Application_s_ByApplicationInstanceGUID");
            comando.Parameters.AddWithValue("@ApplicationInstanceGUID", Par_ApplicationInstanceGUID);

            return MetaDatos.EjecutarComandoSelect(comando);
        }

        /// <summary>
        /// Busca los datos de la tabla AuthenticationTypeUser segun los criterios definidos.
        /// </summary>
        /// <param name="Par_ApplicationInstanceGUID">GUID de instancia de la aplicación</param>
        /// <returns></returns>
        public static AuthenticationTypeUserBE AuthenticationTypeUser_s_MembershipUserID(int UserId, int AuthenticationTypeId)
        {
            AuthenticationTypeUserBE item = null;
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.AuthenticationTypeUser_s_MembershipUserID");
            comando.Parameters.AddWithValue("@UserId", UserId);
            comando.Parameters.AddWithValue("@AuthenticationTypeId", AuthenticationTypeId);
            var dtt = MetaDatos.EjecutarComandoSelect(comando);

            if (dtt.Rows.Count > 0)
            {
                item = new AuthenticationTypeUserBE();
                item.AuthenticationTypeUserId = Convert.ToInt32(dtt.Rows[0]["AuthenticationTypeUserId"]);
                item.AuthenticationTypeUserGUID = Guid.Parse(dtt.Rows[0]["AuthenticationTypeUserGUID"].ToString());
                item.AuthenticationTypeId = Convert.ToInt32(dtt.Rows[0]["AuthenticationTypeId"]);
                item.AuthenticationTypeUserMustChangePassword = Convert.ToBoolean(dtt.Rows[0]["AuthenticationTypeUserMustChangePassword"]);
            }

            
            return item;
            
        }

        public static TransaccionSessionBE TransaccionSession(int Par_EventId, int Par_UserId, int Par_AuthenticationTypeId, string Par_AuditTrailSessionResponse, DateTime Par_AuditTrailSessionEndDate, int Par_DomainId, Guid Par_AuditTrailLoginParentGUID, Guid Par_AuditTrailLoginAppInstanceGUID, string Par_AuditTrailLoginHost, string Par_AuditTrailLoginIP, string Par_AuditTrailLoginResponse, string Par_AuditTrailLoginRequest, int IdEventUserOk, DateTime Par_AuditTrailLoginEndDateUserAuthentic)
        {
            TransaccionSessionBE transaccionSession = new TransaccionSessionBE();
            //Terminar de desarrollar la transacción para login y session            
            ////DataSet dsSessionT = new DataSet();

            ////dsSessionT.Tables.Add("mitabla");

            ////dsSessionT.Tables["mitabla"].Columns.Add("GUID", typeof(Guid));
            ////dsSessionT.Tables["mitabla"].Columns.Add("ID", typeof(int));
            ////dsSessionT.Tables["mitabla"].Columns.Add("AuditTrailLoginId", typeof(int));

            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = Configuracion.Conn;
            connection.Open();

            //Transaction

            SqlTransaction transaction;
            transaction = connection.BeginTransaction();

            //Command
            SqlCommand comando;
            try
            {
                comando = new SqlCommand();
                comando.Connection = connection;
                comando.Transaction = transaction;
                //command.CommandTimeout = 30 'segundos
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "Security.AuditTrailLogin_i";

                comando.Parameters.AddWithValue("@AuditTrailLoginParentGUID", Par_AuditTrailLoginParentGUID);
                comando.Parameters.AddWithValue("@AuditTrailLoginAppInstanceGUID", Par_AuditTrailLoginAppInstanceGUID);
                comando.Parameters.AddWithValue("@EventId", Par_EventId);
                comando.Parameters.AddWithValue("@AuditTrailLoginEndDate", Par_AuditTrailLoginEndDateUserAuthentic);
                comando.Parameters.AddWithValue("@AuditTrailLoginHost", Par_AuditTrailLoginHost);
                comando.Parameters.AddWithValue("@AuditTrailLoginIP", Par_AuditTrailLoginIP);

                if (Par_AuditTrailLoginResponse == string.Empty)
                {
                    comando.Parameters.AddWithValue("@AuditTrailLoginResponse", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@AuditTrailLoginResponse", Par_AuditTrailLoginResponse);
                }
                comando.Parameters.AddWithValue("@AuditTrailLoginRequest", Par_AuditTrailLoginRequest);

                SqlParameter RetornoGUID = new SqlParameter("@AuditTrailLoginGUID", SqlDbType.UniqueIdentifier);
                SqlParameter RetornoID = new SqlParameter("@AuditTrailLoginId", SqlDbType.Int);

                RetornoGUID.Direction = ParameterDirection.Output;
                RetornoID.Direction = ParameterDirection.Output;
                comando.Parameters.Add(RetornoGUID);
                comando.Parameters.Add(RetornoID);

                comando.ExecuteNonQuery();

                Guid Par_AuditTrailLoginGUID = new Guid(RetornoGUID.Value.ToString());
                int Par_AuditTrailLoginId = Convert.ToInt32(RetornoID.Value.ToString());

                comando = new SqlCommand();
                comando.Connection = connection;
                comando.Transaction = transaction;
                //command.CommandTimeout = 30 'segundos
                comando.CommandType = CommandType.StoredProcedure;
                comando.CommandText = "Security.AuditTrailSession_i";

                comando.Parameters.AddWithValue("@AuditTrailLoginId", Par_AuditTrailLoginId);
                comando.Parameters.AddWithValue("@EventId", IdEventUserOk); //<---- Aquí va el evento UserOK
                comando.Parameters.AddWithValue("@UserId", Par_UserId);
                comando.Parameters.AddWithValue("@AuthenticationTypeId", Par_AuthenticationTypeId);

                if (Par_AuditTrailSessionResponse == string.Empty)
                {
                    comando.Parameters.AddWithValue("@AuditTrailSessionResponse", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@AuditTrailSessionResponse", Par_AuditTrailSessionResponse);
                }

                comando.Parameters.AddWithValue("@AuditTrailSessionEndDate", Par_AuditTrailSessionEndDate);

                if (Par_DomainId == 0)
                {
                    comando.Parameters.AddWithValue("@DomainId", DBNull.Value);
                }
                else
                {
                    comando.Parameters.AddWithValue("@DomainId", Par_DomainId);
                }

                SqlParameter RetornoSessionGUID = new SqlParameter("@AuditTrailSessionGUID", SqlDbType.UniqueIdentifier);
                SqlParameter RetornoSessionID = new SqlParameter("@AuditTrailSessionId", SqlDbType.Int);

                RetornoSessionGUID.Direction = ParameterDirection.Output;
                RetornoSessionID.Direction = ParameterDirection.Output;
                comando.Parameters.Add(RetornoSessionGUID);
                comando.Parameters.Add(RetornoSessionID);

                comando.ExecuteNonQuery();

                Guid auditTrailSessionGUID = new Guid(RetornoSessionGUID.Value.ToString());
                int auditTrailSessionId = Convert.ToInt32(RetornoSessionID.Value.ToString());

                //Commit
                transaction.Commit();

                ////DataRow row = dsSessionT.Tables["mitabla"].NewRow();
                ////row["GUID"] = AuditTrailSessionGUID;
                ////row["ID"] =   AuditTrailSessionId;
                ////row["AuditTrailLoginId"] = Par_AuditTrailLoginId;
                ////dsSessionT.Tables["mitabla"].Rows.Add(row);
                transaccionSession.AuditTrailSessionGUID = auditTrailSessionGUID;
                transaccionSession.AuditTrailSessionId = auditTrailSessionId;
                transaccionSession.AuditTrailLoginId = Par_AuditTrailLoginId;
                //Cerramos
                comando.Dispose();
                connection.Close();

            }
            catch
            {

                //Rollback
                transaction.Rollback();
            }

            return transaccionSession;
        }

        /// <summary>
        /// Trae todas las instancias de aplicación
        /// </summary>
        /// <returns></returns>
        public static DataTable ApplicationInstance_s()
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.ApplicationInstance_s");
            return MetaDatos.EjecutarComandoSelect(comando);
        }

        public static DataTable Attributes_s_byPersonGuid(Guid? pPersonGUID)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("Security.PersonAttributeValue_s_ByPersonGuid");
            comando.Parameters.AddWithValue("@PersonGUID", pPersonGUID);
            return MetaDatos.EjecutarComandoSelect(comando);
        }


        public static DataTable Users_s_ByPersonEmail(string UserName, string PersonEmail)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("[Security].[User_s_ByPersonEmail]");
            comando.Parameters.AddWithValue("@UserName", UserName);
            comando.Parameters.AddWithValue("@PersonEmail", PersonEmail);
            return MetaDatos.EjecutarComandoSelect(comando);
        }

        public static DataTable SecurityCode_g_ByParams(string SecurityCode, string UserName)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("[Security].[SecurityCode_by_Params]");
            comando.Parameters.AddWithValue("@SecurityCode", SecurityCode);
            comando.Parameters.AddWithValue("@UserName", UserName);
            return MetaDatos.EjecutarComandoSelect(comando);
        }

        public static DataTable SecurityCode_i(string SecurityCode, string UserName)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("[Security].[SecurityCode_i]");
            comando.Parameters.AddWithValue("@SecurityCode", SecurityCode);
            comando.Parameters.AddWithValue("@UserName", UserName);
            return MetaDatos.EjecutarComandoSelect(comando);
        }

        public static DataTable EmailConfig_ById(int EmailConfigId)
        {
            SqlCommand comando = MetaDatos.CrearComandoProc("[Security].[EmailConfig_g_Id]");
            comando.Parameters.AddWithValue("@EmailConfigId", EmailConfigId);
            return MetaDatos.EjecutarComandoSelect(comando);
        }

    }
}