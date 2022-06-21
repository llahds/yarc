import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { PostsService } from 'src/app/services/posts.service';
import { Post } from 'src/app/services/models/posts';
import { ReportingReason, SPAM_ID } from 'src/app/services/models/reporting';
import { ReportingService } from 'src/app/services/reporting.service';

@Component({
  selector: 'app-forum-post',
  templateUrl: './forum-post.component.html',
  styleUrls: ['./forum-post.component.scss']
})
export class ForumPostComponent implements OnInit {

  public showRemoveModal: boolean = false;
  public showSpamModal: boolean = false;
  public showReportModal: boolean = false;
  public showPostModal: boolean = false;

  public forumId!: number;
  public item!: Post;
  public postId!: number;

  public reportingReasons: ReportingReason[] = [];
  public selectedReportReasonId!: number;
  public isReporting: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private api: PostsService,
    private reporting: ReportingService
  ) { }

  ngOnInit(): void {
    this.reporting.getRules()
      .subscribe(r => this.reportingReasons = r);

    this.route.paramMap.subscribe(params => {
      this.forumId = +this.route.snapshot.params["forumId"];
      const id = +this.route.snapshot.params["postId"];
      this.api.getView(this.forumId, id).subscribe(r => this.item = r);
    });
  }

  confirmRemove() {
    this.showRemoveModal = true;
  }

  confirmSpam() {
    this.showSpamModal = true;
  }

  confirmReport() {
    this.showReportModal = true;
  }

  editPost() {
    this.postId = this.item.id;
    this.showPostModal = true    
  }

  reload() {
    this.api.getView(this.forumId, this.item.id).subscribe(r => this.item = r);
    this.postId = 0;
    this.showPostModal = false;
  }

  report() {
    this.isReporting = true;
    this.reporting
      .report(this.item.id, this.selectedReportReasonId)
      .subscribe(r => {
        this.isReporting = false;
        this.showReportModal = false;
        this.item.canReport = false;
      });
  }

  spam() {
    this.isReporting = true;
    this.reporting
      .report(this.item.id, SPAM_ID)
      .subscribe(r => {
        this.isReporting = false;
        this.showSpamModal = false;
        this.item.canReport = false;
      });    
  }
}
