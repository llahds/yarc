import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.scss']
})
export class SignInComponent implements OnInit {

  @Input() showSignIn: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onSignIn = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void {
  }

  cancel() {
    this.onCancel.emit();
  }

  signIn() {
    this.onSignIn.emit();
  }
}
