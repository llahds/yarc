import { Component, OnInit } from '@angular/core';
import { ForumsService } from '../services/forums.service';
import { SimilarForum } from '../services/models/forums';

@Component({
  selector: 'app-popular-forums',
  templateUrl: './popular-forums.component.html',
  styleUrls: ['./popular-forums.component.scss']
})
export class PopularForumsComponent implements OnInit {
  public list: SimilarForum[] = [];

  constructor(
    private forums: ForumsService
  ) { }

  ngOnInit(): void {
    this.forums.getPopularForums().subscribe(r => this.list = r);
  }
}
