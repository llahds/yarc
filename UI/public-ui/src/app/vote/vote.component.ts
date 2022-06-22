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

  public isAuthenticated: boolean = false;

  constructor(
    private authentication: AuthenticationService
  ) { }

  ngOnInit(): void {
    this.isAuthenticated = !!this.authentication.token;

    this.authentication.onSignOut.subscribe(r => this.isAuthenticated = false);
  }

  up() {
    this.vote = 1;
    this.onUp.emit();
  }

  down() {
    this.vote = -1;
    this.onDown.emit();
  }
}
