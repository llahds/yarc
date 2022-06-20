import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-control-validation-errors',
  templateUrl: './control-validation-errors.component.html',
  styleUrls: ['./control-validation-errors.component.scss']
})
export class ControlValidationErrorsComponent implements OnInit {

  @Input() errors: any = {};
  @Input() fieldName: string = "";

  constructor() { }

  ngOnInit() {
    
  }

}
