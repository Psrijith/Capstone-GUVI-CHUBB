import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class SessionStorageService {
  static getItem(arg0: string) {
    throw new Error('Method not implemented.');
  }
  constructor() {}

  // Save data to sessionStorage
  saveItem(key: string, value: any): void {
    sessionStorage.setItem(key, JSON.stringify(value));
  }

  // Get data from sessionStorage
  getItem(key: string): any {
    const item = sessionStorage.getItem(key);
    return item ? JSON.parse(item) : null;
  }

  // Remove data from sessionStorage
  removeItem(key: string): void {
    sessionStorage.removeItem(key);
  }

  // Clear all sessionStorage
  clear(): void {
    sessionStorage.clear();
  }
}
