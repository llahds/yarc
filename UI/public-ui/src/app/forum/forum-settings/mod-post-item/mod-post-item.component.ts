import { Component, Input, OnInit } from '@angular/core';
import { Post } from 'src/app/services/models/posts';

@Component({
  selector: 'app-mod-post-item',
  templateUrl: './mod-post-item.component.html',
  styleUrls: ['./mod-post-item.component.scss']
})
export class ModPostItemComponent implements OnInit {

  @Input() item!: Post;
  @Input() forumId!: number;
  @Input() reasons: string[] = [];
  
  constructor() { }

  ngOnInit(): void {
  }

}
