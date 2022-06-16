import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-post-edit',
  templateUrl: './post-edit.component.html',
  styleUrls: ['./post-edit.component.scss']
})
export class PostEditComponent implements OnInit {

  @Input() showPost: boolean = false;

@Output() onCancel = new EventEmitter<void>();
@Output() onPost = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void {
  }

  cancel() {
    this.onCancel.emit();
  }

  post() {
    this.onPost.emit();
  }
}
