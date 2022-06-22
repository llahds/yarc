import { Component, Input, OnInit } from '@angular/core';
import { Comment, Post } from 'src/app/services/models/posts';
import { QueueListWorkItem } from 'src/app/services/models/reporting';

@Component({
  selector: 'app-mod-post-item',
  templateUrl: './mod-post-item.component.html',
  styleUrls: ['./mod-post-item.component.scss']
})
export class ModPostItemComponent implements OnInit {

  @Input() item!: QueueListWorkItem;
  @Input() forumId!: number;
  @Input() reasons: string[] = [];
  
  constructor() { }

  ngOnInit(): void {
  }

}
