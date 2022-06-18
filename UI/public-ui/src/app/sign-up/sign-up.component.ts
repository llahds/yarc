import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.scss']
})
export class SignUpComponent implements OnInit {


  @Input() showSignUp: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onSignUp = new EventEmitter<void>();

  constructor() { }

  ngOnInit(): void {
  }

  cancel() {
    this.onCancel.emit();
  }

  signUp() {
    this.onSignUp.emit();
  }
}
