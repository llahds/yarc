import { Component, Input, OnInit } from '@angular/core';
import { Post } from '../services/models/posts';

@Component({
  selector: 'app-post-list',
  templateUrl: './post-list.component.html',
  styleUrls: ['./post-list.component.scss']
})
export class PostListComponent implements OnInit {

  @Input() list: Post[] = [];
  @Input() showForum: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

}
