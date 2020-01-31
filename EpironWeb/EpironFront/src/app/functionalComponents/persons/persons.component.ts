import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { Observable } from 'rxjs';
import { PersonBE } from '../../model/index';
import { CommonService, PersonsService } from '../../service/index';

@Component({
  selector: 'app-persons',
  templateUrl: './persons.component.html'
  
})
export class PersonsComponent implements OnInit {

  public currentPerson:PersonBE;
  public personId :number;

  constructor( 
private personsService :PersonsService,  private route: ActivatedRoute) 
  { 

  }


  ngOnInit() {
    // console.log("-------------------------------------");
    // // console.log(JSON.stringify( this.route.url));
    // console.log(this.route);
     this.personId= this.route.snapshot.params['id'];
     
     this.personsService.getPersonaByParamService$(this.personId.toString(),null,null)
    .subscribe(
       res => {
        
        this.currentPerson= res;
        
       }
     );

    // alert(this.personId);
    // console.log("-------------------------------------");
    // this.persona$ = this.route.paramMap
    // .switchMap((params: ParamMap) => {
    //   // (+) before `params.get()` turns the string into a number
    //   this.selectedId = + params.get('id');
    //   ParientBE patienBe = this.patientsService.getPatientById(this.selectedId);
    //   persona.id=  patienBe.id;
    // });

    
  }

}
