import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment.development';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-mglearner',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './mglearner.component.html',
  styleUrl: './mglearner.component.css',
})
export class MglearnerComponent implements OnInit {
  users: any[] = [];
  isLoading: boolean = false;

  constructor(private http: HttpClient) {}
  private baseUrl: string = environment.apiUrl;
  ngOnInit() {
    this.fetchUsers();
  }

  fetchUsers() {
    this.isLoading = true;
    this.http.get(environment.apiUrl + 'User').subscribe(
      (response: any) => {
        this.users = response.filter((user: any) => user.role === 'Learner');
        this.isLoading = false;
        console.log(this.users);
      },
      (error) => {
        console.error('Error fetching users:', error);
        this.isLoading = false;
      }
    );
  }

  toggleActive(user: any) {
    console.log(user);
    const updatedState = !user.isActive;

    this.http
      .put(`https://localhost:7139/api/User/approve/${user.userId}`, {
        isActive: updatedState,
      })
      .subscribe(
        (response: any) => {
          user.isActive = updatedState;

          alert(
            `User ${user.username} is now ${
              updatedState ? 'Active' : 'Inactive'
            }`
          );
        },
        (error) => {
          console.log('Error updating user:', error);
          alert('Failed to update user status');
        }
      );
  }
}

