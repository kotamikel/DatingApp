import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {

  values: any;

  // Use DI to bring in HTTP client - add in constructor
  // Now you can make HTTP requests to API server
  // Each component has lifecycle events - constructor goes first - right place to inject services
  constructor(private http: HttpClient) { }

  // Call to API
  ngOnInit() {
    this.getValues();
  }

  getValues() {
    // Route to API endpoint in () - Returns to an observable (stream of data) - must subscribe
    this.http.get('http://localhost:5000/api/values').subscribe(response => {
      this.values = response;
    }, error => {
      console.log(error);
    });
  }

}
