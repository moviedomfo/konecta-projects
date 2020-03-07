import { AbstractControl, ValidatorFn } from '@angular/forms';


// this.form = this.fb.group({
//     loadDate: null,
//     deliveryDate: null,
// }, { validator: Validators.compose([
//     DateValidators.dateLessThan('loadDate', 'deliveryDate', { 'loaddate': true }),
//     DateValidators.dateLessThan('cargoLoadDate', 'cargoDeliveryDate', { 'cargoloaddate': true })
// ])});

//Now you can use the validation in HTML.
// <md-error *ngIf="form.hasError('loaddate')">Load date must be before delivery date</md-error>



export class DateValidators {
  static dateLessThan(dateField1: string, dateField2: string, validatorField: { [key: string]: boolean }): ValidatorFn {
      return (c: AbstractControl): { [key: string]: boolean } | null => {
          const date1 = c.get(dateField1).value;
          const date2 = c.get(dateField2).value;
          if ((date1  && date2 ) && date1 > date2) {
              return validatorField;
          }
          return null;
      };
  }


  static dateMoreThan(dateControlDateName: string, dateThanControlName: string, validatorField: { [key: string]: boolean }): ValidatorFn {
    return (c: AbstractControl): { [key: string]: boolean } | null => {
        const control = c.get(dateControlDateName).value;
        const thanControl = c.get(dateThanControlName).value;
        // return null if controls haven't initialised yet
        if (!control || !thanControl) {
              return null;//INVALID
        }
        const date1 = c.get(dateControlDateName).value;
        const dateMoreThan = c.get(dateThanControlName).value;
        //VALID
        if ((date1 && dateMoreThan) && date1 > dateMoreThan) {
            return validatorField;
        }
        return null;
    };
}
}