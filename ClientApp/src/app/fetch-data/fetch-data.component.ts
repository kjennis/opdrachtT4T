import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DataService } from '../data.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public tracks: Track[];

  constructor(private dataService: DataService, private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.getData(baseUrl + 'api/SampleData/Tracks');
      this.dataService.dataUpdated$
      .subscribe(value => this.getData(baseUrl + 'api/SampleData/Tracks'));
    }

    private getData(url:string)
    {
      this.http.get<Track[]>(url).subscribe(result => {
        this.tracks = result;
    }, error => console.error(error));
  }
}

interface Track {
  title: string;
  talks: Talk[];
}

interface Talk {
  time: string;
  title: string;
  duration: number;
}
