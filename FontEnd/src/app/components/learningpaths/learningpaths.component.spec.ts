import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LearningpathsComponent } from './learningpaths.component';

describe('LearningpathsComponent', () => {
  let component: LearningpathsComponent;
  let fixture: ComponentFixture<LearningpathsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LearningpathsComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LearningpathsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
