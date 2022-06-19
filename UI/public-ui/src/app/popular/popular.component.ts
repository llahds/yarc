import { Component, OnInit } from '@angular/core';
import { Post } from '../services/models/posts';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-popular',
  templateUrl: './popular.component.html',
  styleUrls: ['./popular.component.scss']
})
export class PopularComponent implements OnInit {

  public list: Post[] = [];

  constructor(
    private posts: PostsService
  ) { }

  ngOnInit(): void {
    this.posts.getPopularPosts(0).subscribe(r => this.list = r);
  }

}
