// Author: T4professor

import { Component, OnInit, AfterContentInit } from '@angular/core';
import { ICellRendererAngularComp } from 'ag-grid-angular';

@Component({
  selector: 'app-button-renderer',
  templateUrl:'agGridButton.component.html'
  // template: `
  //   <button id="agButtonRenderer" class="{{btnClass}}" type="button" (click)="onClick($event)">{{label}}</button>
  //   `
})
//<div><button class="btn btn-outline-danger btn-sm" onClick="removeAddress($event)"><i class="fas fa-minus-circle pr-2" aria-hidden="true"> </i>quitar </button></div>
export class ButtonRendererComponent implements ICellRendererAngularComp {
    //https://stackblitz.com/edit/angular-ag-grid-button-renderer?file=src%2Fapp%2Fapp.component.ts
  params: any;
  label: string;
  getLabelFunction: any;
  btnClass: string;
  btnIcon:string;
  agInit(params: any): void {
    this.params = params;
    this.label = this.params.label || null;
    this.btnIcon = this.params.btnIcon || null;
    this.btnClass = this.params.btnClass || 'btn btn-primary';
    this.getLabelFunction = this.params.getLabelFunction;

    if(this.getLabelFunction && this.getLabelFunction instanceof Function)
    {
      console.log(this.params);
      this.label = this.getLabelFunction(params.data);
    }
   
    
  }

  refresh(params?: any): boolean {
    return true;
  }

  onClick($event) {
    if (this.params.onClick instanceof Function) {
      // put anything into params u want pass into parents component
      const params = {
        event: $event,
        rowData: this.params.node.data
        // ...something
      }
      this.params.onClick(params);

    }
  }
}