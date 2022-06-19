import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Forum } from './models/forums';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class ForumsService {

  constructor(
    private client: RestApiService
  ) {

  }

  getForum(forumId: number) : Observable<Forum> {
    return this.client.get(`api/1.0/forums/${forumId}`);
  }
}
