import { PersonBE } from './persons.model';
import { IAPIRequest } from './common.model';
import { DomainBE, AuthenticationTypeBE } from './epiron/epiron.security.model';


export class SecurityUser {
    public userName: string;
    public password: string;
    public userId?: string;
    public email: string;

    public AppName: string;
    public createdDate: Date;
    public IsApproved: boolean;
    public IsLockedOut: boolean;
    //public LastActivityDate?: Date;
    public ModifiedDate?: Date;
    public ModifiedByUserId?: number;

    public confirmPassword: string;

    public roles: string[];




    public personId: string;
    public person: PersonBE;
    public GetRolList(): SecurityRole[] {

        var roles: SecurityRole[];

        return roles;
    }


}

export class SecurityRole {

    SecurityRole() {
        this.isChecked = false;
    }
    public Id: string;
    public Name: string;
    public Description: string;

    public isChecked: boolean = false;
}
export class SecurityRulesCategory {


    public CategoryId: string;
    public Name: string;
    public ParentCategoryId: string;

    public SecurityRulesInCategory: SecurityRulesInCategory[];
}
export class SecurityRulesInCategory {
    public CategoryId: string;
    public RuleId: string;
    public SecurityRule: SecurityRule;
    public SecurityRulesCategory: SecurityRulesCategory;
}

export class SecurityRule {
    public Id: string;
    public Name: string;
    public Description: string;
}
export class UserSession {
    public userId: string;
    public userName: string;
    public email: string;
    public password: string;
    public confirmPassword: string;
}


export class CreateUserReq extends IAPIRequest {

    public userName: string;
    public email: string;
    public password: string;
    public roles: string[];
    public person: PersonBE;
    public personIsNew: boolean;

}

export class UpdateUserReq extends IAPIRequest {
    public update_userName: string;
    public email: string;
    public update_UserId: string;
    public roles: string[];
    public person: PersonBE;

}

export class AuthenticationOAutResponse {

    expires_in: number;
    access_token: string;
    token_type: string;
    refresh_token: string;

}

export class UserAutenticacionRes {

    public expires_in: number;
    public access_token: string;
    public token_type: string;
    public refresh_token: string;

    public Token: string
    public WsUserId: number;
    public UserGuid: string;
    public UserName: string;
    public PersonFirstName: string;
    public PersonLastName: string;
    public PersonDocNumber: string;
    public PersonGUID: string;
    public MenuPermisos: string;
    public UserPlaceGuid: string;
    public UserPlaceName: string;
    public UserPlaceDescript: string;
    public PersonModifiedDate: Date;
    public ErrorMessage: string;
    
    public sex: number;
    public photo: ArrayBuffer;

}


export class logingChange {
    public returnUrl: string;
    public isLogued: boolean;
}
export class CurrentLogin {
    public oAuthResult: AuthenticationOAutResponse;
    public currentUser: SecurityUser;
    public userData: PersonBE;
}


export class CurrentLoginEpiron {
    public userData: UserAutenticacionRes;
    // public currentUser: SecurityUser;
    // public userData:PersonBE;
}


