import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Id } from './models/common';
import { ForumEditModel, Forum } from './models/forums';
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
}
