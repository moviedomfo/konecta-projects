import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ServiceError, CurrentLogin } from 'src/app/model/common.model';
import { SerurityService } from 'src/app/service/serurity.service';
import { CommonService } from 'src/app/service/common.service';
import { AppConstants } from 'src/app/model/common.constants';
import { ApiServerInfo } from 'src/app/model/bussinessModel';

@Component({
  selector: 'app-settings',
  templateUrl: './settings.component.html',
  styleUrls: ['./settings.component.css']
})
export class SettingsComponent implements AfterViewInit {
  public globalError: ServiceError;
  public currentLoging:CurrentLogin;

  public apiServerInfo :ApiServerInfo;
  constructor(
    private commonService: CommonService,
    private secService: SerurityService) {
      this.apiServerInfo = new ApiServerInfo();
     }


  ngAfterViewInit(): void {

    this.currentLoging =  this.secService.getCurrenLoging();

    alert(this.currentLoging.currentUser.DomainId);
  }
  ngOnInit() {
    if(!this.secService.isAuth()){
      alert('No autenticado');
    }

    this.currentLoging =  this.secService.getCurrenLoging();

    
    this.secService.getServerInfo$().subscribe(
      res => {
        this.apiServerInfo=res;
        this.apiServerInfo.url_api = AppConstants.AppOAuth_Base;
      },
      err => {this.globalError = err;}
    );
  }

}
