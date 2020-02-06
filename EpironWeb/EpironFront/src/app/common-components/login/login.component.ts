import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { SecurityUser, PersonBE, CurrentLogin, AuthenticationOAutResponse, CurrentLoginEpiron, UserAutenticacionRes } from '../../model/index'
import { AuthenticationService, PersonsService, CommonService } from './../../service/index';
import {  ServiceError,  } from '../../model/common.model';
import { Observable, forkJoin } from 'rxjs';
import { helperFunctions } from 'src/app/service/helperFunctions';
import { Lexer } from '@angular/compiler';
import { EpironSecurityService } from 'src/app/service/epiron.security.service';


@Component({
  templateUrl: 'login.component.html'
  //moduleId: module.id

})

export class LoginComponent implements OnInit {
  public loading: boolean = false;
  public globalError: ServiceError;
  public currentLogin: CurrentLoginEpiron;
  public currentUser: SecurityUser;
  public jwt_decode: any;
  public selectedDomain: string;
  returnUrl: string;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private authenticationService: AuthenticationService,
    private epironSecurityService: EpironSecurityService,
    private personService: PersonsService) {

    // this.Serurity.storage_Domains$().subscribe((res: Domain[]) => {
    //     this.domains = res;
    //     console.log('cargado domains');
    //   },
    //     err => {

    //       this.globalError = err;
    //     }
    //   );
  }

  ngOnInit() {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
    // reset login status
    if (!this.currentUser)//if user is not {} or nullr is {} or null
    {
      this.currentUser = new SecurityUser();
      this.currentUser.roles = [];
    }
    if (!this.currentLogin)//if user is not {} or nullr is {} or null
    {
      this.currentLogin = new CurrentLoginEpiron();
      this.currentLogin.userData = new UserAutenticacionRes();
    }

    //this.authenticationService.checkDomains();

    this.epironSecurityService.validateAppService$().subscribe(
      res=>{
          
          this.authenticationService.getAppInstance();
    },err => {
      
      this.loading = false;
      this.globalError = err;
    })
  }

  authenticate() {

    this.globalError = null;

    this.loading = true;
    //this.currentUser.Domain = this.domains.find(p=> p.DomainId ==this.currentUser.DomainId).Domain;
    this.authenticationService.userAutenticacion$(this.currentUser.userName, this.currentUser.password, '',this.returnUrl).subscribe(
      res => {

        
        this.currentLogin = this.authenticationService.getCurrenLoging();
        this.loading = false;
        //let returnUrl = this.router.routerState.root.queryParams['returnUrl'];

        // this.router.routerState.root.queryParams.subscribe(queryParams => {
        //   let returnUrl = queryParams["returnUrl"];
        //   //alert(returnUrl);
        //   if (helperFunctions.string_IsNullOrEmpty(returnUrl))
        //     this.router.navigate(['']);
        //   else
        //     this.router.navigate(['/' + returnUrl]);
        //     //this.router.navigate(['/'], { queryParams: { returnUrl: returnUrl } });
        // });
       
        //this.router.navigate([this.returnUrl]);
       
        //this.router.navigateByUrl(this.returnUrl);
        //forkJoin(this.retrivePersonData(this.currentLogin.currentUser.UserId));
        
      },
      err => {
        //this.OnComponentError.emit(err);
        this.loading = false;
        this.globalError = err;
      }
    );

  }

  navigate() {
    
        //this.router.navigate([this.returnUrl]);
        this.router.navigate(['/security']);
  }

  
}
