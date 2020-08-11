using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace epironApi.webApi.models
{

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

    }

    /// <summary>
    /// Respuesta del API Bot 
    /// </summary>
    public class BotCommentModeratedReq
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
        /// acción que se debe realizar una vez obtenida la respuesta del ApiBot. (Respoder, Liberar, Cerrar) 
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }
        
    }


    public class BotBE
    {
        /// <summary>
        /// identificador de la tabla. Autoincremental
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// comentario que envía el usuario desde la red social y es enviado a la ApiBot, para ser analizado.
        /// </summary>
        public string CaseCommentTextSent { get; set; }

        public Guid AccountUnique { get; set; }

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
        public int SCInternalCode { get; set; }

        /// <summary>
        /// bit que indica si el comentario es público (1) o privado(0).
        /// </summary>
        public bool ElementTypePublic { get; set; }


        /// <summary>
        /// respuesta al comentario enviado por el usuario, la cual es enviada por la ApiBot. 
        /// </summary>
        public string CaseCommentTextReceived { get; set; }

        /// <summary>
        /// acción que se debe realizar una vez obtenida la respuesta del ApiBot. (Respoder, Liberar, Cerrar) 
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// estado enviado por el ApiBot.Se recibió Ok la información, Error.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// si ocurrió un error, se inserta el detalle del mismo.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// fecha y hora que en que se envía el comentario del usuario. (lleva por defecto el valor getdate()).
        /// </summary>
        public DateTime CreatedRow { get; set; }

        /// <summary>
        ///  fecha y hora en que se recibe una respuesta del comentario.
        /// </summary>
        public DateTime CreatedDateReceived { get; set; }


        /// <summary>
        /// fecha y hora en que se concluyó el procesamiento de insertar en  la mc.casecomment, mc.Publication
        /// </summary>
        public DateTime ProcessDate { get; set; }
    }
}
