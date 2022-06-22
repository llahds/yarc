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

  getReportingReasons() : Observable<ReportingReason[]>{
    return this.client.get(`api/1.0/reporting/reasons`);
  }

  reportPost(postId: number, reasonId: number) {
    return this.client.post(`api/1.0/reporting/posts`, { reasonId: reasonId, postId: postId });
  }

  reportComment(commentId: number, reasonId: number) {
    return this.client.post(`api/1.0/reporting/comments`, { reasonId: reasonId, commentId: commentId });
  }

  getQueueWorkItems(forumId: number, startAt: number, reasonIds: number[]) : Observable<QueueListWorkItem[]> {
    reasonIds = reasonIds || [];
    return this.client.get(`api/1.0/moderation/${forumId}/queue?reasonIds=${reasonIds.join(",")}`);
  }

  approvePost(forumId: number, postId: number) {
    return this.client.post(`api/1.0/moderation/${forumId}/queue/posts/${postId}/approve`, {});
  }

  rejectPost(forumId: number, postId: number) {
    return this.client.post(`api/1.0/moderation/${forumId}/queue/posts/${postId}/reject`, {});
  }

  approveComment(forumId: number, commentId: number) {
    return this.client.post(`api/1.0/moderation/${forumId}/queue/comments/${commentId}/approve`, {});
  }

  rejectComment(forumId: number, commentId: number) {
    return this.client.post(`api/1.0/moderation/${forumId}/queue/comments/${commentId}/reject`, {});
  }
}
