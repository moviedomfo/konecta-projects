import { Component, OnInit, ViewEncapsulation, Input, Output, EventEmitter } from '@angular/core';
import { PersonsService,CommonService } from '../../../service/index';
import {   PersonBE,IContextInformation, AppConstants,ParamTypeEnum, MotivoConsultaEnum, ServiceError } from '../../../model/index';
import { Observable } from 'rxjs';
// rich grid and rich grid declarative
import {DateComponent} from "../../../common-components/controls/ag-grid/date.component";
import {HeaderComponent} from "../../../common-components/controls/ag-grid/header.component";
import {HeaderGroupComponent} from "../../../common-components/controls/ag-grid/header-group.component";

import { Router, CanActivate ,CanDeactivate} from '@angular/router';
import { GridOptions } from 'ag-grid-community';

@Component({
  selector: 'app-person-grid',
  templateUrl: './person-grid.component.html',
  
})
export class PersonGridComponent implements OnInit {
  globalError: ServiceError;
  public personList: PersonBE[];
  public currentperson: PersonBE;
  public txtQuery: string;
  public personCount: number;
  public columnDefs:any[];
  public gridOptions:GridOptions;
 
  @Input() motivoConsulta:MotivoConsultaEnum;
  @Input()tittle:number;
  @Output() onPersonGridDoubleClick = new EventEmitter<PersonBE>();

  constructor(    private commonService: CommonService,private personsService: PersonsService,private router: Router) {
        this.personList = [];
        this.motivoConsulta = MotivoConsultaEnum.QueryPerson_NoUpdate;
      }

 
    ngOnInit() {
     
      //alert( MotivoConsultaEnum[this.motivoConsulta]);
     
      // we pass an empty gridOptions in, so we can grab the api out
      this.gridOptions = <GridOptions>{};
      this.gridOptions.dateComponentFramework = DateComponent;
      // this.gridOptions.defaultColDef = {
      //     headerComponentFramework : <{new():HeaderComponent}>HeaderComponent,
      //     headerComponentParams : {
      //         menuIcon: 'fa-bars'
      //     }
      // }
      //this.gridOptions.getContextMenuItems = this.getContextMenuItems.bind(this);
      this.gridOptions.floatingFilter = true;
 
      this.createColumnDefs();

      //this.retrivePatients();
  }

  private getContextMenuItems(): any {
    let result: any = [
        { // custom item
            name: 'Alert ',
            action: function () {
                window.alert('Alerting about ');
            },
            cssClasses: ['redFont', 'bold']
        }];
    return result;
}

private createColumnDefs() {
  this.columnDefs = [
    { headerName: "Nombre", field: "name" ,width: 150,pinned: true,filter: 'text'},
    { headerName: "Apellido", field: "lastName" ,width: 150,pinned: true,filter: 'text'},
    { headerName: "Documento", field: "identityCardNumber" ,width: 150,pinned: true,filter: 'text'},
    
    // { headerName: "Especialidad", field: "NombreEspecialidad" ,width: 150,pinned: true,filter: 'text'},
    // { headerName: "NombreProfecion", field: "NombreProfecion" ,width: 150,pinned: true,filter: 'text'},
    { headerName: "Fecha alta", field: "entryDate",width: 200,pinned: true }
  ];
}
  onKey_Enter($event) {
   // console.log($event);
    this.retrivePatients();
  }
  
  retrivePatients() {
 
    this.personsService.retrivePersonsGrid$(this.txtQuery, this.motivoConsulta ).subscribe(
      res => {
        this.personList = res;
    
        if(this.personList)
        {
          this.personCount = this.personList.length;
        }
        else
        {
          this.personCount = 0;
        }
        this.globalError = null;
      },
      err => {
        
        this.globalError = err;
      }
    );

  }

  onGridReady(params) {
    params.api.sizeColumnsToFit();
  }
  onCellClicked(event){}
  onGridCellDoubleClick(event){
    //alert(event);
  }
  
  onGridRowDoubleClick(event){
    
    //console.log(event.node.data);
    //let Idpersona = event.node.data.IdPersona;
    this.onPersonGridDoubleClick.emit(event.node.data as PersonBE);
    // http://localhost:4200/patientEdit?id=4350
    //this.router.navigate(['patientEdit'], { queryParams: { id: patienId }}); 
    
    //this.router.navigate(['/personEdit', Idpersona]); 
    

  }
 

}
