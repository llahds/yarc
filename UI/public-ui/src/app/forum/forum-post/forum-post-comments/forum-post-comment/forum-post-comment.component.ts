import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-forum-post-comment',
  templateUrl: './forum-post-comment.component.html',
  styleUrls: ['./forum-post-comment.component.scss']
})
export class ForumPostCommentComponent implements OnInit {

  public showReply: boolean = false;
  public showChildren: boolean = false;
  @Input() depth: number = 0;

  constructor() { }

  ngOnInit(): void {
  }

}
