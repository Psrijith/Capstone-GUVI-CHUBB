import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router,
} from '@angular/router';
import { SessionStorageService } from './sessionstorage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(
    private sessionStorageService: SessionStorageService,
    private router: Router
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    // Check if the token exists in sessionStorage (you can also check for a user object)
    const token = this.sessionStorageService.getItem('token');

    if (token) {
      return true; // Allow access to the route
    } else {
      // Redirect to login page if not authenticated
      this.router.navigate(['/login']);
      return false;
    }
  }
}
