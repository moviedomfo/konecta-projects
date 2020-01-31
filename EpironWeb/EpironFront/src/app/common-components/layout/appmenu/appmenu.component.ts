import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { pipe } from 'rxjs';
import { AuthenticationService } from '../../../service';
import { CurrentLogin,  AppConstants, PersonBE } from '../../../model';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { helperFunctions } from 'src/app/service/helperFunctions';


@Component({
  selector: 'app-appmenu',
  templateUrl: './appmenu.component.html',
  styleUrls: ['./appmenu.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AppmenuComponent implements OnInit {
  public messagesCount: number = 11;
  public providerPhotoUrl: SafeUrl = '';
  public isLogged: boolean = false;
  public apellidoNombre: string = '';
  constructor(
    private authService: AuthenticationService,    
    private domSanitizer: DomSanitizer) {  }

  ngOnInit() {

    this.authService.logingChange_subject$.subscribe(pipe(
      res => {
        
          this.chk_logingFront();
      }
    ));
  

    this.chk_logingFront();
  }


  chk_logingFront() {
    var currentLoging: CurrentLogin = this.authService.getCurrenLoging();
    
    if (currentLoging) {

    
      this.isLogged = true;
     
      this.chk_profDataFront(currentLoging);
      //this.apellidoNombre= currentLoging.currentUser.userName;
    } else {
      //console.log('NOT user logged');
      this.isLogged = false;
      this.loadDefaultPhoto(0);
      this.apellidoNombre='No loging';
      //   alert('NOT user logged')
    }


  }


  chk_profDataFront(currentLoging:CurrentLogin) {

    if (currentLoging.userData) {
      //console.log('user logged');
      this.isLogged = true;

      this.apellidoNombre =helperFunctions.getPersonFullName(currentLoging.userData.name, currentLoging.userData.lastName)

      if (currentLoging.userData.photo !== null) {
        this.providerPhotoUrl = this.domSanitizer.bypassSecurityTrustUrl('data:image/jpg;base64, ' + (currentLoging.userData.photo));
      }
      else {
        this.loadDefaultPhoto(currentLoging.userData.sex);
      }
    }
  }

  loadDefaultPhoto(sexo: number) {

    this.providerPhotoUrl = AppConstants.ImagesSrc_Woman;
    if (sexo === 0) {
      this.providerPhotoUrl = AppConstants.ImagesSrc_Man;
    }
  }

}
