import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ForumsService } from '../services/forums.service';
import { Forum } from '../services/models/forums';

@Component({
  selector: 'app-forum',
  templateUrl: './forum.component.html',
  styleUrls: ['./forum.component.scss']
})
export class ForumComponent implements OnInit {

  public showPostModal: boolean = false;
  public forum!: Forum;
  public id!: number;

  constructor(
    private route: ActivatedRoute,
    private api: ForumsService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      this.id = +this.route.snapshot.params['forumId'];
      this.api.getForum(this.id).subscribe(r => this.forum = r);
    });
  }

  createPost() {
    this.showPostModal = true
  }

  remove() {
    this.api.remove(this.id).subscribe(r => this.router.navigateByUrl("/"));
  }
}
