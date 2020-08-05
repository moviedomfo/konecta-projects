import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { LogingComponent } from './components/loging/loging.component';
import {CommonService} from './service/common.service';
import {SerurityService} from './service/serurity.service';

import { AlertBlockComponent } from './commonComponets/alert-block/alert-block.component';
import { ErrorBoxContainerComponent } from './commonComponets/error-box-container/error-box-container.component';
import { Page404NotFoundComponent } from './commonComponets/page404-not-found/page404-not-found.component';
import { OnlynumberDirective } from './commonComponets/onlynumber.directive';
import { AuthGuard } from './commonComponets/routingGuard/AuthGuard';
import { MustMatchDirective } from './commonComponets/mustMath.directive';

import { HttpClientModule } from '@angular/common/http';
import { NgxSpinnerModule } from 'ngx-spinner';
import { RecaptchaModule } from 'ng-recaptcha'
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SettingsComponent } from './components/settings/settings.component';
import { ChagePwdComponent } from './components/chage-pwd/chage-pwd.component';
import { CheckApisComponent } from './components/check-apis/check-apis.component';
import { ForgotPwdComponent } from './components/forgot-pwd/forgot-pwd.component';
import { ForgotPwdChangeComponent } from './components/forgot-pwd-change/forgot-pwd-change.component';
import { UserRessetComponent } from './components/user-resset/user-resset.component';
import { NavbarComponent } from './components/layout/navbar/navbar.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@NgModule({
  declarations: [
    AppComponent,
    LogingComponent,
    Page404NotFoundComponent,
    AlertBlockComponent,ErrorBoxContainerComponent,
    OnlynumberDirective,
    MustMatchDirective,
    CheckApisComponent,
    ChagePwdComponent,
    UserRessetComponent,
    SettingsComponent,
    ForgotPwdComponent,
    ForgotPwdChangeComponent,
    FooterComponent,
    NavbarComponent


  ],
  imports: [
    NgIdleKeepaliveModule.forRoot(),
    BrowserModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    AppRoutingModule,
    RecaptchaModule,HttpClientModule, NgbModule
  ],
  providers: [CommonService, SerurityService,AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
