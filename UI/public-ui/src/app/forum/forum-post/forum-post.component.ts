import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forum-post',
  templateUrl: './forum-post.component.html',
  styleUrls: ['./forum-post.component.scss']
})
export class ForumPostComponent implements OnInit {

  public showRemoveModal: boolean = false;
  public showSpamModal: boolean = false;
  public showReportModal: boolean = false;
  public showPostModal: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  confirmRemove() {
    this.showRemoveModal = true;
  }

  confirmSpam() {
    this.showSpamModal = true;
  }

  confirmReport() {
    this.showReportModal = true;
  }

}
