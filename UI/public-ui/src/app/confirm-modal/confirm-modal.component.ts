import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-confirm-modal',
  templateUrl: './confirm-modal.component.html',
  styleUrls: ['./confirm-modal.component.scss']
})
export class ConfirmModalComponent implements OnInit {

  @Input() showConfirm: boolean = false;
  @Output() onCancel = new EventEmitter<void>();
  @Output() onConfirm = new EventEmitter<void>();
  @Input() disableButtons: boolean = false;
  @Input() showActive: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  cancel() {
    this.onCancel.emit();
  }

  confirm() {
    this.onConfirm.emit();
  }
}
