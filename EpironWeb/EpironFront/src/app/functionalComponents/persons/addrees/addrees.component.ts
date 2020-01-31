import { Component, OnInit, Input, ViewChild, ElementRef } from '@angular/core';
import { AddressBE, UbicacionItemBE, AppConstants, EntityStatus } from '../../../model';
import { CommonService } from '../../../service';

@Component({
  selector: 'app-addrees',
  templateUrl: './addrees.component.html'

})
export class AddreesComponent implements OnInit {

  public provinceList: UbicacionItemBE[];
  public cityList: UbicacionItemBE[];
  public currentAddress: AddressBE;
  public selectedProvince :UbicacionItemBE ;
  public selectedCity :UbicacionItemBE ;
  @Input()
  public isEditMode: boolean;
//  @ViewChild('addressFormModal',{static:false})   addresForm: ElementRef;

  constructor(private commonService: CommonService) {
    this.currentAddress = new AddressBE();
   }

  ngOnInit() {

    var idProv = 14;
    var provincias$ = this.commonService.searchProvincias$();
    provincias$.subscribe(
      res => {
        this.provinceList = res;// this.commonService.appendExtraParamsCombo(res, CommonParams.SeleccioneUnaOpcion.paramId);
        this.selectedProvince = this.provinceList.find(p=>p.id==idProv);
        
        this.cmbProv_onSelect(this.selectedProvince );

      },
      err => {
        //this.OnComponentError.emit(err);
      }
    );
   
    
   
  }
  cmbProv_onSelect(event) {
    
    this.selectedProvince = this.provinceList.find(p=>p.id==event.id);
    this.fillCombos(this.selectedProvince.id,null,0);

    
  }

  public resetAddress(){

    this.currentAddress = new AddressBE();
    this.currentAddress.cityId=0;
    this.currentAddress.provinceId=14;
    this.currentAddress.state = EntityStatus.New; 
    
    this.fillCombos(14,null,0);
    
  } 
  public setAddress(item: AddressBE) {

    //if(item.cityId && item.provinceId && item )
    // if(item.state == EntityStatus.New )
    // {
    //   this.isEditMode=true;
    // }
    this.currentAddress = item;

    if (item.provinceId === null) {
      this.selectedProvince = this.provinceList.find(p => p.id == -2000);
    }
    else {
      this.selectedProvince = this.provinceList.find(p => p.id == item.provinceId);
    }

    if (item.cityId === null)
      this.fillCombos(this.selectedProvince.id,null,0);
    else
      this.fillCombos(this.selectedProvince.id, item.cityId,null);      

    if (this.selectedProvince && this.selectedProvince.id == this.currentAddress.provinceId) {
      this.selectedCity = this.cityList.find(p => p.id == item.cityId);
      return;
    }


  } 

  
  private fillCombos(provinceId:number,cityId:number,cityIndex:number){

    
    var localidades$ = this.commonService.searchLocalidades$(provinceId);
    localidades$.subscribe(
      res => {
        if (res.cantidad > 0){
          this.cityList = res.localidades;
          if(cityId){
            
            this.selectedCity = this.cityList.find(p=>p.id==cityId);
          }
          if(cityIndex){
            this.selectedCity= this.cityList[cityIndex];
          }  
          //alert(" se encontraron localidades para " + this.selectedProvince.nombre);
        }
      },
      err => {

        //this.OnComponentError.emit(err);
      }
    );
  }
  public getAddress() :AddressBE{

    
    this.currentAddress.city=this.selectedCity.nombre;
    this.currentAddress.cityId=this.selectedCity.id;
    this.currentAddress.province=this.selectedProvince.nombre;
    this.currentAddress.provinceId=this.selectedProvince.id;

    
    return   this.currentAddress;




  }  


}
