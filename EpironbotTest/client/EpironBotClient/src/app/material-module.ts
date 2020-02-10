import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {  MatToolbarModule, MatSidenavModule } from '@angular/material';
import { MatTabsModule } from '@angular/material';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatTabsModule,
    MatSidenavModule
  ],
  exports: [
    MatToolbarModule,
    MatTabsModule,
    MatSidenavModule
  ],
})
export class MaterialModule { }
