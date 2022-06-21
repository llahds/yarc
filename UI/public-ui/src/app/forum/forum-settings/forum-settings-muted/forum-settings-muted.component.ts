import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ForumUserManagementService } from 'src/app/services/forum-user-management.service';
import { FORUM_STATUS_BANNED, FORUM_STATUS_MUTED, UserInfo } from 'src/app/services/models/users';

@Component({
  selector: 'app-forum-settings-muted',
  templateUrl: './forum-settings-muted.component.html',
  styleUrls: ['./forum-settings-muted.component.scss']
})
export class ForumSettingsMutedComponent implements OnInit {

  public showMuteModal: boolean = false;

  public list: UserInfo[] = [];
  public forumId!: number;
  public isLoading: boolean = false;
  public query: string = "";

  constructor(
    private users: ForumUserManagementService,
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
    this.users.get(this.forumId, 0, this.query, FORUM_STATUS_MUTED)
      .subscribe(r => {
        this.list = r;
        this.isLoading = false;
      });
  }

  remove(id: number) {
    this.isLoading = true;
    this.users.remove(this.forumId, id)
      .subscribe(r => this.reload());
  }

  mute(user: UserInfo) {
    this.showMuteModal = false
    this.users.changeStatus(this.forumId, user.id, FORUM_STATUS_MUTED)
      .subscribe(r => this.reload());
  }

}
