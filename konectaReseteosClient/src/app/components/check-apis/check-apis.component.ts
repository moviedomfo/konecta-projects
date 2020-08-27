import { Component, OnInit } from '@angular/core';
import { SerurityService } from 'src/app/service/serurity.service';
import { Domain } from 'src/app/model/bussinessModel';
import { ServiceError, IpInfo } from 'src/app/model/common.model';
import { Observable } from 'rxjs';
import { CommonService } from 'src/app/service/common.service';


@Component({
  selector: 'app-check-apis',
  templateUrl: './check-apis.component.html',
  styleUrls: ['./check-apis.component.css']
})
export class CheckApisComponent implements OnInit {
  public domains: Domain[];
  public globalError: ServiceError;
  public dni:string;
  constructor(
    
    private Serurity: SerurityService,
    private commonService:CommonService) { }

  ngOnInit() {
   
  
  }
 
  stopTimer() {
    
  }
  btn_checkDomainsClick() {
    var dom$: Observable<Domain[]> = this.Serurity.checkDomains$();

    dom$.subscribe(res => {
      this.domains = res;
      //alert( res);
      
      console.log('cargado domains');
    },
      err => { this.globalError = err; }
    );
  }


  btn_ldap_pingClick() {
    var dom$: Observable<Domain[]> = this.Serurity.ldap_ping$();

    dom$.subscribe(res => {
      alert( res);
      
    },
      err => { this.globalError = err; }
    );
  }

  
  btn_getIp_Click() {
    var dom$: Observable<IpInfo> = this.commonService.get_host_ipInfo();

    dom$.subscribe(res => {
      alert( " the current client global IP is " + res.ip);
    },
      err => { this.globalError = err; }
    );
  }

  numberOnly(event): boolean {
    const charCode = (event.which) ? event.which : event.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
      return false;
    }
    return true;

  }
}
