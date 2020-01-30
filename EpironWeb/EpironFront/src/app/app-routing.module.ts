import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './common-components/home/home.component';
import { LoginComponent } from './common-components/login/login.component';
import { providerManageComponent } from './functionalComponents/provider/provider-manage/provider-manage.component';
import { PageNotFoundComponent } from './common-components/page-not-found/page-not-found.component';
import { providerGridComponent } from "./functionalComponents/provider/providerGrid/providerGrid.component";
import { PersonGridComponent } from "./functionalComponents/persons/person-grid/person-grid.component";
import { AuthGuard } from './common-components/routingGuard/AuthGuard';
import { TablesComponent } from './functionalComponents/test/tables/tables.component';
import { StockComponent } from './functionalComponents/stock/stock.component';
import { StockManageComponent } from './functionalComponents/stock/stock-manage/stock-manage.component';
import { StockGridComponent } from './functionalComponents/stock/stock-grid/stock-grid.component';
import { ProviderQuotationRequestComponent } from './functionalComponents/providerQuotationRequest/providerQuotationRequest.component';
import { ProviderQuotationRequestGridComponent } from './functionalComponents/providerQuotationRequest/providerQuotationRequest-grid/providerQuotationRequest-grid.component';
import { ProviderQuotationRequestManageComponent } from './functionalComponents/providerQuotationRequest/providerQuotationRequest-manage/providerQuotationRequest-manage.component';
import { QuotationRequestManagementComponent } from './functionalComponents/quotationRequest/quotationRequest-management/quotationRequest-management.component';
import { QuotationRequestGridComponent } from './functionalComponents/quotationRequest/quotationRequest-grid/quotationRequest-grid.component';
import { QuotationRequestComponent } from './functionalComponents/quotationRequest/quotationRequest.component';
import { ProviderAssignmentComponent } from './functionalComponents/quotationRequest/provider-assignment/provider-assignment.component';
import { StockAlertsComponent } from './functionalComponents/stock/stock-alerts/stock-alerts.component';
import { SendEmailComponent } from './functionalComponents/quotationRequest/send-email/send-email.component';
import { DashboardComponent } from './functionalComponents/security/dashboard/dashboard.component';
import { UserComponent } from './functionalComponents/security/user/user.component';
import { UserGridComponent } from './functionalComponents/security/user-grid/user-grid.component';
import { UserRessetPwdComponent } from './functionalComponents/security/user-resset-pwd/user-resset-pwd.component';
import { ReactiveFornmValidationsComponent } from './functionalComponents/test/reactive-fornm-validations/reactive-fornm-validations.component';
import { TestDashboardComponent } from './functionalComponents/test/dashboard/testDashboard.component';
import { TestTimesComponent } from './functionalComponents/test/test-times/test-times.component';
import { IntersectionsComponent } from './functionalComponents/test/test-times/intersections/intersections.component';

const appRoutes: Routes = [

  { path: 'login', component: LoginComponent },
  { path: '', component: HomeComponent },
  { path: 'personsGrid', component: PersonGridComponent, canActivate: [AuthGuard] },

  {
    path: 'security',
    children: [

      { path: '', component: DashboardComponent, pathMatch: 'full', canActivate: [AuthGuard] },

      { path: 'user/list', component: UserGridComponent, canActivate: [AuthGuard] },
      { path: 'account/resset', component: UserRessetPwdComponent, canActivate: [AuthGuard] },
      { path: 'account/new', component: UserComponent, canActivate: [AuthGuard] },
      { path: 'account/:id', component: UserComponent, canActivate: [AuthGuard] }


    ]
  },
  //  { path: 'providerCreate', component: providerManageComponent,canActivate: [AuthGuard] },
  //  { path: 'provider/:id', component: providerManageComponent ,canActivate: [AuthGuard]},
  //  { path: 'providerGrid', component: providerGridComponent ,canActivate: [AuthGuard]},

  {
    path: 'provider',
    children: [

      { path: '', component: providerGridComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'provider', component: providerGridComponent, canActivate: [AuthGuard] },
      { path: 'new', component: providerManageComponent, canActivate: [AuthGuard] },
      { path: ':id', component: providerManageComponent, canActivate: [AuthGuard] }

    ]
  },

  {
    path: 'stock',
    children: [
      //{ path: 'stock', component: StockComponent },
      { path: '', component: StockComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'stock', component: StockComponent, canActivate: [AuthGuard] },
      { path: 'stockAlerts', component: StockAlertsComponent, canActivate: [AuthGuard] },
      { path: 'stockGrid', component: StockGridComponent, canActivate: [AuthGuard] },
      { path: 'new', component: StockManageComponent, canActivate: [AuthGuard] },
      { path: ':id', component: StockManageComponent, canActivate: [AuthGuard] }

    ]
  },

  {
    path: 'quotationRequest',
    children: [
      { path: '', component: QuotationRequestComponent, pathMatch: 'full', canActivate: [AuthGuard] },
      { path: 'quotationRequest', component: QuotationRequestComponent, canActivate: [AuthGuard] },
      { path: 'quotationRequestGrid', component: QuotationRequestGridComponent, canActivate: [AuthGuard] },
      { path: 'new', component: QuotationRequestManagementComponent, canActivate: [AuthGuard] },
      { path: ':id', component: QuotationRequestManagementComponent, canActivate: [AuthGuard] },
      { path: 'providerAssignment/:id', component: ProviderAssignmentComponent, canActivate: [AuthGuard] },
      { path: 'sendEmailToProviders/:id', component: SendEmailComponent, canActivate: [AuthGuard] },
      { path: 'providerQuotationRequestManage/:id', component: ProviderQuotationRequestManageComponent, canActivate: [AuthGuard] }
    ]
  },





  { path: 'quotations/quotationsGrid', component: ProviderQuotationRequestGridComponent },
  { path: 'quotations', component: ProviderQuotationRequestComponent },
  { path: 'quotations/new', component: ProviderQuotationRequestManageComponent },
  { path: 'quotations/:id', component: ProviderQuotationRequestManageComponent },



  {
    path: 'test',
    children: [
      { path: '', component: TestDashboardComponent, pathMatch: 'full' },
      { path: 'testTables', component: TablesComponent },
      { path: 'reactiveFornmValidations', component: ReactiveFornmValidationsComponent },
      { path: 'times', component: TestTimesComponent },
      { path: 'intersections', component: IntersectionsComponent },
      
    ]
  },


  { path: 'testTables', component: TablesComponent },
  { path: 'reactiveFornmValidations', component: ReactiveFornmValidationsComponent },

  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
