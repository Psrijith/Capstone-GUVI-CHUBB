import { Component, NgModule, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SessionStorageService } from '../../sessionstorage.service';
import { UserService } from '../../services/user.service'; // Assuming you have a service to interact with backend
import { ToastrService } from 'ngx-toastr'; // Import ToastrService
import { CommonModule } from '@angular/common';
import { FormsModule, NgModel } from '@angular/forms';
import { NavbarComponent } from "../navbar/navbar.component";
import { FooterComponent } from "../footer/footer.component";

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule, FormsModule, NavbarComponent, FooterComponent],
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  user: any;
  username = this.sessionStorageService.getItem('user');
  email = this.sessionStorageService.getItem('email');
  role = this.sessionStorageService.getItem('role');
  constructor(
    private sessionStorageService: SessionStorageService,
    private router: Router,
    private userService: UserService,
    private toastr: ToastrService // Inject ToastrService
  ) {}

  ngOnInit(): void {
    // Fetch user data from sessionStorage or API
    const userData = this.sessionStorageService.getItem('user');
    if (userData) {
      this.user = userData; // Assuming user data is stored in sessionStorage
    } else {
      this.router.navigate(['/login']); // Redirect to login if no user data
    }
  }

  // Handle profile form submission (update profile)
  onSubmit(): void {
    if (this.user) {
      // Show toast notification (No API, ask to consult support)
      this.toastr.info(
        'Please consult Admin or Support for profile updates.',
        'Profile Update'
      );
    }
  }

  // Handle logout
  logout(): void {
    this.sessionStorageService.clear();
    this.router.navigate(['/login']);
  }

  changePassword() {
    this.toastr.show('Consult Admin or Support');
  }
}
