
import { Result, EpironApiResponse } from '../model/common.model';
import { AppConstants } from "../model/common.constants";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';
import { ValidarAplicacionRes, UserBE } from '../model/epiron/epiron.security.model';
import { helperFunctions } from './helperFunctions';
import { CurrentLoginEpiron, UserAutenticacionRes } from '../model';
import { AuthenticationService } from './authentication.service';


@Injectable()
export class EpironSecurityService {

  constructor(private http: HttpClient, private commonService: CommonService, private authService: AuthenticationService) {

  }


  public userAutenticacion$(userName: string, password: string, domain: string, returnUrl: string): Observable<any> {

    let token = this.authService.getLastToken();


    //UserAutenticacionReq
    var bussinesData = {
      event_tag: 'USER-AUTENTIC',
      appInstanceGUID: AppConstants.AppInstanceGUID,
      LoginHost: this.commonService.get_host(),
      LoginIP: this.commonService.ipinfo.ip,
      userName: userName,
      userKey: password,
      domainGUID: 'FDEB4B1F-229E-E311-9DD1-0022640637C2', //domain allus-ar,
      AutTypeGUID: '5AD5A762-D147-E311-A348-000C292448BD', // propia de Epiron
      guidintercambio: token,
      UserKey: password,

      //grant_type: 'password',
      //client_id: AppConstants.oaut_client_id,
      //securityProviderName: AppConstants.oaut_securityProviderName,
      //client_secret: AppConstants.oaut_client_secret

    }

    return this.http.post<EpironApiResponse>(AppConstants.AppOAuth_URL,
      bussinesData, AppConstants.httpClientOption_contenttype_json).pipe(
        map(res => {

          let currentLogin: CurrentLoginEpiron = new CurrentLoginEpiron();
          if (!res.Errors) {
            helperFunctions.handleEpironError(res.Errors);
          }

          currentLogin.userData = new UserAutenticacionRes();
          currentLogin.userData = res.Result as UserAutenticacionRes;
          localStorage.setItem('currentLogin', JSON.stringify(currentLogin));
          localStorage.setItem('last_token', JSON.stringify(currentLogin.userData.Token));

          // let tokenInfo = jwt_decode(currentLogin.oAuthResult.access_token); // decode token
          // currentLogin.currentUser = new SecurityUser();
          // currentLogin.currentUser.userId = tokenInfo.userId;
          // currentLogin.currentUser.userName = tokenInfo.unique_name;
          // currentLogin.currentUser.email = tokenInfo.email;
          // currentLogin.currentUser.roles = tokenInfo.roles;
          // currentLogin.currentUser.personId = tokenInfo.personId;
          // this.getUser_Data$(currentLogin.currentUser.personId, '').subscribe(res => {

          //   if (!res) {
          //     alert('No existen datos personales del usuario ' + currentLogin.currentUser.userName);
          //   }
          //   currentLogin.userData = res;
          //   currentLogin.currentUser.email = res.email;
          //   localStorage.setItem('currentLogin', JSON.stringify(currentLogin));
          //   let lcRes: logingChange = new logingChange();
          //   lcRes.isLogued = true;
          //   lcRes.returnUrl = returnUrl;
          //   this.logingChange_subject$.next(lcRes);
          // });


          return currentLogin;
        })).pipe(catchError(helperFunctions.handleError));

  }


  public validateAppService$(): Observable<any> {


    var bussinesData = {
      AppInstanceGuid: AppConstants.AppInstanceGUID,
      LoginHost: this.commonService.get_host(),
      LoginIp: this.commonService.ipinfo.ip


    };


    return this.http.post<EpironApiResponse>(`${AppConstants.AppAPI_URL}security/validarAplicacion`,
      bussinesData, AppConstants.httpClientOption_contenttype_json).pipe(
        map(res => {

          if (res.Errors) {
            helperFunctions.handleEpironError(res.Errors);
          }

          let appInstance = res.Result as ValidarAplicacionRes;
          localStorage.setItem('appInstance', JSON.stringify(appInstance));
          localStorage.setItem('last_token', JSON.stringify(appInstance.Token));



          return appInstance;
        })).pipe(catchError(helperFunctions.handleError));


  }

  //GetSecurityUserByUserGuidService
  public GetSecurityUserByUserGuidRequest$(userGuid: string): Observable<UserBE> {


    let bussinesData = {
      userGuid: AppConstants.AppInstanceGUID
    }

    let outhHeader = this.commonService.get_AuthorizedHeader();
    let executeReq = this.commonService.generete_post_Params("GetSecurityUserByUserGuidService", bussinesData);

    return this.http.post<any>(`${AppConstants.AppExecuteAPI_URL}`, executeReq, { headers: outhHeader }).pipe(
      map(res => {

        let result: Result = JSON.parse(res.Result) as Result;

        if (result.Error) {
          throw Observable.throw(result.Error);
        }

        let list = result.BusinessData as UserBE;
        return list;
      })).pipe(catchError(helperFunctions.handleError));


  }


  public SearchPersonAttributesRequest$(personGUID: string): Observable<UserBE> {


    let bussinesData = {
      userGuid: AppConstants.AppInstanceGUID
    }

    let outhHeader = this.commonService.get_AuthorizedHeader();
    let executeReq = this.commonService.generete_post_Params("SearchPersonAttributesService", bussinesData, "EpironSecurity");

    return this.http.post<any>(`${AppConstants.AppExecuteAPI_URL}`, executeReq, { headers: outhHeader }).pipe(
      map(res => {

        let result: Result = JSON.parse(res.Result) as Result;

        if (result.Error) {
          throw Observable.throw(result.Error);
        }

        let list = result.BusinessData as UserBE;
        return list;
      })).pipe(catchError(helperFunctions.handleError));


  }

}

