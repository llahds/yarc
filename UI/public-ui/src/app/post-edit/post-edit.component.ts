import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ForumsService } from '../services/forums.service';
import { Id } from '../services/models/common';
import { EditPost } from '../services/models/posts';
import { PostsService } from '../services/posts.service';

@Component({
  selector: 'app-post-edit',
  templateUrl: './post-edit.component.html',
  styleUrls: ['./post-edit.component.scss']
})
export class PostEditComponent implements OnInit {

  public entity: EditPost = { title: "", text: "" };;
  public errors: any = {};
  public isSaving: boolean = false;
  public guidelines: string = "";

  //@Input() forumId!: number;
  private _forumId!: number;
  private _postId!: number | undefined;
  @Input() showPost: boolean = false;

  @Output() onCancel = new EventEmitter<void>();
  @Output() onPost = new EventEmitter<number>();

  constructor(
    private posts: PostsService,
    private forums: ForumsService
  ) { }

  ngOnInit(): void {
  }

  cancel() {
    this.errors = {};
    this.isSaving = false;
    if (!this._postId) {
      this.entity = { title: "", text: "" };
    }
    this.onCancel.emit();
  }

  post() {
    this.errors = {};
    this.isSaving = true;
    if (this._postId) {
      this.update();
    } else {
      this.create();
    }
  }

  create() {
    this.errors = {};
    this.isSaving = true;
    this.posts.create(this.forumId, this.entity).subscribe(r => {
      this.isSaving = false;
      this.entity = { title: "", text: "" };
      this.onPost.emit(r.id);
    }, e => {
      this.errors = e.error;
      this.isSaving = false;
    });
  }

  update() {
    this.errors = {};
    this.isSaving = true;
    this.posts.update(this.forumId, this._postId!, this.entity).subscribe(r => {
      this.isSaving = false;
      this.onPost.emit(this._postId);
    }, e => {
      this.errors = e.error;
      this.isSaving = false;
    });
  }

  @Input() set postId(value: number | undefined) {
    if (value) {
      this.posts.getView(this.forumId, value).subscribe(r => this.entity = r);
    }

    this._postId = value;
  }

  @Input() set forumId(value: number) {
    if (value) {
      this.forums.getGuideLines(value).subscribe(r => this.guidelines = r.guideLines);
    }

    this._forumId = value;
  }

  get forumId() {
    return this._forumId;
  }
}
