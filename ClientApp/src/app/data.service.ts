import { Injectable } from '@angular/core';
import { Subject }    from 'rxjs';

@Injectable()
export class DataService {

  constructor() { }

  private dataUpdatedSource = new Subject<string>();
  dataUpdated$ = this.dataUpdatedSource.asObservable();
  dataUpdate(text:string) {
    this.dataUpdatedSource.next(text);
  }
  }