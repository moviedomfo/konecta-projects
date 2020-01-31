import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/service';
import { ServiceError, UserSession } from 'src/app/model';

@Component({
  selector: 'app-user-resset-pwd',
  templateUrl: './user-resset-pwd.component.html'
})


export class UserRessetPwdComponent implements OnInit {

  public globalError: ServiceError;
  public userSession: UserSession;
  public enableChange:boolean;
  public userToFind = "";
  constructor(private securityService: AuthenticationService) {
    this.enableChange = false;
   this.cleanUser();
  }

  ngOnInit() {

  }
 cleanUser(){
   
  this.userSession = new UserSession();
  this.userSession.userName='';
 }
  btnCheckUserName_Click() {
    this.cleanUser();
    this.globalError = null;
    if (!this.userToFind) {
      alert('Ingrese nombre de usuario');
      return;
    }
    this.securityService.getUser$(this.userToFind, null).subscribe(
      res => {

        if (!res) {
          alert('El usuario no existe ');
          this.enableChange = false;
        }
        else {
          this.enableChange = true;
          this.userSession.email = res.email;
          this.userSession.userName = res.userName;
          this.userSession.userId = res.userId;

        }

      },
      err => {
        this.enableChange = false;
        this.globalError = err;
      }
    );
  }

  btnResetPwd_Click() {


    //mostrar "Esta a punto de reestablacer la clave de inicio de sesión del usuario, esta seguro ?"
    if (!this.userSession.userName || this.userSession.userName == '') {
      alert('Falta nombre de usuario');
    }
    if (!this.userSession.password || this.userSession.password == '') {
      alert('falta  Password');
      return;
    }
    this.securityService.resetUserPassword$(this.userSession.userName, this.userSession.password).subscribe(
      res => {
        if (res === true) {
          alert('La password se cambio con exito !!');
        }

      },
      err => {

        this.globalError = err;
      }
    );
  }
}
