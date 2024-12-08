import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms'; // Import ReactiveFormsModule
import { environment } from '../../../environments/environment.development';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-discussion',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule], // Add ReactiveFormsModule here
  templateUrl: './discussion.component.html',
  styleUrls: ['./discussion.component.css'],
})
export class DiscussionComponent implements OnInit {
  discussions: any[] = []; // List of discussions
  newDiscussionForm: FormGroup; // Form to submit a new discussion
  apiUrl: string = `${environment.apiUrl}Discussion/discussions`; // API URL for fetching discussions

  constructor(private http: HttpClient, private fb: FormBuilder) {
    // Create form group for submitting new discussions
    this.newDiscussionForm = this.fb.group({
      courseId: [0], // Course ID
      userId: [0], // User ID (this could be dynamically set based on logged-in user)
      postContent: [''], // Content of the post
      postDate: [new Date().toISOString()], // Current date and time
      status: ['Closed'], // Default status is 'Closed'
    });
  }

  ngOnInit(): void {
    // Fetch discussions when the component initializes
    this.fetchDiscussions();
  }

  // Fetch all discussions
  fetchDiscussions(): void {
    this.http.get<any[]>(this.apiUrl).subscribe(
      (data) => {
        this.discussions = data;
      },
      (error) => {
        console.error('Error fetching discussions', error);
      }
    );
  }

  // Post a new discussion
  postDiscussion(): void {
    const newDiscussion = this.newDiscussionForm.value;
    this.http
      .post<any>(`${environment.apiUrl}Discussion/discussion`, newDiscussion)
      .subscribe(
        (response) => {
          // After posting, fetch the updated list of discussions
          this.fetchDiscussions();
          this.newDiscussionForm.reset(); // Reset form after posting
        },
        (error) => {
          console.error('Error posting discussion', error);
        }
      );
  }

  // Mark a discussion as "open" after receiving clarification/reply
  markDiscussionAsOpen(discussionId: number): void {
    this.http
      .put<any>(
        `${environment.apiUrl}Discussion/discussion/${discussionId}/open`,
        {}
      )
      .subscribe(
        (response) => {
          // Update the status of the discussion after marking it as open
          this.fetchDiscussions();
        },
        (error) => {
          console.error('Error marking discussion as open', error);
        }
      );
  }

  // Mark a discussion as "closed" after resolving it
  markDiscussionAsClosed(discussionId: number): void {
    this.http
      .put<any>(
        `${environment.apiUrl}Discussion/discussion/${discussionId}/close`,
        {}
      )
      .subscribe(
        (response) => {
          // Update the status of the discussion after marking it as closed
          this.fetchDiscussions();
        },
        (error) => {
          console.error('Error marking discussion as closed', error);
        }
      );
  }
}
