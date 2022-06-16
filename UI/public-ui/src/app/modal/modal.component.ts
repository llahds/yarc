import { Component, ContentChild, ElementRef, EventEmitter, Input, OnInit, Output, TemplateRef, ViewChild } from '@angular/core';
import { Subject } from 'rxjs';
import { debounceTime, throttleTime } from 'rxjs/operators';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss']
})
export class ModalComponent implements OnInit {

  @Input() showModal: boolean = false;
  @Input() header: string = "";
  @ContentChild("content", { static: false }) contentTemplateRef: any;
  @ContentChild("actions", { static: false }) actionTemplateRef: any;
  @ViewChild('modelBody') modelBody?: ElementRef;

  @Input() showLarge: boolean = false;
  @Input() showSmall: boolean = false;

  private scroll = new Subject<any>();
  @Output() onPositionChanged = new EventEmitter<any>();

  constructor() {
    this.scroll.pipe(
      debounceTime(200)
    ).subscribe(r => {
      const scrollTop = this.modelBody?.nativeElement.scrollTop;
      const rect = this.modelBody?.nativeElement.getBoundingClientRect();
      this.onPositionChanged.emit({ scrollTop: scrollTop, left: rect.left, top: rect.top });
    });
  }

  ngOnInit(): void {
  }

  onScroll() {
    this.scroll.next({});
  }
}