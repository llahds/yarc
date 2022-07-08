import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  public startAt: number = 0;
  public pageSize!: number;
  public total!: number;

  constructor(
    private posts: PostsService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {

    this.route.queryParamMap.subscribe(params => {
      this.sortBy = params.get("sort") || ForumListComponent._sortBy || "top";
      this.startAt = +params.get("startAt")! || 0;
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
      this.posts.getPosts(this.id, this.startAt, this.sortBy).subscribe(r => {
        this.list = r.list;
        this.isLoading = false;
        this.sortBy = r.sortBy;
        ForumListComponent._sortBy = r.sortBy;
        this.pageSize = r.pageSize;
        this.total = r.total;        
      });
    }
  }

  first() {
    let urlTree = this.router.parseUrl(this.router.url);
    urlTree.queryParams["startAt"] = 0;
    this.router.navigateByUrl(urlTree);
  }

  previous() {
    let urlTree = this.router.parseUrl(this.router.url);
    urlTree.queryParams["startAt"] = this.startAt - this.pageSize;
    this.router.navigateByUrl(urlTree);
  }

  next() {
    let urlTree = this.router.parseUrl(this.router.url);
    urlTree.queryParams["startAt"] = this.startAt + this.pageSize;
    this.router.navigateByUrl(urlTree);
  }
}
