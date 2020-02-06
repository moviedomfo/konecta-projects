import { DomainBE, AuthenticationTypeBE } from './epiron.security.model';

   export class ApplicationSettingBE
    {
        public  APId : number; 
        public  APAccountId? : number; 
        public  APSettingId : number; 
        public  APValue : string; 
        public  Particular : boolean; 

    }

    export class AppInstance {

        
        public Domains: DomainBE[];
        public AuthenticationTypes: AuthenticationTypeBE[];
        public ControlEntity: boolean;
        public ApplicationInstanceName: string;
        public ApplicationName: string;


        //atributos que se rellenan luego de obtener la entidad
        public AppInstanceGuid: string;
    }

    export class  SearchCaseByUserGuidRequest{
        public  UserGuid : string; 
        public  State : number; 
    }

    export class CaseByUserGuidBE{
        public CaseId:number;
        public CaseModifiedDate:string;
        public CaseCreated:string;
        public StateName:string;
        public AttentionQueueName:string;
        public StateId:number;
        public GroupAccountId:number;
        public ElementTypeId:number;
        public UCUserName:string;
        public ProfileImage: ArrayBuffer;
        public Notifications:number;
        public ChannelImage: ArrayBuffer;
        public NotificationImage:ArrayBuffer;
        public AttentionQueueEnableNotificationsPanel:boolean;
        public AccountDetailId:number;
        public UserChannelId:number;
        public SCOnline:boolean;
        public UCPublicationTo:string;
        public AccountId:number;
        public SCInternalCode:number;
        public Followers:number;
        public  AttentionQueueImage:ArrayBuffer;
        public Cliente:string;
        public  ClienteImage:ArrayBuffer;
        public  FirstCommentElementGUID:string;
        public AlertColorsCode:string;
        public TimeLastComentClient:string;
        public AttentionQueueId:number;
        public UserAssignedId:number;
        public  ChatRobotGreetingsInserted;
        public  ClassificationDetailId?:number;
        public  StateShowCase:boolean;
        public CaseTypeName:string;
        public  CaseTypeInternalCode?:number;
        public ClientFirstName:string;
        public ClientLastName:string;
    }