import { Component, OnInit } from '@angular/core';
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

  constructor(
    private posts: PostsService,
    private authentication: AuthenticationService
  ) {
    this.authentication.onSignOut.subscribe(r => this.isAuthenticated = false);

    this.authentication.onNewSignIn.subscribe(r => this.isAuthenticated = true);

    this.isAuthenticated = !!this.authentication.token;
  }

  ngOnInit(): void {
    this.isLoading = true;
    this.posts.getPopularPosts(0).subscribe(r => {
      this.list = r;
      this.isLoading = false;
    });
  }

}
