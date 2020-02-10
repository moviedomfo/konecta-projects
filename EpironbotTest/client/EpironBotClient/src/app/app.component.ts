import { Component } from '@angular/core';
import { AppConstants } from './model/common.constants';
import { EpironBotTestService } from './service/EpironBotTest.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  public version  = '';

 
}
