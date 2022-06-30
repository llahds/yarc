import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthenticationService } from '../services/authentication.service';
import { Post } from '../services/models/posts';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-popular',
  templateUrl: './popular.component.html',
  styleUrls: ['./popular.component.scss']
})
export class PopularComponent implements OnInit {

  public list: Post[] = [];
  public isLoading: boolean = false;
  public isAuthenticated: boolean = false;
  public sortBy: string = "";

  constructor(
    private posts: PostsService,
    private authentication: AuthenticationService,
    private route: ActivatedRoute
  ) {
    this.authentication.onSignOut.subscribe(r => this.isAuthenticated = false);

    this.authentication.onNewSignIn.subscribe(r => this.isAuthenticated = true);

    this.isAuthenticated = !!this.authentication.token;
  }

  ngOnInit(): void {
    this.route.queryParamMap.subscribe(params => {
      this.isLoading = true;
      this.sortBy = params.get('sort') || "";
      this.posts.getPopularPosts(0, this.sortBy).subscribe(r => {
        this.list = r.list;
        this.sortBy = r.sortBy;
        this.isLoading = false;
      });
    })

  }

}
