import { Injectable, Inject } from '@angular/core';

import { AppConstants, contextInfo, PersonStatus } from "../model/common.constants";
import { ParamBE, IContextInformation, IRequest, IResponse, Result, AuthenticationOAutResponse, SecurityUser, CurrentLogin, ApiServerInfo, PersonBE, logingChange, CreateUserReq, UpdateUserReq, UserAutenticacionRes, CurrentLoginEpiron, EpironApiResponse, AppInstance } from '../model/index';
import { map, catchError } from 'rxjs/operators';
import { Observable, Subject, throwError } from 'rxjs';
import { Router } from '@angular/router';
import { CommonService } from '../service/common.service';

import { HttpParams, HttpHeaders, HttpClient } from '@angular/common/http';
import * as jwt_decode from "jwt-decode";
import { helperFunctions } from './helperFunctions';


@Injectable()
export class AuthenticationService {
 


  public logingChange_subject$: Subject<logingChange> = new Subject<logingChange>();


  constructor(private commonService: CommonService, private http: HttpClient, private router: Router) {
    // set token if saved in local storage
  }

  isAuth() {

    var currentUser: CurrentLogin = this.getCurrenLoging();
    if (currentUser)
      return true;
    else
      return false;

  }

  get_logingChange$(): Observable<logingChange> {
    return this.logingChange_subject$.asObservable();
  }

  //Este método de autenticacion usa jwk contra un rest asp api
  public oauthToken_owin$(userName: string, password: string): Observable<CurrentLogin> {

    const bodyParams = new HttpParams()
      .set(`username`, userName)
      .set(`password`, password)
      .set(`grant_type`, 'password')
      .set(`client_id`, AppConstants.oaut_client_id)
      .set(`securityProviderName`, AppConstants.oaut_securityProviderName)
      .set(`client_secret`, AppConstants.oaut_client_secret);


    return this.http.post<string>(`${AppConstants.AppOAuth_URL}`,
      bodyParams, AppConstants.httpClientOption_form_urlencoded).pipe(
        map(res => {
          alert(AppConstants.AppOAuth_URL);

          var currentLogin: CurrentLogin = new CurrentLogin();
          currentLogin.oAuthResult = new AuthenticationOAutResponse();
          currentLogin.oAuthResult.access_token = res;
          let tokenInfo = jwt_decode(currentLogin.oAuthResult.access_token); // decode token


          currentLogin.currentUser = new SecurityUser();
          currentLogin.currentUser.userId = tokenInfo.userId;
          currentLogin.currentUser.userName = tokenInfo.userName;
          currentLogin.currentUser.email = tokenInfo.email;
          currentLogin.currentUser.roles = tokenInfo.roles;
          // currentLogin.currentUser.FirstName = tokenInfo.FirstName;
          // currentLogin.currentUser.LastName = tokenInfo.LastName;
          // currentLogin.currentUser.sex = tokenInfo.sex;
          // currentLogin.currentUser.photo = tokenInfo.photo;
          //currentLogin.currentUser.providerName = tokenInfo.unique_name;
          localStorage.setItem('currentLogin', JSON.stringify(currentLogin));

          return currentLogin;
        })).pipe(catchError(this.commonService.handleError2));

  }
  ///Este método de autenticacion usa jwk contra un rest asp api
  public oauthToken$(userName: string, password: string, domain: string, returnUrl: string): Observable<any> {

    var bussinesData = {
      userName: userName,
      password: password,
      domainGUID: 'FDEB4B1F-229E-E311-9DD1-0022640637C2', //domain allus-ar,
      event_tag : 'USER-AUTENTIC',
      AppInstanceGUID : AppConstants.AppInstanceGUID,
      guidintercambio :'75CFEFE4-5A79-E411-BD73-0022640637C2',
      UserKey : password,

      grant_type: 'password',
      client_id: AppConstants.oaut_client_id,
      securityProviderName: AppConstants.oaut_securityProviderName,
      client_secret: AppConstants.oaut_client_secret
    }

    return this.http.post<any>(AppConstants.AppOAuth_URL,
      bussinesData, AppConstants.httpClientOption_contenttype_json).pipe(
        map(res => {

          let currentLogin: CurrentLogin = new CurrentLogin();
          currentLogin.oAuthResult = new AuthenticationOAutResponse();
          currentLogin.oAuthResult.access_token = res.token;
          let tokenInfo = jwt_decode(currentLogin.oAuthResult.access_token); // decode token


          currentLogin.currentUser = new SecurityUser();
          currentLogin.currentUser.userId = tokenInfo.userId;
          currentLogin.currentUser.userName = tokenInfo.unique_name;
          currentLogin.currentUser.email = tokenInfo.email;
          currentLogin.currentUser.roles = tokenInfo.roles;
          currentLogin.currentUser.personId = tokenInfo.personId;
          // currentLogin.currentUser.firstName = tokenInfo.firstName;
          // currentLogin.currentUser.firstName = tokenInfo.firstName;
          //currentLogin.currentUser.providerName = tokenInfo.unique_name;
          localStorage.setItem('currentLogin', JSON.stringify(currentLogin));
          this.getUser_Data$(currentLogin.currentUser.personId, '').subscribe(res => {

            if (!res) {
              alert('No existen datos personales del usuario ' + currentLogin.currentUser.userName);
            }
            currentLogin.userData = res;
            currentLogin.currentUser.email = res.email;
            localStorage.setItem('currentLogin', JSON.stringify(currentLogin));
            let lcRes: logingChange = new logingChange();
            lcRes.isLogued = true;
            lcRes.returnUrl = returnUrl;
            this.logingChange_subject$.next(lcRes);
          });


          return currentLogin;
        })).pipe(catchError(helperFunctions.handleError));

  }

  public refreshoauthToken(): Observable<AuthenticationOAutResponse> {

    let currentLogin: CurrentLogin = JSON.parse(localStorage.getItem('currentLogin'));

    //console.log(currentLogin.oAuthResult.refresh_token);
    const bodyParams = new HttpParams()
      .set(`refresh_token`, currentLogin.oAuthResult.refresh_token)
      .set(`grant_type`, 'refresh_token')
      .set(`client_id`, AppConstants.oaut_client_id)
      .set(`client_secret`, AppConstants.oaut_client_secret);


    return this.http.post<any>(`${AppConstants.AppOAuth_URL}`,
      bodyParams, AppConstants.httpClientOption_form_urlencoded).pipe(
        map(res => {
          var currentLogin: CurrentLogin = new CurrentLogin();
          currentLogin.oAuthResult = new AuthenticationOAutResponse();
          currentLogin.oAuthResult = res;
          currentLogin.currentUser = new SecurityUser();

          let tokenInfo = jwt_decode(currentLogin.oAuthResult.access_token); // decode token

          currentLogin.currentUser.userId = tokenInfo.userId;
          currentLogin.currentUser.userName = tokenInfo.userName;
          currentLogin.currentUser.email = tokenInfo.email;
          currentLogin.currentUser.roles = tokenInfo.roles;
          currentLogin.currentUser.personId = tokenInfo.personId;

          this.getUser_Data$('', currentLogin.currentUser.userId).subscribe(res => {

            currentLogin.userData = res;
            localStorage.setItem('currentLogin', JSON.stringify(currentLogin));
            let lcRes: logingChange = new logingChange();
            lcRes.isLogued = true;
            lcRes.returnUrl = "";
            this.logingChange_subject$.next(lcRes);

          });

          return res;
        })).pipe(catchError(this.commonService.handleError));


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


  signOut(): void {
    // clear token remove user from local storage to log user out

    localStorage.removeItem('currentLogin');
    let lcRes: logingChange = new logingChange();
    lcRes.isLogued = false;
    lcRes.returnUrl = '';
    this.logingChange_subject$.next(lcRes);
  }
  // get_currentproviderData(): Provider_FullViewBE {
  //   var currentItem: providerFullData = new Provider_FullViewBE();
  //   let str = localStorage.getItem('currentproviderData');
  //   currentItem = JSON.parse(str);

  //   return currentItem;
  // }
  getCurrenLoging(): CurrentLogin {
    var currentLogin: CurrentLogin;
    let str = localStorage.getItem('currentLogin');
    if (str) {
      currentLogin = JSON.parse(str);
      if (currentLogin.userData) {
        return currentLogin;
      }
    }
    return null;

  }


  getAppInstance(): AppInstance {
    var appInstance: AppInstance;
    let str = localStorage.getItem('appInstance');
    if (str) {
      appInstance = JSON.parse(str);
      if (appInstance.Token) {
        return appInstance;
      }
    }
    return null;

  }



  getServerInfo$(): Observable<ApiServerInfo> {
    var bussinesData = {};
    var apiURL = AppConstants.AppOAuth_BaseUrl + "getServerInfo";
    let outhHeader = this.commonService.get_AuthorizedHeader();
    return this.http.get<ApiServerInfo>(apiURL, { headers: outhHeader }).pipe(
      map(res => {
        return res;
      })).pipe(catchError(this.commonService.handleError));
  }


  resetUserPassword$(userName: string, password: string): Observable<any> {

    var bussinesData = {
      userName: userName,
      password: password,
      domain:'',
      securityProviderName:AppConstants.oaut_securityProviderName
    };

    let outhHeader = this.commonService.get_AuthorizedHeader();

    return this.http.post<boolean>(`${AppConstants.AppAPI_URL}security/resetPwd`, bussinesData, { headers: outhHeader }).pipe(
      map(res => {
        return res;
      })).pipe(catchError(this.commonService.handleError));


  }

  validateUserExist$(userName: string): Observable<any> {

    let outhHeader = this.commonService.get_AuthorizedHeader();
    let httpParams = new HttpParams()
      .set(`userName`, userName);

    return this.http.get<boolean>(`${AppConstants.AppAPI_URL}security/validateUserExist`, { headers: outhHeader, params: httpParams }).pipe(
      map(res => {

        return res;
      })).pipe(catchError(this.commonService.handleError));


  }


  getUser_Data$(personId: string, userId: string): Observable<PersonBE> {


    let outhHeader = this.commonService.get_AuthorizedHeader();
    const httpParams = new HttpParams()
      .set(`personId`, personId)
      .set(`userId`, userId);


    return this.http.get<PersonBE>(`${AppConstants.AppAPI_URL}security/getUserData`, { headers: outhHeader, params: httpParams }).pipe(

      map(res => {

        return res;
      })).pipe(catchError(this.commonService.handleError));


  }
  getUsers$(userName: string) {
  
    let outhHeader = this.commonService.get_AuthorizedHeader();
    const httpParams = new HttpParams()
      .set(`userName`, name)
      .set('securityProviderName',AppConstants.oaut_securityProviderName);

    return this.http.get<SecurityUser[]>(`${AppConstants.AppAPI_URL}security/getUsers`, { headers: outhHeader, params: httpParams }).pipe(

      map(res => {

        return res;
      })).pipe(catchError(this.commonService.handleError));

  }
  
  getUser$(userName: string, userId: string): Observable<SecurityUser> {

    let outhHeader = this.commonService.get_AuthorizedHeader();

    
    if(helperFunctions.string_IsNullOrEmpty(userName))
    userName='';
    if(helperFunctions.string_IsNullOrEmpty(userId))
        userId=AppConstants.emptyGuid;
    const httpParams = new HttpParams()
      .set(`userName`, userName)
      .set(`userId`, userId);


    return this.http.get<SecurityUser>(`${AppConstants.AppAPI_URL}security/getUser`, { headers: outhHeader, params: httpParams }).pipe(

      map(res => {

        return res;
      })).pipe(catchError(this.commonService.handleError));


  }


  createUserService$(user: SecurityUser) {
    var req: CreateUserReq = new CreateUserReq();
    req.userName = user.userName;
    req.password = user.password;
    req.email = user.email;
    req.roles = user.roles;
    req.person = user.person;
    req.person.status= PersonStatus.Activo;
    if(req.person.personId=== AppConstants.emptyGuid)
      req.personIsNew = true;
      else
      req.personIsNew = false;

    let outhHeader = this.commonService.get_AuthorizedHeader();

    this.commonService.set_Fwk_API_REQ(req);

    return this.http.post<any>(`${AppConstants.AppAPI_URL}security/createUser`, req, { headers: outhHeader }).pipe(
      map(res => {
        return res;
      })).pipe(catchError(this.commonService.handleError));
  }


  /**
   * 
   * @param user 
   */
  UpdateUserService$(user: SecurityUser) {
    var req: UpdateUserReq = new UpdateUserReq();
    req.update_userName = user.userName;
    req.update_UserId = user.userId;
    req.email = user.email;
    req.person = user.person;
    req.person.status= PersonStatus.Activo;
    req.person.email = user.email;
    req.roles = user.roles;
    
    
    let outhHeader = this.commonService.get_AuthorizedHeader();

    this.commonService.set_Fwk_API_REQ(req);

    return this.http.post<any>(`${AppConstants.AppAPI_URL}security/updateUser`, req, { headers: outhHeader }).pipe(
      map(res => {
       
        return res;
      })).pipe(catchError(this.commonService.handleError));
  }





}