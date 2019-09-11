import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { Page404NotFoundComponent } from './commonComponets/page404-not-found/page404-not-found.component';
import { LogingComponent } from './components/loging/loging.component';
import { UserResetComponent } from './components/user-reset/user-reset-component';
import { SettingsComponent } from './components/settings/settings.component';
import { CheckApisComponent } from './components/check-apis/check-apis.component';

const routes: Routes = [
  { path: '', component: LogingComponent },
  { path: 'login', component: LogingComponent },
  { path: 'userReset', component: UserResetComponent },
  { path: 'settings', component: SettingsComponent },
  { path: 'checkApis', component: CheckApisComponent },
  
  { path: '**', component: Page404NotFoundComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})


export class AppRoutingModule {

  constructor() { }

  ngOnInit() { }
}
