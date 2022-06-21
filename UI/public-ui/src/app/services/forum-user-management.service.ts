import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UserInfo } from './models/users';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class ForumUserManagementService {

  constructor(
    private client: RestApiService
  ) { }

  get(forumId: number, startAt: number, query: string, status: number) : Observable<UserInfo[]> {
    return this.client.get(`api/1.0/moderation/forums/${forumId}/users?startAt=${startAt}&query=${query}&status=${status}`);
  }

  searchUsers(query: string) : Observable<UserInfo[]> {
    return this.client.get(`api/1.0/moderation/users?query=${query}`);
  }

  remove(forumId: number, userId: number) {
    return this.client.delete(`api/1.0/moderation/forums/${forumId}/users/${userId}`);
  }

  changeStatus(forumId: number, userId: number, status: number) {
    return this.client.post(`api/1.0/moderation/forums/${forumId}/users/${userId}/status`, { status: status });
  }
}
