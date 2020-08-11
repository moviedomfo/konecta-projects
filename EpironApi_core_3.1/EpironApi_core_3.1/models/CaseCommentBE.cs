using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpironApi.webApi.BE
{
    public class CaseCommentBE
    {
        public int UserChannelId { get; set; }
        public int AccountId { get; internal set; }
        public string CaseCommentPublicationTo { get;  set; }
        public string CaseCommentText { get;  set; }
        public string UCUserName { get; internal set; }
        public int AccountDetailIdOutput { get;  set; }
        public int? ElementTypeId { get;  set; }
        public int? AttentionQueueId { get; set; }
        
        public string UCPublicationTo { get;  set; }
        public string Subject { get;  set; }
        public string AccountEmailBodyTemplate { get;  set; }
        public Guid AccountDetailUniqueOutput { get;  set; }
    }

    public class CaseCommentCreationBE
    {
        //@CaseCommentId INT OUTPUT,    
        //@CaseCommentGUID uniqueidentifier output,
        public Guid CaseCommentGUID { get; set; }
        public int CaseCommentId { get; set; }
        public int CaseId { get; set; }
        public string CaseCommentText { get; set; }
        public int UserChannelId { get; set; }
        public int AccountDetailId { get; set; }
        public int? CaseConfigurationId { get; set; }

        /// <summary>
        /// @ProcessDetailsId INT = NULL, 
        /// </summary>
        public int? ProcessDetailsId { get; set; }

        public int? ElementId { get; set; }

        public int ElementTypeId { get; set; }

        public string CaseCommentPublicationTo { get; set; }

        public int? CaseCommentModifiedByUserId { get; set; }

        public int? StateValidationId { get; set; }

        public int? AttentionQueueId { get; set; }

        public int? ReplyToCaseCommentId { get; set; }
        
    }

    public class DetailCaseCommentBE
    {
        public Guid CaseCommentGUID { get; set; }
        public int CaseCommentId { get; set; }

        public string DCCSubject { get; set; }
        public string DCCTextEmailBody { get; set; }

        /// <summary>
        /// Url que indica la ubicación en la redsocial del posteo raíz (root) del cual proviene el comentario. (para los cometarios salientes este campo se guarda como NULL)
        /// </summary>
        public int? DCCPermlinkRoot { get; set; }

        /// <summary>
        /// descripción relacionada a un comentario. (para los cometarios salientes este campo se guarda como NULL
        /// </summary>
        public string DCCDescription { get; set; }
     
    }

    public class PublicationBE
    {
        /// <summary>
        /// Parámetro de salida OUTPUT
        /// </summary>
        public int PublicationId { get; set; }

        /// <summary>
        ///@PComment NVARCHAR(max),
        /// </summary>
        public String PComment { get; set; }

        /// <summary>
        /// @PublicationTo varchar(256),
        /// </summary>
        public String PublicationTo { get; set; }

        /// <summary>
        ///  CaseCommentGUID uniqueidentifier
        /// </summary>
        public Guid CaseCommentGUID { get; set; }

        /// <summary>
        /// AccountDetailUnique UNIQUEIDENTIFIER,
        /// </summary>
        public Guid AccountDetailUnique { get; set; }


        public int PChannelType { get; set; }

        public String CaseCommentTextToPublication { get; set; }
        
        public int  PModifiedByUserId { get; set; }

        public DateTime? PublicationDate { get; set; }
        public int? PublicationErrorId { get; set; }

        /// <summary>
        ///@PAwaitingUserConfirmation BIT,
        /// </summary>
        public Boolean      PAwaitingUserConfirmation { get; set; }


        //@PChannelType INT,
        //@PublicationErrorId INT = NULL,

        ///@PRetriesQuantity INT
        public int PRetriesQuantity { get; set; }

        //@ProcessDetailId INT = NULL,
        public int? ProcessDetailId { get; set; }

        //@PActiveFlag BIT,
        public bool PActiveFlag { get; set; }
        //@PSourcePublicationId varchar(256) = NULL
        public string PSourcePublicationId { get; set; }

       
    }

    public class PublicationDetailBE
    {
        /// <summary>
        /// Parámetro de salida.
        /// </summary>
        public int PublicationId { get; set; }

        /// <summary>
        /// asunto para el canal mail. (PChannelType=315). Valor del campo Subject obtenido de [Api].[ApplicationSettings_s_BySettingId]
        /// </summary>
        public String PDCommentAux1 { get; set; }



    }

    

}
