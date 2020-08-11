using epironApi.webApi.common;
using epironApi.webApi.DAC;
using epironApi.webApi.models;
using EpironApi.webApi.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static epironApi.webApi.common.Common;

namespace EpironApi.webApi.BC
{
    public class Update_recivedStatusBC
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        public void Proc(BotCommentModeratedReq req)
        {
            var actionId = (int)Enum.Parse(typeof(Common.HookResponsesEnum), req.Action);
            //Actualizo
            EpironDAC.Bot_update_response(req.CaseCommentGUID, req.CaseId, actionId, req.Text);



            if (actionId == (int)Common.HookResponsesEnum.Responder)
            {
                AccountOutputChannelTypeBE selectedAccountOutputChannelType = null;

                #region	Obtener tipo de elemento de salida:

                List<AccountOutputChannelTypeBE> etOutputList = EpironDAC.ElementTypeOutput(req.CaseCommentGUID, req.CaseId);

                if (etOutputList.Count == 0)
                {
                    throw new Exception("No hay datos al obtener tipo de elemento de salida para el caso ");
                }

                // sp devuelve solo un registro se debe tomar el valor del campo ElementTypeOutputId
                if (etOutputList.Count == 1)
                {
                    selectedAccountOutputChannelType = etOutputList[0];
                }
                //Si el sp devuelve más de un registro Se debe tomar el valor del campo ElementTypeOutputId donde InputPublic = OutputPublic
                if (etOutputList.Count > 1)
                {
                    selectedAccountOutputChannelType = etOutputList.Where(p => p.OutputPublic == p.InputPublic).FirstOrDefault();


                }


                #endregion


                #region Obtención de datos para publicar
                var caseComment = EpironDAC.CaseComment(req.CaseCommentGUID);
                int? accountId = caseComment.AccountId;
                #endregion


                #region Obtención de usuario bot
                //Identificador del registro correspondiente al usuario Bot – API Epiron (valor = 216)
                var settings = EpironDAC.ApplicationSettings(216, accountId);
                #endregion

                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }))
                {
                    #region Insertar en la Comentario saliente y publicación:

                    //Identificador del registro correspondiente al usuario Bot – API Epiron (valor = 216)
                    CaseCommentCreationBE caseComment_to_create = new CaseCommentCreationBE();

                    //identificador del caso al cual se le está dando una respuesta. 
                    caseComment_to_create.CaseId = req.CaseId;

                    //Se pasa el valor del parámetro @CaseCommentTextToPublication.

                    if (selectedAccountOutputChannelType.PChannelType == (int)PChannelTypeEnum.Mail)
                    {
                        StringBuilder str = new StringBuilder();
                        str.AppendLine(req.Text);
                        str.AppendLine(caseComment.AccountEmailBodyTemplate);
                        str.AppendLine("--------------------------------------------------------------------");

                        str.AppendLine(caseComment.CaseCommentText);
                        caseComment_to_create.CaseCommentText = str.ToString();
                    }

                    if (selectedAccountOutputChannelType.PChannelType == (int)PChannelTypeEnum.Twitter)
                        caseComment_to_create.CaseCommentText = "@" + caseComment.UCUserName + req.Text;

                    else
                    {
                        caseComment_to_create.CaseCommentText = req.Text;
                    }
                    //Identificador del usuario al cual se envía el mensaje. Valor del campo UserChannelId 
                    caseComment_to_create.AccountDetailId = caseComment.AccountDetailIdOutput;
                    caseComment_to_create.UserChannelId = caseComment.UserChannelId;
                    caseComment_to_create.ElementTypeId = caseComment.ElementTypeId.Value;
                    //identificador utilizado para publicar en respuesta a un comentario el 
                    //cual contendrá valor si el canal es público (OutputPublic = 1). 
                    if (selectedAccountOutputChannelType.OutputPublic)
                    {
                        caseComment_to_create.CaseCommentPublicationTo = caseComment.CaseCommentPublicationTo;
                    } //Si (OutputPublic = 0) se coloca NULL. nada en este caso . se lo deja ''

                    //identificador del usuario que envía el mensaje.Valor del campo Value obtenido de[Api].[ApplicationSettings_s_BySettingId]
                    if (!String.IsNullOrEmpty(settings.Value))
                        caseComment_to_create.CaseCommentModifiedByUserId = Convert.ToInt32(settings.Value);

                    //Identificador del estado de validación. Se pasa NULL.
                    caseComment_to_create.StateValidationId = null;


                    caseComment_to_create.AttentionQueueId = caseComment.AttentionQueueId;

                    //Identificador del comentario al cual se envía una respuesta. Se pasa NULL.
                    caseComment_to_create.ReplyToCaseCommentId = null;


                    EpironDAC.Insert_CaseComment(caseComment_to_create);

                    if (selectedAccountOutputChannelType.PChannelType == (int)PChannelTypeEnum.Mail)
                    {
                        DetailCaseCommentBE detailCaseComment = new DetailCaseCommentBE();

                        detailCaseComment.CaseCommentId = caseComment_to_create.CaseCommentId;
                        detailCaseComment.CaseCommentGUID = caseComment_to_create.CaseCommentGUID;
                        detailCaseComment.DCCSubject = caseComment.Subject;
                        detailCaseComment.DCCTextEmailBody = caseComment.CaseCommentText;
                        //Url que indica la ubicación en la redsocial del posteo raíz (root) del cual proviene el comentario. (para los cometarios salientes este campo se guarda como NULL)
                        detailCaseComment.DCCPermlinkRoot = null;
                        //descripción relacionada a un comentario. (para los cometarios salientes este campo se guarda como NULL)
                        detailCaseComment.DCCDescription = null;
                        EpironDAC.Insert_DetailCaseComment(detailCaseComment);
                    }
                    #endregion

                    #region Insert Publicacion
                    var publicacion = new PublicationBE();
                    publicacion.CaseCommentGUID = caseComment_to_create.CaseCommentGUID;

                    publicacion.PComment = caseComment_to_create.CaseCommentText;

                    if (selectedAccountOutputChannelType.OutputPublic)
                    {
                        publicacion.PublicationTo = caseComment.CaseCommentPublicationTo;
                    }
                    else
                    {
                        //si es privado(OutputPublic = 0), se pasa el valor del campo UCPublicationTo.
                        publicacion.PublicationTo = caseComment.UCPublicationTo;
                    }

                    //GUID de la cuenta de salida. 
                    publicacion.AccountDetailUnique = caseComment.AccountDetailUniqueOutput;
                    publicacion.PChannelType = selectedAccountOutputChannelType.PChannelType;
                    //TODO : identificador del usuario que envía el mensaje. @PModifiedByUserId: identificador del usuario que envía el mensaje. Valor del campo Value obtenido de [Api].[ApplicationSettings_s_BySettingId]
                    if (!String.IsNullOrEmpty(settings.Value))
                        publicacion.PModifiedByUserId = Convert.ToInt32(settings.Value);


                    EpironDAC.Insert_Publication(publicacion);


                    if (selectedAccountOutputChannelType.PChannelType == (int)PChannelTypeEnum.Mail)
                    {
                        PublicationDetailBE publicationDetail = new PublicationDetailBE();

                        publicationDetail.PublicationId = publicacion.PublicationId;
                        publicationDetail.PDCommentAux1 = caseComment.Subject;

                        EpironDAC.Insert_PublicationDetail(publicationDetail);
                    }
                    #endregion

                    scope.Complete();
                }
            }

            if (actionId == (int)Common.HookResponsesEnum.Liberar)
            {
                EpironDAC.Realease_CaseComment(req.CaseId);
            }
            if (actionId == (int)Common.HookResponsesEnum.Cerrar)
            {
                EpironDAC.Close_CaseComment(req.CaseId);
            }
        }
    }
}
