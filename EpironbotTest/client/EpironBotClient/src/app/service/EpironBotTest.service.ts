
import { AppConstants } from "../model/common.constants";
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { CommonService } from './common.service';
import { Injectable } from '@angular/core';

import { helperFunctions } from './helperFunctions';
import { BotCommentModeratedReq, ModeratedComment } from '../model/entities.models';





@Injectable()
export class EpironBotTestService {

  constructor(private http: HttpClient, private commonService: CommonService) {

  }


  /**
 * GET moderador 
 */
  public Searchcomments$(): Observable<BotCommentModeratedReq[]> {


    const httpParams = new HttpParams();
    // .set(`searchText`, searchText)
    // .set(`nroDoc`, dni)
    let outhHeader = this.commonService.get_AuthorizedHeader();

    return this.http.get<BotCommentModeratedReq[]>(`${AppConstants.AppAPI_URL}bot/retriveAll`, { headers: outhHeader, params: httpParams }).pipe(
      map(res => {

        return res;
      })).pipe(catchError(this.commonService.handleError));


  }


  /**
   * POST moderador 
   * @param moderatedComment 
   */
  responseModeratedBotComment$(moderatedComment: ModeratedComment): Observable<any> {


    return this.http.post<number>(`${AppConstants.AppAPI_URL}bot/responseModeratedBotComment`, moderatedComment).pipe(
      map(res => {
        return res;
      })).pipe(catchError(this.commonService.handleError));
  }
}

