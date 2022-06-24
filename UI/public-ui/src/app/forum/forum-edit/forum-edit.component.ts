import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ForumsService } from 'src/app/services/forums.service';
import { ForumEditModel } from 'src/app/services/models/forums';

@Component({
  selector: 'app-forum-edit',
  templateUrl: './forum-edit.component.html',
  styleUrls: ['./forum-edit.component.scss']
})
export class ForumEditComponent implements OnInit {

  public entity: ForumEditModel = { name: "", description: "", slug: "", topics: [], moderators: [], isPrivate: false };
  public suggestedTopics: any[] = [];
  public suggestedModerators: any[] = [];
  public id!: number;
  public errors: any = {};
  public isSaving: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private forums: ForumsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.id = +params.get('forumId')!;
      if (this.id) {
        this.forums.getForum(this.id).subscribe(r => {
          this.entity = r;
        });
      }
    });
  }

  searchTopics(query: string) {
    this.suggestedTopics = [];
    if (query) {
      this.forums.suggestTopics(query).subscribe(r => {
        let s = r.filter((f: any) => this.entity.topics.find((m: any) => m.id === f.id) === undefined);
        this.suggestedTopics = s;
      });
    }
  }

  searchModerators(query: string) {
    this.suggestedModerators = [];
    if (query) {
      this.forums.suggestUsers(query).subscribe(r => {
        let s = r.filter((f: any) => this.entity.moderators.find((m: any) => m.id === f.id) === undefined);
        this.suggestedModerators = s;
      });
    }
  }

  save() {
    this.errors = {};
    this.isSaving = true;
    if (this.id > 0) {
      this.update();
    } else {
      this.create();
    }
  }

  create() {
    this.errors = {};
    this.isSaving = true;
    this.forums.create(this.entity).subscribe(r => {
      this.isSaving = false;
      this.entity = { name: "", description: "", slug: "", topics: [], moderators: [], isPrivate: false };
      this.router.navigateByUrl("/r/" + r.id);
    }, e => {
      this.errors = e.error;
      this.isSaving = false;
    });
  }

  update() {
    this.errors = {};
    this.isSaving = true;
    this.forums.update(this.id, this.entity).subscribe(r => {
      this.isSaving = false;
      this.entity = { name: "", description: "", slug: "", topics: [], moderators: [], isPrivate: false };
      this.router.navigateByUrl("/r/" + this.id);
    }, e => {
      this.errors = e.error;
      this.isSaving = false;
    });
  }

  cancel() {
    if (this.id) {
      this.router.navigateByUrl("/r/" + this.id);
    } else {
      this.router.navigateByUrl("/");
    }
  }

  get hasInvalidData() {
    return !this.entity.name
      || !this.entity.slug;
  }
}
