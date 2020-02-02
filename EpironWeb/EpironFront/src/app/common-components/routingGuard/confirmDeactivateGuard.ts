
import { Observable } from 'rxjs';
import {CanDeactivate} from '@angular/router';
export interface CanDeactivateComponent {
  
  hasChanges: () => Observable<boolean> | Promise<boolean> | boolean;

 }
 
 export class ConfirmDeactivateGuard implements CanDeactivate<CanDeactivateComponent> {

   canDeactivate(target: CanDeactivateComponent) {
     if(target.hasChanges()){
         return window.confirm('Realmente quiere salir de esta página?');
     }
     return true;
   }
 }