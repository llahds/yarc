import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ForumsService } from 'src/app/services/forums.service';
import { ForumPostSettings } from 'src/app/services/models/forums';

@Component({
  selector: 'app-forum-settings-posts',
  templateUrl: './forum-settings-posts.component.html',
  styleUrls: ['./forum-settings-posts.component.scss']
})
export class ForumSettingsPostsComponent implements OnInit {

  public entity: ForumPostSettings = { guideLines: "", requiredTitleWords: [], bannedTitleWords: [], postTextBannedWords: [], isDomainWhitelist: false, domains: [] };
  public forumId!: number;
  public isSaving: boolean = false;
  public isLoading: boolean = true;

  constructor(
    private forums: ForumsService,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.forumId = +this.route.snapshot.params["forumId"];
      this.forums.getSettings(this.forumId)
        .subscribe(r => {
          this.isLoading = false;
          this.entity = r;
        });
    });
  }

  update() {
    this.isSaving = true;
    this.forums.updateSettings(this.forumId, this.entity)
      .subscribe(r => this.isSaving = false);
  }
}
