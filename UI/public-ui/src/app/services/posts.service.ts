import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Id, ListResult } from './models/common';
import { Comment, EditPost, Post } from './models/posts';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class PostsService {
  constructor(
    private client: RestApiService
  ) {

  }

  getPopularPosts(startAt: number, sort: string): Observable<ListResult<Post>> {
    return this.client.get(`api/1.0/forums/posts/popular?startAt=${startAt}&sort=${sort}`);
  }

  getPosts(forumId: number, startAt: number, sort: string): Observable<ListResult<Post>> {
    return this.client.get(`api/1.0/forums/${forumId}/posts?startAt=${startAt}&sort=${sort}`);
  }

  getView(forumId: number, postId: number): Observable<Post> {
    return this.client.get(`api/1.0/forums/${forumId}/posts/${postId}`);
  }

  create(forumId: number, model: EditPost): Observable<Id> {
    return this.client.post(`api/1.0/forums/${forumId}/posts`, model);
  }

  update(forumId: number, postId: number, model: EditPost): Observable<Id> {
    return this.client.put(`api/1.0/forums/${forumId}/posts/${postId}`, model);
  }

  remove(forumId: number, postId: number): Observable<Id> {
    return this.client.delete(`api/1.0/forums/${forumId}/posts/${postId}`);
  }

  getComments(forumId: number, postId: number, parentId: number | undefined): Observable<Comment[]> {
    return this.client.get(`api/1.0/forums/${forumId}/posts/${postId}/comments?parentId=${parentId}`);
  }

  createComment(forumId: number, postId: number, text: string, parentId: number | undefined): Observable<Comment> {
    if (!parentId) {
      return this.client.post(`api/1.0/forums/${forumId}/posts/${postId}/comments`, { text: text })
    } else {
      return this.client.post(`api/1.0/forums/${forumId}/posts/${postId}/comments/${parentId}`, { text: text })
    }
  }

  updateComment(forumId: number, postId: number, commentId: number, text: string) { 
    return this.client.put(`api/1.0/forums/${forumId}/posts/${postId}/comments/${commentId}`, { text: text });
  }

  upvotePost(forumId: number, postId: number) {
    return this.client.post(`api/1.0/forums/${forumId}/posts/${postId}/up`, {  });
  }

  downvotePost(forumId: number, postId: number) {
    return this.client.post(`api/1.0/forums/${forumId}/posts/${postId}/down`, {  });
  }

  upvoteComment(forumId: number, postId: number, commentId: number) {
    return this.client.post(`api/1.0/forums/${forumId}/posts/${postId}/comments/${commentId}/up`, {  });
  }

  downvoteComment(forumId: number, postId: number, commentId: number) {
    return this.client.post(`api/1.0/forums/${forumId}/posts/${postId}/comments/${commentId}/down`, {  });
  }  

  removeComment(forumId: number, postId: number, commentId: number) {
    return this.client.delete(`api/1.0/forums/${forumId}/posts/${postId}/comments/${commentId}`);
  }
}
