
import {  Result, EpironApiResponse } from '../model/common.model';
import { AppConstants } from "../model/common.constants";
import { HttpClient } from '@angular/common/http';
import {  Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';
import {  ValidarAplicacionRes } from '../model/epiron/epiron.security.model';
import { helperFunctions } from './helperFunctions';
import { CurrentLoginEpiron, UserAutenticacionRes } from '../model';
import { AuthenticationService } from './authentication.service';


@Injectable()
export class EpironSecurityService {
    
  constructor(private http: HttpClient, private commonService: CommonService,private authService:AuthenticationService ) {
  
  }


  public userAutenticacion$(userName: string, password: string, domain: string, returnUrl: string): Observable<any> {

    
    //UserAutenticacionReq
    var bussinesData = {
      event_tag : 'USER-AUTENTIC',
      appInstanceGUID : AppConstants.AppInstanceGUID,
      LoginHost:this.commonService.get_host(),
      LoginIP: this.commonService.ipinfo.ip,
      userName: userName,
      userKey: password,
      domainGUID: 'FDEB4B1F-229E-E311-9DD1-0022640637C2', //domain allus-ar,
      AutTypeGUID: '71C15455-D147-E311-A348-000C292448BD',
      guidintercambio :'75CFEFE4-5A79-E411-BD73-0022640637C2',
      UserKey : password,

      //grant_type: 'password',
      //client_id: AppConstants.oaut_client_id,
      //securityProviderName: AppConstants.oaut_securityProviderName,
      //client_secret: AppConstants.oaut_client_secret
    }

    return this.http.post<EpironApiResponse>(AppConstants.AppOAuth_URL,
      bussinesData, AppConstants.httpClientOption_contenttype_json).pipe(
        map(res => {

          let currentLogin: CurrentLoginEpiron = new CurrentLoginEpiron();

          if(res.Errors){
              alert(JSON.stringify);
          }
          currentLogin.oAuthResult = new UserAutenticacionRes();
          currentLogin.oAuthResult = res.Result as UserAutenticacionRes;

          if(!res.Errors){
              helperFunctions.handleEpironError(res.Errors);
          }
          // let tokenInfo = jwt_decode(currentLogin.oAuthResult.access_token); // decode token


          // currentLogin.currentUser = new SecurityUser();
          // currentLogin.currentUser.userId = tokenInfo.userId;
          // currentLogin.currentUser.userName = tokenInfo.unique_name;
          // currentLogin.currentUser.email = tokenInfo.email;
          // currentLogin.currentUser.roles = tokenInfo.roles;
          // currentLogin.currentUser.personId = tokenInfo.personId;
      
          localStorage.setItem('currentLogin', JSON.stringify(currentLogin));
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

          

          if(res.Errors){
              alert(JSON.stringify);
          }
          
          let result= res.Result as ValidarAplicacionRes;

          if(!res.Errors){
              helperFunctions.handleEpironError(res.Errors);
          }
     
      
          localStorage.setItem('allInstance', JSON.stringify(result));
          let appInst =  this.authService.getAppInstance();

            alert(JSON.stringify(appInst));

          return appInst;
        })).pipe(catchError(helperFunctions.handleError));




}
}

