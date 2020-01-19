using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace EpironAPI.classes
{
   
    public class Validaciones
    {
        public DataTable Valida(string Par_Event_Tag,
            Guid Par_RequestSessionGuid,
            Guid Par_ApplicationInstanceGuid,
            string Par_AutenticationTypeTag,
            Guid Par_DomainGUID,
            Guid Par_MenuGUID,
            string Par_ListOfEntityGuids,
            string Par_PersonDocument,
            Guid? Par_PersonGuidForAttributes = null)
        {
            DataTable dtGuidSessionValido, dtError, dtEvento, dtUser, dtPermisos, dtUsersEnabled, dtInstanceApp, dtDomain = new DataTable();//dtLogSession
            DataTable dtValida = new DataTable();
            DataSet dsXml = new DataSet();
            DataSet dsXmlError = new DataSet();
            DataSet dsPermisos = new DataSet();
            int codigoError, Par_AuditTrailSessionId;
            
            string txtError, xmlRequest, xmlResponse;
            bool primeravez = true;
            DataView dvPermisos = new DataView();
            DataRow row;

            if (Par_Event_Tag == string.Empty | Par_RequestSessionGuid == Guid.Empty | Par_ApplicationInstanceGuid == Guid.Empty)
            {
                //No recibe todos los parametros
                dtError = AccesoDatos.EventResponse_s_ByInternalCode(2).Copy();
                dtError.TableName = "tablaError2";
                dtValida = dtError;

            }
            else
            {
                DataRow rowxml;
                DataRow rowxmlError;
                dsXml.Tables.Add("AuditTrailSessionDetRequest");
                dsXml.Tables["AuditTrailSessionDetRequest"].Columns.Add("Par_Event_Tag", typeof(string));
                dsXml.Tables["AuditTrailSessionDetRequest"].Columns.Add("Par_RequestSessionGuid", typeof(Guid));
                dsXml.Tables["AuditTrailSessionDetRequest"].Columns.Add("Par_ApplicationInstanceGuid", typeof(Guid));
                dsXml.Tables["AuditTrailSessionDetRequest"].Columns.Add("Par_AutenticationTypeTag", typeof(string));
                dsXml.Tables["AuditTrailSessionDetRequest"].Columns.Add("Par_DomainGUID", typeof(Guid));
                dsXml.Tables["AuditTrailSessionDetRequest"].Columns.Add("Par_MenuGUID", typeof(Guid));
                dsXml.Tables["AuditTrailSessionDetRequest"].Columns.Add("Par_ListOfEntityGuids", typeof(string));
                dsXml.Tables["AuditTrailSessionDetRequest"].Columns.Add("Par_PersonDocument", typeof(string));

                rowxml = dsXml.Tables["AuditTrailSessionDetRequest"].NewRow();
                rowxml["Par_Event_Tag"] = Par_Event_Tag;
                rowxml["Par_RequestSessionGuid"] = Par_RequestSessionGuid;
                rowxml["Par_ApplicationInstanceGuid"] = Par_ApplicationInstanceGuid;
                rowxml["Par_AutenticationTypeTag"] = Par_AutenticationTypeTag;
                rowxml["Par_DomainGUID"] = Par_DomainGUID;
                rowxml["Par_MenuGUID"] = Par_MenuGUID;
                rowxml["Par_ListOfEntityGuids"] = Par_ListOfEntityGuids;
                rowxml["Par_PersonDocument"] = Par_PersonDocument;

                dsXml.Tables["AuditTrailSessionDetRequest"].Rows.Add(rowxml);
                rowxml.AcceptChanges();

                //Estructura del dataset error
                dsXmlError.Tables.Add("AuditTrailSessionDetResponse");
                dsXmlError.Tables["AuditTrailSessionDetResponse"].Columns.Add("EventResponseId", typeof(int));
                dsXmlError.Tables["AuditTrailSessionDetResponse"].Columns.Add("EventResponseText", typeof(string));
                dsXmlError.Tables["AuditTrailSessionDetResponse"].Columns.Add("EventResponseInternalCode", typeof(int));

                if (Par_Event_Tag == "USERS-ENABLED" || Par_Event_Tag == "MENU-PERMISOS")
                {
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_ApplicationInstanceGuid"].ColumnMapping = MappingType.Hidden;
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_AutenticationTypeTag"].ColumnMapping = MappingType.Hidden;
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_DomainGUID"].ColumnMapping = MappingType.Hidden;
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_MenuGUID"].ColumnMapping = MappingType.Hidden;
                }
                else if (Par_Event_Tag == "APPLICATION-BYPERSONS")
                {
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_RequestSessionGUID"].ColumnMapping = MappingType.Hidden;
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_PersonDocument"].ColumnMapping = MappingType.Hidden;
                }
                else if (Par_Event_Tag == "ENTITY-PERMISSIONS")
                {
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_RequestSessionGUID"].ColumnMapping = MappingType.Hidden;
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_ApplicationInstanceGuid"].ColumnMapping = MappingType.Hidden;
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_ListOfEntityGuids"].ColumnMapping = MappingType.Hidden;
                }
                else
                {
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_ApplicationInstanceGuid"].ColumnMapping = MappingType.Hidden;
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_AutenticationTypeTag"].ColumnMapping = MappingType.Hidden;
                    dsXml.Tables["AuditTrailSessionDetRequest"].Columns["Par_DomainGUID"].ColumnMapping = MappingType.Hidden;
                }

                xmlRequest = ToStringAsXml(dsXml);

                dtEvento = AccesoDatos.Event_s_ByTag(Par_Event_Tag);
                //Existen los datos del evento?
                if (dtEvento.Rows.Count > 0)
                {
                    DataSet dsGuidSessionValido = new DataSet();
                    int idEvento = int.Parse(dtEvento.Rows[0][0].ToString());
                    dsGuidSessionValido = AccesoDatos.AuditTrailSession_s_ByAuditTrailSessionGUID_Valid(Par_RequestSessionGuid);

                    dtGuidSessionValido = dsGuidSessionValido.Tables[0];

                    //El guid de session es valido?
                    if (dtGuidSessionValido.Columns.Count > 1)
                    {
                        Par_AuditTrailSessionId = int.Parse(dtGuidSessionValido.Rows[0][0].ToString());
                        //El usuario es valido?
                        dtUser = AccesoDatos.User_s_BySessionGUID_Valid(Par_RequestSessionGuid);

                        if (dtUser.Columns.Count > 1)
                        {
                            Guid wUserGuid = new Guid(dtUser.Rows[0]["UserGUID"].ToString());
                            int wUserId = (int)dtUser.Rows[0]["UserId"];


                            //Consulto si la instancia de la aplicacion es valida
                            dtInstanceApp = AccesoDatos.ApplicationInstance_s_ByGUID_Valid(Par_ApplicationInstanceGuid);

                            if (dtInstanceApp.Rows.Count > 0)
                            {
                                //verifica si la instancia de aplicación valida grupos de Active Directory para conceder permisos sobre los controles de las UI
                                bool validaAD = bool.Parse(dtInstanceApp.Rows[0][6].ToString());
                                DataTable dtGrupos, dtGuidGrupo, dtAuxPermisos, dtUserMenuPermission = new DataTable();//dtAuxMenuPermisos
                                string strNombreGrupo;
                                Guid guidGrupo;
                                int wApplicationId = (int)dtInstanceApp.Rows[0]["ApplicationId"];
                                int wApplicationInstanceId = (int)dtInstanceApp.Rows[0]["ApplicationInstanceId"];
                                dtPermisos = null;
                                var dtPruebaPermisos = new DataTable();

                                switch (Par_Event_Tag)
                                {
                                    case "CONTROL-PERMISOS":
                                        #region CaseControlPermisos

                                        {
                                            if (Par_MenuGUID != Guid.Empty & Par_MenuGUID.ToString() != "" & Par_MenuGUID != null)
                                            {
                                                DataTable dtMenuValid = new DataTable();
                                                dtMenuValid = AccesoDatos.Menu_s_ByMenuGUID_Valid(Par_MenuGUID);
                                                //Guid de menu valido?
                                                if (dtMenuValid.Rows.Count > 0)
                                                {
                                                    //Verifica que la instancia de aplicación valida grupos de Active Directory para conceder permisos sobre los controles de las UI
                                                    if (validaAD)
                                                    {
                                                        //Verifica si el usuario inició sesión con autenticación “Windows”
                                                        if (Par_AutenticationTypeTag == "WINDOWS")
                                                        {
                                                            //El sistema obtiene los datos del dominio con que el usuario inició sesión
                                                            dtDomain = AccesoDatos.Domain_s_ByGUID(Par_DomainGUID);

                                                            if (dtDomain.Rows.Count > 0)
                                                            {
                                                                //verifica si puede conectarse al controlador del dominio de sesión mediante el protocolo LDAP

                                                                ActiveDirectory objAD = new ActiveDirectory();
                                                                //Pregunto si se puede conectar al controlador de dominio
                                                                if (objAD.IsAuthenticated(dtDomain.Rows[0][2].ToString(), dtDomain.Rows[0][3].ToString(), Encriptador.desencriptar(dtDomain.Rows[0][4].ToString())) == true)
                                                                {
                                                                    dtGrupos = objAD.GetGroups(dtDomain.Rows[0][2].ToString(), dtDomain.Rows[0][3].ToString(), dtDomain.Rows[0][4].ToString(), dtUser.Rows[0][1].ToString()).Copy();
                                                                    //existen grupos?
                                                                    if (dtGrupos.Rows.Count > 0)
                                                                    {
                                                                        //dtPermisos = dtAuxPermisos;
                                                                        //CICLO FOR RECORRIENDO DTGRUPOS
                                                                        bool entro = false;
                                                                        for (int i = 0; i <= dtGrupos.Rows.Count - 1; i++)
                                                                        {
                                                                            strNombreGrupo = dtGrupos.Rows[i][0].ToString();

                                                                            //POR CADA GRUPO OBTENER LOS GUID CORRESPONDIENTES                                                               
                                                                            dtGuidGrupo = AccesoDatos.Group_s_ByName(strNombreGrupo).Copy();

                                                                            if (dtGuidGrupo.Rows.Count > 0)
                                                                            {
                                                                                guidGrupo = new Guid(dtGuidGrupo.Rows[0][5].ToString());
                                                                                dtUserMenuPermission = AccesoDatos.MenuPermission_s_ByMenuGUID(Par_ApplicationInstanceGuid, guidGrupo, Par_DomainGUID, Par_MenuGUID);

                                                                                //El usuario tiene permiso sobre los elementos del menu?
                                                                                if (dtUserMenuPermission.Rows.Count > 0)
                                                                                {
                                                                                    entro = true;
                                                                                    DataTable dtOnlyAllowedControls = AccesoDatos.ControlPermission_s_ByGroupGUID(Par_ApplicationInstanceGuid, guidGrupo, Par_MenuGUID, Par_DomainGUID);

                                                                                    //Busca todos los controles que tiene configurada la aplicación para ese menu
                                                                                    DataTable dtFullControls = AccesoDatos.ControlUserPermission_s_byMenuGuid(Par_MenuGUID);

                                                                                    dtPermisos = RemoveDuplicateRows(dtFullControls, dtOnlyAllowedControls);
                                                                                }
                                                                                else
                                                                                {
                                                                                    if ((entro != true) && (i == dtGrupos.Rows.Count - 1))
                                                                                    {
                                                                                        //El usuario no tiene permisos sobre los elementos del menu
                                                                                        dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                                                        rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                                        rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                                        rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                                        rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                                        dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                                        txtError = ToStringAsXml(dsXmlError);
                                                                                        AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                                        dtValida = dtError;
                                                                                        dtValida.TableName = "Error";
                                                                                        return dtValida;
                                                                                    }

                                                                                }
                                                                            }
                                                                        }


                                                                        //if (primeravez == false)
                                                                        //{
                                                                        if (dtPermisos.Rows.Count > 0)
                                                                        {
                                                                            if (dtPermisos.Rows.Count > 0)
                                                                            {

                                                                                dsPermisos.Tables.Add(dtPermisos);
                                                                                dsPermisos.Tables[0].TableName = "CONTROL-PERMISOS";
                                                                                xmlResponse = ToStringAsXml(dsPermisos);

                                                                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, xmlResponse, xmlRequest);

                                                                                dtValida.Columns.Add("XMLCONTROLES", typeof(string));
                                                                                row = dtValida.NewRow();
                                                                                row[0] = xmlResponse;
                                                                                dtValida.Rows.Add(row);
                                                                            }
                                                                            else
                                                                            {
                                                                                //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                                //los grupos activedirectory del usuario
                                                                                //dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                                                txtError = string.Empty; //dtError.Rows[0][1].ToString();
                                                                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                                //dtValida = dtError;
                                                                            }

                                                                        }
                                                                        else
                                                                        {
                                                                            //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                            //los grupos activedirectory del usuario
                                                                            //dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                                            txtError = string.Empty; //dtError.Rows[0][1].ToString();
                                                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                            //dtValida = dtError;
                                                                        }
                                                                        //}
                                                                        //else
                                                                        //{
                                                                        //    //Ninguno de los Grupos Active Directory del usuario se encuentra 
                                                                        //    //registrado en el Sistema de Seguridad
                                                                        //    dtError = AccesoDatos.EventResponse_s_ByInternalCode(43).Copy();
                                                                        //    rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                        //    rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                        //    rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                        //    rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                        //    dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                        //    txtError = ToStringAsXml(dsXmlError);
                                                                        //    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                        //    dtValida = dtError;
                                                                        //}

                                                                    }
                                                                    else
                                                                    {
                                                                        //No encuentra grupos de AD para el usuario
                                                                        dtError = AccesoDatos.EventResponse_s_ByInternalCode(27).Copy();
                                                                        rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                        rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                        rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                        rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                        dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                        txtError = ToStringAsXml(dsXmlError);
                                                                        AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                        dtValida = dtError;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //No puede contectarse al controlador de dominio
                                                                    dtError = AccesoDatos.EventResponse_s_ByInternalCode(24).Copy();
                                                                    rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                    rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                    rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                    rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                    dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                    txtError = ToStringAsXml(dsXmlError);
                                                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                    dtValida = dtError;
                                                                }

                                                            }
                                                        }
                                                        else
                                                        {
                                                            //No valida grupos de AD
                                                            DataTable dtValidaUserPermission = new DataTable();

                                                            dtValidaUserPermission = AccesoDatos.MenuUserPermission_s_ByMenuGUID_Valid(Par_ApplicationInstanceGuid, wUserGuid, Par_MenuGUID);

                                                            if (dtValidaUserPermission.Rows.Count > 0)
                                                            {
                                                                //Busca los permisos sobre los controles que tiene un usuario
                                                                DataTable dtOnlyAllowedControls = AccesoDatos.ControlUserPermission_s(Par_ApplicationInstanceGuid, wUserGuid, Par_MenuGUID);

                                                                //Busca todos los controles que tiene configurada la aplicación para ese menu
                                                                DataTable dtFullControls = AccesoDatos.ControlUserPermission_s_byMenuGuid(Par_MenuGUID);

                                                                dtPermisos = RemoveDuplicateRows(dtFullControls, dtOnlyAllowedControls);

                                                                ////Elimina todos los repetidos a partir de un registro que sea ControlEnabled = False
                                                                if (dtPermisos.Rows.Count > 0)
                                                                {

                                                                    if (dtPermisos.Rows.Count > 0)
                                                                    {
                                                                        //Ocultamos las siguientes columnas definidas en el datatable
                                                                        dtPermisos.Columns["ControlId"].ColumnMapping = MappingType.Hidden;

                                                                        dsPermisos.Tables.Add(dtPermisos);
                                                                        dsPermisos.Tables[0].TableName = "CONTROL-PERMISOS";
                                                                        xmlResponse = ToStringAsXml(dsPermisos);

                                                                        AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, xmlResponse, xmlRequest);

                                                                        dtValida.Columns.Add("XMLCONTROLES", typeof(string));
                                                                        row = dtValida.NewRow();
                                                                        row[0] = xmlResponse;

                                                                        dtValida.Rows.Add(row);
                                                                    }
                                                                    else
                                                                    {
                                                                        //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                        //los grupos activedirectory del usuario

                                                                        txtError = string.Empty;
                                                                        AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                        //dtValida = dtError;
                                                                    }

                                                                }
                                                                else
                                                                {
                                                                    //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                    //los grupos activedirectory del usuario

                                                                    txtError = string.Empty;
                                                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                    //dtValida = dtError;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //El usuario no posee permiso sobre el elemento del menu seleccionado
                                                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                                rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                txtError = ToStringAsXml(dsXmlError);

                                                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                                                dtValida = dtError;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //No valida grupos de AD
                                                        DataTable dtValidaUserPermission = new DataTable();

                                                        dtValidaUserPermission = AccesoDatos.MenuUserPermission_s_ByMenuGUID_Valid(Par_ApplicationInstanceGuid, wUserGuid, Par_MenuGUID);

                                                        if (dtValidaUserPermission.Rows.Count > 0)
                                                        {
                                                            //Busca los permisos sobre los controles que tiene un usuario
                                                            DataTable dtOnlyAllowedControls = AccesoDatos.ControlUserPermission_s(Par_ApplicationInstanceGuid, wUserGuid, Par_MenuGUID);

                                                            //Busca todos los controles que tiene configurada la aplicación para ese menu
                                                            DataTable dtFullControls = AccesoDatos.ControlUserPermission_s_byMenuGuid(Par_MenuGUID);

                                                            dtPermisos = RemoveDuplicateRows(dtFullControls, dtOnlyAllowedControls);

                                                            ////Elimina todos los repetidos a partir de un registro que sea ControlEnabled = False
                                                            if (dtPermisos.Rows.Count > 0)
                                                            {

                                                                if (dtPermisos.Rows.Count > 0)
                                                                {
                                                                    dsPermisos.Tables.Add(dtPermisos);
                                                                    dsPermisos.Tables[0].TableName = "CONTROL-PERMISOS";
                                                                    xmlResponse = ToStringAsXml(dsPermisos);

                                                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, xmlResponse, xmlRequest);

                                                                    dtValida.Columns.Add("XMLCONTROLES", typeof(string));
                                                                    row = dtValida.NewRow();
                                                                    row[0] = xmlResponse;

                                                                    dtValida.Rows.Add(row);
                                                                }
                                                                else
                                                                {
                                                                    //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                    //los grupos activedirectory del usuario
                                                                    txtError = string.Empty;
                                                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                    //dtValida = dtError;
                                                                }

                                                            }
                                                            else
                                                            {
                                                                //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                //los grupos activedirectory del usuario

                                                                txtError = string.Empty;
                                                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                //dtValida = dtError;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //El usuario no posee permiso sobre el elemento del menu seleccionado
                                                            dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                            rowxmlError = dsXmlError.Tables[0].NewRow();
                                                            rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                            rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                            rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                            dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                            txtError = ToStringAsXml(dsXmlError);

                                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                                            dtValida = dtError;
                                                        }

                                                    }

                                                }
                                                else
                                                {
                                                    //Guid de menu no valido
                                                    dtError = AccesoDatos.EventResponse_s_ByInternalCode(26).Copy();
                                                    dtValida = dtError;
                                                }
                                            }
                                            else
                                            {
                                                //No recibe todos los parametros
                                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(2).Copy();
                                                dtValida = dtError;
                                            }

                                        }
                                        break;
                                    #endregion
                                    case "USERS-ENABLED":
                                        #region CaseUsersEnabled

                                        //obtiene todos los usuarios habilitados para la instancia de aplicación
                                        dtUsersEnabled = AccesoDatos.User_s_ByApplicationInstanceGUID(Par_ApplicationInstanceGuid).Copy();

                                        if (dtUsersEnabled.Rows.Count > 0)
                                        {
                                            DataSet dsUsers = new DataSet();
                                            //Ocultamos las siguientes columnas definidas en el datatable
                                            dtUsersEnabled.Columns["UserId"].ColumnMapping = MappingType.Hidden;
                                            dtUsersEnabled.Columns["PersonId"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["UserActiveFlag"].ColumnMapping = MappingType.Hidden;
                                            dtUsersEnabled.Columns["UserModifiedByUser"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["Par_ApplicationInstanceGuid"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["Par_AutenticationTypeTag"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["Par_DomainGUID"].ColumnMapping = MappingType.Hidden;

                                            dtValida = dtUsersEnabled;

                                            dsUsers.Tables.Add(dtUsersEnabled);
                                            dsUsers.Tables[0].TableName = "USER-ENABLED";
                                            xmlResponse = ToStringAsXml(dsUsers);

                                            //dtValida.Columns.Add("XMLUSERS", typeof(string));
                                            //row = dtValida.NewRow();
                                            //row[0] = xmlResponse;
                                            //dtValida.Rows.Add(row);

                                            //registra en el detalle de pista de auditoría de sesión  una solicitud de usuarios habilitados de una instancia de aplicación
                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, xmlResponse, xmlRequest);

                                        }
                                        else
                                        {
                                            //Usuario no existe en la instancia de aplicacion o bien no esta vigente en la misma
                                            dtError = AccesoDatos.EventResponse_s_ByInternalCode(25).Copy();
                                            rowxmlError = dsXmlError.Tables[0].NewRow();
                                            rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                            rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                            rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                            dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                            txtError = ToStringAsXml(dsXmlError);

                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                            dtValida = dtError;
                                        }
                                        break;
                                    #endregion
                                    case "APPLICATION-BYPERSONS":
                                        #region CaseUsersEnabled

                                        //obtiene todos los usuarios habilitados para la instancia de aplicación
                                        dtUsersEnabled = AccesoDatos.Application_s_byPersonDocument(Par_PersonDocument).Copy();

                                        if (dtUsersEnabled.Rows.Count > 0)
                                        {
                                            DataSet dsUsers = new DataSet();
                                            //Ocultamos las siguientes columnas definidas en el datatable
                                            //dtUsersEnabled.Columns["UserId"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["PersonId"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["UserActiveFlag"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["UserModifiedByUser"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["Par_ApplicationInstanceGuid"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["Par_AutenticationTypeTag"].ColumnMapping = MappingType.Hidden;
                                            //dtUsersEnabled.Columns["Par_DomainGUID"].ColumnMapping = MappingType.Hidden;

                                            dtValida = dtUsersEnabled;

                                            dsUsers.Tables.Add(dtUsersEnabled);
                                            dsUsers.Tables[0].TableName = "USER-ENABLED";
                                            xmlResponse = ToStringAsXml(dsUsers);

                                            //dtValida.Columns.Add("XMLUSERS", typeof(string));
                                            //row = dtValida.NewRow();
                                            //row[0] = xmlResponse;
                                            //dtValida.Rows.Add(row);

                                            //registra en el detalle de pista de auditoría de sesión  una solicitud de usuarios habilitados de una instancia de aplicación
                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, xmlResponse, xmlRequest);

                                        }
                                        else
                                        {
                                            //Usuario no existe en la instancia de aplicacion o bien no esta vigente en la misma
                                            dtError = AccesoDatos.EventResponse_s_ByInternalCode(25).Copy();
                                            rowxmlError = dsXmlError.Tables[0].NewRow();
                                            rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                            rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                            rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                            dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                            txtError = ToStringAsXml(dsXmlError);

                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                            dtValida = dtError;
                                        }
                                        break;
                                    #endregion
                                    case "MENU-PERMISOS":
                                        #region CaseMenuPermisos

                                        //Verifica que la instancia de aplicación valida grupos de Active Directory para conceder permisos sobre los controles de las UI
                                        if (validaAD)
                                        {
                                            //Verifica si el usuario inició sesión con autenticación “Windows”
                                            if (Par_AutenticationTypeTag == "WINDOWS")
                                            {
                                                //El sistema obtiene los datos del dominio con que el usuario inició sesión
                                                dtDomain = AccesoDatos.Domain_s_ByGUID(Par_DomainGUID);

                                                if (dtDomain.Rows.Count > 0)
                                                {
                                                    //verifica si puede conectarse al controlador del dominio de sesión mediante el protocolo LDAP

                                                    ActiveDirectory objAD = new ActiveDirectory();
                                                    //Pregunto si se puede conectar al controlador de dominio
                                                    if (objAD.IsAuthenticated(dtDomain.Rows[0][2].ToString(), dtDomain.Rows[0][3].ToString(), Encriptador.desencriptar(dtDomain.Rows[0][4].ToString())) == true)
                                                    {

                                                        dtGrupos = objAD.GetGroups(dtDomain.Rows[0][2].ToString(), dtDomain.Rows[0][3].ToString(), Encriptador.desencriptar(dtDomain.Rows[0][4].ToString()), dtUser.Rows[0][1].ToString()).Copy();
                                                        //existen grupos?
                                                        if (dtGrupos.Rows.Count > 0)
                                                        {
                                                            int groupRegistrados = 0;
                                                            //CICLO FOR RECORRIENDO DTGRUPOS
                                                            for (int i = 0; i <= dtGrupos.Rows.Count - 1; i++)
                                                            {
                                                                strNombreGrupo = dtGrupos.Rows[i][0].ToString();

                                                                //POR CADA GRUPO OBTENER LOS GUID CORRESPONDIENTES                                                               
                                                                dtGuidGrupo = AccesoDatos.Group_s_ByName(strNombreGrupo).Copy();

                                                                if (dtGuidGrupo.Rows.Count > 0)
                                                                {
                                                                    //Controlo la cantidad de grupos que se tuvo en cuenta
                                                                    groupRegistrados += 1;

                                                                    guidGrupo = new Guid(dtGuidGrupo.Rows[0][5].ToString());
                                                                    dtAuxPermisos = AccesoDatos.MenuPermission_s_ByGroupGUID(Par_ApplicationInstanceGuid, guidGrupo, Par_DomainGUID).Copy();


                                                                    if (primeravez)
                                                                    {
                                                                        dtPermisos = dtAuxPermisos.Clone();
                                                                        primeravez = false;
                                                                    }

                                                                    for (int k = 0; k <= dtAuxPermisos.Rows.Count - 1; k++)
                                                                    {
                                                                        dtPermisos.ImportRow(dtAuxPermisos.Rows[k]);
                                                                    }

                                                                }

                                                            }

                                                            if (primeravez == false)
                                                            {
                                                                if (dtPermisos != null)
                                                                {
                                                                    if (dtPermisos.Rows.Count > 0)
                                                                    {

                                                                        //Recorro para eliminar los repetidos a partir de un menu con enabled=false
                                                                        for (int k = 0; k <= dtPermisos.Rows.Count - 1; k++)
                                                                        {

                                                                            //Empiezo a controlar cuando un elemento es false
                                                                            if (bool.Parse(dtPermisos.Rows[k][5].ToString()) == false)
                                                                            {
                                                                                for (int j = 0; j <= dtPermisos.Rows.Count - 1; j++)
                                                                                {
                                                                                    if (k == -1)
                                                                                    {
                                                                                        k = 0;
                                                                                    }
                                                                                    //Si es igual menos en la posicion en la que encontre el elemento falso
                                                                                    if (dtPermisos.Rows[k][1].ToString() == dtPermisos.Rows[j][1].ToString() & j != k)
                                                                                    {
                                                                                        dtPermisos.Rows[j].Delete();
                                                                                        dtPermisos.AcceptChanges();
                                                                                        j -= 1;
                                                                                        k -= 1;
                                                                                    }

                                                                                }
                                                                            }
                                                                        }

                                                                        //Filtro para que me devuelva solo un resultado en caso de repetidos
                                                                        DataView vista = new DataView(dtPermisos);
                                                                        DataTable dtPermisosFinal = vista.ToTable(true, "MenuId", "MenuName", "MenuParentId", "MenuParentGUID", "MenuGUID", "MenuEnabled", "MenuLevel", "MenuLabel", "MenuOrder", "MenuCalledUI", "TypeCalledUITag", "MenuIcono", "MenuCreated", "MenuActiveFlag", "MenuModifiedByUser", "UserName");

                                                                        //Filtramos los registros habilitados                                                            
                                                                        dvPermisos = dtPermisosFinal.DefaultView;
                                                                        dvPermisos.RowFilter = "MenuEnabled = True";
                                                                        dtPermisos = dvPermisos.ToTable();
                                                                        if (dtPermisos.Rows.Count > 0)
                                                                        {
                                                                            //Ocultamos las siguientes columnas definidas en el datatable
                                                                            //dtPermisos.Columns["MenuId"].ColumnMapping = MappingType.Hidden;
                                                                            //dtPermisos.Columns["MenuParentId"].ColumnMapping = MappingType.Hidden;

                                                                            dtPermisos.Columns["MenuEnabled"].ColumnMapping = MappingType.Hidden;
                                                                            //dtPermisos.Columns["TypeCalledUIId"].ColumnMapping = MappingType.Hidden;

                                                                            dsPermisos.Tables.Add(dtPermisos);
                                                                            dsPermisos.Tables[0].TableName = "MENU-PERMISOS";
                                                                            xmlResponse = ToStringAsXml(dsPermisos);

                                                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, xmlResponse, xmlRequest);

                                                                            dtValida.Columns.Add("XMLMENU", typeof(string));
                                                                            row = dtValida.NewRow();
                                                                            row[0] = xmlResponse;
                                                                            dtValida.Rows.Add(row);
                                                                        }
                                                                        else
                                                                        {
                                                                            //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                            //los grupos activedirectory del usuario
                                                                            dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                                            rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                            rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                            rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                            rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                            dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                            txtError = ToStringAsXml(dsXmlError);
                                                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                            dtValida = dtError;
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                        //los grupos activedirectory del usuario
                                                                        dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                                        rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                        rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                        rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                        rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                        dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                        txtError = ToStringAsXml(dsXmlError);
                                                                        AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                        dtValida = dtError;
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                                    //los grupos activedirectory del usuario
                                                                    dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                                    rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                    rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                    rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                    rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                    dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                    txtError = ToStringAsXml(dsXmlError);
                                                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                    dtValida = dtError;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                //Ninguno de los Grupos Active Directory del usuario se encuentra 
                                                                //registrado en el Sistema de Seguridad
                                                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(43).Copy();
                                                                rowxmlError = dsXmlError.Tables[0].NewRow();
                                                                rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                                rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                                rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                                dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                                txtError = ToStringAsXml(dsXmlError);
                                                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                                dtValida = dtError;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //el sistema no encontro ningun grupo active directory configurado para el usuario
                                                            dtError = AccesoDatos.EventResponse_s_ByInternalCode(27).Copy();
                                                            rowxmlError = dsXmlError.Tables[0].NewRow();
                                                            rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                            rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                            rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                            dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                            txtError = ToStringAsXml(dsXmlError);
                                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                            dtValida = dtError;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        //No puede conectarse con el controlado de dominio
                                                        dtError = AccesoDatos.EventResponse_s_ByInternalCode(24).Copy();
                                                        rowxmlError = dsXmlError.Tables[0].NewRow();
                                                        rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                        rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                        rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                        dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                        txtError = ToStringAsXml(dsXmlError);
                                                        AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                        dtValida = dtError;
                                                    }
                                                }
                                            }
                                            //Si no es WINDOWS
                                            else
                                            {
                                                //busca los permisos del usuario sobre los controles de las UI asociada al elemento de menú seleccionado, 
                                                //teniendo en cuenta los perfiles asignados y los permisos concedidos individualmente como usuario
                                                dtPermisos = AccesoDatos.MenuUserPermission_s(Par_ApplicationInstanceGuid, wUserGuid);

                                                //VENIR

                                                if (dtPermisos.Rows.Count > 0)
                                                {
                                                    //Recorro para eliminar los repetidos
                                                    for (int k = 0; k <= dtPermisos.Rows.Count - 1; k++)
                                                    {
                                                        //Empiezo a controlar cuando un elemento es false
                                                        if (bool.Parse(dtPermisos.Rows[k][5].ToString()) == false)
                                                        {
                                                            for (int j = 0; j <= dtPermisos.Rows.Count - 1; j++)
                                                            {
                                                                if (k == -1)
                                                                {
                                                                    k = 0;
                                                                }
                                                                //Si es igual menos en la posicion en la que encontre el elemento falso
                                                                if (dtPermisos.Rows[k][1].ToString() == dtPermisos.Rows[j][1].ToString() & j != k)
                                                                {
                                                                    dtPermisos.Rows[j].Delete();
                                                                    dtPermisos.AcceptChanges();
                                                                    j -= 1;
                                                                    k -= 1;
                                                                }
                                                            }
                                                        }
                                                    }

                                                    //Filtramos los registros habilitados
                                                    dvPermisos = dtPermisos.DefaultView;
                                                    dvPermisos.RowFilter = "MenuEnabled = True";
                                                    dtPermisos = dvPermisos.ToTable();

                                                    if (dtPermisos.Rows.Count > 0)
                                                    {

                                                        //Ocultamos las siguientes columnas definidas en el datatable
                                                        //dtPermisos.Columns["MenuId"].ColumnMapping = MappingType.Hidden;
                                                        //dtPermisos.Columns["MenuParentId"].ColumnMapping = MappingType.Hidden;
                                                        dtPermisos.Columns["MenuEnabled"].ColumnMapping = MappingType.Hidden;
                                                        dtPermisos.Columns["TypeCalledUIId"].ColumnMapping = MappingType.Hidden;
                                                        dtPermisos.Columns["MenuModifiedDate"].ColumnMapping = MappingType.Hidden;

                                                        dsPermisos.Tables.Add(dtPermisos);
                                                        dsPermisos.Tables[0].TableName = "MENU-PERMISOS";
                                                        xmlResponse = ToStringAsXml(dsPermisos);

                                                        AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, xmlResponse, xmlRequest);

                                                        dtValida.Columns.Add("XMLMENU", typeof(string));
                                                        row = dtValida.NewRow();
                                                        row[0] = xmlResponse;

                                                        dtValida.Rows.Add(row);
                                                    }
                                                    else
                                                    {
                                                        //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                        //los grupos activedirectory del usuario
                                                        dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                        rowxmlError = dsXmlError.Tables[0].NewRow();
                                                        rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                        rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                        rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                        dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                        txtError = ToStringAsXml(dsXmlError);
                                                        AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                        dtValida = dtError;
                                                    }
                                                }
                                                else
                                                {
                                                    //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                    //los grupos activedirectory del usuario
                                                    dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                    rowxmlError = dsXmlError.Tables[0].NewRow();
                                                    rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                    rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                    rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                    dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                    txtError = ToStringAsXml(dsXmlError);
                                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                    dtValida = dtError;
                                                }
                                            }
                                        }
                                        else
                                        {

                                            dtPermisos = AccesoDatos.MenuUserPermission_s(Par_ApplicationInstanceGuid, wUserGuid);

                                            if (dtPermisos.Rows.Count > 0)
                                            {
                                                //Recorro para eliminar los repetidos
                                                for (int k = 0; k <= dtPermisos.Rows.Count - 1; k++)
                                                {
                                                    //Empiezo a controlar cuando un elemento es false
                                                    if (bool.Parse(dtPermisos.Rows[k][5].ToString()) == false)
                                                    {
                                                        for (int j = 0; j <= dtPermisos.Rows.Count - 1; j++)
                                                        {
                                                            if (k == -1)
                                                            {
                                                                k = 0;
                                                            }
                                                            //Si es igual menos en la posicion en la que encontre el elemento falso
                                                            if (dtPermisos.Rows[k][1].ToString() == dtPermisos.Rows[j][1].ToString() & j != k)
                                                            {
                                                                dtPermisos.Rows[j].Delete();
                                                                dtPermisos.AcceptChanges();
                                                                j -= 1;
                                                                k -= 1;
                                                            }
                                                        }
                                                    }
                                                }

                                                //Filtramos los registros habilitados
                                                dvPermisos = dtPermisos.DefaultView;
                                                dvPermisos.RowFilter = "MenuEnabled = True";
                                                dtPermisos = dvPermisos.ToTable();

                                                if (dtPermisos.Rows.Count > 0)
                                                {

                                                    //Ocultamos las siguientes columnas definidas en el datatable
                                                    //dtPermisos.Columns["MenuId"].ColumnMapping = MappingType.Hidden;
                                                    //dtPermisos.Columns["MenuParentId"].ColumnMapping = MappingType.Hidden;
                                                    dtPermisos.Columns["MenuEnabled"].ColumnMapping = MappingType.Hidden;
                                                    dtPermisos.Columns["TypeCalledUIId"].ColumnMapping = MappingType.Hidden;
                                                    dtPermisos.Columns["MenuModifiedDate"].ColumnMapping = MappingType.Hidden;

                                                    dsPermisos.Tables.Add(dtPermisos);
                                                    dsPermisos.Tables[0].TableName = "MENU-PERMISOS";
                                                    xmlResponse = ToStringAsXml(dsPermisos);

                                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, xmlResponse, xmlRequest);

                                                    dtValida.Columns.Add("XMLMENU", typeof(string));
                                                    row = dtValida.NewRow();
                                                    row[0] = xmlResponse;

                                                    dtValida.Rows.Add(row);
                                                }
                                                else
                                                {
                                                    //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                    //los grupos activedirectory del usuario
                                                    dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                    rowxmlError = dsXmlError.Tables[0].NewRow();
                                                    rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                    rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                    rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                    dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                    txtError = ToStringAsXml(dsXmlError);
                                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                    dtValida = dtError;
                                                }
                                            }
                                            else
                                            {
                                                //el sistemano encuentra ningun permiso configurado para los perfiles de
                                                //los grupos activedirectory del usuario
                                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(9).Copy();
                                                rowxmlError = dsXmlError.Tables[0].NewRow();
                                                rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                                rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                txtError = ToStringAsXml(dsXmlError);
                                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);

                                                dtValida = dtError;
                                            }
                                        }
                                        break;
                                    #endregion
                                    case "ENTITY-PERMISSIONS":
                                        #region CaseEntityPermissions
                                        //paso 1 El sistema busca las entidades asociadas 
                                        var paso1 = AccesoDatos.Entity_s_ApplicationId(wApplicationId);
                                        if (paso1.Count == 0)
                                        {
                                            dtError = AccesoDatos.EventResponse_s_ByInternalCode(70).Copy();
                                            rowxmlError = dsXmlError.Tables[0].NewRow();
                                            rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                            rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                            rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                            dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                            txtError = ToStringAsXml(dsXmlError);
                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                            dtError.TableName = "Error";

                                            return dtError;
                                        }
                                        List<Guid> listOfEntityGuids = new List<Guid>();
                                        string[] pars = Par_ListOfEntityGuids.Split(';');
                                        foreach (var p in pars)
                                            if (p.Trim().Length > 0)
                                                listOfEntityGuids.Add(new Guid(p));


                                        //paso 2 El sistema verifica si existe cada entidad recibida por parámetro, y se cumple ésta condición  
                                        foreach (var wCurrentGuid in listOfEntityGuids)
                                        {
                                            if (paso1.FirstOrDefault(x => x.EntityGUID == wCurrentGuid) == null)//si es null no se encontro el guid
                                            {
                                                //7.C.2 CURSO A
                                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(71).Copy();
                                                dtError.Rows[0][1] = dtError.Rows[0][1].ToString() + " " + wCurrentGuid.ToString();
                                                dtError.TableName = "Error";
                                                dtError.AcceptChanges();
                                                rowxmlError = dsXmlError.Tables[0].NewRow();
                                                rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                                rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString() + " " + wCurrentGuid.ToString();
                                                rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                                dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                                txtError = ToStringAsXml(dsXmlError);
                                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);


                                                return dtError;
                                            }


                                        }

                                        //paso 3 El sistema busca, por cada entidad recibida por parámetro, 
                                        //los permisos de selección, edición y eliminación a cada registro de la misma, 
                                        //para el usuario. Y encuentra registros para al menos una entidad
                                        var wPermisosUnificados = new List<EntityRegisterUserPermissionBE>();

                                        foreach (var wCurrentGuid in listOfEntityGuids)
                                        {
                                            int wEntityId = paso1.FirstOrDefault(x => x.EntityGUID == wCurrentGuid).EntiyId;
                                            var permisos = AccesoDatos.EntityRegisterUserPermission_s(wEntityId, wUserId, wApplicationInstanceId);

                                            foreach (var p in permisos)
                                            {
                                                var PermisoActual = wPermisosUnificados.FirstOrDefault(x => x.EntityRegisterId == p.EntityRegisterId);
                                                if (PermisoActual == null)
                                                {
                                                    wPermisosUnificados.Add((EntityRegisterUserPermissionBE)p.Clone());
                                                }
                                                else
                                                {
                                                    PermisoActual.Edicion |= p.Edicion;
                                                    PermisoActual.Eliminacion |= p.Eliminacion;
                                                    PermisoActual.Seleccion |= p.Seleccion;
                                                }
                                            }
                                        }

                                        if (wPermisosUnificados.Count == 0)
                                        {

                                            dtError = AccesoDatos.EventResponse_s_ByInternalCode(72).Copy();
                                            rowxmlError = dsXmlError.Tables[0].NewRow();
                                            rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                            rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                            rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                            dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                            txtError = ToStringAsXml(dsXmlError);
                                            AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                            dtError.TableName = "Error";

                                            return dtError;
                                        }
                                        DataTable rv = new DataTable();
                                        rv.Columns.Add("EntityRegisterId", typeof(int));
                                        rv.Columns.Add("EntityRegisterName", typeof(string));
                                        rv.Columns.Add("EntityRegisterGUID", typeof(Guid));
                                        rv.Columns.Add("EntityRegisterPk", typeof(string));
                                        rv.Columns.Add("EntityRegisterElementGUID", typeof(Guid));
                                        rv.Columns.Add("EntityRegisterParentGUID", typeof(Guid));
                                        rv.Columns.Add("Edicion", typeof(bool));
                                        rv.Columns.Add("Eliminacion", typeof(bool));
                                        rv.Columns.Add("Seleccion", typeof(bool));
                                        rv.TableName = "EntityPermissions";
                                        foreach (var n in wPermisosUnificados)
                                        {
                                            rv.Rows.Add(new object[] { n.EntityRegisterId,
                                                n.EntityRegisterName,
                                                n.EntityRegisterGUID,
                                                n.EntityRegisterPk,
                                                n.EntityRegisterElementGUID,
                                                n.EntityRegisterParentGUID,
                                                n.Edicion,
                                                n.Eliminacion,
                                                n.Seleccion });
                                        }
                                        dtValida = rv;

                                        break;
                                    #endregion
                                    case "PERSON-ATTRIBUTES":
                                        #region PERSON-ATTRIBUTES
                                        var atributos = AccesoDatos.Attributes_s_byPersonGuid(Par_PersonGuidForAttributes);
                                        dtValida = atributos;
                                        break;
                                        #endregion
                                }
                            }
                            else
                            {
                                //Detecta que la instancia de la aplicacion no es valida
                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(1).Copy();
                                //CONSULTAR SOBRE EL PARAMETRO PAR_AUDITTRAILSESSIONDETREQUEST
                                rowxmlError = dsXmlError.Tables[0].NewRow();
                                rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                txtError = ToStringAsXml(dsXmlError);
                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                dtValida = dtError;
                            }
                        }
                        else
                        {
                            codigoError = int.Parse(dtUser.Rows[0][0].ToString());
                            switch (codigoError)
                            {
                                //Usuario no existe, no esta activo o no vigente
                                case 6:
                                    dtError = AccesoDatos.EventResponse_s_ByInternalCode(6).Copy();
                                    //CONSULTAR SOBRE EL PARAMETRO PAR_AUDITTRAILSESSIONDETREQUEST
                                    rowxmlError = dsXmlError.Tables[0].NewRow();
                                    rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                    rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                    rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                    dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                    txtError = ToStringAsXml(dsXmlError);
                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                    dtValida = dtError;
                                    break;
                                //Usuario no existe o no esta vigente para la instancia de aplicacion
                                case 25:
                                    dtError = AccesoDatos.EventResponse_s_ByInternalCode(25).Copy();
                                    rowxmlError = dsXmlError.Tables[0].NewRow();
                                    rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                    rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                    rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                    dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                    txtError = ToStringAsXml(dsXmlError);
                                    //CONSULTAR SOBRE EL PARAMETRO PAR_AUDITTRAILSESSIONDETREQUEST
                                    AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                    dtValida = dtError;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        codigoError = int.Parse(dtGuidSessionValido.Rows[0][0].ToString());

                        switch (codigoError)
                        {
                            //No existe el Guid de session
                            case 5:
                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(5).Copy();
                                dtValida = dtError;
                                break;
                            //Caduco el Guid de session
                            case 20:

                                Par_AuditTrailSessionId = int.Parse(dsGuidSessionValido.Tables[1].Rows[0][0].ToString());
                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(20).Copy();
                                rowxmlError = dsXmlError.Tables[0].NewRow();
                                rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                txtError = ToStringAsXml(dsXmlError);
                                //CONSULTAR SOBRE EL PARAMETRO PAR_AUDITTRAILSESSIONDETREQUEST
                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                dtValida = dtError;
                                break;
                            //GUID VALIDO PERO ALGUNA TABLA NO CUMPLE CON ACTIVEFLAG O VIGENCIA
                            case 40:
                                Par_AuditTrailSessionId = int.Parse(dsGuidSessionValido.Tables[1].Rows[0][0].ToString());
                                dtError = AccesoDatos.EventResponse_s_ByInternalCode(40).Copy();
                                rowxmlError = dsXmlError.Tables[0].NewRow();
                                rowxmlError["EventResponseId"] = dtError.Rows[0][0].ToString();
                                rowxmlError["EventResponseText"] = dtError.Rows[0][1].ToString();
                                rowxmlError["EventResponseInternalCode"] = dtError.Rows[0][2].ToString();
                                dsXmlError.Tables[0].Rows.Add(rowxmlError);
                                txtError = ToStringAsXml(dsXmlError);
                                //CONSULTAR SOBRE EL PARAMETRO PAR_AUDITTRAILSESSIONDETREQUEST
                                AccesoDatos.AuditTrailSessionDet_i(Par_AuditTrailSessionId, idEvento, txtError, xmlRequest);
                                dtValida = dtError;
                                break;
                        }
                    }
                }
                else
                {
                    //No encuentra los datos del evento
                    dtError = AccesoDatos.EventResponse_s_ByInternalCode(22).Copy();
                    dtValida = dtError;
                }
            }

            if (dtValida.TableName.Length == 0)
                dtValida.TableName = "NewTable";
            return dtValida;

        }

        public string ToStringAsXml(DataSet ds)
        {

            return ds.GetXml();
            //StringWriter sw = new StringWriter();
            //ds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
            //string s = sw.ToString();

            //return s;

        }

        public DataTable ToXmlAsDatatable(string xmlString)
        {
            DataSet ds = new DataSet();
            StringReader sr = new StringReader(xmlString);
            ds.ReadXml(sr);
            return ds.Tables[0];

        }


        /// <summary>
        /// Compara dos datatables con controles del sistema de seguridad, evaluando si dicho control debe o no ser mostrado, además se eliminan controles repetidos 
        /// </summary>
        /// <param name="dTableFull">DataTable con todos los controles que posee la aplicación</param>
        /// <param name="dTableOnlyAllowed">DataTable con todos los controles que posee EL USUARIO a través de los permisos de su perfil o grupo</param>
        /// <returns>DataTable con dTableFull con una columna que indica si el control tiene permisos para ser usado por el usuario </returns>
        public DataTable RemoveDuplicateRows(DataTable dTableFull, DataTable dTableOnlyAllowed)
        {
            //dTableFull.Columns.Add("ControlEnabled");
            Hashtable hTable = new Hashtable(); //<-- contiene como clave el id del control, y como valor el estado del control (true/false)
            List<DataRow> duplicateList = new List<DataRow>(); //<--- en esta lista guardare las rows repetidas, para luego reconocerlas en el datatable y eliminarlas

            int contadorNumRow = 0;
            foreach (DataRow drow in dTableFull.Rows)
            {

                if (hTable.ContainsKey(drow["ControlId"].ToString()))
                {

                    duplicateList.Add(drow); //<<--guardo el dublipado en la lista de los q se van

                }

                else
                {
                    hTable.Add(drow["ControlId"].ToString(), drow["ControlEnabled"]);
                }

                foreach (DataRow wRow in dTableOnlyAllowed.Rows)
                {


                    if (drow["ControlId"].ToString() == wRow["ControlId"].ToString())
                    {
                        drow["ControlEnabled"] = wRow["ControlEnabled"];

                        if (!bool.Parse(wRow["ControlEnabled"].ToString()))//<---Se descubrio que el control tiene permisos denegados para este usuario. No importa que tenga permisos mediante otra via, este control no podra ser visto por el usuario
                            break;
                    }


                }
                contadorNumRow++; //<-- aumento el contador
            }



            foreach (DataRow dRow in duplicateList)
                dTableFull.Rows.Remove(dRow);

            return dTableFull;
        }



    }
}