import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LogingComponent } from './components/loging/loging.component';
import {CommonService} from './service/common.service';
import {SerurityService} from './service/serurity.service';
import { AlertBlockComponent } from './commonComponets/alert-block/alert-block.component';
import { Page404NotFoundComponent } from './commonComponets/page404-not-found/page404-not-found.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LayoutModule } from '@angular/cdk/layout';
import { NgxSpinnerModule } from 'ngx-spinner';
import {NgxPaginationModule} from 'ngx-pagination';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './material.module';
import { ErrorBoxContainerComponent } from './commonComponets/error-box-container/error-box-container.component';
import { NavbarComponent } from './components/layout/navbar/navbar.component';
import { FooterComponent } from './components/layout/footer/footer.component';
import { UserResetComponent} from './components/user-reset/user-reset-component';
import { SettingsComponent } from './components/settings/settings.component';
import { CheckApisComponent } from './components/check-apis/check-apis.component';
import { NgxSmartModalModule ,NgxSmartModalService} from 'ngx-smart-modal';
import { OnlynumberDirective } from './commonComponets/onlynumber.directive';
import { AuthGuard } from './commonComponets/routingGuard/AuthGuard';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';

import { DigitOnlyModule } from '@uiowa/digit-only';
import { ChagePwdComponent } from './components/chage-pwd/chage-pwd.component';
import { MustMatchDirective } from './commonComponets/mustMath.directive';
import { ForgotPwdComponent } from './components/forgot-pwd/forgot-pwd.component';
import { ForgotPwdChangeComponent } from './components/forgot-pwd-change/forgot-pwd-change.component';

import { RecaptchaModule, RecaptchaFormsModule } from 'ng-recaptcha'
import { fwkSeriveHttpInterseptor } from './service/httpInterseptor';



@NgModule({
  declarations: [
    AppComponent,
    LogingComponent,
    Page404NotFoundComponent,
    AlertBlockComponent,ErrorBoxContainerComponent,
    NavbarComponent,
    UserResetComponent,
    SettingsComponent,
    FooterComponent,
    CheckApisComponent,
    OnlynumberDirective,
    ChagePwdComponent,
    MustMatchDirective,
    ForgotPwdComponent,
    ForgotPwdChangeComponent
    
    
  ],
  imports: [
    NgxSmartModalModule.forRoot(),
    NgIdleKeepaliveModule.forRoot(),
    FormsModule,
    ReactiveFormsModule,
    BrowserModule,
    AppRoutingModule,
    MaterialModule,
    HttpClientModule,
    BrowserAnimationsModule,
    LayoutModule,
    NgxSpinnerModule,
    NgxPaginationModule,
    AppRoutingModule,DigitOnlyModule,RecaptchaModule
    
  ],
  providers: [CommonService, SerurityService,NgxSmartModalService,AuthGuard,
  
      ],
  bootstrap: [AppComponent]
})
export class AppModule { }
