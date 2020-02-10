import { Component, OnInit } from '@angular/core';
import { AppConstants } from '../model/common.constants';
import { EpironBotTestService } from '../service/EpironBotTest.service';
import { BotCommentModeratedReq, ModeratedComment } from '../model/entities.models';



@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  public version  = '';
 public commentsToModerate:BotCommentModeratedReq[];

 public current_commentToModerate:BotCommentModeratedReq;

  constructor(  private svc: EpironBotTestService,){
    this.version = AppConstants.AppVersion;
  }

  ngOnInit() {

    this.retriveData();
  }


  public executeSelectedChange = (event) => {
    console.log(event);
  }

  retriveData(){

    this.svc.Searchcomments$().subscribe(res=>{
      this.commentsToModerate=res;
    });
  }

  responseModeratedBotComment(){

    let moderatedComment:ModeratedComment= new ModeratedComment();
    moderatedComment.CaseId=  this.current_commentToModerate.CaseId;
    moderatedComment.CaseCommentGUID=  this.current_commentToModerate.CaseCommentGUID;
    //moderatedComment.Action =  this.Action;
    //moderatedComment.Text =  this.Text;

    this.svc.responseModeratedBotComment$(moderatedComment).subscribe(res=>{
      
    });
  }
  getRecord(row){
    alert(row);
  }

 
}

const ELEMENT_DATA: BotCommentModeratedReq[] = [];

export class TableBasicExample {
  displayedColumns: string[] = ['CaseId', 'CaseCommentGUID', 'SCInternalCode', 'ElementTypePublic','CaseCommentTextSent','ArrivedDate'];
  dataSource = ELEMENT_DATA;
}