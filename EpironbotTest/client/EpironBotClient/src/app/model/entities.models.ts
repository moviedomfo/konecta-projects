export class BotCommentModeratedReq{
    public  CaseId : number;
    public  CaseCommentGUID : string;
    public  SCInternalCode : string;
    public  ElementTypePublic : boolean;
    public  CaseCommentTextSent : string;
    public  AccountUnique : string;

    public ArrivedDate:Date;
}


export class ModeratedComment{
    public  CaseId : number;
    public  CaseCommentGUID : string;

    //acción que se debe realizar una vez obtenida la respuesta del ApiBot. (Respoder, Liberar, Cerrar) 

    public  Action : string;
    
    public  Text : string;
   
}










/// Contiene informacion del error de un servicio.-
// if(e instanceof EvalError)
export class ServiceError extends Error {


    Message: string;
    StackTrace: string;
    Type: string;
    Machine: string;
    Status: number;

}
