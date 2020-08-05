import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';

import { AuthenticationOAutResponse, CurrentLogin, User, ServiceError } from 'src/app/model/common.model';
import { SerurityService } from 'src/app/service/serurity.service';
import { Observable } from 'rxjs';

import { AppConstants } from 'src/app/model/common.constants';
import { Domain } from 'src/app/model/bussinessModel';
import { runInThisContext } from 'vm';
import { RouterModule, Routes, ActivatedRoute, Router } from '@angular/router';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-loging',
  templateUrl: './loging.component.html',
  styleUrls: ['./loging.component.css']

})
export class LogingComponent implements OnInit {
  public loading: boolean = false;
  public globalError: ServiceError;
  public currentLogin: CurrentLogin;
  public currentUser: User;
  public jwt_decode: any;
  public domains: Domain[];
  public selectedDomain: string;
  public loadingDomains : boolean = false;
  //@Output() OnComponentError = new EventEmitter<ServiceError>();
  //allRoles :Rol[]=[];

  //implementar angular-user-idle
  // https://www.npmjs.com/package/angular-user-idle
  constructor(
    private Serurity: SerurityService,
    private route: Router) {


    this.Serurity.storage_Domains$().subscribe((res: Domain[]) => {
      this.domains = res;
      console.log('cargado domains');
    },
      err => {

        this.globalError = err;
      }
    );
  }



  ngOnInit() {
   
    if (!this.currentUser)//if user is not {} or nullr is {} or null
    {
      this.currentUser = new User();
      this.currentUser.Roles = [];
    }
    if (!this.currentLogin)//if user is not {} or nullr is {} or null
    {
      this.currentLogin = new CurrentLogin();
      this.currentLogin.isRessetUser=false;
      this.currentLogin.oAuth = new AuthenticationOAutResponse();
    }
    
    this.retriveDomains();
    //this.Serurity.checkDomains();
    //this.loadDomains();
  }



  submitForm(form: NgForm) {
    
    if(!form.valid) {
      return false;
    } else {
   // alert(JSON.stringify(form.value))
    }
  }
  authenticate() {
    if(!this.currentUser.Domain)
    {
      alert("Debe seleccionar un dominio");
      return;
    }
    this.loading = true;
    //this.currentUser.Domain = this.domains.find(p=> p.DomainId ==this.currentUser.DomainId).Domain;
    var authRes$: Observable<CurrentLogin> = this.Serurity.oauthToken$(this.currentUser.UserName, this.currentUser.Password, this.currentUser.Domain);

    authRes$.subscribe(
      res => {
        //console.log(JSON.stringify(res));

        this.currentLogin = res;
        this.loading = false;
        
       
        if(this.currentLogin.isRessetUser)
        {
          this.route.navigate(['/userReset']);
        }
        else
        {
          this.route.navigate(['/changePwd']);
        }

       
        
      },
      err => {

        //this.OnComponentError.emit(err);
        this.loading = false;
        this.globalError = err;
      }
    );

  }


  retriveDomains(){
    var dom$ : Observable<Domain[]> =  this.Serurity.checkDomains$();
    this.loadingDomains=true;
    dom$.subscribe(
      res => {
        //console.log(JSON.stringify(res));
       this.domains=res;
       this.loadingDomains=false;
      },
      err => {
        
        //this.OnComponentError.emit(err);
        this.loadingDomains = false;
        this.globalError = err;
      }
    );
    
  }
  loadDomains() {
    this.Serurity.storage_Domains$().subscribe((res: Domain[]) => {
      this.domains = res;
    
    },
      err => {

        this.globalError = err;
      }
    );
  }
  radioClick(){
    //alert(this.currentUser.DomainId);
  }
}

class UserSession {
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
}


