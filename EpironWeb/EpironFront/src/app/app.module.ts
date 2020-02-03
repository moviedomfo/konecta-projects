import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';


// common-components
import { PageNotFoundComponent } from './common-components/page-not-found/page-not-found.component';
//import { ModalDialogComponent } from './common-components/modal-dialog/modal-dialog.component';
import { AuthGuard } from './common-components/routingGuard/AuthGuard';

import { AppsettingComponent } from './common-components/layout/appsetting/appsetting.component';
import { AppmenuComponent } from './common-components/layout/appmenu/appmenu.component';
import { AppfooterComponent } from './common-components/layout/appfooter/appfooter.component';
import { AppheaderComponent } from './common-components/layout/appheader/appheader.component';
import { AgGridModule } from 'ag-grid-angular/main';
import { DateComponent } from './common-components/controls/ag-grid/date.component';
import { HeaderComponent } from './common-components/controls/ag-grid/header.component';
import { HeaderGroupComponent } from './common-components/controls/ag-grid/header-group.component';
import { WeekDaysCheckEditComponent } from './common-components/controls/week-days-check-edit/week-days-check-edit.component';
import { EmailValidator } from './common-components/controls/validate-email.directive';
import { ValidateEqualDirective } from './common-components/controls/validate-equal.directive';

import { CommonService,  PersonsService,   AuthenticationService, EpironSecurityService} from './service/index';
import { HomeComponent } from './common-components/home/home.component';
import { ErrorBoxContainerComponent } from './common-components/error-box-container/error-box-container.component';
import { UserTasksComponent } from './common-components/userAlerts/user-tasks/user-tasks.component';
import { UserMessagesComponent } from './common-components/userAlerts/user-messages/user-messages.component';

// //Bussines components
import { PersonsComponent } from './functionalComponents/persons/persons.component';
import { PersonCardComponent } from './functionalComponents/persons/person-card/person-card.component';
import { LoginComponent } from './common-components/login/login.component';
import { AddressListComponent } from './functionalComponents/persons/address-list/address-list.component';
import { AddreesComponent } from './functionalComponents/persons/addrees/addrees.component';

import { OnlyNumbersDirective } from './common-components/controls/only-numbers.directive';
import { SessionSettingComponent } from './functionalComponents/security/sessionSetting/sessionSetting.component';
import { ContactComponent } from './functionalComponents/persons/contact/contact.component';

import { ButtonRendererComponent } from './common-components/controls/ag-grid/agGridButton.component';

import {PersonAddressComponent} from './functionalComponents/persons/person-address/person-address.component';
import { UserComponent } from './functionalComponents/security/user/user.component';
import { UserGridComponent } from './functionalComponents/security/user-grid/user-grid.component';
import { DashboardComponent } from './functionalComponents/security/dashboard/dashboard.component';
import { UserRessetPwdComponent } from './functionalComponents/security/user-resset-pwd/user-resset-pwd.component';
import { MustMatchDirective } from './common-components/controls/TemplateDrivenValidator/mustMatch.validator.directive';
import { DateMoreThanDirective } from './common-components/controls/ReactiveFormsValidators/dateMoreThan.validator';

import {  DateValueAccessorModule } from './common-components/controls/DateValueAccessor';
import { AlertBlockComponent } from './common-components/alert-block/alert-block.component';
import { PersonGridComponent } from './functionalComponents/persons/person-grid/person-grid.component';
import { JwtAuthInterceptor } from './service/jwtIntersept';



@NgModule({
  declarations: [
    PersonGridComponent ,
    DateMoreThanDirective,
    MustMatchDirective,
    ButtonRendererComponent,
    DateComponent,HeaderComponent,HeaderGroupComponent,
    AppComponent,
    HomeComponent ,
    PageNotFoundComponent,
    AppsettingComponent,AppmenuComponent,AppfooterComponent,AppheaderComponent,
    PersonsComponent,  
    PersonAddressComponent,
    PersonCardComponent,
    LoginComponent,
    ErrorBoxContainerComponent,
    AlertBlockComponent,
    WeekDaysCheckEditComponent,
    EmailValidator,  SessionSettingComponent, ValidateEqualDirective,  ContactComponent,
    AppsettingComponent, AppmenuComponent, AppfooterComponent, AppheaderComponent, 
    HomeComponent, UserMessagesComponent,UserTasksComponent, AddressListComponent, AddreesComponent, 
    OnlyNumbersDirective, 
    UserComponent, UserGridComponent, DashboardComponent
    ,UserRessetPwdComponent, 
  ],
  imports: [
    NgIdleKeepaliveModule.forRoot(),
    AgGridModule.withComponents([ButtonRendererComponent,DateComponent,HeaderGroupComponent,HeaderComponent]),
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,DateValueAccessorModule 
    
  ],
  providers: [EpironSecurityService,PersonsService,  CommonService,  AuthenticationService,AuthGuard,JwtAuthInterceptor],
  bootstrap: [AppComponent]
})
export class AppModule { }
