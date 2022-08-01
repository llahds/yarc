import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { Post } from '../services/models/posts';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.scss']
})
export class FeedComponent implements OnInit {

  public list: Post[] = [];
  public isLoading: boolean = false;
  public isAuthenticated: boolean = false;
  public sortBy: string = "";
  public startAt: number = 0;
  public pageSize!: number;
  public total!: number;

  private view: string = "popular";

  constructor(
    private posts: PostsService,
    private authentication: AuthenticationService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.authentication.onSignOut.subscribe(r => this.isAuthenticated = false);

    this.authentication.onNewSignIn.subscribe(r => this.isAuthenticated = true);

    this.isAuthenticated = !!this.authentication.token;
  }

  ngOnInit(): void {
    this.route.data.subscribe(d => {
      this.view = d["view"];
    });

    this.route.queryParamMap.subscribe(params => {
      this.isLoading = true;
      this.sortBy = params.get('sort') || "";
      this.startAt = +params.get("startAt")! || 0;
      
      if (this.view === "popular") {
        this.posts.getPopularPosts(this.startAt, this.sortBy).subscribe(r => {
          this.list = r.list;
          this.sortBy = r.sortBy;
          this.isLoading = false;
          this.pageSize = r.pageSize;
          this.total = r.total;
        });
      } else if (this.view === "recommended") {
        this.posts.getRecommendedPosts(this.startAt).subscribe(r => {
          this.list = r.list;
          this.sortBy = "";
          this.isLoading = false;
          this.pageSize = r.pageSize;
          this.total = r.total;
        });
      }
    })
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
