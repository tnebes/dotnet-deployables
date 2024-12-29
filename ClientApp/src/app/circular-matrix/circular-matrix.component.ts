import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../Constants';

@Component({
  selector: 'app-circular-matrix',
  templateUrl: './circular-matrix.component.html',
  styleUrls: ['./circular-matrix.component.css']
})
export class CircularMatrixComponent {
  rows = 3;
  cols = 3;
  clockwise = true;
  startPosition = 1;
  matrix: number[][] = [];
  error = '';

  constructor(private readonly http: HttpClient) {}

  generateMatrix(): void {
    if (!this.validateInputs()) {
      return;
    }

    const params = {
      rows: this.rows,
      cols: this.cols,
      clockwise: this.clockwise,
      startPosition: this.startPosition
    };

    this.http.get<number[][]>(Constants.matrix, { params })
      .subscribe({
        next: (result) => {
          this.matrix = result;
          this.error = '';
        },
        error: (err) => {
          this.error = 'Error generating matrix. Please check your inputs.';
          console.error('Matrix generation failed:', err);
        }
      });
  }

  private validateInputs(): boolean {
    if (this.rows <= 0 || this.cols <= 0 || this.startPosition < 0 || this.startPosition > 4) {
      console.error('Invalid parameters', { 
        rows: this.rows, 
        cols: this.cols,
        startPosition: this.startPosition 
      });
      this.error = 'Invalid parameters';
      return false;
    }
    return true;
  }
}

enum StartPosition {
  TopLeft = 0,
  TopRight = 1,
  BottomRight = 2,
  BottomLeft = 3,
  Centre = 4
}
