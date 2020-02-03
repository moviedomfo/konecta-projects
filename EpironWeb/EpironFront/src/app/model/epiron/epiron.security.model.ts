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
    public DomainListBE : DomainBE[]; 
    public  AuthenticationTypes : AuthenticationTypeBE[];
    public  ControlEntity :boolean;
    public  ApplicationInstanceName :string;
    public  ApplicationName :string;
}
