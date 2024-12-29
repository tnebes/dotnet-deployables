import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CircularMatrixComponent } from './circular-matrix.component';

describe('CircularMatrixComponent', () => {
  let component: CircularMatrixComponent;
  let fixture: ComponentFixture<CircularMatrixComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CircularMatrixComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CircularMatrixComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
