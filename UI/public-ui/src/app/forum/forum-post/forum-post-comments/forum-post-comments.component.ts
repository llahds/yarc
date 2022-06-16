import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-forum-post-comments',
  templateUrl: './forum-post-comments.component.html',
  styleUrls: ['./forum-post-comments.component.scss']
})
export class ForumPostCommentsComponent implements OnInit {

  @Input() public depth: number = 0;

  constructor() { }

  ngOnInit(): void {
  }

}
