import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from '../data.service';

@Component({
  selector: 'manual-entry-component',
  templateUrl: './manual-entry.component.html'
})
export class ManualEntryComponent {

  public talk:Talk;
  private _baseUrl;

  constructor(private dataService: DataService, private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this._baseUrl = baseUrl;
    this.talk = new Talk();
    }
    onSubmit() {
      this.http.post(this._baseUrl + 'api/ManualEntry/PostTalk',this.talk)
      .subscribe(value => this.dataService.dataUpdate(""));
     }
}

class Talk {

  constructor(
    public duration: number = 0,
    public title: string = "",
  ) {  }

}