import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Page404NotFoundComponent } from './commonComponets/page404-not-found/page404-not-found.component';
import { LogingComponent } from './components/loging/loging.component';
import { UserRessetComponent } from './components/user-resset/user-resset.component';
import { SettingsComponent } from './components/settings/settings.component';
import { CheckApisComponent } from './components/check-apis/check-apis.component';
import { ChagePwdComponent } from './components/chage-pwd/chage-pwd.component';
import { ForgotPwdComponent } from './components/forgot-pwd/forgot-pwd.component';
import { ForgotPwdChangeComponent } from './components/forgot-pwd-change/forgot-pwd-change.component';


const routes: Routes = [
  { path: '', component: UserRessetComponent },
  { path: 'login', component: LogingComponent },
  { path: 'userReset', component: UserRessetComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'checkApis', component: CheckApisComponent },
  { path: 'changePwd', component: ChagePwdComponent },
  { path: 'forgotPwd', component: ForgotPwdComponent },
  //{ path: 'reset/:code/:dni', component: ForgotPwdChangeComponent },
  { path: 'reset', component: ForgotPwdChangeComponent },
  
  // { path: 'reset', component: ForgotPwdChangeComponent },
  { path: '**', component: Page404NotFoundComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
