import { Component, AfterContentInit, Input } from '@angular/core';
import {  CommonService } from '../../../service/index';
import { PersonBE, AddressBE } from '../../../model/index';


@Component({
  selector: 'app-person-address',
  templateUrl: './person-address.component.html'
  
})
export class PersonAddressComponent implements AfterContentInit {
  @Input()
  public currentAddress: AddressBE;

  constructor(   private commonService: CommonService) { }

  ngAfterContentInit() { }

  ngOnInit() {
    this.preInitializePerson();

   

  }
  private preInitializePerson() {



    //  this.currentPerson.Street = "";
    //  this.currentPerson.StreetNumber = -1;
    //  this.currentPerson.Floor = "";

    //  this.currentPerson.Telefono1 = "";
    //  this.currentPerson.Telefono2 = "";

    //  this.currentPerson.ProvinceId = CommonParams.SeleccioneUnaOpcion.IdParametro;
    //  this.currentPerson.CountryId = CommonParams.SeleccioneUnaOpcion.IdParametro;
    //  this.currentPerson.CityId = CommonParams.SeleccioneUnaOpcion.IdParametro;
  }
}
