  import { Component } from '@angular/core';
  import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
  import { MaterialModule } from '../../material.modules';
  import { RouterModule, Router } from '@angular/router';
  import { UserService } from '../../services/user.service';
  import { ToastrService } from 'ngx-toastr'; 
  import { SessionStorageService } from '../../sessionstorage.service';
  import { CommonModule } from '@angular/common';

  @Component({
    selector: 'app-login',
    standalone: true,
    imports: [ReactiveFormsModule, MaterialModule, RouterModule, CommonModule],
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css'],
  })
  export class LoginComponent {
    constructor(
      private builder: FormBuilder,
      private service: UserService,
      private toastr: ToastrService,
      private router: Router,
      private sessionStorageService: SessionStorageService // Inject the service
    ) {}

    loginForm = this.builder.group({
      username: this.builder.control('', Validators.required),
      password: this.builder.control('', Validators.required),
    });

    onSubmit() {
      if (this.loginForm.invalid) {
        this.toastr.error('Please fill in all required fields.');
        return;
      }

      const loginData = {
        username: this.loginForm.value.username as string,
        password: this.loginForm.value.password as string,
      };

      this.service.UserLogin(loginData).subscribe({
        next: (response: any) => {
          console.log('Full response:', response);

          // Detailed debugging
          console.log('response exists:', !!response);
          console.log('response.token exists:', !!response.token);
          console.log('typeof response.token:', typeof response.token);
          console.log('response.user exists:', !!response.user);
          console.log(
            'response.user.username exists:',
            !!(response.user && response.user.username)
          );

          // Explicitly check each property with more verbose logging
          if (
            response &&
            response.token &&
            typeof response.token === 'string' &&
            response.user &&
            response.user.username
          ) {
            this.toastr.success('Login successful');

            this.sessionStorageService.saveItem('user', response.user.username);
            this.sessionStorageService.saveItem('userId', response.user.userId);
            this.sessionStorageService.saveItem('email', response.user.email);
            this.sessionStorageService.saveItem('token', response.token);
            this.sessionStorageService.saveItem('role', response.user.role);

            // Navigate to dashboard
            this.router.navigate(['/dashboard']);
          } else {
            console.log('Detailed response breakdown:', {
              hasResponse: !!response,
              hasToken: !!response.token,
              tokenType: typeof response.token,
              hasUser: !!response.user,
              hasUsername: !!(response.user && response.user.username),
            });
            this.toastr.error('Invalid login response' + response.error);
          }
        },
        error: (err: any) => {
          console.error('Full error details:', err);

          // More detailed error handling
          if (err.status === 401) {
            this.toastr.error('Unauthorized: '+ err.error);
          } else if (err.status === 0) {
            this.toastr.error('Network error: Unable to connect to server');
          } else {
            this.toastr.error(
              err.error?.message || err.message || 'Login failed'
            );
          }
        },
      });
    }
  }
