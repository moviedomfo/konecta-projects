import { Component, OnInit, Input, ViewChild, ElementRef, Renderer2, AfterContentInit, AfterViewInit, Output, EventEmitter,forwardRef} from '@angular/core';

import { Observable } from 'rxjs';
import { PersonsService, CommonService } from '../../../service/index';
import { PersonBE, IContextInformation, ParamBE, CommonValuesEnum, EventType, ParamTypeEnum, CommonParams, AppConstants, MotivoConsultaEnum } from '../../../model/index';
import { } from '@angular/core';
// Base 64 IMage display issues with unsafe image
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';

import { AlertBlockComponent } from '../../../common-components/alert-block/alert-block.component';
import { ServiceError } from '../../../model/common.model';

import { NG_VALUE_ACCESSOR,  FormBuilder, FormGroup, Validators, FormControl, ControlValueAccessor, AbstractControl } from '@angular/forms';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-person-card-reactive',
  templateUrl: './person-card-reactive.component.html',
  providers:[
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => PersonCardREactiveComponent),
      multi: true
    }
  ]

})
export class PersonCardREactiveComponent implements AfterViewInit,ControlValueAccessor{
  
 
  
  

  @Input() 
  public currentPerson: PersonBE;
  @Input() 
  public motivoConsulta: MotivoConsultaEnum;
  public selectedTipoDoc: number;

  //estadoCivilList: ParamBE[];
  public tipoDocumentoList$: Observable<ParamBE[]>;
  public tipoDocumentoList: ParamBE[];
  public fullImagePath: SafeUrl = '';
  public currentNroDocumento: string='';
  @ViewChild('alertBlock1', { static: false }) alertBlock1: AlertBlockComponent;
  @ViewChild('cmbEstadoCivil', { static: false }) cmbEstadoCivil: ElementRef;
  @ViewChild('img2', { static: false }) img2: ElementRef;
  @ViewChild('img1', { static: false }) img1: ElementRef;
  @Output() OnComponentError = new EventEmitter<ServiceError>();

  notQuery_motivoConsulta: Array<MotivoConsultaEnum> = [
    MotivoConsultaEnum.CreateAccount, MotivoConsultaEnum.CreateProvider,
    MotivoConsultaEnum.UpdateAccount, MotivoConsultaEnum.UpdateProvider
  ];
  public enableControls: boolean = false;
  public errorText ;
  
  constructor(private formBuilder: FormBuilder,private personService: PersonsService, private commonService: CommonService, private domSanitizer: DomSanitizer,
    private rd: Renderer2) {

    this.currentNroDocumento = '';
  }

  //Se ejecuta antes q ngOnInit
  ngOnChanges() {

    if (this.currentPerson) {

      if (this.currentPerson.photo !== null) {

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
  }

  ngAfterViewInit() {


  }

  get f() { return this.personCardForm.controls; }
  public personCardForm: FormGroup = new FormGroup({
    name: new FormControl("",[Validators.required]),
    lastName: new FormControl('', [Validators.required]),
    identityCardNumberType:new FormControl(''),
    identityCardNumber: new FormControl('', [Validators.required]),
    email: new FormControl('', [Validators.required,Validators.email]),
    birthDate  :new FormControl( new Date(),  []),
    
  });
  
  ngOnInit() {

    // this.personCardForm = this.formBuilder.group({
      
    //   name: ['', Validators.required],
    //   lastName: ['', Validators.required],
    //   email: ['', [Validators.required, Validators.email]]
    // });



    var item = this.notQuery_motivoConsulta.find(p => p === this.motivoConsulta);
    if (item) {
      this.enableControls = true;
    } else { this.enableControls = false; }


    this.tipoDocumentoList$ = this.commonService.searchParametroByParams$(ParamTypeEnum.TipoDocumento, null);
    this.tipoDocumentoList$.subscribe(
      res => {
        this.tipoDocumentoList = this.commonService.appendExtraParamsCombo(res, CommonParams.SeleccioneUnaOpcion.paramId);
      },
      err => {

        this.OnComponentError.emit(err);
      }
    );

    // this.estadoCivilList$ = this.commonService.searchParametroByParams$(ParamTypeEnum.EstadoCivil, null);
    // this.estadoCivilList$.subscribe(
    //   res => {
    //     this.estadoCivilList = this.commonService.appendExtraParamsCombo(res, CommonParams.SeleccioneUnaOpcion.paramId);
    //   },
    //   err => {
    //     // console.info('result de llamada al servicio searchParametroByParams');
    //     // console.info(err.error);
    //     //alert('handleError' +  JSON.stringify(err));
    //     this.OnComponentError.emit(err);
    //   }
    // );

    this.preInitializePerson();


  }

  public onTouched: () => void = () => {};
  writeValue(val: any): void {
    val && this.personCardForm.setValue(val, { emitEvent: false });
  }
  registerOnChange(fn: any): void {
    console.log("on change");
    this.personCardForm.valueChanges.subscribe(fn);
  }
  registerOnTouched(fn: any): void {
    console.log("on blur");
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    isDisabled ? this.personCardForm.disable() : this.personCardForm.enable();
  }

  private preInitializePerson() {

    this.fullImagePath = AppConstants.ImagesSrc_Man;
    if (this.currentPerson == null) {

      this.currentPerson = new PersonBE(AppConstants.emptyGuid, "");

      //alert(JSON.stringify(this.currentPerson));
      this.currentPerson.name = "";

      this.currentPerson.identityCardNumberType = CommonParams.SeleccioneUnaOpcion.paramId;
      //this.currentPerson.IdEstadocivil = CommonParams.SeleccioneUnaOpcion.paramId;
      this.currentPerson.birthDate = new Date();
      this.currentPerson.identityCardNumber = "0";
    }

    this.currentPerson.identityCardNumber =  this.personCardForm.controls['identityCardNumber'].value;
    this.currentPerson.identityCardNumberType =  this.personCardForm.controls['identityCardNumberType'].value;
    this.currentPerson.email =  this.personCardForm.controls['email'].value;
    this.currentPerson.name =  this.personCardForm.controls['name'].value;
    this.currentPerson.lastName =  this.personCardForm.controls['lastName'].value;
    this.currentNroDocumento = this.currentPerson.identityCardNumber;
    this.currentPerson.birthDate = this.personCardForm.controls['birthDate'].value;


  }


  
  onPersonGridDoubleClick($event) {

    this.currentPerson = $event as PersonBE;

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

    this.currentPerson.identityCardNumber =  this.personCardForm.controls['identityCardNumber'].value;

    if (!this.currentPerson.identityCardNumber || this.currentPerson.identityCardNumber.trim() == '')
      return true;
    if (this.currentPerson.identityCardNumber.trim() == this.currentNroDocumento.trim())
      return true;
    this.currentPerson.identityCardNumber = this.currentPerson.identityCardNumber.trim();


    this.personService.getPersonaByParamService$("", this.currentPerson.identityCardNumber, "").subscribe(
      res => {
        var person: PersonBE = res as PersonBE;

        if (person == null) {
          return true;
        }
        //console.log(JSON.stringify(person));
        //console.log('MotivoConsultaEnum = ' + this.motivoConsulta); 

        if (this.motivoConsulta == MotivoConsultaEnum.UpdateProvider ||
          this.motivoConsulta == MotivoConsultaEnum.CreateProvider) {
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
    
    let name = this.personCardForm.controls['name'];
    var errors = this.collectErrors(name);

    if(!this.personCardForm.valid)
    {
      this.errorText = errors;
      return false;
    }
      

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

    this.currentPerson.name = this.personCardForm.controls['name'].value;
    this.currentPerson.lastName = this.personCardForm.controls['lastName'].value;
    this.currentPerson.identityCardNumber = this.personCardForm.controls['identityCardNumber'].value;
    this.currentPerson.identityCardNumberType = this.personCardForm.controls['identityCardNumberType'].value;

    //this.currentPerson.birthDate = this.personCardForm.controls['birthDate'].value;
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


   isFormGroup(control: AbstractControl): control is FormGroup {
    return !!(<FormGroup>control).controls;
  }
  
   collectErrors(control: AbstractControl): any | null {
    if (this.isFormGroup(control)) {
      return Object.entries(control.controls)
        .reduce(
          (acc, [key, childControl]) => {
            const childErrors = this.collectErrors(childControl);
            if (childErrors) {
              acc = {...acc, [key]: childErrors};
            }
            return acc;
          },
          null
        );
    } else {
      return control.errors;
    }
  }
}
