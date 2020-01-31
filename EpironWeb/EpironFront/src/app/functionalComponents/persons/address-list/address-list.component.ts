import { Component, OnInit, Output, EventEmitter, Input, ViewChild, ElementRef } from '@angular/core';
import { PersonBE, AddressBE, ServiceError, EntityStatus } from '../../../model';
import { GridOptions } from 'ag-grid-community';
import { AddreesComponent } from '../addrees/addrees.component';
import { AgGridAngular } from 'ag-grid-angular';

import { ButtonRendererComponent } from 'src/app/common-components/controls/ag-grid/agGridButton.component';
declare var jQuery: any;
declare var $: any;

@Component({
  selector: 'app-address-list',
  templateUrl: './address-list.component.html'
})





export class AddressListComponent implements OnInit {
  public globalError: ServiceError;
  @Input()  public currentPerson: PersonBE;
  @Output() public onGridAddressDoubleClick = new EventEmitter<AddressBE>();
  @Output() public onGridAddressClick = new EventEmitter<AddressBE>();
  @Output() public onAddressCreated = new EventEmitter<AddressBE>();
  @ViewChild("addresComponent1", { static: false }) addresComponent: AddreesComponent;
  public addressComponentFormTittle: string = 'Crear dirección física';


  @ViewChild('closeBtn', { static: false }) closeBtn: ElementRef;
  @ViewChild('btnShowAddressFormModal', { static: false }) btnShowAddressFormModal: ElementRef;
  //@ViewChild('addressFormModal', { static: false }) addressFormModal: ElementRef;
  public currentAddress: AddressBE;
  public columnDefs: any[];
  public gridOptions: GridOptions;
  @ViewChild('agGridAddress', { static: false }) agGrid: AgGridAngular;
  @Input()  public  frameworkComponents;

  
  constructor() {
   
    this.createColumnDefs();
  }


  ngOnInit() {

    this.gridOptions = <GridOptions>{};
    //this.gridOptions.dateComponentFramework = DateComponent;

    // this.gridOptions = {
    //   components:{
        
        
    // }
    // }
    // this.gridOptions.defaultColDef = {
    //     headerComponentFramework : <{new():HeaderComponent}>HeaderComponent,
    //     headerComponentParams : {
    //         menuIcon: 'fa-bars'
    //     }
    // }
    //this.gridOptions.getContextMenuItems = this.getContextMenuItems.bind(this);
    this.gridOptions.floatingFilter = true;
    
  }

  
  private createColumnDefs() {
    this.columnDefs = [
      { headerName: "Id", field: "addressId", width: 50, pinned: true, filter: 'text' },
      { headerName: "Calle", field: "street", width: 250, pinned: true, filter: 'text' },
      { headerName: "Nro", field: "streetNumber", width: 100, pinned: true, filter: 'text' },
      { headerName: "Piso", field: "floor", width: 50, pinned: true, filter: 'text' },
      { headerName: "Localidad", field: "city", width: 150, pinned: true, filter: 'text' },
      { headerName: "Provincia", field: "province", width: 150, pinned: true, filter: 'text' },
         {
        headerName: "Action", 
        field: "", width: 150,
        cellRenderer: 'buttonRenderer',
        cellRendererParams: {
           onClick: this.removeAddress.bind(this),        
           label: 'Quitar',
           btnIcon: 'quit',
           //getLabelFunction: this.getLabel.bind(this),
           btnClass: 'btn btn-outline-danger btn-sm'
         }
      },

    ];
    this.frameworkComponents = {
      buttonRenderer: ButtonRendererComponent,
    }
  }
  getLabel(rowData)
  {    
    //console.log(rowData);
     if(rowData && rowData.hasIndicator)
         return 'Republish';
       else return 'Quitar';
  }
  removeAddress($event) {
    
    this.currentAddress= $event.rowData;
    console.log('eliminar ' +  this.currentAddress.addressId);

    var index = this.currentPerson.addressBEList.findIndex(obj => obj.addressId === this.currentAddress.addressId);
    this.currentPerson.addressBEList.splice(index,1);
    this.agGrid.api.setRowData(this.currentPerson.addressBEList);
  }

  onGridReady(params) {
    params.api.sizeColumnsToFit();
    params.api.columbApi();
  }
  onCellClicked(event) {
    // let item = event.node.data as AddressBE;
    // alert(JSON.stringify(item.city));

    this.onGridAddressClick.emit(event.node.data as AddressBE);

  }

  onGridCellDoubleClick(event) {


    //this.onGridAddressClick.emit(event.node.data as AddressBE);
  }

  //Crea Nueva
  btnShowAddressFormModal_click() {
    this.addressComponentFormTittle = "Nueva dirección física";
    this.addresComponent.resetAddress();

  }

  //Modifica
  onGridRowDoubleClick(event) {


    let item = event.node.data as AddressBE;
    item.state = EntityStatus.Added;
    //alert(JSON.stringify(item.city));
    this.onGridAddressDoubleClick.emit(event.node.data as AddressBE);
    this.addresComponent.setAddress(item);
    //this.btnShowAddressFormModal.nativeElement.click();
    //this.addressFormModal.nativeElement.modal('show');


    this.addressComponentFormTittle = "Modificar dirección física";
    $('#addressFormModal').modal('show');
    // http://localhost:4200/patientEdit?id=4350
    //this.router.navigate(['patientEdit'], { queryParams: { id: patienId }}); 

    //this.router.navigate(['/personEdit', Idpersona]); 

  }

  address_dialog_Acept() {

    this.appendNewItem();

  }

  public appendNewItem() {

    // var item: PerUsonBE = new PersonBE();
    // item.Nombre = nombre;
    // item.IdPersona = id;

    var a = this.addresComponent.getAddress();

    if (a.state == EntityStatus.New) {
      this.currentPerson.addressBEList.push(a);
      a.state == EntityStatus.Added;
    }


    this.agGrid.api.setRowData(this.currentPerson.addressBEList);
    this.closeBtn.nativeElement.click();
  }
}
