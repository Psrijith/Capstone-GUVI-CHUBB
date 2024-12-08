import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { ToastrService } from 'ngx-toastr';
import { SessionStorageService } from '../../sessionstorage.service';
import { CourseService } from '../../services/course.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-assignment',
  standalone: true,
  imports:[CommonModule,FormsModule],
  templateUrl: './assignment.component.html',
  styleUrls: ['./assignment.component.css'],
})
export class AssignmentComponent implements OnInit {
  enrolledCourses: any[] = [];
  assignments: { [key: number]: any[] } = {};
  allCourses: any[] = [];
  showAddAssignmentForm = false;
  isInstructorOrAdmin = false;
  newAssignment: any = {
    title: '',
    description: '',
    dueDate: '',
    gradingCriteria: '',
    courseId: null,
  };

  constructor(
    private http: HttpClient,
    private toastr: ToastrService,
    private sessionStorageService: SessionStorageService,
    private courseService: CourseService
  ) {}

  ngOnInit(): void {
    const role = this.sessionStorageService.getItem('role');
    this.isInstructorOrAdmin = role === 'Instructor' || role === 'Admin';
    this.fetchEnrolledCourses();
    this.fetchAllCourses();
  }

  fetchAllCourses(): void {
    this.courseService.getCourses().subscribe(
      (courses) => (this.allCourses = courses),
      (error) => this.toastr.error('Failed to fetch courses.')
    );
  }

  toggleAddAssignmentForm(): void {
    this.showAddAssignmentForm = !this.showAddAssignmentForm;
  }

  onAddAssignment(assignmentForm: any): void {
    if (!assignmentForm.valid) {
      this.toastr.warning('Please fill in all required fields!');
      return;
    }

    const assignmentUrl = `${environment.apiUrl}Assignment`;
    this.http.post(assignmentUrl, this.newAssignment).subscribe(
      (response) => {
        this.toastr.success('Assignment added successfully!');
        this.showAddAssignmentForm = false;
        this.fetchAssignments(this.newAssignment.courseId);
      },
      (error) => this.toastr.error('Failed to add assignment.')
    );
  }

  fetchEnrolledCourses(): void {
    const userId = this.sessionStorageService.getItem('userId');
    const enrolledCoursesUrl = `${environment.apiUrl}Enrollment/user/${userId}/courses`;

    this.http.get<any[]>(enrolledCoursesUrl).subscribe(
      (courses) => {
        this.enrolledCourses = courses;
        this.enrolledCourses.forEach((course) =>
          this.fetchAssignments(course.courseId)
        );
      },
      (error) => this.toastr.error('Failed to fetch enrolled courses.')
    );
  }

  fetchAssignments(courseId: number): void {
    const assignmentsUrl = `${environment.apiUrl}Assignment/course/${courseId}`;
    this.http.get<any[]>(assignmentsUrl).subscribe(
      (assignments) => (this.assignments[courseId] = assignments),
      (error) =>
        this.toastr.error(`Failed to fetch assignments for course ${courseId}.`)
    );
  }
}
