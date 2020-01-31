import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ServiceError, MotivoConsultaEnum, SecurityUser, SecurityRole, AppConstants, PersonBE, EventType, CommonParams } from 'src/app/model';
import { AuthenticationService, CommonService } from 'src/app/service';
import { ActivatedRoute } from '@angular/router';
// import custom validator to validate that password and confirm password fields match

import { FormBuilder, FormGroup, Validators, FormControl, NgForm } from '@angular/forms';

import { SessionSettingComponent } from '../sessionSetting/sessionSetting.component';
import { PersonCardComponent } from '../../persons/person-card/person-card.component';
import { AlertBlockComponent } from 'src/app/common-components/alert-block/alert-block.component';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html'

})
export class UserComponent implements OnInit {

  submitted = false;


  public motivoConsulta: MotivoConsultaEnum;
  public globalError: ServiceError;
  public currentUser: SecurityUser;
  private currentRole: SecurityRole;
  public btnText: string;
  public mainComponentTitle: string;
  public btnAceptEnabled: boolean;
  public actulaPersonId: string;

  //@ViewChild('sessionSetting', { static: false }) sessionSetting: SessionSettingComponent;
  @ViewChild('personCardForm', { static: false }) personCard: PersonCardComponent;
  @ViewChild('alertOnSumbmit', { static: false }) alertOnSumbmit: AlertBlockComponent;

  public sessionSettingsForm_valid: boolean;
  public personCardForm_valid: boolean;

  constructor(private securityService: AuthenticationService, private commonService: CommonService, private route: ActivatedRoute) {

    this.currentUser = new SecurityUser();
    this.currentUser.person = new PersonBE(AppConstants.emptyGuid, "");

  }

  ngOnInit() {

    this.preInitialize();
  }


  private preInitialize() {


    //this.resourceSchedulingManageComponent.currentResourceScheduling = this.currentResourceScheduling;
    //alert('ngOnInit preInitialize providerManageComponent');
    //let element_btnAcept = document.getElementById('btnAcept');

    let getPArams: any;

    this.route.params.subscribe(params => {
      getPArams = params;
    });

    if (getPArams.id != null) {
      this.motivoConsulta = MotivoConsultaEnum.UpdateAccount;
      //this.commonService.Set_mainComponentTitle("Edición de proveedor");
      this.mainComponentTitle = "Modificación de usuario";
      this.getUser(getPArams.id);
      //element_btnAcept.textContent = "Actualizar";
      this.btnText = "Actualizar";
      this.actulaPersonId = this.currentUser.personId;
      return;
    }

    if (this.route.snapshot.routeConfig.path.includes('new')) {
      this.motivoConsulta = MotivoConsultaEnum.CreateAccount;

      this.currentUser.person = new PersonBE();
      this.currentUser.personId = AppConstants.emptyGuid;

      //this.commonService.Set_mainComponentTitle("Alta de proveedor");
      this.mainComponentTitle = "Alta de usuario";
      //element_btnAcept.textContent = "Crear nuevo";
      this.btnText = "Crear nuevo";
    }
    this.btnAceptEnabled = false;
  }

  getUser(id: string) {
    this.securityService.getUser$('', id).subscribe(
      res => {
        this.currentUser = res;
       if(!this.currentUser.person){
        this.currentUser.person = new PersonBE(AppConstants.emptyGuid, "");
        this.currentUser.person.birthDate = new Date();
        this.currentUser.person.identityCardNumberType =  CommonParams.SeleccioneUnaOpcion.paramId;
        this.currentUser.person.identityCardNumber =  "00000000";
       }
        if (this.currentUser == null) {
          this.globalError = new ServiceError();
          this.globalError.message = "El usuario no existe en nuestra base de datos ";

        }
      },
      err => {

        this.globalError = err;
      }
    );

  }


  OnPersonChange($event) {
    this.currentUser.person = $event;

  }
  OnComponentError_personCard(err: ServiceError) {
    this.globalError = err;
  }
  OnComponentError_SessionSetting(err: ServiceError) {
    this.globalError = err;
  }



  onSubmit(f: NgForm) {
    this.alertOnSumbmit.Hide();
    let isValid = false;

    // alert("sessionSettingForm = " +JSON.stringify(f.value.sessionSettingsForm  ));
    //alert("personCardForm = " +JSON.stringify(f.value.personCardForm));
    this.sessionSettingsForm_valid = f.controls.sessionSettingsForm.valid;
    this.personCardForm_valid = f.controls.personCardForm.valid;
    //let c1 :FormGroup= f.form.controls['sessionSettingsForm'] as ;
    isValid = this.sessionSettingsForm_valid && this.personCardForm_valid;

    this.currentUser.person.email = this.currentUser.email;
    if (isValid) {
      if (this.motivoConsulta == MotivoConsultaEnum.UpdateAccount) {
        this.update();
      }
      else {
        this.create();
      }

      return;
    }

    //show the error in wour own position
    if (f.controls.sessionSettingsForm.valid == false) {

      $('.nav-tabs a[href="#session"]').tab('show');
      isValid = false;
    } else {
      if (f.controls.personCardForm.valid == false) {
        $('.nav-tabs a[href="#person"]').tab('show');
        isValid = false;
      }
    }


    if (!isValid) {
      this.alertOnSumbmit.Show("", "Verifique si ah ingresado correctamente todos los datos correspondientes al usuario", "", true, EventType.Warning);
      return;
    }





  }

  update() {
    this.securityService.UpdateUserService$(this.currentUser).subscribe(
      res => {
        
        if (res) {
          this.currentUser.userId = res.userId;
          this.currentUser.personId = res.personId;
          this.currentUser.person.personId = res.personId;
          alert('El usuario fue actualizado correctamente !!');
        }

      },
      err => {
        this.globalError = err;

      }
    );
  }
  create() {
    this.securityService.createUserService$(this.currentUser).subscribe(
      res => {
        if (res) {


          this.currentUser.userId = res.userId;
          this.currentUser.personId = res.personId;
          this.currentUser.person.personId = res.personId;

          alert('El usuatio fue dado de alta correctamente !!');
        }

      },
      err => {
        this.globalError = err;

      }
    );
  }


}



