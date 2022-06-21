import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ForumUserManagementService } from 'src/app/services/forum-user-management.service';
import { FORUM_STATUS_BANNED, UserInfo } from 'src/app/services/models/users';

@Component({
  selector: 'app-forum-settings-banned',
  templateUrl: './forum-settings-banned.component.html',
  styleUrls: ['./forum-settings-banned.component.scss']
})
export class ForumSettingsBannedComponent implements OnInit {

  public showBanModal: boolean = false;

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
    this.users.get(this.forumId, 0, this.query, FORUM_STATUS_BANNED)
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

  ban(user: UserInfo) {
    this.showBanModal = false
    this.users.changeStatus(this.forumId, user.id, FORUM_STATUS_BANNED)
      .subscribe(r => this.reload());
  }
}
