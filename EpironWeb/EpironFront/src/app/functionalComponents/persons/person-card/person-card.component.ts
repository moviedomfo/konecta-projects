import { Component,  Input, ViewChild, ElementRef, Renderer2, AfterContentInit, AfterViewInit, Output, EventEmitter, ViewEncapsulation, SecurityContext} from '@angular/core';

import { PersonsService, CommonService } from '../../../service/index';
import { PersonBE,  ParamBE,  ParamTypeEnum, CommonParams, AppConstants, MotivoConsultaEnum } from '../../../model/index';
import { } from '@angular/core';
// Base 64 IMage display issues with unsafe image
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

import { AlertBlockComponent } from '../../../common-components/alert-block/alert-block.component';
import { ServiceError } from '../../../model/common.model';

import {     ControlContainer, NgForm } from '@angular/forms';
import { helperFunctions } from 'src/app/service/helperFunctions';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';

declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-person-card',
  templateUrl: './person-card.component.html',
  //encapsulation: ViewEncapsulation.None,
  viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]


})
export class PersonCardComponent implements AfterViewInit{

  @Input() public currentPerson: PersonBE;
  @Input() public motivoConsulta: MotivoConsultaEnum;
  public selectedTipoDoc: number;

  public submitted:boolean= false;
  
  public tipoDocumentoList: ParamBE[];
  public fullImagePath: SafeUrl = '';
  public currentNroDocumento: string='';
  @ViewChild('alertBlock1', { static: false }) alertBlock1: AlertBlockComponent;
  @ViewChild('cmbEstadoCivil', { static: false }) cmbEstadoCivil: ElementRef;
  @ViewChild('img2', { static: false }) img2: ElementRef;
  
  @Output() OnComponentError = new EventEmitter<ServiceError>();
  @Output() OnPersonChange = new EventEmitter<PersonBE>();
 public enableControls:Boolean;
  CREATE_motivoConsulta: Array<MotivoConsultaEnum> = [
    MotivoConsultaEnum.CreateAccount, MotivoConsultaEnum.CreateProvider
    
  ];
  
  public errorText ;
  
  constructor(private personService: PersonsService, private commonService: CommonService,
     private domSanitizer: DomSanitizer,    private rd: Renderer2) {

      this.enableControls = false;
    this.currentNroDocumento = '';
   
  }

  //Se ejecuta antes q ngOnInit
  ngOnChanges() {

   
  }

  ngAfterViewInit() {

   
  }

  
  ngOnInit() {
    this.preInitializePerson();
    if (this.currentPerson) {

      if (this.currentPerson.photo) {

        //Convert the ArrayBuffer to a typed array 
        // const TYPED_ARRAY = new Uint8Array(prof.provider.Persona.Foto);
        // // converts the typed array to string of characters
        // const STRING_CHAR = String.fromCharCode.apply(null, TYPED_ARRAY);
        // let base64String = btoa(STRING_CHAR);
        // console.log("Base 64 de la foto:   " + prof.provider.Persona.Foto);


        this.fullImagePath = this.domSanitizer.bypassSecurityTrustUrl('data:image/jpg;base64, ' + this.currentPerson.photo);

      }
      else {
        this.loadDefaultPhoto(this.currentPerson.sex);
      }
    }


    var item = this.CREATE_motivoConsulta.find(p => p === this.motivoConsulta);
    if ( item) {
      this.enableControls = true;
    } else { this.enableControls = false; }


     this.commonService.searchParametroByParams$(ParamTypeEnum.TipoDocumento, null).subscribe(
      res => {
        this.tipoDocumentoList = this.commonService.appendExtraParamsCombo(res, CommonParams.SeleccioneUnaOpcion.paramId);
      },
      err => {

        this.OnComponentError.emit(err);
      }
    );

  }

  
  private preInitializePerson() {

    this.fullImagePath = AppConstants.ImagesSrc_Man;
    
    
    // if (!this.currentPerson && !this.currentPerson.personId) {
     
    //   this.currentPerson.personId = AppConstants.emptyGuid;
     
    //   this.currentPerson.identityCardNumberType = CommonParams.SeleccioneUnaOpcion.paramId;
      
    //   this.currentPerson.birthDate = new Date();
    //   this.currentPerson.identityCardNumber = "-3000";


    // }
   
    this.currentNroDocumento = this.currentPerson.identityCardNumber;
    


  }


  
  onPersonGridDoubleClick($event) {

    this.currentPerson = $event;
    this.OnPersonChange.emit(this.currentPerson);
    //alert(JSON.stringify(this.currentPerson ))
    this.currentNroDocumento = this.currentPerson.identityCardNumber;
    $('#findPersonModal').modal('hide');
  }

  txtBox_NroDocumento_onKeyEnter(value: string) {
    //this.txtQuery += value + ' | ';
    // console.log(value);
  }
  txtBox_NroDocumento_onBlur() {
 
    this.validate_txtDocumento();
  }
  onSexChanged(sexo: number) {

    if (this.currentPerson.photo) {
      // this.base64Image =''+this.currentPerson.Foto;
      //this.img1.src  = "'data:image/jpg;base64,' + fullImagePath";
      //this.fullImagePath = ''+this.currentPerson.Foto;
      return;
    }
    this.currentPerson.sex = sexo;
    this.loadDefaultPhoto(this.currentPerson.sex);
  }

  loadImg() {

    if (this.currentPerson.photo) {
      this.fullImagePath = this.domSanitizer.bypassSecurityTrustUrl('data:image/jpg;base64, ' + this.currentPerson.photo);
    }
    else {
      this.loadDefaultPhoto(this.currentPerson.sex);
    }

  }

  
  loadDefaultPhoto(sexo: number) {

    this.fullImagePath = AppConstants.ImagesSrc_Woman;
    if (sexo === 0) {
      this.fullImagePath = AppConstants.ImagesSrc_Man;
    }
  }

  photoURL(imgUrl) {
    return this.domSanitizer.bypassSecurityTrustUrl(imgUrl);
  }




  validate_txtDocumento(): boolean {

    console.log("validate_txtDocumento --> " + this.currentNroDocumento)

    if (helperFunctions.string_IsNullOrEmpty(this.currentPerson.identityCardNumber) == true)
      return true;

    // if (helperFunctions.string_IsNullOrEmpty(this.currentNroDocumento) == true)
    //   return true;
  if(this.currentNroDocumento){
    if ( this.currentPerson.identityCardNumber.trim() == this.currentNroDocumento.trim())
      return true;
    }
    this.currentPerson.identityCardNumber = this.currentPerson.identityCardNumber.trim();


    this.personService.getPersonaByParamService$("", this.currentPerson.identityCardNumber, "").subscribe(
      res => {
        var person: PersonBE = res as PersonBE;

        if (person == null) {
          return true;
        }
        //console.log(JSON.stringify(person));
        //console.log('MotivoConsultaEnum = ' + this.motivoConsulta); 

        if (this.motivoConsulta == MotivoConsultaEnum.UpdateProvider || this.motivoConsulta == MotivoConsultaEnum.CreateProvider ||
          this.motivoConsulta == MotivoConsultaEnum.CreateAccount || this.motivoConsulta == MotivoConsultaEnum.UpdateAccount) {
          alert("El Nro de doc ingresado pertenece a " + PersonBE.getFullName(person.lastName, person.name) + " Si la persona que quiere crear/modificar posee este documento \r\n cierre esta pantalla y valla a buscar paciente actualice sus datos");
          return false;
        }


      },
      err => {
        alert(err.Message);
        //this.globalError = err;
      }
    );
    return true;
  }
 
  public isValid(): boolean {

    if (!this.currentPerson.identityCardNumber || this.currentPerson.identityCardNumber.trim() == '')
    {
      this.errorText ="Falta Nro documento";
      return false;
    }

    if (!this.currentPerson.name || this.currentPerson.name.trim() == '')
    {
      this.errorText ="Falta nombre persona";
      return false;
    }

  
      this.errorText='';

    return true;
  }

  public setPersonModel(){


  }

  byParam(item1: number, item2: number) {
    //console.log(JSON.stringify(item1));
    return item1 === item2;

  }
  onPaisSelection(event) {
    //alert(this.selectedPais); 
  }

  onEstadoCivilSelection(event) {
    // console.log(event);
    // console.log(this.selectedEstadoCivil);

  }


 
}
