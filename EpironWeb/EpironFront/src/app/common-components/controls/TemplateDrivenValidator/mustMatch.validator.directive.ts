// The custom [mustMatch] directive wraps the custom MustMatch validator so we can attach it to the form. 
// A custom validator directive is required when using template-driven forms because 
// we don't have direct access to the FormGroup like in reactive forms.

import { Directive, Input } from '@angular/core';
import { NG_VALIDATORS, Validator, ValidationErrors, FormGroup } from '@angular/forms';
import { MustMatch } from './mustMatch.validator';



@Directive({
    selector: '[mustMatch]',
    providers: [{ provide: NG_VALIDATORS, useExisting: MustMatchDirective, multi: true }]
})
export class MustMatchDirective implements Validator {
    @Input('mustMatch') mustMatch: string[] = [];

    validate(formGroup: FormGroup): ValidationErrors {
        return MustMatch(this.mustMatch[0], this.mustMatch[1])(formGroup);
    }
}