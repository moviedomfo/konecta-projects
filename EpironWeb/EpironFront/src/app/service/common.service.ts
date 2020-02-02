import { Injectable } from '@angular/core';
import { AppConstants,  CommonParams, ParamTypeEnum } from "../model/common.constants";
import { IAPIRequest, ContextInformation, ExecuteReq, Request, ServiceError,  IpInfo, ParamBE, UbicacionItemBE, localidadesResponse, ApiServerInfo } from '../model/common.model';

// permmite cambiar la variable obsevada
import { Observable, Subject, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
//import { element } from 'protractor';
import { Router } from '@angular/router'
import { HttpHeaders, HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { helperFunctions } from './helperFunctions';
import { headersToString } from 'selenium-webdriver/http';
import { CurrentLogin } from '../model';

//var colors = require('colors/safe');
@Injectable()
export class CommonService {
  public paramList: ParamBE[] = [];
  public paramList$: Subject<ParamBE[]> = new Subject<ParamBE[]>();

  public mainComponentTitle_subject$: Subject<string> = new Subject<string>();
  public ipinfo: IpInfo;
  
  constructor(private http: HttpClient, private router: Router) {
  
    this.ipinfo = new IpInfo();
    this.get_host_ipInfo().subscribe(res => {
      this.ipinfo = res;
     
    });
   }

  //permite subscripcion añl Subject con el titulo
  get_mainComponentTitle$(): Observable<string> {
    return this.mainComponentTitle_subject$.asObservable();
  }
  //permite q un componente cualquiera emita cambio de titulo y este altere el header del dasboard
  Set_mainComponentTitle(tittle: string) {

    this.mainComponentTitle_subject$.next(tittle);
  }
  get_host(): string {

    return  "Ip: " + this.ipinfo.ip + ", city: " +  this.ipinfo.city + ", region :" +  this.ipinfo.region + ", country :" + this.ipinfo.country;
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
      })).pipe(catchError(this.handleError));
  }

  
  
  /**
   * @paramId : Nombre de tabla
   * @culture : Cultuer en-ES
   */
  searchParametroByParams$(parentId: number, culture: string): Observable<ParamBE[]> {

   
    if (!parentId)
       parentId = -2000;
  if (!culture)
      culture = AppConstants.Culture.toString();

    
    const httpParams = new HttpParams()
    .set(`parentId`, parentId.toString())
    .set(`enables`, 'true')
    .set(`culture`, culture);

    return this.http.get<ParamBE[]>(`${AppConstants.AppAPI_URL}params/retriveAll`, {params:httpParams}).pipe(
      map(res => {
        
        return res;
      })).pipe(catchError(this.handleError));
  }


  searchProvincias$(): Observable<UbicacionItemBE[]> {

    return this.http.get<UbicacionItemBE[]>(`${AppConstants.AppAPI_URL}params/Provincias`).pipe(
      map(res => {
        let selectOption :UbicacionItemBE =new UbicacionItemBE();
        selectOption.id=-2000;
        selectOption.nombre = 'Seleccione opción';
        res.push(selectOption);
        return res;
      })).pipe(catchError(this.handleError));
  }
    
  searchLocalidades$(provinceId: number): Observable<localidadesResponse> {
    const httpParams = new HttpParams()
    .set(`provinceId`, provinceId.toString());

    return this.http.get<localidadesResponse>(`${AppConstants.AppAPI_URL}params/Localidades`,{params:httpParams}).pipe(
      map(res => {
        let selectOption :UbicacionItemBE =new UbicacionItemBE();
        selectOption.id=-2000;
        selectOption.nombre = 'Seleccione opción';
        res.localidades.push(selectOption);
        return res;
      })).pipe(catchError(this.handleError));
  }
      
  getServerInfo$(): Observable<ApiServerInfo> {
    const httpParams = new HttpParams();
    

    return this.http.get<ApiServerInfo>(`${AppConstants.AppAPI_URL}params/serverInfo`,{params:httpParams}).pipe(
      map(res => {
       console.log(JSON.stringify(res));
        return res;
      })).pipe(catchError(this.handleError));
  }
  
  /**
  * @params : parametros 
  * @parameterToAppend : CommonParam.Id
  */
  appendExtraParamsCombo(params: ParamBE[], parameterToAppend: number): ParamBE[] {


    var p: ParamBE = new ParamBE();


    switch (parameterToAppend) {
      case CommonParams.SeleccioneUnaOpcion.paramId:
        {
          p.description = CommonParams.SeleccioneUnaOpcion.name;
          p.paramId = CommonParams.SeleccioneUnaOpcion.paramId;
          break;
        }
      case CommonParams.Ninguno.paramId:
        {
          p.description = CommonParams.Ninguno.name;
          p.paramId = CommonParams.Ninguno.paramId;
          break;
        }
      case CommonParams.TodosComboBoxValue.paramId:
        {
          
          p.description = CommonParams.TodosComboBoxValue.name;
          p.paramId = CommonParams.TodosComboBoxValue.paramId;
          break;
        }
      case CommonParams.VariosComboBoxValue.paramId:
        {
          p.description = CommonParams.VariosComboBoxValue.name;
          p.paramId = CommonParams.VariosComboBoxValue.paramId;
          break;
        }

    }
    params.push(p);

    return params;

  }

  //Metodo directo sin observable
  getById(parentId: number): ParamBE {
    let param: ParamBE;
    param = this.paramList.filter(p => p.parentId === parentId)[0];
    return param;
  }

  // retornra el objeto Request de un URLSearchParams: Este contiene las siguientes clases
  //  SecurityProviderName?: string;
  // Encrypt?: boolean;
  // Error?:object;
  // ServiceName?: string;
  // BusinessData?:object;
  // CacheSettings?:object;
  // ContextInformation:ContextInformation;
  retrive_Request(searchParams: URLSearchParams) {

    let REQ: Request = JSON.parse(searchParams.get("jsonRequest")) as Request;
    //alert(JSON.stringify(context));
    return REQ;
  }

/**
 * rellena un objetto  ExecuteReq que recive el método POST / Execuete del fwk
 * @param serviceName : Nombre del servicio de capa SVC
 * @param bussinesData : Objeto de negocio del SVC(REQ) donde bussinesData => IReq.BussinesData
 * @param serviceProviderName : fwk service metadata provider : en la configuracion por defecto que uilizaran los servicios SVC del FWK
 */
  generete_get_searchParams(serviceName, bussinesData,serviceProviderName = ''): URLSearchParams {
    let searchParams: URLSearchParams = new URLSearchParams();
    var req = this.createFwk_SOA_REQ(bussinesData);
    req.ServiceName = serviceName;
    
    if(helperFunctions.string_IsNullOrEmpty(serviceProviderName))
        serviceProviderName = AppConstants.fwkServiceProvider_Epiron;
 

    searchParams.set("serviceProviderName", serviceProviderName);//defaul 
    searchParams.set("serviceName", serviceName);
    searchParams.set("jsonRequest", JSON.stringify(req));


    return searchParams;
  }

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
    let currentLogin: CurrentLogin = JSON.parse(localStorage.getItem('currentLogin'));
    contextInfo.Culture = AppConstants.Culture;
    contextInfo.ProviderNameWithCultureInfo = "";
    contextInfo.HostName =  this.ipinfo.ip;
    contextInfo.HostIp = this.ipinfo.ip;
    contextInfo.HostTime = new Date(),
    //contextInfo.ServerName = 'WebAPIDispatcherClienteWeb';
    contextInfo.ServerTime = new Date();

    if(currentLogin){
      if (currentLogin.currentUser.userName) { contextInfo.userName = currentLogin.currentUser.userName; }
      
      if (currentLogin.currentUser.userId)  { contextInfo.userId = currentLogin.currentUser.userId;}
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
       
    let currentLogin: CurrentLogin = JSON.parse(localStorage.getItem('currentLogin'));
    req.Culture = AppConstants.Culture;
    req.SecurityProviderName = AppConstants.oaut_securityProviderName;
    req.AppId = AppConstants.AppId;
    req.ClientIp = this.ipinfo.ip;
    
    if (currentLogin.currentUser.userName) { 
      req.UserId = currentLogin.currentUser.userId;
    }
    

    

    return req;
  }

  //Retorna un HttpHeaders con CORS y 'Authorization': "Bearer + TOKEN"
  public get_AuthorizedHeader(): HttpHeaders {
    let currentLogin: CurrentLogin = JSON.parse(localStorage.getItem('currentLogin'));
    if(currentLogin){
        let headers = new HttpHeaders({ 'Authorization': "Bearer " + currentLogin.oAuthResult.access_token }).set('securityProviderName',AppConstants.oaut_securityProviderName);
        headers.append('Access-Control-Allow-Methods', '*');
        headers.append('Access-Control-Allow-Headers', 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');
        headers.append('Access-Control-Allow-Origin', '*');
        return headers;
    }

  
  }
  public get_AuthorizedHeader$(): Observable<HttpHeaders> {

    const obs = Observable.create(()=>{
      let currentLogin: CurrentLogin = JSON.parse(localStorage.getItem('currentLogin'));
      if(currentLogin){
          let headers = new HttpHeaders({ 'Authorization': "Bearer " + currentLogin.oAuthResult.access_token }).set('securityProviderName',AppConstants.oaut_securityProviderName);
          headers.append('Access-Control-Allow-Methods', '*');
          headers.append('Access-Control-Allow-Headers', 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');
          headers.append('Access-Control-Allow-Origin', '*');
          return headers;
  
         
      }else{
        let  er:ServiceError = new   ServiceError();
        er.Message = "Not user authenticattion, the user needs to supply credentials";
        er.Status = 401;
        return throwError(er);
      }
    });
   
    return obs;
  
  }
  public handleErrorService(serviceError: ServiceError) {
    if (serviceError) {
      alert("Se encontraron errores " + serviceError.Message);
    }
  }
  public handleError2(httpError :any) {
    let errorMessage = '';
    if (httpError.error instanceof ErrorEvent) {
      // client-side error
      errorMessage = `Error: ${httpError.error.message}`;
    } 
    
    if (httpError.error instanceof ProgressEvent) {
      
      console.log(httpError.message);
    }

    return throwError(errorMessage);
  }

  ///Error inspection, interpretation, and resolution is something you want to do in the service, not in the component.
  public handleError(httpError: HttpErrorResponse | any) {
    console.log(httpError);
    let ex: ServiceError = new ServiceError();
    ex.Machine = 'PC-Desarrollo';
    // A client-side or network error occurred. Handle it accordingly.
    if (httpError.error instanceof ProgressEvent) {
      ex.Message = 'Client-side error occurred : ' + ex.Message;


      if (ex.Message.includes('api/oauth/authenticate')) {
        ex.Message = ex.Message + ' Can not authenticate ';
      }
      if (httpError.status == 0) {
        ex.Message = ex.Message + ' No es posible autenticar. Por favor comuníquese con el administrador';
      }
      ex.Message = ex.Message + "\r\n" + httpError.message;
      ex.Status = httpError.status;
      return throwError(ex);
    }

    // The backend returned an unsuccessful response code.
    // The response body may contain clues as to what went wrong,
    if (httpError instanceof HttpErrorResponse) {
      //alert(error.error);
      
      ex.Status = httpError.status;
      if(ex.Status === 401){
        ex.Message = "No está autorizado para realizar esta acción. Intente iniciar sesion nuevamente, si el problema persiste, por favor comuníquese con el administrador";
        return throwError(ex);
      }

     

      if (httpError.error) {
        ex.Type =  httpError.error.ExceptionType || httpError.error.exceptionType || httpError.error.ClassName;
        if(ex.Type){
          ex.Message  = httpError.error.ExceptionMessage || httpError.error.exceptionMessage || httpError.error.Message;
          if (httpError.error.InnerException) {
            ex.Message = ex.Message + "\r\n" + httpError.error.ExceptionMessage || httpError.error.InnerException.exceptionMessage || httpError.error.InnerException.Message;
          }
        }
        else{
          ex.Message = httpError.error;
        }
      
        return throwError(ex);
      }

      if (httpError.message) {
        ex.Message = ex.Message + "\r\n" + httpError.message;
      }
      return throwError(ex);
    }


    ex.Message = httpError.message;

    return throwError(ex);







    // return an observable with a user-facing error message
    //return Observable.throw(ex); // <= B // se comenta para la version 7
  }



  public handleErrorObservable(error: ServiceError) {

    console.error(error.Message || error);
    return Observable.throw(error.message);
  }
  public handleErrorPromise(error: Response | any) {

    return Promise.reject(error.message || error);
  }

  public handleHttpError(error) {
    console.log(JSON.stringify(error));
    if (error.status == '401') {
      //Error de permisos
      this.router.navigate(['login']);
    } else {
      console.log('Oto error status : ' + error.status);
    }

    return Observable.throw(error._body)
  }
  //cuando se le pasa un byte[] retorna su base64 string
  public convert_byteArrayTobase64(arrayBuffer: ArrayBuffer): string {
    var base64String = btoa(String.fromCharCode.apply(null, new Uint8Array(arrayBuffer)));
    return base64String;
  }
}
