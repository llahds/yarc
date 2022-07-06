import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { concat, map, merge, of, reduce } from 'rxjs';
import { Post } from 'src/app/services/models/posts';
import { PostsService } from 'src/app/services/posts.service';

@Component({
  selector: 'app-forum-list',
  templateUrl: './forum-list.component.html',
  styleUrls: ['./forum-list.component.scss']
})
export class ForumListComponent implements OnInit {

  private static _sortBy: string = "";

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
      this.sortBy = params.get("sort") || ForumListComponent._sortBy || "top";
      this.refresh();
    });

    this.route.paramMap.subscribe(params => {
      this.id = +this.route.snapshot.params['forumId'];
      this.refresh();
    });
  }

  refresh() {
    if (this.id) {
      this.isLoading = true;
      this.posts.getPosts(this.id, 0, this.sortBy).subscribe(r => {
        this.list = r.list;
        this.isLoading = false;
        this.sortBy = r.sortBy;
        ForumListComponent._sortBy = r.sortBy;
      });
    }
  }
}
