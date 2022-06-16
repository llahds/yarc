import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-forum-edit',
  templateUrl: './forum-edit.component.html',
  styleUrls: ['./forum-edit.component.scss']
})
export class ForumEditComponent implements OnInit {

  entity: any = { topics: [], moderators: [] };
  suggestedTopics: any[] = [];
  suggestedModerators: any[] = [];

  constructor() { }

  ngOnInit(): void {
  }

  searchTopics(query: string) {
    this.suggestedTopics = [];
    if (query) {
      this.suggestedTopics = [
        { id: 1, name: "topic 1" },
        { id: 2, name: "topic 2" },
      ]
      // this.api.suggestTopics(query).subscribe(r => {
      //   let s = r.filter((f: any) => this.entity.topics.find((m: any) => m.id === f.id) === undefined);
      //   this.suggestedTopics = s;
      // });
    }
  }

  searchModerators(query: string) {
    this.suggestedModerators = [];
    if (query) {
      this.suggestedModerators = [
        { id: 1, name: "moderator 1" },
        { id: 2, name: "moderator 2" },
      ]
      // this.api.suggestTopics(query).subscribe(r => {
      //   let s = r.filter((f: any) => this.entity.topics.find((m: any) => m.id === f.id) === undefined);
      //   this.suggestedTopics = s;
      // });
    }
  }
}
