import { Injectable } from '@angular/core';
import { Observable, Subject, throwError } from 'rxjs';
import { Router } from '@angular/router'
import { HttpHeaders, HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { ServiceError } from '../model/entities.models';

//var colors = require('colors/safe');
@Injectable()
export class CommonService {
  
  public mainComponentTitle_subject$: Subject<string> = new Subject<string>();
  
  
  constructor(private http: HttpClient, private router: Router) {
  
  
  
   }

  //permite subscripcion añl Subject con el titulo
  get_mainComponentTitle$(): Observable<string> {
    return this.mainComponentTitle_subject$.asObservable();
  }
  //permite q un componente cualquiera emita cambio de titulo y este altere el header del dasboard
  Set_mainComponentTitle(tittle: string) {

    this.mainComponentTitle_subject$.next(tittle);
  }

  
  //Retorna un HttpHeaders con CORS y 'Authorization': "Bearer + TOKEN"
  public get_AuthorizedHeader(): HttpHeaders {

        let headers  = new HttpHeaders();
        headers.append('Access-Control-Allow-Methods', '*');
        headers.append('Access-Control-Allow-Headers', 'Content-Type, Access-Control-Allow-Headers, Authorization, X-Requested-With');
        headers.append('Access-Control-Allow-Origin', '*');
        return headers;
    

  
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






}
