// The custom Date more than  validator is used to validate that both of the password fields 



import { FormGroup } from '@angular/forms';

// custom validator to check that two fields match
export function DateMoreThan(controlName: string, thanControlName: string) {
    return (formGroup: FormGroup) => {
        const control = formGroup.controls[controlName];
        const thanControl = formGroup.controls[thanControlName];

        // return null if controls haven't initialised yet
        if (!control || !thanControl) {
            return null;
          }
        // return if another validator has already found an error on the matchingControl
        if (control.errors && !control.errors.mustBeMoreThan) {
            return;
        }

        // set error on matchingControl if validation fails
        if (new Date(control.value) < new Date(thanControl.value)) {
            control.setErrors({ mustBeMoreThan: true });
          } else {
            control.setErrors(null);
          }
    }
}


import { NG_VALIDATORS, Validator, ValidationErrors } from '@angular/forms';
import { Directive, Input } from '@angular/core';



@Directive({
    selector: '[dateMoreThan]',
    providers: [{ provide: NG_VALIDATORS, useExisting: DateMoreThanDirective, multi: true }]
})
export class DateMoreThanDirective implements Validator {
    @Input('dateMoreThan') dateMoreThan: string[] = [];

    validate(formGroup: FormGroup): ValidationErrors {
        return DateMoreThan(this.dateMoreThan[0], this.dateMoreThan[1])(formGroup);
    }
}