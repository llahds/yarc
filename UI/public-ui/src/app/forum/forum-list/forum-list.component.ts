import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Post } from 'src/app/services/models/posts';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-forum-list',
  templateUrl: './forum-list.component.html',
  styleUrls: ['./forum-list.component.scss']
})
export class ForumListComponent implements OnInit {

  public showPostModal: boolean = false;
  public list: Post[] = [];

  constructor(
    private posts: PostsService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const id = +this.route.snapshot.params['id'];
      this.posts.getPosts(id, 0).subscribe(r => this.list = r);
    });
  }
}
