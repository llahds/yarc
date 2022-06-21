import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { QueueListWorkItem, SPAM_ID } from 'src/app/services/models/reporting';
import { ReportingService } from 'src/app/services/reporting.service';

@Component({
  selector: 'app-forum-settings-spam',
  templateUrl: './forum-settings-spam.component.html',
  styleUrls: ['./forum-settings-spam.component.scss']
})
export class ForumSettingsSpamComponent implements OnInit {

  public list: QueueListWorkItem[] = [];
  public forumId!: number;
  public isLoading: boolean = false;

  constructor(
    private reporting: ReportingService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.forumId = +this.route.snapshot.params["forumId"];
      this.reload();
    });
  }

  reload() {
    this.isLoading = true;
    this.reporting.getQueueWorkItems(this.forumId, 0, [SPAM_ID])
      .subscribe(r => {
        this.list = r;
        this.isLoading = false;
      });
  }

  approve(id: number) {
    this.isLoading = true;
    this.reporting.approve(this.forumId, id).subscribe(() => this.reload());
  }

  reject(id: number) {
    this.isLoading = true;
    this.reporting.reject(this.forumId, id).subscribe(() => this.reload());
  }

}
