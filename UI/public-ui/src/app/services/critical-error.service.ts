import { Injectable } from '@angular/core';
import { catchError, Observable, Subject, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CriticalErrorService {
  onError = new Subject<string>();

  constructor(
    //private toastr: ToastrService
  ) {
    
  }

  setError(text: string) {
    //this.toastr.error("An error occurred processing your request", text);
  }

  wrap(observable: Observable<any>) {
    return observable.pipe(
      catchError(err => {
        if (err.status == 400 || err.status == 401) {
          return throwError(() => new Error(err));
        }
        else {
          this.setError(err.statusText);
          return throwError(() => new Error(err));
        }
      })
    );
  }  

}
