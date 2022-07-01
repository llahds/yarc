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

  public list: Post[] = [];
  public id!: number;
  public postId!: number;
  public isLoading: boolean = false;
  public sortBy: string = "";

  constructor(
    private posts: PostsService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.isLoading = true;

      if (this.id) {
        this.sortBy = params.get("sort") || "top";
        this.posts.getPosts(this.id, 0, this.sortBy).subscribe(r => {
          this.list = r.list;
          this.isLoading = false;
          this.sortBy = r.sortBy;
        });
      }
    });

    this.route.paramMap.subscribe(params => {
      this.isLoading = true;
      this.id = +this.route.snapshot.params['forumId'];
      this.posts.getPosts(this.id, 0, this.sortBy).subscribe(r => {
        this.list = r.list;
        this.isLoading = false;
        this.sortBy = r.sortBy;
      });
    });
  }
}
