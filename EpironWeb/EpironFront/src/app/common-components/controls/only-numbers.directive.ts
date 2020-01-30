import { Directive, ElementRef } from '@angular/core';
import { Input, HostListener } from "@angular/core";

@Directive({
  selector: '[appOnlyNumbers]'
})
export class OnlyNumbersDirective {
  regexStr = '^[0-9]*$';
  constructor(private _el: ElementRef) { }
  @Input() appOnlyNumbers: boolean;

  @HostListener('input', ['$event']) onKeyDown(event) {
  {
    const initalValue = this._el.nativeElement.value;

    this._el.nativeElement.value = initalValue.replace(/[^0-9]*/g, '');
    if ( initalValue !== this._el.nativeElement.value) {
      event.stopPropagation();
    }

    // let e = <KeyboardEvent> event;
    
    // if (this.appOnlyNumbers) {
    //     if ([46, 8, 9, 27, 13, 110, 190].indexOf(e.keyCode) !== -1 ||
    //     // Allow: Ctrl+A
    //     (e.keyCode == 65 && e.ctrlKey === true) ||
    //     // Allow: Ctrl+C
    //     (e.keyCode == 67 && e.ctrlKey === true) ||
    //     // Allow: Ctrl+V
    //     (e.keyCode == 86 && e.ctrlKey === true) ||
    //     // Allow: Ctrl+X
    //     (e.keyCode == 88 && e.ctrlKey === true) ||
    //     // Allow: home, end, left, right
    //     (e.keyCode >= 35 && e.keyCode <= 39)) {
    //       // let it happen, don't do anything
    //       return;
    //     }
    //   let ch = String.fromCharCode(e.keyCode);
    //   let regEx =  new RegExp(this.regexStr);    
    //   if(regEx.test(ch))
    //     return;
    //   else
    //      e.preventDefault();
      }
 }
}
