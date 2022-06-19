import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Post } from './models/posts';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  constructor(
    private client: RestApiService
  ) {

  }

  getPopularPosts(startAt: number) : Observable<Post[]> {
    return this.client.get(`api/1.0/forums/posts/popular?startAt=${startAt}`);
  }

  getPosts(forumId: number, startAt: number) : Observable<Post[]> {
    return this.client.get(`api/1.0/forums/${forumId}/posts?startAt=${startAt}`);
  }

  getView(forumId: number, postId: number) : Observable<Post> {
    return this.client.get(`api/1.0/forums/${forumId}/posts/${postId}`);
  }
}
