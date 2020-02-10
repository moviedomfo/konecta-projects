using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpironBotTestAPI
{

    /// <summary>
    /// comment to moderate
    /// </summary>
    public class EnqueueCommentBotReq
    {
        /// <summary>
        /// Identificador del caso.
        /// </summary>
        public int CaseId { get; set; }



        /// <summary>
        ///  guid del comentario
        /// </summary>
        public Guid CaseCommentGUID { get; set; }

        /// <summary>
        /// Código interno del canal.
        /// </summary>
        public string SCInternalCode { get; set; }


        /// <summary>
        /// bit que indica si el comentario es público (1) o privado(0).
        /// </summary>
        public bool ElementTypePublic { get; set; }

        /// <summary>
        /// comentario que envía el usuario desde la red social y es enviado a la ApiBot, para ser analizado.
        /// </summary>
        public string CaseCommentTextSent { get; set; }

        /// <summary>
        /// guid de la unidad de negocio.
        /// </summary>
        public string AccountUnique { get; set; }


        public DateTime? ArrivedDate { get; set; }
        public DateTime? SendedDate { get; set; }


        
    }


    [Serializable]
    /// <summary>
    /// comment to moderate
    /// </summary>
    public class EnqueueCommentBotReq_serializable
    {

        public EnqueueCommentBotReq_serializable() { }

        public EnqueueCommentBotReq_serializable(EnqueueCommentBotReq item) {

            CaseId = item.CaseId;
            CaseCommentGUID = item.CaseCommentGUID;
            SCInternalCode = item.SCInternalCode;
            ElementTypePublic = item.ElementTypePublic;
            CaseCommentTextSent = item.CaseCommentTextSent;
            AccountUnique = item.AccountUnique;

            SendedDate = item.SendedDate;

            ArrivedDate = item.ArrivedDate;


        }

        public EnqueueCommentBotReq get()
        {
            EnqueueCommentBotReq item = new EnqueueCommentBotReq();

             item.CaseId = CaseId;
             item.CaseCommentGUID = CaseCommentGUID;
             item.SCInternalCode = SCInternalCode;
             item.ElementTypePublic = ElementTypePublic;
             item.CaseCommentTextSent = CaseCommentTextSent;
             item.AccountUnique = AccountUnique;

             item.SendedDate = SendedDate;

             item.ArrivedDate = ArrivedDate;

            return item;
        }
        

        /// <summary>
        /// Identificador del caso.
        /// </summary>
        public int CaseId { get; set; }



        /// <summary>
        ///  guid del comentario
        /// </summary>
        public Guid CaseCommentGUID { get; set; }

        /// <summary>
        /// Código interno del canal.
        /// </summary>
        public string SCInternalCode { get; set; }


        /// <summary>
        /// bit que indica si el comentario es público (1) o privado(0).
        /// </summary>
        public bool ElementTypePublic { get; set; }

        /// <summary>
        /// comentario que envía el usuario desde la red social y es enviado a la ApiBot, para ser analizado.
        /// </summary>
        public string CaseCommentTextSent { get; set; }

        /// <summary>
        /// guid de la unidad de negocio.
        /// </summary>
        public string AccountUnique { get; set; }


        public DateTime? ArrivedDate { get; set; }
        public DateTime? SendedDate { get; set; }
    }


    /// <summary>
    /// Respuesta del API Bot 
    /// </summary>
    public class BotCommentModeratedReq
    {
        /// <summary>
        /// Identificador del caso.
        /// </summary>
        public string CaseId { get; set; }



        /// <summary>
        ///  guid del comentario
        /// </summary>
        public Guid CaseCommentGUID { get; set; }



        /// <summary>
        /// acción que se debe realizar una vez obtenida la respuesta del ApiBot. (Respoder, Liberar, Cerrar) 
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

    }
}
