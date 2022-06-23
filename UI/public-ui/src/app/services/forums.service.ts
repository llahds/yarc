import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Id, KeyValueModel } from './models/common';
import { ForumEditModel, Forum, ForumPostSettings, ForumPostGuideLines, SimilarForum } from './models/forums';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class ForumsService {

  constructor(
    private client: RestApiService
  ) {

  }

  getForum(forumId: number): Observable<Forum> {
    return this.client.get(`api/1.0/forums/${forumId}`);
  }

  create(model: ForumEditModel): Observable<Id> {
    return this.client.post(`api/1.0/forums`, model);
  }

  update(id: number, model: ForumEditModel) {
    return this.client.put(`api/1.0/forums/${id}`, model);
  }

  remove(id: number) {
    return this.client.delete(`api/1.0/forums/${id}`);
  }

  getSettings(id: number) : Observable<ForumPostSettings> {
    return this.client.get(`api/1.0/moderation/forums/${id}/posts/settings`);
  }

  updateSettings(id: number, model: ForumPostSettings) {
    return this.client.put(`api/1.0/moderation/forums/${id}/posts/settings`, model);
  }

  getGuideLines(id: number) : Observable<ForumPostGuideLines> {
    return this.client.get(`api/1.0/forums/${id}/posts/guide-lines`);
  }

  suggestTopics(queryText: string) : Observable<KeyValueModel[]> {
    return this.client.get(`api/1.0/forums/topics/suggest?queryText=${queryText}`);
  }

  similarForums(id: number) : Observable<SimilarForum[]> {
    return this.client.get(`api/1.0/forums/${id}/similar`);
  }

  suggestUsers(queryText: string) : Observable<KeyValueModel[]> {
    return this.client.get(`api/1.0/forums/users/suggest?queryText=${queryText}`);
  }
}
