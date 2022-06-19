import { Component, Input, OnInit } from '@angular/core';
import { Post } from '../services/models/posts';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrls: ['./post-item.component.scss']
})
export class PostItemComponent implements OnInit {

  @Input() item!: Post;
  @Input() showForum: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

}
