export class DomainBE{
    public  DomainName : string;
    public  DomainGuid : string;
}
export class AuthenticationTypeBE{
    public  AuthenticationTypeName: string;
    public  AuthenticationTypeTag: string;
    public  AuthenticationTypeGuid: string;
    public  GUID: string;
}

export class ValidateAppRequest {

    public LoginHost :string;
    public LoginIp: string;
    public WebServiceUrl: string;
    public AppInstanceGuid: string;
    public Token: string;

}

export class ValidarAplicacionRes {

    public Token: string;
    public Domains : DomainBE[]; 
    public  AuthenticationTypes : AuthenticationTypeBE[];
    public  ControlEntity :boolean;
    public  ApplicationInstanceName :string;
    public  ApplicationName :string;
}

export class UserBE {
    public  UserId: number; 
    public  UserGUID: string; 
    public  UserName: string; 
    public  PersonId: number; 
    public  UserActiveFlag: boolean; 
    public  AllowAutomatedTagView?: boolean; 
    //Dialer.  Discador.. esto en true hace que los casos se habran de forma automatica
    //en orden segun distintas prioridades definidas por el sistema.
    public  PersonAutomaticOpenCases: boolean; 
}


export class PersonFromWebserviceBE {
    public  UserId: number; 
    public  UserName: string; 
    public  UserModifiedDate: Date; 
    public UserModifiedByUser: number; 
    public  UserCreated: Date; 
    public  UserActiveFlag: boolean; 
    public  PersonId: number; 
    public  UserGUID: string; 
    public  PersonGUID: string; 
    public  UserEndDate?: Date; 
    public  ModifiedUserName: string; 
    public  PersonFirstName: string; 
    public  PersonLastName: string; 
    public  PersonDocNumber: string; 
    public  UserPlaceGuid: string; 
    public  UserPlaceName: string; 
    public  UserPlaceDescript: string; 
    public  PersonModifiedDate: Date; 
    public  AccountPersonStartDate: Date; 
    public  AccountPersonEndDate?: Date; 
}

export class  AttributeBE
{   
    public  AttributeId? : number; 
    public  AttributeInternalCode  : number; 
    public  AttributeActiveFlag :boolean; 
    public  AttributeGuid:string; 
    public  AttributeName :string; 
    public  AttributeModifiedDate :Date; 

}

export class  PersonAttributeBE
{
    public  PersonAttributeId? : number; 
    public  PersonId? : number; 
    public  PersonGuid :string; 
    public  Attribute :AttributeBE; 
    public  PersonAttributeValueGuid :string; 
    public  PersonAttributeValueValue :string; 
    public  PersonAttributeValueCreated :Date; 
    public  PersonAttributeValueStartDate :Date; 
    public  PersonAttributeValueModifiedDate :Date; 
    public  PersonAttributeValueEndDate? : Date; 
    public  PersonAttributeValueActiveFlag  :boolean; 
    public  PersonAttributeValueId : number; 
    public  ExisteEnWS : boolean; 
}