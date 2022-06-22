import { Component, Input, OnInit } from '@angular/core';
import { Post } from '../services/models/posts';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-post-item',
  templateUrl: './post-item.component.html',
  styleUrls: ['./post-item.component.scss']
})
export class PostItemComponent implements OnInit {

  @Input() item!: Post;
  @Input() showForum: boolean = false;

  constructor(
    private posts: PostsService
  ) { }

  ngOnInit(): void {
  }

  up() {
    this.posts.up(this.item.forum.id, this.item.id).subscribe();
  }

  down() {
    this.posts.down(this.item.forum.id, this.item.id).subscribe();
  }
}
