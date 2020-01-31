import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { ServiceError, MotivoConsultaEnum, SecurityUser, SecurityRole, AppConstants, PersonBE, EventType } from 'src/app/model';
import { AuthenticationService, CommonService } from 'src/app/service';
import { ActivatedRoute, Router } from '@angular/router';
import { GridOptions } from 'ag-grid-community';
import { DateComponent } from 'src/app/common-components/controls/ag-grid/date.component';


@Component({
  selector: 'app-user-grid',
  templateUrl: './user-grid.component.html'

})

export class UserGridComponent implements OnInit {

  public userList: SecurityUser[];
  public globalError: ServiceError;
  public currentUser: SecurityUser;
  public txtQuery: string;
  public userCount: number;
  public columnDefs: any[];

  public gridOptions: GridOptions;

  constructor(private securityService: AuthenticationService, private router: Router) { }

  ngOnInit() {
    this.currentUser = new SecurityUser();
    this.gridOptions = <GridOptions>{};
    this.gridOptions.dateComponentFramework = DateComponent;
    this.gridOptions.floatingFilter = true;
    this.gridOptions.rowStyle = { background: 'black' };
    this.createColumnDefs();

    this.retriveData();
  }

  private createColumnDefs() {
    this.columnDefs = [
      { headerName: "Ususario", field: "userName", width: 250, pinned: true, filter: 'text' },

      { headerName: "email", field: "email", width: 80, pinned: true, filter: 'text' },
      { headerName: "Nombre ", field: "cost", width: 80, pinned: true, filter: 'text' },
      //{ headerName: "Tipo", field: "typeName", width: 150, pinned: true, filter: 'text' },

    ];
  }

  onChange_cmbStockType($event) {
    this.retriveData();
  }

  onKey_Enter() {

    this.retriveData();
  }
  
  retriveData() {

    //alert(this.inputKeyList);
    //StockType
    this.securityService.getUsers$('').subscribe(
      res => {
        this.userList = res;
        if (this.userList) {
          this.userCount = this.userList.length;
        }
        else {
          this.userCount = 0;
        }
      },
      err => {

        this.globalError = err;
      }
    );

  }

  onGridReady(params) {
    params.api.sizeColumnsToFit();
  }
  onCellClicked(event) {

    this.currentUser = event.node.data as SecurityUser;

  }

  onGridCellDoubleClick(event) {
    //alert(event);
  }

  onGridRowDoubleClick(event) {

    this.currentUser = event.node.data as SecurityUser;

    this.router.navigate(['security/account/', this.currentUser.userId]);
  }


}
