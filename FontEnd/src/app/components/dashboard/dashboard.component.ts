import { Component, OnInit } from '@angular/core';
import { SessionStorageService } from '../../sessionstorage.service';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../navbar/navbar.component';
import { FooterComponent } from '../footer/footer.component';
import { MginstructorComponent } from "../mginstructor/mginstructor.component";
import { MglearnerComponent } from "../mglearner/mglearner.component";
import { AdminDashboardComponent } from "../admin-dashboard/admin-dashboard.component";
import { AllcoursesComponent } from "../allcourses/allcourses.component";
import { EnrolledCoursesComponent } from "../enrolledcourses/enrolledcourses.component";
import { ViewreviewsComponent } from "../viewreviews/viewreviews.component";
import { DiscussionComponent } from "../discussion/discussion.component"; 
import { CreateCourseComponent } from "../createcourse/createcourse.component";
import { AssignmentComponent } from "../assignment/assignment.component";
import { CourseContentComponent } from "../coursecontent/coursecontent.component";
import { LearningPathsComponent } from "../learningpaths/learningpaths.component";
import { QuizComponent } from "../quiz/quiz.component"; 

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, NavbarComponent, FooterComponent, MginstructorComponent, MglearnerComponent, AdminDashboardComponent, AllcoursesComponent, EnrolledCoursesComponent, ViewreviewsComponent, DiscussionComponent, CreateCourseComponent, AssignmentComponent, CourseContentComponent, LearningPathsComponent, QuizComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'], 
})
export class DashboardComponent implements OnInit {
  userRole: string | null = null;
  isPanelCollapsed = false; // Side panel collapsed state
  currentComponent: string = 'dashboard'; // Default loaded component

  constructor(private sessionStorageService: SessionStorageService) {}

  ngOnInit() {
    const user = this.sessionStorageService.getItem('role');
    if (user) {
      this.userRole = user;
    }
  }

  togglePanel() {
    this.isPanelCollapsed = !this.isPanelCollapsed;
  }

  loadComponent(component: string) {
    this.currentComponent = component;
  }
}
