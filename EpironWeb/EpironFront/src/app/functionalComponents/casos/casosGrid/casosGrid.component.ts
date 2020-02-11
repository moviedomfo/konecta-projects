import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { CommonService, AuthenticationService } from 'src/app/service';
import { Router } from '@angular/router';
import { GridOptions } from 'ag-grid-community';
import { ServiceError, CaseByUserGuidBE } from 'src/app/model';
import { EpironService } from 'src/app/service/epiron.service';

@Component({
  selector: 'app-casosGrid',
  templateUrl: './casosGrid.component.html'
  
})
export class CasosGridComponent implements OnInit {
  globalError: ServiceError;
  public caseList: CaseByUserGuidBE[];
  public currentCase: CaseByUserGuidBE;
  public txtQuery: string;
  public count: number;
  public columnDefs:any[];
  public gridOptions:GridOptions;

  @Output() onCaseGridDoubleClick = new EventEmitter<CaseByUserGuidBE>();

  constructor(private epironService : EpironService, 
    private commonService: CommonService,
     private authService: AuthenticationService, private router: Router) { }

  ngOnInit() {
    this.gridOptions = <GridOptions>{};
    
    this.gridOptions.floatingFilter = true;
    this.gridOptions.rowStyle = {background: 'black'};
    this.createColumnDefs();

    

   this.retriveData();

    
  }

  onKey_Enter($event) {
    // console.log($event);
     this.retriveData();
   }
   
  retriveData() {
 
    this.epironService.SearchCaseByUserGuidService$( ).subscribe(
      res => {
        this.caseList = res;
    
        if(this.caseList)
        {
          this.count = this.caseList.length;
        }
        else
        {
          this.count = 0;
        }
        this.globalError = null;
      },
      err => {
        
        this.globalError = err;
      }
    );

  }
  private createColumnDefs() {
    this.columnDefs = [
      { headerName: "CaseId", field: "CaseId" ,width: 150,pinned: true,filter: 'text'},
      { headerName: "CaseModifiedDate", field: "CaseModifiedDate" ,width: 150,pinned: true,filter: 'text'},
      { headerName: "CaseCreated", field: "CaseCreated" ,width: 150,pinned: true,filter: 'text'},
      
      { headerName: "AttentionQueueName", field: "AttentionQueueName" ,width: 150,pinned: true,filter: 'text'},
      // { headerName: "NombreProfecion", field: "NombreProfecion" ,width: 150,pinned: true,filter: 'text'},
      { headerName: "UCUserName", field: "UCUserName",width: 200,pinned: true }
    ];
  }


  onGridRowDoubleClick(event){
    
    this.onCaseGridDoubleClick.emit(event.node.data as CaseByUserGuidBE);
    // http://localhost:4200/caseEdit?id=4350
    //this.router.navigate(['caseEdit'], { queryParams: { id: caseId }}); 
    
    
    

  }

  
  onGridReady(params) {
    params.api.sizeColumnsToFit();
  }


  onCellClicked(event) {
    
    this.currentCase = event.node.data as CaseByUserGuidBE;
        
  }

  onGridCellDoubleClick(event) {
    //alert(event);
  }

}
