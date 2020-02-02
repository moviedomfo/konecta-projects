
import {  Result } from '../model/common.model';
import { AppConstants } from "../model/common.constants";
import { HttpClient } from '@angular/common/http';
import {  Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';
import { ValidateAppResponse, ValidateAppRequest } from '../model/epiron/epiron.security.model';


@Injectable()
export class EpironSecurityService {
    
  constructor(private http: HttpClient, private commonService: CommonService) {
  
  }


  
  public validateAppService$(): Observable<any> {
    
    
    var bussinesData = {
        AppInstanceGuid: AppConstants.AppInstanceGUID,
        LoginHost: this.commonService.get_host(),
        LoginIp: this.commonService.ipinfo.ip,
        WebServiceUrl: ""
    
    };

    let outhHeader = this.commonService.get_AuthorizedHeader();
    let executeReq=  this.commonService.generete_post_Params("ValidateAppService", bussinesData);
    
    return  this.http.post<any>(`${AppConstants.AppExecuteAPI_URL}`,executeReq,{ headers: outhHeader }).pipe(
       map(res => {

        let result :Result= JSON.parse(res.Result) as Result;
        if (result.Error) {
          throw  Observable.throw(result.Error);
        }

        let item: ValidateAppResponse = result.BusinessData as ValidateAppResponse;
       return item;
      })).pipe(catchError(this.commonService.handleError));
  }


}

