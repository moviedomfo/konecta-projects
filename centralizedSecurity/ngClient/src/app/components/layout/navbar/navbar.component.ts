import { Component, OnInit } from '@angular/core';
import { SerurityService } from 'src/app/service/serurity.service';
import { CurrentLogin } from 'src/app/model/common.model';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { pipe } from 'rxjs';
import { Router } from '@angular/router';
import { AppConstants } from 'src/app/model/common.constants';



@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  
  constructor(private authService: SerurityService, private route: Router) { }
  public app_name: string;
  public isLogged: boolean = false;
  public isRessetUser: boolean = false;
  public userName:string;
  registerForm: FormGroup;
  public prod=false;

  ngOnInit() {

    this.app_name ='Reseteos web ' +  AppConstants.AppVersion ; 
    this.chk_logingFront();
    this.prod = AppConstants.AppProducion;
    this.authService.logingChange_subject$.subscribe(pipe(
      res=>{
         this.isLogged= res as boolean;
         
         this.chk_logingFront();
      }
    ));
  }

 

  chk_logingFront(){
    var currentLoging: CurrentLogin = this.authService.getCurrenLoging();
    if (currentLoging) {
      //console.log('user logged');
      this.isLogged = true;
      this.userName= currentLoging.currentUser.UserName;
      this.isRessetUser = currentLoging.isRessetUser;
     
    } else {
      //console.log('NOT user logged');
      this.isLogged = false;
    }
  }

  onLogout() {
    this.authService.signOut();
    this.route.navigate(['/login']);
  }


}
