import {Component, AfterViewChecked, ElementRef, ViewChild} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Constants} from '../Constants';

interface MatrixResponse {
  rows: number;
  cols: number;
  matrix: number[][];
}

@Component({
  selector: 'app-circular-matrix',
  templateUrl: './circular-matrix.component.html',
  styleUrls: ['./circular-matrix.component.css']
})
export class CircularMatrixComponent implements AfterViewChecked {
  @ViewChild('matrixWrapper') matrixWrapper!: ElementRef;
  @ViewChild('matrixContainer') matrixContainer!: ElementRef;

  rows: number = 5;
  cols: number = 5;
  startPosition: number = 0;
  clockwise: boolean = true;
  startFromOne: boolean = true;
  matrix: number[][] = [];
  error: string = '';

  constructor(private readonly http: HttpClient) {
  }

  generateMatrix() {
    if (!this.validateInputs()) {
      return;
    }

    this.error = '';
    const params = new HttpParams()
      .set('rows', this.rows)
      .set('cols', this.cols)
      .set('startPosition', this.startPosition)
      .set('clockwise', this.clockwise)
      .set('startFromOne', this.startFromOne);

    this.http.get<MatrixResponse>(Constants.MATRIX, {params}).subscribe({
      next: (response) => {
        this.matrix = response.matrix;
        document.documentElement.style.setProperty('--cols', this.cols.toString());
      },
      error: (error) => {
        this.error = error.error || 'An error occurred while generating the matrix';
        this.matrix = [];
      }
    });
  }

  private validateInputs(): boolean {
    if (this.rows <= 0 || this.cols <= 0 || this.startPosition < 0 || this.startPosition > 4) {
      console.error('Invalid parameters', {
        rows: this.rows,
        cols: this.cols,
        startPosition: this.startPosition,
        clockwise: this.clockwise,
        startFromOne: this.startFromOne
      });
      this.error = 'Invalid parameters';
      return false;
    }
    return true;
  }

  ngAfterViewChecked() {
    this.scaleMatrix();
  }

  private scaleMatrix() {
    if (!this.matrixWrapper || !this.matrixContainer || this.matrix.length === 0) return;

    const wrapper = this.matrixWrapper.nativeElement;
    const container = this.matrixContainer.nativeElement;

    wrapper.style.transform = 'scale(1)';

    const wrapperRect = wrapper.getBoundingClientRect();
    const containerRect = container.getBoundingClientRect();

    const scaleX = (containerRect.width - 40) / wrapperRect.width;
    const scaleY = (containerRect.height - 40) / wrapperRect.height;
    const scale = Math.min(scaleX, scaleY, 1);

    wrapper.style.transform = `scale(${scale})`;
  }
}

enum StartPosition {
  TopLeft = 0,
  TopRight = 1,
  BottomRight = 2,
  BottomLeft = 3,
  Centre = 4
}
