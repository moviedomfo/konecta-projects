import { Component } from '@angular/core';
import { Idle, DEFAULT_INTERRUPTSOURCES } from '@ng-idle/core';
import { Keepalive } from '@ng-idle/keepalive';
import { Router } from '@angular/router';
import { SerurityService } from './service/serurity.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'angularClient';
  public idleState = "";//'Not_Started.';
  public timedOut = false;
  public lastPing?: Date = null;

  //idle timeout of 5 
  public iddleTimeout_seconds = 600;

  //period of time in seconds. after 10 seconds of inactivity, the user will be considered timed out.
  public iddle_waite_Timeout_seconds = 10;

  constructor(
    private router: Router,
    private authService: SerurityService,
    private idle: Idle, private keepalive: Keepalive) { // initiate it in your component constructor

    //detecto inicio session
    this.authService.logingChange_subject$.subscribe((res) => {
      //inicio el countdown para el patron user idle
      if (res === true) {
        this.reset();
      }

    });// sets an idle timeout of 5 seconds, for testing purposes.
    idle.setIdle(this.iddleTimeout_seconds);
    // sets a timeout period of 5 seconds. after 10 seconds of inactivity, the user will be considered timed out.
    idle.setTimeout(this.iddle_waite_Timeout_seconds);
    // sets the default interrupts, in this case, things like clicks, scrolls, touches to the document
    idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);

    //enent -> Ya no está inactivo
    idle.onIdleEnd.subscribe(() =>
        {// this.idleState = 'Ya no está inactivo.');
          this.idleState = ''
          //alert('Ya no está inactivo');
        }
      );


    idle.onTimeout.subscribe(() => {
      this.idleState = '';
      this.timedOut = true;
      if (this.authService.isAuth() === true) {
        this.authService.signOut();
        this.router.navigate(['/login']);
      }
    });

    //inicio de inactividad
    idle.onIdleStart.subscribe(
      () => {
      this.idleState = '';
        //this.idleState = 'Te has quedado inactivo'
      });

    //Estera estando inactivo 
    idle.onTimeoutWarning.subscribe((countdown) => 
            this.idleState = 'Su sessión expirará por inactividad  en ' + countdown + ' segundos!'
       );

    // sets the ping interval to 15 seconds
    keepalive.interval(15);

    keepalive.onPing.subscribe(() => this.lastPing = new Date());

    this.reset();
  }


  reset() {

    //Iniciar el patronidle solo si esta autenticado
    if(this.authService.isAuth())
    {
      this.idle.watch();
      this.idleState = '';//'Started.';
      this.timedOut = false;
    }
  }

}