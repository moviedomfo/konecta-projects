import { HttpRequest, HttpInterceptor, HttpEvent, HttpHandler } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { IContextInformation, ContextInformation, ExecuteReq, Request, IRequest, IResponse, Result, ServiceError,  IpInfo } from '../model/common.model';
import { CommonService } from './common.service';
import { catchError } from 'rxjs/operators';
import { helperFunctions } from './helperFunctions';

@Injectable()
export class fwkSeriveHttpInterseptor implements HttpInterceptor{
    constructor(private commonService: CommonService){   }

    
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {


        
        console.log("------------intercept------------");
        console.log(JSON.stringify(req));
        let outhHeader = this.commonService.get_AuthorizedHeader();
        const reqCloned = req.clone({
             headers: outhHeader 
        } );


        return  next.handle(reqCloned).pipe(catchError(helperFunctions.handleError));;
    }

   //Una req que se manipuila ya no la podemos volver a llamar: por eso hay q clonarla
  




    // createFwk_SOA_REQ(bussinesData: any): Request {
    //     let contextInfo: ContextInformation = new ContextInformation();
    //     let req: Request = new Request();
    //     let currentLogin: CurrentLogin = JSON.parse(localStorage.getItem('currentResetLogin'));
    //     contextInfo.Culture = "ES-AR";
    //     contextInfo.ProviderNameWithCultureInfo = "";
    //     contextInfo.HostName = this.ipinfo.city + this.ipinfo.country;
    //     contextInfo.HostIp = this.ipinfo.ip;
    //     contextInfo.HostTime = new Date(),
    //       contextInfo.ServerName = '';
    //     contextInfo.ServerTime = new Date();
    
    
    //     if (currentLogin && currentLogin.currentUser.UserName) { contextInfo.UserName = currentLogin.currentUser.UserName; }
    //     else { contextInfo.UserName = 'mrenaudo'; }
    
    //     contextInfo.UserId = 'mrenaudo';
    //     contextInfo.AppId = 'App';
    //     contextInfo.ProviderName = 'AppTesting';
    //     req.ContextInformation = contextInfo;
    //     req.BusinessData = bussinesData;
    //     req.Error = null;
    //     req.SecurityProviderName = "";
    //     req.Encrypt = false;
    
    //     return req;
    //   }
}