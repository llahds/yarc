import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { QueueListWorkItem, ReportingReason } from './models/reporting';
import { RestApiService } from './rest-api.service';

@Injectable({
  providedIn: 'root'
})
export class ReportingService {

  constructor(
    private client: RestApiService
  ) { }

  getRules() : Observable<ReportingReason[]>{
    return this.client.get(`api/1.0/reporting/reasons`);
  }

  report(postId: number, reasonId: number) {
    return this.client.post(`api/1.0/reporting/posts`, { reasonId: reasonId, postId: postId });
  }

  getQueueWorkItems(forumId: number, startAt: number, reasonIds: number[]) : Observable<QueueListWorkItem[]> {
    reasonIds = reasonIds || [];
    return this.client.get(`api/1.0/moderation/${forumId}/queue?reasonIds=${reasonIds.join(",")}`);
  }

  approve(forumId: number, postId: number) {
    return this.client.post(`api/1.0/moderation/${forumId}/queue/${postId}/approve`, {});
  }

  reject(forumId: number, postId: number) {
    return this.client.post(`api/1.0/moderation/${forumId}/queue/${postId}/reject`, {});
  }
}
