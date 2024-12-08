import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MginstructorComponent } from './mginstructor.component';

describe('MginstructorComponent', () => {
  let component: MginstructorComponent;
  let fixture: ComponentFixture<MginstructorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MginstructorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MginstructorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
