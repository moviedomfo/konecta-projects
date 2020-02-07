
import { Result, EpironApiResponse } from '../model/common.model';
import { AppConstants } from "../model/common.constants";
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';
import { ValidarAplicacionRes } from '../model/epiron/epiron.security.model';
import { helperFunctions } from './helperFunctions';
import { CurrentLoginEpiron, UserAutenticacionRes } from '../model';
import { AuthenticationService } from './authentication.service';
import { ApplicationSettingBE, CaseByUserGuidBE } from '../model/epiron/epiron.model';


@Injectable()
export class EpironService {

  constructor(private http: HttpClient, private commonService: CommonService, private authService: AuthenticationService) {

  }


  public SearchAllApplicationSettings$(): Observable<ApplicationSettingBE[]> {


    //UserAutenticacionReq
    var bussinesData = {
  
      appInstanceGUID: AppConstants.AppInstanceGUID
  
    }

    let outhHeader = this.commonService.get_AuthorizedHeader();
    let executeReq=  this.commonService.generete_post_Params("AllApplicationSettings_s_Service", bussinesData);

    return  this.http.post<any>(`${AppConstants.AppExecuteAPI_URL}`,executeReq,{ headers: outhHeader }).pipe(
       map(res => {

        let result :Result= JSON.parse(res.Result) as Result;

        if (result.Error) {
          throw  Observable.throw(result.Error);
        }

        let list = result.BusinessData as ApplicationSettingBE[];
        return list;
     })).pipe(catchError(helperFunctions.handleError));


  

  }
/**
 * Trae el tipo o el modo en el que se van a tomar los nuevos y pendientes. 
 */
  public GetAssigementCaseType$(): Observable<ApplicationSettingBE[]> {


    //UserAutenticacionReq
    var bussinesData = {
  
      appInstanceGUID: AppConstants.AppInstanceGUID
  
    }

    let outhHeader = this.commonService.get_AuthorizedHeader();
    let executeReq=  this.commonService.generete_post_Params("AllApplicationSettings_s_Service", bussinesData);
    
    return  this.http.post<any>(`${AppConstants.AppExecuteAPI_URL}`,executeReq,{ headers: outhHeader }).pipe(
       map(res => {

        let result :Result= JSON.parse(res.Result) as Result;

        if (result.Error) {
          throw  Observable.throw(result.Error);
        }

        let list = result.BusinessData as ApplicationSettingBE[];
        return list;
     })).pipe(catchError(helperFunctions.handleError));


  

  }


  /**
 * Trae el tipo o el modo en el que se van a tomar los nuevos y pendientes. 
 */
public SearchCaseByUserGuidService$(): Observable<CaseByUserGuidBE[]> {

  let currentLogin = this.authService.getCurrenLoging();

  //UserAutenticacionReq
  var bussinesData = {

    UserGuid: AppConstants.emptyGuid,//currentLogin.userData.UserGuid,
    State: 1

  }

  let outhHeader = this.commonService.get_AuthorizedHeader();
  let executeReq=  this.commonService.generete_post_Params("SearchCaseByUserGuidService", bussinesData);
  
  return  this.http.post<any>(`${AppConstants.AppExecuteAPI_URL}`,executeReq,{ headers: outhHeader }).pipe(
     map(res => {

      
      let result :Result= JSON.parse(res.Result.Result) as Result;

      if (result.Error) {
        throw  Observable.throw(result.Error);
      }

      let list = result.BusinessData as CaseByUserGuidBE[];
      return list;
   })).pipe(catchError(helperFunctions.handleError));




}


  
}

