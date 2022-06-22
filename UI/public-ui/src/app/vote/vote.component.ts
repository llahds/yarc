import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';

@Component({
  selector: 'app-vote',
  templateUrl: './vote.component.html',
  styleUrls: ['./vote.component.scss']
})
export class VoteComponent implements OnInit {

  @Output() onUp = new EventEmitter<void>();
  @Output() onDown = new EventEmitter<void>();

  @Input() ups!: number | undefined;
  @Input() downs!: number | undefined;
  @Input() vote!: number | undefined;

  constructor(
    private authentication: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.authentication.onSignOut.subscribe(() => this.vote = 0);
  }

  up() {
    if (!this.authentication.token) {
      this.authentication.challengeCredentials();
    } else {
      this.vote = 1;
      this.onUp.emit();
    }
  }

  down() {
    if (!this.authentication.token) {
      this.authentication.challengeCredentials();
    } else {
      this.vote = -1;
      this.onDown.emit();
    }
  }
}
