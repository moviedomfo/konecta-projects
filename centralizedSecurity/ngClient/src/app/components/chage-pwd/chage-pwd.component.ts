import { Component, OnInit} from '@angular/core';

import {  CurrentLogin,  ServiceError } from 'src/app/model/common.model';
import { SerurityService } from 'src/app/service/serurity.service';
import {  userChangePasswordReq } from 'src/app/model/bussinessModel';
import { runInThisContext } from 'vm';
import {  Router } from '@angular/router';



@Component({
  selector: 'app-chage-pwd',
  templateUrl: './chage-pwd.component.html'
  
})
export class ChagePwdComponent implements OnInit {

  public globalError: ServiceError;
  public currentLogin: CurrentLogin;
  public loadingDomains : boolean = false;
  public showSpinner:boolean=false;
  public apiResponseMessage : string;
  public confirmPwd : string;
  public pwd : string;
  public error_mustMatch = false;
  public  confirmPwd_error_required = false;
  public  pwd_error_required = false;
  public  pwd_error_not_valid = false;
  public  pwd_error_min = false;
  public  showSubmit = false;

  constructor(private secService: SerurityService,    private route: Router) {    }

  ngOnInit() { 
    this.currentLogin = this.secService.getCurrenLoging();
    this.confirmPwd='';
    this.pwd='';
  
   }

  
  validate(){
    let ifErrors = false;

    this.error_mustMatch = false;
    this.confirmPwd_error_required = false;
    this.pwd_error_required = false;
    this.pwd_error_not_valid = false;
    this.pwd_error_min = false;

    if(!this.pwd || this.pwd.length === 0)
      this.pwd_error_required = true;

    if(!this.confirmPwd || this.confirmPwd.length === 0)
      this.confirmPwd_error_required = true;

    if(this.pwd_error_required === false && this.confirmPwd_error_required === false){
      if(this.confirmPwd !== this.pwd){
        this.error_mustMatch= true;
        return false;
      }
      
    }
    
    if(this.pwd.length < 8)
    {
      this.pwd_error_min = true;
      return false;
    }

    let regExpString = /^(?=.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).*$/;

    let regexp = new RegExp(regExpString);
    
    
    this.pwd_error_not_valid =  !regexp.test(this.pwd); //si no es valido (test) retorna false x lo tanto pwd_error_not_valid == true
   

// si todas son true
    ifErrors = this.error_mustMatch  || this.pwd_error_required ||  this.confirmPwd_error_required  ||  this.pwd_error_not_valid  ||  this.pwd_error_min;

      return !ifErrors;
  }

   onSubmit(){
    
    
    this.apiResponseMessage = '';
   
   
    var req:userChangePasswordReq = new userChangePasswordReq();
                          
    req.domainName = this.currentLogin.currentUser.Domain;
    req.dom_id = this.currentLogin.currentUser.DomainId;
    req.emp_Id = this.currentLogin.currentUser.Emp_Id;
    req.userName = this.currentLogin.currentUser.UserName;
    req.userId = this.currentLogin.currentUser.Emp_Id;
    req.newPassword = this.pwd;
    if(!this.validate())
    {
      return;
    } 


   
    var resetPassword$= this.secService.userChangePassword$(req);
    this.showSubmit=true;
    this.showSpinner=true;
    resetPassword$.subscribe(
      res => {
        this.apiResponseMessage= res;
        this.globalError=null;
        this.showSpinner= false;
        alert('La contraseña fue cambiada con éxito');    
        //onLogout 
          this.secService.signOut();
          this.route.navigate(['/login']);
          this.showSpinner= false;

      },
      err => {
        this.globalError = err;
        this.showSpinner= false;
        this.showSubmit=false;
        
        //this.btnCloseResset.nativeElement.click();
      }
    );
  }
}
