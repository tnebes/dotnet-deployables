import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { FormsModule }   from '@angular/forms';
import { Constants } from '../Constants';

@Component({
  selector: 'app-love-calculator',
  templateUrl: './love-calculator.component.html',
  styleUrls: ['./love-calculator.component.css']
})
export class LoveCalculatorComponent implements OnInit {

  name1: string = '';
  name2: string = '';
  result: string = '';

  constructor(private http: HttpClient) { }

  ngOnInit(): void { }

  calculateLove(): void {
    if (this.name1.trim() == '' || this.name2.trim() == '')
    {
      console.error("One of the names is blank.");
      this.result = this.writeError("One or both of the names cannot be blank.");
      return;
    }

    const params = new HttpParams()
      .set('name1', this.name1)
      .set('name2', this.name2);

    this.http.get<{ result: number }>(Constants.LOVE, { params })
      .subscribe({
        next: response => this.result = this.writeMessage(response.result),
        error: error => {
          console.error('Error calling love calculator API:', error);
          this.result = this.writeError('Error calling love calculator API:');
        }
      });
  }

  private writeMessage(response: number): string {
    return `${this.name1} and ${this.name2} love themselves ${response}%!`
  }

  private writeError(message: string): string {
    return message;
  }
}
