import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr'; // Import ToastrService
import { CommonModule } from '@angular/common';
import { SessionStorageService } from '../../sessionstorage.service';

@Component({
  selector: 'app-createcourse',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './createcourse.component.html',
  styleUrls: ['./createcourse.component.css'],
})
export class CreateCourseComponent {
  createCourseForm: FormGroup;
  loading: boolean = false;
  errorMessage: string = '';

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private sessionStorageService: SessionStorageService,
    private toastr: ToastrService // Inject ToastrService
  ) {
    this.createCourseForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(100)]],
      description: ['', [Validators.required, Validators.maxLength(500)]],
      category: ['', [Validators.required]],
      difficultyLevel: ['', [Validators.required]],
      duration: ['', [Validators.required]],
      status: ['Active', [Validators.required]],
    });
  }

  get formControls() {
    return this.createCourseForm.controls;
  }

  onSubmit() {
    if (this.createCourseForm.invalid) {
      return;
    }

    this.loading = true;
    this.errorMessage = '';

    const formData = this.createCourseForm.value;
    const token = this.sessionStorageService.getItem('token');

    if (!token) {
      this.errorMessage = 'Authorization token is missing.';
      this.toastr.error('Authorization token is missing.', 'Error');
      this.loading = false;
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json',
    });

    const body = {
      title: formData['title'],
      description: formData['description'],
      category: formData['category'],
      difficultyLevel: formData['difficultyLevel'],
      duration: formData['duration'],
      status: formData['status'],
    };

    this.http
      .post('https://localhost:7139/api/Course', body, { headers })
      .subscribe({
        next: (response) => {
          this.loading = false;
          this.toastr.success('Course created successfully!', 'Success');
          this.router.navigate(['/courses']);
        },
        error: (error) => {
          this.loading = false;
          this.toastr.error(
            error.error.message ||
              'An error occurred while creating the course.',
            'Error'
          );
        },
      });
  }
}
