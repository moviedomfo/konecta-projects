import { Component,  ViewChild, ElementRef,  Renderer2, AfterViewInit } from '@angular/core';
import { Empleado, retriveEmpleadosReseteosReq, userResetPasswordReq, WindosUser,  userUnlockReq } from 'src/app/model/bussinessModel';
import { CurrentLogin, ServiceError } from 'src/app/model/common.model';
import { Router } from '@angular/router';
import { SerurityService } from 'src/app/service/serurity.service';

@Component({
  selector: 'app-user-resset',
  templateUrl: './user-resset.component.html',
  styleUrls: ['./user-resset.component.css']
})
export class UserRessetComponent implements AfterViewInit {
  public globalError: ServiceError;
  public currentEmpleado :Empleado;
  public selectedWindosUser:WindosUser;
  public winsowsUserList :WindosUser[];
  public currentLogin: CurrentLogin;
  public req :retriveEmpleadosReseteosReq;
  public nroTicket:string;
  
  @ViewChild('inputNroTicket') inputNroTicket: ElementRef;
  @ViewChild('btnReset') btnReset: ElementRef;
  @ViewChild('btnCloseResset') btnCloseResset : ElementRef 
  @ViewChild('btnCloseUnlock') btnCloseUnlock : ElementRef 
  
  public afterRessetOrUnlock:boolean = false;
  public showSpinner:boolean=false;
  public apiResponseMessage:string='';
  //Renderer2 proporciona una API para acceder de forma segura a elementos nativos, 
  //incluso cuando no están soportados por la plataforma (web workers, server-side rendering, etc).
  constructor(private renderer: Renderer2,private secService :SerurityService,private route: Router) { }
  ngAfterViewInit() {
    //const obj: WindosUser = {    };

    this.set_front();
  }
  ngOnInit() {

    if(!this.secService.isAuth()){
      this.route.navigate(['/login']);

    }
    this.currentLogin=  this.secService.getCurrenLoging();
    
    //TODO: A los input de resul se les puede agregar la clase is-valid,is-invalid o ninguna para reflejar estados de busqueda
    if(this.currentLogin){
    if(this.currentLogin.isRessetUser === false){
     
      this.route.navigate(['/changePwd']);
    }}
    this.req = new retriveEmpleadosReseteosReq();
    this.currentEmpleado = new Empleado();
    this.selectedWindosUser = new WindosUser();
   
    // let  currentLogin=  this.secService.getCurrenLoging();
    // alert(JSON.stringify(currentLogin.currentUser));
   
    
  }
  closeModal(){
    this.showSpinner = false;
    this.afterRessetOrUnlock = false;

    this.apiResponseMessage = '';
    
  }
 set_front(){
  
  this.currentLogin=  this.secService.getCurrenLoging();
 
  if(this.currentLogin.currentUser.CAIS === true){
    //	Nº Ticket, tipo texto, editable (solo cuando el usuario pertenezca a la CAIS,
    this.renderer.removeAttribute(this.inputNroTicket.nativeElement,'disabled');
  
  }
  else
  {
    
    //	Nº Ticket, tipo texto, disabled cuando el usuario NO pertenezca a la CAIS
    this.renderer.setAttribute(this.inputNroTicket.nativeElement,'disabled','true');
    
  }
  
 }
  //busqueda por DNI
  retriveWindowsUser(){
 
    this.globalError = null;
    
    
    
    //if(!this.req.dni || Object.keys(this.req.dni.length === 0)){
    if(!this.req.dni)
    {
      this.globalError = new  ServiceError();
      this.globalError.Message="El DNI es requerido .-";
      return;
    }
    this.req.domain=this.currentLogin.currentUser.Domain;
    this.req.userName =this.currentLogin.currentUser.UserName;
    this.req.userCAIS =this.currentLogin.currentUser.CAIS;
    
    
    //2.	Si el usuario pertenece al CAIS será obligatoria la carga del Nº de Ticket.
    this.winsowsUserList = [];
    
    var retriveEmpleadosReseteos$= this.secService.retriveEmpleadosReseteos(this.req);
    retriveEmpleadosReseteos$.subscribe(
      res => {
        this.winsowsUserList = res.WindosUserList;
        this.currentEmpleado= res;
      },
      err => {     
        this.cleanCurrentUserControls();
        this.globalError = err;  }
    );
  }
  selectedWindowsUser(item :WindosUser){

    this.globalError=null;
    this.selectedWindosUser=item;
  }

  
  reset(){
    this.afterRessetOrUnlock=true;
    this.apiResponseMessage = '';
    
    
    if(this.currentLogin.currentUser.CAIS === true)
    {
      if(!this.nroTicket)
      {
        this.afterRessetOrUnlock=false;
        
        alert("Nro de ticket es obligatorio");
        return;
      }
    }

   
    var req:userResetPasswordReq = new userResetPasswordReq();
    req.ticket = this.nroTicket;
    req.DomainName = this.selectedWindosUser.Dominio;
    req.dom_id = this.selectedWindosUser.dom_id;
    req.WindowsUser = this.selectedWindosUser.WindowsUser;
    req.Emp_Id = this.currentEmpleado.Emp_id;
    req.ResetUserName = this.currentLogin.currentUser.UserName;
    req.ResetUserId = this.currentLogin.currentUser.Emp_Id;
    req.ResetUserCAIS = this.currentLogin.currentUser.CAIS;
    
    
    var resetPassword$= this.secService.resetPassword$(req);
    
    this.showSpinner=true;
    resetPassword$.subscribe(
      res => {
        this.apiResponseMessage= res;
        this.globalError=null;
        this.showSpinner= false;
        this.afterRessetOrUnlock=true;
      },
      err => {
        this.globalError = err;
        this.showSpinner= false;
        this.afterRessetOrUnlock=true;
        this.btnCloseResset.nativeElement.click();
      }
    );
  }
  unlock(){
    this.afterRessetOrUnlock=true;
    this.apiResponseMessage = '';
    
    if(this.currentLogin.currentUser.CAIS === true)
    {
      if(!this.nroTicket )
      {
        this.afterRessetOrUnlock=false;
        alert("Nro de ticket es obligatorio");
        
        return;
      }
    }

    
    var req:userUnlockReq = new userUnlockReq();
    req.ticket = this.nroTicket;
    req.DomainName = this.selectedWindosUser.Dominio;
    req.dom_id = this.selectedWindosUser.dom_id;
    req.WindowsUser = this.selectedWindosUser.WindowsUser;
    req.Emp_Id = this.currentEmpleado.Emp_id;
    req.ResetUserName = this.currentLogin.currentUser.UserName;
    req.ResetUserId = this.currentLogin.currentUser.Emp_Id;
    req.ResetUserCAIS = this.currentLogin.currentUser.CAIS;
    var resetPassword$= this.secService.unlock$(req);
    this.showSpinner=true;
    resetPassword$.subscribe(
      res => {
        
        this.apiResponseMessage= res;
        this.globalError=null;
        this.showSpinner= false;
        this.afterRessetOrUnlock=true;
      },
      err => {
        this.globalError = err;
        this.showSpinner= false;
        this.afterRessetOrUnlock=true;
        this.btnCloseUnlock.nativeElement.click();
      }
    );
  }

  grid_singleClick(item:any){}
  grid_doubleClick(item:any){}

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }

  cleanCurrentUserControls(){
    this.req = null;
    this.currentEmpleado = null;
    this.selectedWindosUser = null;
    this.req = new retriveEmpleadosReseteosReq();
    this.currentEmpleado = new Empleado();
    this.selectedWindosUser = new WindosUser();
  }
}


