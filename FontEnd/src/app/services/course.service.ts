import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CourseService {
  private apiUrl = 'https://localhost:7139/api/Course'; // API for courses
  private feedbackUrl = 'https://localhost:7139/api/Feedback/course/'; // API for course reviews

  constructor(private http: HttpClient) {}

  // Fetch the list of courses
  getCourses(): Observable<any[]> {
    return this.http.get<any[]>(this.apiUrl);
  }

  createCourse(courseData: any): Observable<any> {
    const token = localStorage.getItem('token'); // Assuming the JWT token is stored in localStorage
    if (!token) {
      throw new Error('Token is not available');
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`, // Add token in the Authorization header
    });

    return this.http.post<any>(this.apiUrl, courseData, { headers });
  }

  // Fetch reviews for a specific course
  getReviews(courseId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.feedbackUrl}${courseId}`);
  }
}
