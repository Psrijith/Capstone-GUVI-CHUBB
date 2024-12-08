import { Component } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { routes } from './../../app.routes';
import { SessionStorageService } from '../../sessionstorage.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterModule, CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  constructor(
    private sessionStorageService: SessionStorageService,
    private router: Router
  ) {}

  userRole = this.sessionStorageService.getItem('role');
  logout() {
    this.sessionStorageService.clear();

    this.router.navigate(['/login']);
  }
  mobileMenuOpen: boolean = false;

  toggleMobileMenu() {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }
}
