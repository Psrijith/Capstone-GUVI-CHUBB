import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit, Input } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SessionStorageService } from '../../sessionstorage.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-allcourses',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './allcourses.component.html',
  styleUrls: ['./allcourses.component.css'],
})
export class AllcoursesComponent implements OnInit {
  courses: any[] = [];
  filteredCourses: any[] = [];
  isLoading: boolean = true;
  searchQuery: string = '';
  sortOption: string = ''; // For sorting
  @Input() hideSearchAndSort: boolean = false; // Input flag to hide search and sort

  categoryImages: { [key: string]: string } = {
    Programming:
      'https://media.licdn.com/dms/image/D5612AQGx8uN_6_lQVA/article-cover_image-shrink_720_1280/0/1712410035493?e=2147483647&v=beta&t=JeX6LK0sS8MyI0Edu8NqOvaJW5yO7by3qkuSmwQkhWs',
    'Web Development':
      'https://jessup.edu/wp-content/uploads/2024/01/Is-Web-Development-Oversaturated.jpg',
    'Artificial Intelligence':
      'https://media.geeksforgeeks.org/wp-content/uploads/20240319155102/what-is-ai-artificial-intelligence.webp',
    'Data Science':
      'https://emeritus.org/in/wp-content/uploads/sites/3/2023/01/What-is-machine-learning-Definition-types.jpg.optimal.jpg',
    'Cyber Security':
      'https://miro.medium.com/v2/resize:fit:1400/1*gQRjtlREyWDo8szt0XkKcw.png',
    'Design':
      'https://i.ytimg.com/vi/ewJgxHQo0TU/hq720.jpg?sqp=-oaymwEhCK4FEIIDSFryq4qpAxMIARUAAAAAGAElAADIQj0AgKJD&rs=AOn4CLBuNtwUe95aS0A28rB82KEI2nCyNg'
  };

  constructor(
    private toastr: ToastrService,
    private http: HttpClient,
    private SessionStorageService: SessionStorageService
  ) {}

  ngOnInit(): void {
    this.fetchCourses();
  }

  fetchCourses(): void {
    const apiUrl = 'https://localhost:7139/api/Course';
    this.http.get<any[]>(apiUrl).subscribe({
      next: (response) => {
        this.courses = response;
        this.filteredCourses = [...response];
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error fetching courses:', error);
        this.isLoading = false;
      },
    });
  }

  enrollCourse(courseId: number): void {
    this.isLoading = true; // Start loading before making the request
    const userId = this.SessionStorageService.getItem('userId');
    const apiUrl = `https://localhost:7139/api/Enrollment/course/${courseId}/user/${this.SessionStorageService.getItem(
      'userId'
    )}`;

    this.http.post<any>(apiUrl, null).subscribe({
      next: (response) => {
        this.isLoading = false; // Stop loading
        console.log(response);

        // If successful, show a success toaster
        if (response && response.enrollmentId) {
          console.log(
            'Enrollment successful! Enrollment ID:',
            response.enrollmentId
          );
          this.toastr.success(
            'You have successfully enrolled in the course!',
            'Enrollment Successful'
          );
        } else {
          console.log('Unexpected response:', response);
          this.toastr.error(response.message);
        }
      },
      error: (error) => {
        this.isLoading = false; // Stop loading
        console.error('Error enrolling in course:', error.error.message);
        this.toastr.error(error.error.message);
      },
    });
  }

  getCategoryImage(category: string): string {
    return (
      this.categoryImages[category] ||
      'https://via.placeholder.com/50?text=Default'
    );
  }

  filterCourses(): void {
    const query = this.searchQuery.toLowerCase();
    this.filteredCourses = this.courses.filter((course) => {
      return (
        course.title.toLowerCase().includes(query) ||
        course.category.toLowerCase().includes(query) ||
        course.instructor.username.toLowerCase().includes(query)
      );
    });
    this.sortCourses(); // Ensure sorting is applied after filtering
  }

  sortCourses(): void {
    if (!this.sortOption) return; // Default order

    this.filteredCourses.sort((a, b) => {
      if (this.sortOption === 'category') {
        return a.category.localeCompare(b.category);
      } else if (this.sortOption === 'difficulty') {
        return a.difficultyLevel.localeCompare(b.difficultyLevel);
      } else if (this.sortOption === 'duration') {
        // Convert durations to seconds for comparison
        const durationToSeconds = (duration: string) => {
          const [hours, minutes, seconds] = duration.split(':').map(Number);
          return hours * 3600 + minutes * 60 + seconds;
        };
        return durationToSeconds(a.duration) - durationToSeconds(b.duration);
      }
      return 0;
    });
  }
}
