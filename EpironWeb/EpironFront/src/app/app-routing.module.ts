import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './common-components/home/home.component';
import { LoginComponent } from './common-components/login/login.component';

import { PageNotFoundComponent } from './common-components/page-not-found/page-not-found.component';

import { PersonGridComponent } from "./functionalComponents/persons/person-grid/person-grid.component";
import { AuthGuard } from './common-components/routingGuard/AuthGuard';

import { DashboardComponent } from './functionalComponents/security/dashboard/dashboard.component';
import { UserComponent } from './functionalComponents/security/user/user.component';
import { UserGridComponent } from './functionalComponents/security/user-grid/user-grid.component';
import { UserRessetPwdComponent } from './functionalComponents/security/user-resset-pwd/user-resset-pwd.component';
import { CasosGridComponent } from './functionalComponents/casos/casosGrid/casosGrid.component';

const appRoutes: Routes = [

  { path: 'login', component: LoginComponent },
  { path: '', component: HomeComponent },
  { path: 'casosGrid', component: CasosGridComponent },

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


    
 



];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
