import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import Chart from 'chart.js/auto';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css'],
})
export class AdminDashboardComponent implements OnInit {
  totalUsers: number = 0;
  totalCourses: number = 0;
  activeInstructors: number = 0;
  totalEnrollments: number = 0;
  userRoles: { [key: string]: number } = {};
  coursesByCategory: { [key: string]: number } = {};

  constructor(private http: HttpClient) {}

  ngOnInit() {
    this.fetchUsers();
    this.fetchCourses();
    this.initializeCharts();
  }

  fetchUsers() {
    this.http.get(environment.apiUrl + 'User').subscribe((response: any) => {
      this.totalUsers = response.length;

      response.forEach((user: any) => {
        this.userRoles[user.role] = (this.userRoles[user.role] || 0) + 1;
      });

      this.activeInstructors = response.filter(
        (user: any) => user.isActive && user.role === 'Instructor'
      ).length;

      this.initializeUserRolesChart();
    });
  }

  fetchCourses() {
    this.http.get(environment.apiUrl + 'Course').subscribe((response: any) => {
      this.totalCourses = response.length;

      response.forEach((course: any) => {
        this.coursesByCategory[course.category] =
          (this.coursesByCategory[course.category] || 0) + 1;
      });

      this.initializeCoursesCategoryChart();
    });

    this.http
      .get('https://localhost:7139/api/Enrollment/total/enrollments/count')
      .subscribe((response: any) => {
        this.totalEnrollments = response;
        this.initializeEnrollmentDistributionChart(); // Line Chart
      });
  }

  initializeUserRolesChart() {
    const ctx = document.getElementById('userRolesChart') as HTMLCanvasElement;
    new Chart(ctx, {
      type: 'pie',
      data: {
        labels: Object.keys(this.userRoles),
        datasets: [
          {
            data: Object.values(this.userRoles),
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: true, // Ensure circle for pie chart
      },
    });
  }

  initializeCoursesCategoryChart() {
    const ctx = document.getElementById(
      'coursesCategoryChart'
    ) as HTMLCanvasElement;
    new Chart(ctx, {
      type: 'bar',
      data: {
        labels: Object.keys(this.coursesByCategory),
        datasets: [
          {
            label: 'Courses per Category',
            data: Object.values(this.coursesByCategory),
            backgroundColor: '#36A2EB',
          },
        ],
      },
    });
  }

  // Initialize Line Chart for Enrollment Distribution
  initializeEnrollmentDistributionChart() {
    const ctx = document.getElementById(
      'enrollmentDistributionChart'
    ) as HTMLCanvasElement;
    new Chart(ctx, {
      type: 'line', // Changing to line chart
      data: {
        labels: ['January', 'February', 'March', 'April', 'May'], // Example labels
        datasets: [
          {
            label: 'Enrollments Over Time',
            data: [
              this.totalEnrollments,
              this.totalEnrollments + 100,
              this.totalEnrollments + 200,
              this.totalEnrollments + 150,
              this.totalEnrollments + 250,
            ], // Example data
            borderColor: '#FF6384',
            backgroundColor: 'rgba(255, 99, 132, 0.2)',
            fill: true,
            tension: 0.4,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false, // To ensure proper size of the line chart
      },
    });
  }

  initializeCharts() {
    const ctx = document.getElementById('yourChartId') as HTMLCanvasElement;
    new Chart(ctx, {
      type: 'pie',
      data: {
        labels: ['Label1', 'Label2', 'Label3'],
        datasets: [
          {
            data: [10, 20, 30],
            backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56'],
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
      },
    });
  }
}
