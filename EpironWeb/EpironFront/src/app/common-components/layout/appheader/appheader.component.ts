import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { pipe } from 'rxjs';
import { AuthenticationService } from '../../../service';
import { CurrentLogin,  AppConstants, UserTask, UserMessage, PersonBE, CurrentLoginEpiron } from '../../../model';
import * as moment from 'moment';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { helperFunctions } from '../../../service/helperFunctions';
import { Router } from '@angular/router';


@Component({
  selector: 'app-appheader',
  templateUrl: './appheader.component.html',
  encapsulation: ViewEncapsulation.None

})
export class AppheaderComponent implements OnInit {
  public isLogged: boolean = false;
  public userName: string = '';
  public apellidoNombre: string = '';

  public desde: string = '';
  public providerPhotoUrl: SafeUrl = '';

  public taskCount: number;
  public tasks: UserTask[];
  public messagesCount: number;
  public messages: UserMessage[];
  public prod = false;
  public app_name: string;
  constructor(
    private router: Router,
    private authService: AuthenticationService,
    
    private domSanitizer: DomSanitizer) {


    //subscriptions to provider data chenges
    // this.profService.currentproviderChange_subject$.subscribe(pipe(
    //   res => {
    //     let p: Provider_FullViewBE = res as Provider_FullViewBE;

    //     this.chk_profDataFront(p);
    //   }
    // ));


  }

  ngOnInit() {
    this.app_name = 'Reseteos web ' + AppConstants.AppVersion;
    this.prod = AppConstants.AppProducion;

    //subscriptions to loging changes
    this.authService.logingChange_subject$.subscribe((res) => {
      //inicio el countdown para el patron user idle
      if (res.isLogued === true) {
        this.chk_logingFront();
      }
    });

    this.chk_logingFront();

    this.initializeTasks();
  }

  onBtnLogin_click() {

    this.router.navigate(['/login']);
  }

  chk_logingFront() {

    
    var currentLoging: CurrentLoginEpiron = this.authService.getCurrenLoging();
    
    
    if (currentLoging) {

      this.chk_profDataFront(currentLoging);

    } else {

      this.isLogged = false;
      this.onLogout();
    }


  }

  onLogout() {
  
    this.authService.signOut();
    this.chk_logingFront();
    //this.router.navigate(['/login']);
  }

  chk_profDataFront(currentLoging: CurrentLoginEpiron) {
    if (currentLoging.userData) {

      if(!currentLoging.userData.sex)
          currentLoging.userData.sex =0;

      this.isLogged = true;
      this.apellidoNombre = helperFunctions.getPersonFullName(currentLoging.userData.PersonFirstName, currentLoging.userData.PersonLastName)

      // if (currentLoging.userData.photo) {
        
      //   this.providerPhotoUrl = this.domSanitizer.bypassSecurityTrustUrl('data:image/jpg;base64, ' + (currentLoging.userData.photo));
      // }
      // else {
      //   this.loadDefaultPhoto(currentLoging.userData.sex);
      // }
      //this.nombreEspecialidad = prof.NombreEspecialidad;
      //var sinceDate = moment(prof.FechaAlta).format('MMMM Do YYYY, h:mm:ss a');
      //var since = moment(currentLoging.entryDate, "YYYYMMDD").fromNow();
      //this.desde = sinceDate;



    } else {

      this.isLogged = false;
    }
  }


  loadDefaultPhoto(sexo: number) {

    this.providerPhotoUrl = AppConstants.ImagesSrc_Woman;
    if (sexo === 0) {
      this.providerPhotoUrl = AppConstants.ImagesSrc_Man;
    }
  }


  initializeTasks() {
    let d = new Date();

    this.tasks = [
      new UserTask({ taskId: 0, tittle: "Realiza examen físico al paciente Moreno", completedPercent: 50, descripción: '', createdDate: d, priority: "low" }),
      new UserTask({ taskId: 1, tittle: "Planifica el estudio del paciente y determina el tratamiento a seguir.", completedPercent: 50, descripción: '', createdDate: d, priority: "medium" }),
      new UserTask({ taskId: 2, tittle: "Visitas domiciliarias hoy 13:30", completedPercent: 50, descripción: 'Visita a Alan Mcdonalls', createdDate: d, priority: "hight" }),
    ];
    this.taskCount = this.tasks.length;

    this.messages = [
      new UserMessage({ messageId: 0, tittle: "Realiza examen físico al paciente Moreno", completedPercent: 50, body: '', createdDate: moment().subtract(33, 'minutes').toDate() }),
      new UserMessage({ messageId: 1, tittle: "Planifica el estudio del paciente y determina el tratamiento a seguir.", completedPercent: 50, body: '', createdDate: moment().subtract(6, 'hours').toDate() }),
      new UserMessage({ messageId: 2, tittle: "Visitas domiciliarias hoy 13:30", completedPercent: 50, body: 'Visita a Alan Mcdonalls', createdDate: moment().subtract(30, 'seconds').toDate() }),
    ];
    this.messagesCount = this.messages.length;
  }
}
