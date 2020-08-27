import { Injectable, Inject } from '@angular/core';
import { AuthenticationOAutResponse, CurrentLogin, User } from '../model/common.model';
import { AppConstants } from "../model/common.constants";
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Subject, throwError } from 'rxjs';
// permite observar
//import {  Response, RequestOptions, Headers, URLSearchParams, RequestOptionsArgs } from '@angular/common/http';
import { CommonService } from '../service/common.service';
import { map, catchError } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { Empleado, userResetPasswordReq, retriveEmpleadosReseteosReq, Domain, userUnlockReq, ApiServerInfo, userChangePasswordReq, forgotPassword_requetsReq, userForgotPasswordVerifyReq } from '../model/bussinessModel';
import { UseExistingWebDriver } from 'protractor/built/driverProviders';
import * as jwt_decode from "jwt-decode";

@Injectable({
  providedIn: 'root'
})
export class SerurityService {
  private static _token: any;

  private simpleObservableDomain = new Observable((observer) => {
    var domains: Domain[] = JSON.parse(localStorage.getItem('domains'));
    // observable execution
    observer.next(domains);
    observer.complete();
  });

  public domains$: Subject<Domain[]> = new Subject<Domain[]>();
  public paramList: Domain[] = [];
  public logingChange_subject$: Subject<boolean> = new Subject<boolean>();

  constructor(private http: HttpClient, private commonService: CommonService) {

    //localStorage.removeItem('domains');
    //this.checkDomains();

  }
  get_logingChange$(): Observable<boolean> {
    return this.logingChange_subject$.asObservable();
  }
  ///Este método de autenticacion usa jwk contra un rest asp api
  oauthToken_owin$(userName: string, password: string): Observable<AuthenticationOAutResponse> {
   

    const bodyParams = new HttpParams()
      .set(`username`, userName)
      .set(`password`, password)
      .set(`grant_type`, 'password')
      .set(`client_id`, AppConstants.oaut_client_id)
      .set(`securityProviderName`, AppConstants.oaut_securityProviderName)
      .set(`client_secret`, AppConstants.oaut_client_secret);

    //let header_httpClient_contentTypeJson = new HttpHeaders({ 'Content-Type': 'application/json' });

    return this.http.post<AuthenticationOAutResponse>(`${AppConstants.AppOAuth_URL}`,
      bodyParams, { headers: AppConstants.HttpHeaders_json_api }).pipe(
        map(res => {
          console.log(res);

          var currentLogin: CurrentLogin = new CurrentLogin();
          currentLogin.oAuth = new AuthenticationOAutResponse();
          currentLogin.oAuth.access_token = res.access_token;
          currentLogin.currentUser = new User();
          currentLogin.currentUser.UserName = userName;

          localStorage.setItem('currentResetLogin', JSON.stringify(currentLogin));

          return res;
        })).pipe(catchError(this.commonService.handleError));

  }


  ///Este método de autenticacion usa jwk contra un rest asp api
  oauthToken$(userName: string, password: string, domain: string): Observable<CurrentLogin> {

    var bussinesData = {
      username: userName,
      password: password,
      domain: domain,
      grant_type: 'password',
      client_id: AppConstants.oaut_client_id,
      securityProviderName: AppConstants.oaut_securityProviderName,
      client_secret: AppConstants.oaut_client_secret
    }



    return this.http.post<any>(AppConstants.AppOAuth_URL, bussinesData, 
      AppConstants.httpClientOption_contenttype_json).pipe(
      map(res => {

        //console.log(res);

        var currentLogin: CurrentLogin = new CurrentLogin();
        
        currentLogin.oAuth = new AuthenticationOAutResponse();
        currentLogin.oAuth.access_token = res;
        currentLogin.currentUser = new User();
        currentLogin.currentUser.UserName = userName;
        currentLogin.currentUser.Domain = domain;

        let tokenInfo = jwt_decode(currentLogin.oAuth.access_token); // decode token
        //alert(tokenInfo.exp);
        
        currentLogin.currentUser.CAIS =   tokenInfo.CAIS.toLowerCase() == 'true' ? true : false; 
        
       
        currentLogin.currentUser.DomainId = tokenInfo.dom_id as number;
        currentLogin.currentUser.UserName = tokenInfo.unique_name;
        currentLogin.currentUser.Emp_Id = tokenInfo.Emp_id;
        currentLogin.currentUser.Cuenta = tokenInfo.cuenta as number;
        currentLogin.currentUser.Cargo = tokenInfo.cargo as number;
        currentLogin.currentUser.Legajo = tokenInfo.Legajo as number;

        currentLogin.isRessetUser =  tokenInfo.isRessetUser.toLowerCase() == 'true' ? true : false;  

        localStorage.setItem('currentResetLogin', JSON.stringify(currentLogin));
        this.logingChange_subject$.next(true);
        return currentLogin;


      })).pipe(catchError(this.commonService.handleError));

  }

  isAuth() {
    
    var currentUser: CurrentLogin = this.getCurrenLoging();
    if (currentUser)
      return true;
    else
      return false;

  }

  resetPassword$(req: userResetPasswordReq): Observable<string> {

   
    var bussinesData = {
      WindowsUser: req.WindowsUser,
      DomainName: req.DomainName,
      dom_id: req.dom_id,
      Emp_Id: req.Emp_Id,
      ResetUserId:req.ResetUserId,
      ResetUserName: req.ResetUserName,
      ResetUserCAIS: req.ResetUserCAIS,
      ticket : req.ticket ,
      host:  this.commonService.get_host()
    }
    let apiURL = AppConstants.AppOAuth_Base_LDAP + "userResetPassword";
     let outhHeader = this.commonService.get_AuthorizedHeader();
    return this.http.post<any>(apiURL, bussinesData, { headers: outhHeader }).pipe(
      map(res => {
        return res;

      })).pipe(catchError(this.commonService.handleError));

  }

  userForgotPassword_checkDNI$(req: forgotPassword_requetsReq): Observable<any> {

   
    var bussinesData = {

      dni: req.dni,
      host:  this.commonService.get_host()
    }

    let apiURL = AppConstants.AppOAuth_Base_LDAP + "userForgotPassword_checkDNI";

    return this.http.post<any>(apiURL, bussinesData).pipe(
      map(res => {
        //retorna usuario
        return res;

      })).pipe(catchError(this.commonService.handleError));

  }

  forgotPassword_requets$(req: forgotPassword_requetsReq): Observable<any> {

   
    var bussinesData = {
      //dom_id: req.dom_id,
      dni: req.dni,
      host:  this.commonService.get_host()
    }

    let apiURL = AppConstants.AppOAuth_Base_LDAP + "forgotPassword_requets";
    //let outhHeader = this.commonService.get_AuthorizedHeader();
    return this.http.post<any>(apiURL, bussinesData).pipe(
      map(res => {
        //retorna msg de que se le enviaara un correo: 
        return res;

      })).pipe(catchError(this.commonService.handleError));

  }


 

  userForgotPasswordVerify$(req:userForgotPasswordVerifyReq ){
    
    var bussinesData = {
      //dom_id: req.dom_id,
      //dni: req.dni,
      code:  req.code 
    }

    let apiURL = AppConstants.AppOAuth_Base_LDAP + "userForgotPasswordVerify";
    
    return this.http.post<any>(apiURL, bussinesData).pipe(
      map(res => {
        //retorna msg de que se le enviaara un correo: 
        return res;

      })).pipe(catchError(this.commonService.handleError));

  }


  
  userChangePasswordSelf$(req: userChangePasswordReq): Observable<string> {

   
    var bussinesData = {

      domainName: req.domainName,
      dom_id: req.dom_id,
      emp_Id: req.emp_Id,
      userId:req.userId,
      userName: req.userName,
      newPassword:req.newPassword,
      host:  this.commonService.get_host()
    }
    let apiURL = AppConstants.AppOAuth_Base_LDAP + "userChangePasswordSelf";
    
    return this.http.post<any>(apiURL, bussinesData).pipe(
      map(res => {
        return res;

      })).pipe(catchError(this.commonService.handleError));

  }
  public userChangePassword$(req: userChangePasswordReq): Observable<string> {

   
    var bussinesData = {

      domainName: req.domainName,
      dom_id: req.dom_id,
      emp_Id: req.emp_Id,
      userId:req.userId,
      userName: req.userName,
      newPassword:req.newPassword,
      host:  this.commonService.get_host()
    }
    let apiURL = AppConstants.AppOAuth_Base_LDAP + "userchangePassword";
     let outhHeader = this.commonService.get_AuthorizedHeader();
    return this.http.post<any>(apiURL, bussinesData, { headers: outhHeader }).pipe(
      map(res => {
        return res;

      })).pipe(catchError(this.commonService.handleError));

  }


  unlock$(req: userUnlockReq): Observable<string> {

    var bussinesData = {
      WindowsUser: req.WindowsUser,
      DomainName: req.DomainName,
      dom_id: req.dom_id,
      Emp_Id: req.Emp_Id,
      ResetUserId: req.ResetUserId,
      ResetUserName: req.ResetUserName,
      ResetUserCAIS: req.ResetUserCAIS,
      ticket : req.ticket ,
      host:  this.commonService.get_host()
    }
    let apiURL = AppConstants.AppOAuth_Base_LDAP + "userUnlock";
     let outhHeader = this.commonService.get_AuthorizedHeader();
    return this.http.post<any>(apiURL, bussinesData, { headers: outhHeader }).pipe(
      map(res => {
        return res;

      })).pipe(catchError(this.commonService.handleError));

  }

  signOut(): void {
    // clear token remove user from local storage to log user out

    localStorage.removeItem('currentResetLogin');
    this.logingChange_subject$.next(false);
  }

  getLastetDomain(domain:string){
    let str = localStorage.setItem('lastetDomainReseteos',domain);
    return str;
  }
  setLastetDomain(){
    let str = localStorage.getItem('lastetDomainReseteos');
    return str;
  }
  getCurrenLoging(): CurrentLogin {
    var currentLogin: CurrentLogin = new CurrentLogin();
    let str = localStorage.getItem('currentResetLogin');
    currentLogin = JSON.parse(str);

    return currentLogin;
  }

  getServerInfo$(): Observable<ApiServerInfo> {
    var bussinesData = {    };
    

    var apiURL = AppConstants.AppOAuth_Base_Meucci + "getServerInfo";

    let outhHeader = this.commonService.get_AuthorizedHeader();
    
    return this.http.get<ApiServerInfo>(apiURL,   { headers: outhHeader }).pipe(
      map(res => {
        //var toJSON = JSON.stringify(res);
        
        return res;

      })).pipe(catchError(this.commonService.handleError));
  }

  retriveEmpleadosReseteos(req: retriveEmpleadosReseteosReq): Observable<Empleado> {
    var bussinesData = {
      username: req.userName,
      dni: req.dni,
      domain: req.domain

    }
    var apiURL = AppConstants.AppOAuth_Base_Meucci + "retriveEmpleadosReseteos";
    let outhHeader = this.commonService.get_AuthorizedHeader();
    
    return this.http.post<Empleado>(apiURL, bussinesData,  { headers: outhHeader }).pipe(
      map(res => {
        
        return res;

      })).pipe(catchError(this.commonService.handleError));
  }


  //permite subscripcion  to the observable
  storage_Domains$(): Observable<Domain[]> {
    return this.domains$.asObservable();
  }

  checkDomains() {
    var domains: Domain[] = JSON.parse(localStorage.getItem('domains'));
    if (domains) {
      let domJson: Domain[] = domains;
      this.domains$.next(domains);
    }

    this.checkDomains$().subscribe(
      res => {
        
      },
      err => {
      });

  }

  checkDomains$(): Observable<Domain[]> {
    var bussinesData = {};
    var apiURL = AppConstants.AppOAuth_Base_Meucci + "retriveDomains";
    
    return this.http.post<Domain[]>(`${apiURL}`, bussinesData, AppConstants.httpClientOption_contenttype_json).pipe(
      map(res => {

        // localStorage.setItem('domains', JSON.stringify(res));
        // var domains: Domain[] = JSON.parse(localStorage.getItem('domains'));
        this.domains$.next(res);

        return res;

      })).pipe(
        //alert('error en el server al obtener los dominios');
        catchError(this.commonService.handleError)
      );
  }


  ldap_ping$(): Observable<any> {
    var bussinesData = {};
    var apiURL = AppConstants.AppOAuth_Base_LDAP + "ping";

    let headers = new HttpHeaders().set('Content-Type', 'application/json');
    //let headers = new (HttpHeaders)({ 'Content-Type': 'application/json' });
    headers= headers.append('Access-Control-Allow-Methods', '*');
    headers= headers.append('Access-Control-Allow-Headers', 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');
    headers= headers.append('Access-Control-Allow-Origin', '*');


    // const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    // headers.append('Access-Control-Allow-Headers', 'Content-Type');
    // headers.append('Access-Control-Allow-Methods', '*');
    // headers.append('Access-Control-Allow-Origin', '*');

    var httpHeader = { headers: headers };
    apiURL='http://localhost:50009/api/oauth/ping';
    
    return this.http.get<any>(`${apiURL}`,  { headers: headers }).pipe(
      map(res => {


        return res;

      })).pipe(
        //alert('error en el server al obtener los dominios');
        catchError(this.commonService.handleError)
      );
  }
}


