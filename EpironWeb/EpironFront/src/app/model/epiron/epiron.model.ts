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

        public Token: string;
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