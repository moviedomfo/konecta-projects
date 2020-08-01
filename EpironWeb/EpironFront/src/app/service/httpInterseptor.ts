import { HttpRequest, HttpInterceptor, HttpEvent, HttpHandler, HttpHeaders, HttpClient } from '@angular/common/http';
import { Observable, Subject, throwError } from 'rxjs';
import { Injectable } from '@angular/core';
import {map, catchError } from 'rxjs/operators';
import { helperFunctions } from './helperFunctions';
import {  ContextInformation, ExecuteReq, Request, IRequest, IResponse, Result, ServiceError,  IpInfo, IAPIRequest } from '../model/common.model';
import { CurrentLoginEpiron,  AppConstants } from '../model';

@Injectable()
export class fwkSeriveHttpInterseptor implements HttpInterceptor{

    public ipinfo: IpInfo;
    constructor(private http: HttpClient){
        this.ipinfo = new IpInfo();

        this.get_host_ipInfo().subscribe(res => {
                 this.ipinfo = res;
     
        });
    }


    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        console.log("------------intercept------------");
        if(req.url.includes('api/disp/execute/',0))
        {

        }
        
        
        
        let outhHeader = this.get_AuthorizedHeader();
         const reqCloned = req.clone({
        //      headers: outhHeader 
         } );


        return  next.handle(reqCloned).pipe(catchError(helperFunctions.handleError));;
    }



  /**
  * utiliza una api paara retornar informacion hacerca del host cliente
  * @param 
  * @returns json con ip,country de la clase ipinfo
  */
 get_host_ipInfo(): Observable<IpInfo> {

    return this.http.get<IpInfo>('https://ipinfo.io?token=21ea63fe5267b3').pipe(
      map(function (res) {
        return res;
      })).pipe(catchError(helperFunctions.handleError));
  }

    public get_AuthorizedHeader(): HttpHeaders {
        //let currentLogin: CurrentLogin = JSON.parse(localStorage.getItem('currentLogin'));
    
        let currentLogin: CurrentLoginEpiron = JSON.parse(localStorage.getItem('currentLogin'));
        if(currentLogin){
            //let headers = new HttpHeaders({ 'Authorization': "Bearer " + currentLogin.userData.access_token }).set('securityProviderName',AppConstants.oaut_securityProviderName);
            let headers  = new HttpHeaders();
            headers.append('Access-Control-Allow-Methods', '*');
            headers.append('Access-Control-Allow-Headers', 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');
            headers.append('Access-Control-Allow-Origin', '*');
            return headers;
        }
    
      
      }
   //Una req que se manipuila ya no la podemos volver a llamar: por eso hay q clonarla
  


/**
 * rellena un objetto  ExecuteReq que recive el método POST / Execuete del fwk
 * @param serviceName : Nombre del servicio de capa SVC
 * @param bussinesData : Objeto de negocio del SVC(REQ) donde bussinesData => IReq.BussinesData
 * @param serviceProviderName : fwk service metadata provider : en la configuracion por defecto que uilizaran los servicios SVC del FWK
 */
generete_post_Params(serviceName, bussinesData,serviceProviderName = ''): ExecuteReq {
    var req = this.createFwk_SOA_REQ(bussinesData);
    var executeReq: ExecuteReq = new ExecuteReq();

    if(helperFunctions.string_IsNullOrEmpty(serviceProviderName))
      executeReq.serviceProviderName = AppConstants.fwkServiceProvider_Epiron;
    else
      executeReq.serviceProviderName = serviceProviderName;

    executeReq.serviceName = serviceName;
    executeReq.jsonRequest = JSON.stringify(req);

    return executeReq;
  }

  
/**
 * Construye un Request que si utiliza como parametro para los servicios Fwk-SVC
 *  Los Request contiennen 
 *    contextInfo : inofrmacion de contexto fwk
 *    bussinesData : cualquier entity
 *    SecurityProviderName proveedor de seguridad que se utilizara
 * @param bussinesData 
 */
createFwk_SOA_REQ(bussinesData: any): Request {
    let contextInfo: ContextInformation = new ContextInformation();
    let req: Request = new Request();
    let currentLogin: CurrentLoginEpiron = JSON.parse(localStorage.getItem('currentLogin'));
    contextInfo.Culture = AppConstants.Culture;
    contextInfo.ProviderNameWithCultureInfo = "";
    contextInfo.HostName =  this.ipinfo.ip;
    contextInfo.HostIp = this.ipinfo.ip;
    contextInfo.HostTime = new Date(),
    //contextInfo.ServerName = 'WebAPIDispatcherClienteWeb';
    contextInfo.ServerTime = new Date();

    if(currentLogin){
      if (currentLogin.userData.UserName) { contextInfo.userName = currentLogin.userData.UserName; }
      
      if (currentLogin.userData.UserGuid)  { contextInfo.userId = currentLogin.userData.UserGuid;}
    }
    
    contextInfo.AppId = AppConstants.AppId;

    contextInfo.ProviderName = AppConstants.fwkServiceProvider_Epiron;
    req.ContextInformation = contextInfo;
    req.BusinessData = bussinesData;
    req.SecurityProviderName = AppConstants.oaut_securityProviderName;
    req.Encrypt = false;
    req.Error = null;
    return req;
  }


/**
 * 
 * @param req Cualquier servicio que implemente IAPIRequest. Esta interfaz es un standar utilizxado para servicios 
 *  API rest Full. No es un IRequets de los servicios Fwk-SVC que sonm invocados a travez del metodo post/execute
 * 
 */
  set_Fwk_API_REQ(req:IAPIRequest) : IAPIRequest {
       
    let currentLogin: CurrentLoginEpiron = JSON.parse(localStorage.getItem('currentLogin'));
    req.Culture = AppConstants.Culture;
    req.SecurityProviderName = AppConstants.oaut_securityProviderName;
    req.AppId = AppConstants.AppId;
    req.ClientIp = this.ipinfo.ip;
    
    if (currentLogin.userData.UserName) { 
      req.UserId = currentLogin.userData.UserPlaceGuid;
    }

    return req;
  }
}