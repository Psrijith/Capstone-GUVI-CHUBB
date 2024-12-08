import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { SessionStorageService } from '../../sessionstorage.service';
import { ToastrService } from 'ngx-toastr';
import { FormsModule } from '@angular/forms';
import { CourseContentComponent } from '../coursecontent/coursecontent.component';

@Component({
  selector: 'app-enrolledcourses',
  standalone: true,
  imports: [CommonModule, FormsModule, CourseContentComponent],
  templateUrl: './enrolledcourses.component.html',
  styleUrls: ['./enrolledcourses.component.css'],
})
export class EnrolledCoursesComponent implements OnInit {
  enrolledCourses: any[] = [];
  isLoading: boolean = true;
  userId: number | null = null;
  isFeedbackModalOpen: boolean = false;
  ratingValue: number = 5; // Default rating value
  reviewComments: string = '';
  selectedCourseId: number | null = null;
  currentComponent: string = '';

  categoryImages: { [key: string]: string } = {
    'Data Science':
      'https://emeritus.org/in/wp-content/uploads/sites/3/2023/01/What-is-machine-learning-Definition-types.jpg.optimal.jpg',
    'Web Development':
      'https://jessup.edu/wp-content/uploads/2024/01/Is-Web-Development-Oversaturated.jpg',
    Programming:
      'https://media.licdn.com/dms/image/D5612AQGx8uN_6_lQVA/article-cover_image-shrink_720_1280/0/1712410035493?e=2147483647&v=beta&t=JeX6LK0sS8MyI0Edu8NqOvaJW5yO7by3qkuSmwQkhWs',
    'Artificial Intelligence':
      'https://media.geeksforgeeks.org/wp-content/uploads/20240319155102/what-is-ai-artificial-intelligence.webp',
  };

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router,
    private sessionService: SessionStorageService,
    private toastr: ToastrService
  ) {}

  ngOnInit(): void {
    this.userId = this.sessionService.getItem('userId');
    if (this.userId) {
      this.fetchEnrolledCourses();
    } else {
      console.error('User ID is not available in session.');
    }
  }

  fetchEnrolledCourses(): void {
    const apiUrl = `https://localhost:7139/api/Enrollment/user/${this.userId}/courses`;
    this.http.get<any[]>(apiUrl).subscribe({
      next: (response) => {
        this.enrolledCourses = response;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching enrolled courses:', error);
        this.isLoading = false;
      },
    });
  }

  getCategoryImage(category: string): string {
    return (
      this.categoryImages[category] ||
      'https://via.placeholder.com/50?text=Default'
    );
  }

  unenrollCourse(courseId: number): void {
    const apiUrl = `https://localhost:7139/api/Enrollment/course/${courseId}/user/${this.userId}`;
    this.http.delete(apiUrl).subscribe({
      next: (response) => {
        this.toastr.success(
          'Successfully unenrolled from the course!',
          'Unenroll Successful'
        );
        this.fetchEnrolledCourses();
      },
      error: (error) => {
        console.error('Error unenrolling from course:', error);
        this.toastr.error(
          'An error occurred while unenrolling. Please try again.',
          'Unenroll Failed'
        );
      },
    });
  }

  loadComponent(component: string) {
    this.currentComponent = component;
  }

  goBackToCourses() {
    this.currentComponent = '';
  }

  openFeedbackModal(courseId: number): void {
    this.selectedCourseId = courseId;
    this.isFeedbackModalOpen = true;
    this.ratingValue = 5;
    this.reviewComments = '';
  }

  closeFeedbackModal(): void {
    this.isFeedbackModalOpen = false;
    this.selectedCourseId = null;
    this.ratingValue = 5;
    this.reviewComments = '';
  }

  submitFeedback(courseId: number): void {
    if (!this.reviewComments) {
      this.toastr.error('Please provide a comment!', 'Feedback Failed');
      return;
    }

    const feedback = {
      courseId: courseId,
      userId: this.userId,
      ratingValue: this.ratingValue,
      reviewComments: this.reviewComments,
      ratingDate: new Date().toISOString(),
    };

    const apiUrl = `https://localhost:7139/api/Feedback`;
    this.http.post(apiUrl, feedback).subscribe({
      next: (response) => {
        this.toastr.success(
          'Feedback submitted successfully!',
          'Thank you for your feedback'
        );
        this.closeFeedbackModal();
      },
      error: (error) => {
        console.error('Error submitting feedback:', error);
        this.toastr.error(
          'An error occurred while submitting feedback. Please try again.',
          'Feedback Failed'
        );
      },
    });
  }
}
