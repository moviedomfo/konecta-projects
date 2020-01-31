import { Component, OnInit, ViewEncapsulation, Input, Output, EventEmitter, AfterViewInit, ViewChild, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, NgForm, FormBuilder, FormGroup, Validators, FormControl, ControlValueAccessor } from '@angular/forms';
import { SecurityUser, ServiceError, SecurityRole } from "../../../../model/index";
import { AuthenticationService } from '../../../../service/index';

import { Observable } from "rxjs";
import { MustMatch } from 'src/app/common-components/controls/TemplateDrivenValidator/mustMatch.validator';


class UserSession {
  userName: string;
  email: string;
  password: string;
  confirmPassword: string;
}

@Component({
  selector: 'app-session-setting',
  templateUrl: './sessionSetting.component-reactive.html',
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => SessionSettinRreactiveComponent),
      multi: true
    }
  ]
  // encapsulation: ViewEncapsulation.None,
  // viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})


export class SessionSettinRreactiveComponent implements AfterViewInit, ControlValueAccessor {

  public confirmPassword: boolean;
  public globalError: ServiceError;
  @Input()
  public currentUser: SecurityUser;
  @Output()
  public OnComponentError = new EventEmitter<ServiceError>();
  public allRoles: SecurityRole[] = [];
  public sessionSettingsForm: FormGroup;
  submitted = false;
  constructor(private formBuilder: FormBuilder, private securityService: AuthenticationService) { }

  ngOnChanges() {
    this.MachRolesGrid();
  }

  ngAfterViewInit() {

  }

  // public sessionSettingsForm: FormGroup = new FormGroup({
  //   username: new FormControl("",[Validators.required, Validators.minLength(4)]),

  //   password:new FormControl(''),
  //   confirmPassword: new FormControl('', [Validators.required]),
  //   email: new FormControl('', [Validators.required,Validators.email ])

  // });


  ngOnInit() {

    this.sessionSettingsForm = this.formBuilder.group({

      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', Validators.required],
      acceptTerms: [false, Validators.requiredTrue]
    }, {
        validator: MustMatch('password', 'confirmPassword')
      });

    if (!this.currentUser)//if user is not {} or nullr is {} or null
    {
      this.currentUser = new SecurityUser();
      this.currentUser.roles = [];
    }

    this.sessionSettingsForm.controls['username'].setValue(this.currentUser.userName);
    this.sessionSettingsForm.controls['email'].setValue(this.currentUser.email);


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

  get f() { return this.sessionSettingsForm.controls; }
  public onTouched: () => void = () => { };

  writeValue(val: any): void {
    val && this.sessionSettingsForm.setValue(val, { emitEvent: false });
  }
  registerOnChange(fn: any): void {
    console.log("on change");
    this.sessionSettingsForm.valueChanges.subscribe(fn);
  }
  registerOnTouched(fn: any): void {
    console.log("on blur");
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    isDisabled ? this.sessionSettingsForm.disable() : this.sessionSettingsForm.enable();
  }

  checkRol(rolName) {

    var any = this.allRoles.find(r => r.Name == rolName);
    if (any) {

      any.isChecked = true;
    }

  }

  MachRolesGrid() {

    if (!this.currentUser || !this.currentUser.roles) {
      console.log('MachRolesGrid return');
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
    var username = this.sessionSettingsForm.controls['username'].value;

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
    let email = this.sessionSettingsForm.controls['email'].value;
    if (!email || email.trim() == '') {
      //this.errorText ="Falta email";

      //this.sessionSettingsForm.controls['email'].setErrors({'incorrect': true});

    }
    return this.sessionSettingsForm.valid;
  }

  public setUserModel() {

    if (this.sessionSettingsForm.get('password') != null)
      this.currentUser.password = this.sessionSettingsForm.get('password').value;
    if (this.sessionSettingsForm.get('username') != null)
      this.currentUser.userName = this.sessionSettingsForm.controls['username'].value;
    if (this.sessionSettingsForm.get('email') != null)
      this.currentUser.email = this.sessionSettingsForm.controls['email'].value;
  }

  clearError() {

    this.sessionSettingsForm.setErrors(null);
    this.sessionSettingsForm.controls['email'].setErrors(null);
    this.sessionSettingsForm.controls['pasword'].setErrors(null);
    this.sessionSettingsForm.controls['username'].setErrors(null);
  }
}

