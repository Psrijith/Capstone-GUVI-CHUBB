import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MglearnerComponent } from './mglearner.component';

describe('MglearnerComponent', () => {
  let component: MglearnerComponent;
  let fixture: ComponentFixture<MglearnerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MglearnerComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MglearnerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
