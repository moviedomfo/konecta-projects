import { Component, OnInit,  AfterViewInit} from '@angular/core';
import { User, ServiceError } from 'src/app/model/common.model';

import { SerurityService } from 'src/app/service/serurity.service';

import {  forgotPassword_requetsReq, Empleado } from 'src/app/model/bussinessModel';
import { CommonService } from 'src/app/service/common.service';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';


@Component({
  selector: 'app-forgot-pwd',
  templateUrl: './forgot-pwd.component.html',
  styleUrls: ['./forgot-pwd.component.css']
  
})
export class ForgotPwdComponent implements OnInit , AfterViewInit{

  public loading:boolean= false;
  public empleado:Empleado;
  public messageResult:string='';
  public globalError: ServiceError;
  public currentUser: User;
  public solicitudEnviada_ok : boolean;
  public mail_toshow:string;
  public emailNotFound : boolean;
  public captcha_key : string;
 public captcha_true = false;
  constructor(
    private commonService: CommonService,
    private Serurity: SerurityService,
    private route: Router) {
      this.currentUser = new User();
      
      this.solicitudEnviada_ok = false;

  }
  ngAfterViewInit() {
    
}

  ngOnInit() {
  
    this.captcha_key = environment.recaptcha_key;
  }
  onloadCallback = function() {
    alert("grecaptcha is ready!");
  }
  public resolved(captchaResponse: string) {
    //console.log (`Resolved captcha with response: ${captchaResponse}`);

    this.captcha_true = true;
  }
  retriveWindowsUser(){
    this.emailNotFound=false;
    this.globalError =null;
    var req :forgotPassword_requetsReq   = new forgotPassword_requetsReq();
    req.dni = this.currentUser.DNI;
    req.host = this.commonService.get_host();
    //req.userName = this.currentUser.UserName;
    this.loading = true;
    

    var forgot$ = this.Serurity.userForgotPassword_checkDNI$(req)
    forgot$.subscribe(
      res => {
       this.empleado = res;
       var mail = this.empleado.Email ;
     
       var arrobaIndex = mail.indexOf('@');
       var mail_2 = mail.slice(arrobaIndex -3,mail.length);

       this.mail_toshow = mail.slice(0,arrobaIndex -3);
       var reg = /./g;
       this.mail_toshow = this.mail_toshow.replace(reg, '*');
       this.mail_toshow =  this.mail_toshow + mail_2;

       this.empleado.DNI = mail.slice(0,3);


       this.loading = false;
      },
      err => {
        
        this.globalError = err;
        this.loading = false;

        //No contiene email
        if(this.globalError.ErrorId==='1000'){
          this.emailNotFound=true;
        }
      }
    );
    
  }

   escapeRegExp(string) {
    return string.replace(/[.*+\-?^${}()|[\]\\]/g, '\\$&'); // $& means the whole matched string
  }

  forgotPassword_requets(){
    this.globalError =null;
    var req :forgotPassword_requetsReq   = new forgotPassword_requetsReq();
    req.dni = this.currentUser.DNI;
    req.host = this.commonService.get_host();
    //req.userName = this.currentUser.UserName;
    this.loading = true;
    var forgot$ = this.Serurity.forgotPassword_requets$(req)
    forgot$.subscribe(
      res => {

        this.loading = false;
        //console.log(JSON.stringify(res));
        if(res.Status === 'Success' ) {
          this.messageResult = res.Message;
          this.solicitudEnviada_ok = true;
        }else
        {
          this.solicitudEnviada_ok = false;
          this.messageResult = '';
          this.globalError = new ServiceError();
          this.globalError.Message = res.Message;
          
        }
       
      },
      err => {
         this.loading = false;
        this.globalError = err;
      }
    );
    
  }

  goback(){
  this.route.navigate(['/login']);
}
  // retriveDomains(){
  //   var checkDomains$ : Observable<Domain[]> =  this.Serurity.checkDomains$();
  //   this.loadingDomains=true;
  //   checkDomains$.subscribe(
  //     res => {
  //       //console.log(JSON.stringify(res));
  //      this.domains=res;
  //      this.loadingDomains=false;
  //     },
  //     err => {
        
  //       //this.OnComponentError.emit(err);
  //       this.loadingDomains = false;
  //       this.globalError = err;
  //     }
  //   );
    
  //}


 

}

