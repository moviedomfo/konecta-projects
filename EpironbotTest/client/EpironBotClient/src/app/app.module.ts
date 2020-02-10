import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';

import { AppmenuComponent } from './layout/appmenu/appmenu.component';
import { AppheaderComponent } from './layout/appheader/appheader.component';
import { HomeComponent } from './home/home.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { MaterialModule } from './material-module';
import { LayoutComponent } from './layout/layout.component';
import {FlexLayoutModule} from '@angular/flex-layout';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent,
    AppmenuComponent,
    AppheaderComponent,
    HomeComponent,
    LayoutComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    FlexLayoutModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
