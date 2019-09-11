import { Component,  ViewChild, ElementRef,  Renderer2, AfterViewInit } from '@angular/core';
import { Empleado, retriveEmpleadosReseteosReq, userResetPasswordReq, WindosUser,  userUnlockReq } from 'src/app/model/bussinessModel';
import { SerurityService } from 'src/app/service/serurity.service';
import { RouterModule, Routes, Router } from '@angular/router';
import { ServiceError, CurrentLogin } from 'src/app/model/common.model';
import { NgxSmartModalService } from 'ngx-smart-modal';
@Component({
  selector: 'app-user-reset',
  templateUrl: './user-reset-component.html',
  styleUrls: ['./user-reset-component.css']
})
export class UserResetComponent implements AfterViewInit  {
  public globalError: ServiceError;
  public currentEmpleado :Empleado;
  public selectedWindosUser:WindosUser;
  public winsowsUserList :WindosUser[];
  public currentLogin: CurrentLogin;
  public req :retriveEmpleadosReseteosReq;
  public nroTicket:string;
  
  @ViewChild('inputNroTicket',{ static: false }) inputNroTicket: ElementRef;
  @ViewChild('btnReset',{ static: false }) btnReset: ElementRef;
  @ViewChild('btnCloseResset',{ static: false }) btnCloseResset : ElementRef 
  @ViewChild('btnCloseUnlock',{ static: false }) btnCloseUnlock : ElementRef 
  
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
    //TODO: A los input de resul se les puede agregar la clase is-valid,is-invalid o ninguna para reflejar estados de busqueda
    
    this.req = new retriveEmpleadosReseteosReq();
    this.currentEmpleado = new Empleado();
    this.selectedWindosUser = new WindosUser();
    if(!this.secService.isAuth()){
      this.route.navigate(['/login']);

    }
    // let  currentLogin=  this.secService.getCurrenLoging();
    // alert(JSON.stringify(currentLogin.currentUser));
   
    
  }
  closeModal(){
    this.showSpinner = false;
    this.afterRessetOrUnlock = false;

    this.apiResponseMessage = '';
    
  }
 set_front(){
  this.globalError = null;
  this.currentLogin=  this.secService.getCurrenLoging();
  if(this.currentLogin.currentUser.CAIS){
    //	Nº Ticket, tipo texto, editable (solo cuando el usuario pertenezca a la CAIS,
    this.renderer.removeAttribute(this.inputNroTicket.nativeElement,'disabled');

  }
  else
  {
    //	Nº Ticket, tipo texto, disabled cuando el usuario NO pertenezca a la CAIS
    this.renderer.setAttribute(this.inputNroTicket.nativeElement,'disabled','true');

  }
  
 }
  retriveWindowsUser(){

    this.globalError = null;
    if(!this.req.dni || Object.keys(this.req.dni.trim()).length === 0){
      
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
        this.globalError=null;
        this.winsowsUserList = res.WindosUserList;
        this.currentEmpleado= res;
      },
      err => {this.globalError = err;     }
    );
  }
  selectedWindowsUser(item :WindosUser){

    this.globalError=null;
    this.selectedWindosUser=item;
  }

  
  reset(){
    this.afterRessetOrUnlock=true;
    this.apiResponseMessage = '';
    if(this.currentLogin.currentUser.CAIS)
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
    req.ResetUserId = this.currentLogin.currentUser.Legajo;
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
    
    if(this.currentLogin.currentUser.CAIS)
    {
      if(!this.nroTicket)
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
    req.ResetUserId = this.currentLogin.currentUser.Legajo;
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
}


