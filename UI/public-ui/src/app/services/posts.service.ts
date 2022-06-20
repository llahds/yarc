import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Id } from './models/common';
import { EditPost, Post } from './models/posts';
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

  create(forumId: number, model: EditPost) : Observable<Id> {
    return this.client.post(`api/1.0/forums/${forumId}/posts`, model);
  }

  update(forumId: number, postId: number, model: EditPost) : Observable<Id> {
    return this.client.put(`api/1.0/forums/${forumId}/posts/${postId}`, model);
  }
}
