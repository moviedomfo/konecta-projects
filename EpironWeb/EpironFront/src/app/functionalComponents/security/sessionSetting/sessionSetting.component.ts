import { Component, OnInit, ViewEncapsulation, Input, Output, EventEmitter, AfterViewInit, ViewChild, forwardRef } from '@angular/core';
import {  NgForm, FormBuilder, FormGroup, ControlContainer } from '@angular/forms';
import { SecurityUser, ServiceError, SecurityRole, AppConstants } from "../../../model/index";
import { AuthenticationService } from '../../../service/index';
import { helperFunctions } from 'src/app/service/helperFunctions';
@Component({
  selector: 'app-session-setting',
  templateUrl: './sessionSetting.component.html',
   encapsulation: ViewEncapsulation.None,
   viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
  
})

export class SessionSettingComponent implements AfterViewInit {

  public confirmPassword: boolean;
  public globalError: ServiceError;
  @Input()
  public currentUser: SecurityUser;
  @Output()
  public OnComponentError = new EventEmitter<ServiceError>();
  public allRoles: SecurityRole[] = [];
  public submitted = false;
  
  constructor(private formBuilder: FormBuilder, private securityService: AuthenticationService) {
    this.currentUser = new SecurityUser();
    this.currentUser.personId= AppConstants.emptyGuid;
    this.currentUser.roles = [];
   }

   // Se ejecuta antes ngAfterViewInit
  ngOnChanges() {
    //alert('ngOnChanges SessionSetting ');
    //this.MachRolesGrid();
  }
  // pruduce cuando llegan los datos del usuario
  ngAfterViewInit() {
    //alert('ngAfterViewInit SessionSetting ');
    this.MachRolesGrid();
  }

  ngOnInit() {

    
    if (!this.currentUser)//if user is not {} or nullr is {} or null
    {
      this.currentUser = new SecurityUser();
      this.currentUser.personId= AppConstants.emptyGuid;
      this.currentUser.roles = [];
    }

   

    this.allRoles = this.currentUser.GetRolList();
    // var allRoles$ :Observable<SecurityRole[]>= this.providerService.getAllRoles$(this.currentUser.userName);

    //  allRoles$.subscribe(
    //    res => {
    //      this.allRoles = res;

    //    },
    //    err => {

    //      this.OnComponentError.emit(err);
    //    }
    //  );

  }

  checkRol(rolName) {

    var any = this.allRoles.find(r => r.Name == rolName);
    if (any) {

      any.isChecked = true;
    }

  }

  MachRolesGrid() {

    if (!this.currentUser || !this.currentUser.roles) {
      //console.log('MachRolesGrid return');
      return;
    }

    this.allRoles.forEach((item) => {
      //busca dento de los role del uario 
      var any = this.currentUser.roles.find(r => r == item.Name);
      if (any) {
        item.isChecked = true;
        console.log(item.Name + ' isChecked =', item.isChecked);
      }
      else {
        item.isChecked = false;
      }

    });
  }

  btnCheckUserName_Click() {
    //this.authenticationService.
    //bool exist = Fwk.UI.Controller.SecurityController.ValidateUserExist(txtUsername.Text.Trim());
    //El nombre de usuario ya se encuentra registrado \r\n por favor elija otro
    //Nombre de usuario disponible
    var username = this.currentUser.userName;

    if (!username) {
      alert('Ingrese nombre de usuario');
      return;
    }
    this.securityService.validateUserExist$(username).subscribe(
      res => {
        if (res === true) {
          alert('El nombre de usuario esta en uso !!');
        }
        else {
          alert('El nombre de usuario esta disponible');
        }
        this.OnComponentError.emit(null);
      },
      err => {

        this.OnComponentError.emit(err);
      }
    );
  }




  public isValid(): boolean {
    
    if (helperFunctions.string_IsNullOrEmpty(this.currentUser.email)) {
      //this.errorText ="Falta email";
      return false;
      //this.sessionSettingsForm.controls['email'].setErrors({'incorrect': true});

    }
    return true;
  }

 
}

// export class UserSession {
//   userName: string;
//   email: string;
//   password: string;
//   confirmPassword: string;
// }