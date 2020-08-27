import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { CurrentLogin, ServiceError } from 'src/app/model/common.model';
import { SerurityService } from 'src/app/service/serurity.service';
import { userChangePasswordReq, Domain, userForgotPasswordVerifyReq, Empleado, WindosUser } from 'src/app/model/bussinessModel';
import { Router, ActivatedRoute } from '@angular/router';


declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-forgot-pwd-change',
  templateUrl: './forgot-pwd-change.component.html'

})
export class ForgotPwdChangeComponent implements OnInit {

  public globalError: ServiceError;
  
  public empleado:Empleado;
  public selectedwindowsUser :WindosUser;
  public showSpinner: boolean = false;
  public apiResponseMessage: string;
  public confirmPwd: string;
  public pwd: string;
  public code: string;
  public dni: string;
  
  public loadingDomains: boolean = false;
  
  public error_mustMatch = false;
  public confirmPwd_error_required = false;
  public pwd_error_required = false;
  public pwd_error_not_valid = false;
  public pwd_error_min = false;


  constructor(private secService: SerurityService, 
    private router: Router,
    private route: ActivatedRoute) { }

  ngOnInit() {

    this.empleado= new Empleado();
     

     this.route.queryParams.subscribe(params=>{
      this.code = params.code;
      
      this.checkCode();
     })

     //para un ruiteo url/code/dni
    // this.route.paramMap.subscribe(params => {
    //     this.code = params.get('code') ;

    // });

    this.confirmPwd = '';
    
    this.pwd = '';

  }


  validate() {

    let ifErrors = false;

    this.error_mustMatch = false;
    this.confirmPwd_error_required = false;
    this.pwd_error_required = false;
    this.pwd_error_not_valid = false;
    this.pwd_error_min = false;



    if (!this.pwd || this.pwd.length === 0)
      this.pwd_error_required = true;

    if (!this.confirmPwd || this.confirmPwd.length === 0)
      this.confirmPwd_error_required = true;

    if (this.pwd_error_required === false && this.confirmPwd_error_required === false) {
      if (this.confirmPwd !== this.pwd) {
        this.error_mustMatch = true;
        return false;
      }

    }

    if (this.pwd.length < 8) {
      this.pwd_error_min = true;
      return false;
    }

    let regExpString = /^(?=.{8,}$)(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*\W).*$/;

    let regexp = new RegExp(regExpString);


    this.pwd_error_not_valid = !regexp.test(this.pwd); //si no es valido (test) retorna false x lo tanto pwd_error_not_valid == true


    // si todas son true
    ifErrors = this.error_mustMatch || this.pwd_error_required || this.confirmPwd_error_required || this.pwd_error_not_valid || this.pwd_error_min;

    return !ifErrors;
  }
  
  goback(){
    this.router.navigate(['/login']);
  }

  /**
   * Cambio de contraseña.--> submit del form
   */
  onSubmit() {

    if(!this.selectedwindowsUser){
      alert("Debe seleccionar un usuario");
    
    }
    this.apiResponseMessage = '';
    var req: userChangePasswordReq = new userChangePasswordReq();

    req.domainName = this.selectedwindowsUser.Dominio;

    req.dom_id = this.selectedwindowsUser.dom_id;
    req.emp_Id = this.empleado.Emp_id;
    req.userName = this.selectedwindowsUser.WindowsUser;
    
    req.newPassword = this.pwd;


    if (!this.validate()) {
      return;
    }

    
    var resetPassword$ = this.secService.userChangePasswordSelf$(req);

    this.showSpinner = true;
    resetPassword$.subscribe(
      res => {
        this.apiResponseMessage = res;
        this.globalError = null;
        this.showSpinner = false;
        alert(res);
      },
      err => {
        this.globalError = err;
        this.showSpinner = false;

        
      }
    );
  }

  checkCode() {


    this.apiResponseMessage = '';
    this.globalError=null;

    var req: userForgotPasswordVerifyReq = new userForgotPasswordVerifyReq();

    
    req.code = this.code;

    var userForgotPasswordVerify$ = this.secService.userForgotPasswordVerify$(req);

    this.showSpinner = true;
    userForgotPasswordVerify$.subscribe(
      res => {
 
        this.showSpinner = false;
        this.empleado = res;
      },
      err => {
        
        this.globalError = err;
        this.showSpinner = false;

        //this.btnCloseResset.nativeElement.click();
      }
    );
  }

 
  grid_singleClick(item:any,i){
    this.selectedwindowsUser= item;
    $('#rnd_'+i).prop('checked', true);
   
  }
 
  radioClick (){

//     console.log("radio buton selecccionado " +  this.selectedwindowsUser.Dominio);
  }
}



