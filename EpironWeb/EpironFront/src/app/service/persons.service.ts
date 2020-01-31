import { Injectable, Inject } from '@angular/core';
// permmite cambiar la variable obsevada
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import {PersonBE, AddressBE} from '../../app/model/persons.model'
import { AppConstants, contextInfo, MotivoConsultaEnum } from "../model/common.constants";
import { ParamBE,  IContextInformation, IRequest, IResponse, Result } from '../model/common.model';
import { CommonService } from '../service/common.service';
import { HttpClient, HttpParams } from '@angular/common/http';


@Injectable()
export class PersonsService {
  private contextInfo: IContextInformation;
  
  constructor(private http: HttpClient,private commonService: CommonService) {
    this.contextInfo = contextInfo;
    
  }


  retrivePersonsGrid$ (searchText: string,  motivoConsulta: MotivoConsultaEnum):   Observable<PersonBE[]> {

      

    let outhHeader = this.commonService.get_AuthorizedHeader();
    const httpParams = new HttpParams()
    .set(`searchText`, searchText)
    .set(`motivoConsulta`, MotivoConsultaEnum[motivoConsulta].toString()) ;

    
    return  this.http.get<PersonBE[]>(`${AppConstants.AppAPI_URL}persons/retriveAll`,{ headers: outhHeader,params:httpParams }).pipe(
       map(res => {
       
        return res;
        
      })).pipe(catchError(this.commonService.handleError));
  }

  getPersonaByParamService$(personId: string, identityCardNumber: string,userId:string): Observable<PersonBE> {

      
    let outhHeader = this.commonService.get_AuthorizedHeader();
    const httpParams = new HttpParams()
    .set(`personId`, personId)
    .set(`userId`, userId)
    .set(`identityCardNumber`, identityCardNumber);

    return  this.http.get<PersonBE>(`${AppConstants.AppAPI_URL}persons/get`,{ headers: outhHeader,params:httpParams }).pipe(
       map(res => {
     
        //let result :PersonBE= JSON.parse(res) as PersonBE;
        return res;
        
      })).pipe(catchError(this.commonService.handleError));
  }


  retrivePersonaService$(searchText: string): Observable<PersonBE[]> {
    let outhHeader = this.commonService.get_AuthorizedHeader();
    const httpParams = new HttpParams()
    .set(`searchText`, searchText);
    return  this.http.get<PersonBE[]>(`${AppConstants.AppAPI_URL}persons/retriveAll`,{ headers: outhHeader,params:httpParams }).pipe(
       map(res => {
        return res;
      })).pipe(catchError(this.commonService.handleError));
  }




 
}

