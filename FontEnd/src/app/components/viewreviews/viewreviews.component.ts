import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CourseService } from '../../services/course.service';

@Component({
  selector: 'app-viewreviews',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './viewreviews.component.html',
  styleUrl: './viewreviews.component.css',
})
export class ViewreviewsComponent implements OnInit {
  courses: any[] = []; // Stores the list of courses
  reviews: { [key: number]: any[] } = {}; // Stores reviews for each course
  loading: { [key: number]: boolean } = {}; // Tracks loading state for reviews
  showReviews: { [key: number]: boolean } = {}; // Tracks visibility state of reviews

  constructor(private courseService: CourseService) {}

  ngOnInit(): void {
    // Fetch courses when component loads
    this.courseService.getCourses().subscribe(
      (data) => {
        this.courses = data;
      },
      (error) => {
        console.error('Error fetching courses:', error);
      }
    );
  }

  // Fetch reviews for a specific course
  fetchReviews(courseId: number): void {
    if (this.reviews[courseId]) {
      // If reviews are already loaded, do nothing
      return;
    }

    this.loading[courseId] = true;
    this.courseService.getReviews(courseId).subscribe(
      (data) => {
        this.reviews[courseId] = data;
        this.loading[courseId] = false;
      },
      (error) => {
        console.error('Error fetching reviews:', error);
        this.loading[courseId] = false;
      }
    );
  }

  // Toggle reviews visibility
  toggleReviews(courseId: number): void {
    if (!this.showReviews[courseId]) {
      // If reviews are not visible, fetch them
      this.fetchReviews(courseId);
    }

    // Toggle the visibility of the reviews
    this.showReviews[courseId] = !this.showReviews[courseId];
  }
}