
import { Component, OnInit, ViewEncapsulation, Input, Output, EventEmitter } from '@angular/core';
import { PersonBE, IContextInformation, ParamBE, CommonValuesEnum, ParamTypeEnum, CommonParams, AppConstants, contextInfo, UbicacionItemBE } from '../../../model/index';
import { PlaceBE, AddressBE } from "../../../model/persons.model";
import { CommonService } from '../../../service';
import { ControlContainer, NgForm } from '@angular/forms';
@Component({
  selector: 'app-contact',
  templateUrl: './contact.component.html',

  viewProviders: [{ provide: ControlContainer, useExisting: NgForm }]
})
export class ContactComponent implements OnInit {

  @Input()  public currentPerson: PersonBE;
  public currentAddress: AddressBE;
  
  constructor(private commonService: CommonService) { }


  ngAfterViewInit(): void {

  }

  ngOnInit() {
    

    if (!this.currentPerson.addressBEList) {
      this.currentPerson.addressBEList = [];
      this.currentAddress = new AddressBE();
      //this.currentPerson.addressBEList.push(this.currentAddress);

    }
    else {
      if (!this.currentPerson.addressBEList[0]) {
          this.currentAddress = this.currentPerson.addressBEList[0]
          
        }
    }

   

  }

  onPlaceChanged(placeBE: PlaceBE) {

    if (!this.currentPerson.places) {
      this.currentPerson.places = [];
    }

    let exist = this.currentPerson.places.find(p => p.place_id == placeBE.place_id);
    if (exist) {
      alert(placeBE.formatted_address + ' ya existe');
      return;
    }
    //this.currentPlace= placeBE;

  }


  onGridAddressDoubleClick(item: AddressBE) {

    if (!this.currentPerson.addressBEList) {
      this.currentPerson.addressBEList = [];
    }
    this.currentAddress = item;
  }

  onGridAddressClick(item: AddressBE) {
    
    // if(!this.currentPerson.addressBEList)
    // {
    //   this.currentPerson.addressBEList = [];
    // }
    this.currentAddress = item;

    // let exist = this.currentPerson.addressBEList.find(p=>p.cityId==address.cityId);
    // if(exist)
    // {
    //   alert(address.city + ' ya existe' );
    //   return;
    // }
    //this.currentPlace= placeBE;

  }

  onAddressCreated(item: AddressBE){
    // let exist = this.currentPerson.addressBEList.find(p=>p.cityId==item.cityId);
    //  if(exist)
    //  {
    //    alert(item.city + ' ya existe' );
    //    return;
    //  }
    // this.currentPerson.addressBEList.push(item);

    alert('Se agrego direccion')

  }
  btnAddPlace_click() {

    //this.currentPerson.places.push(this.currentPlace);
  }
}
