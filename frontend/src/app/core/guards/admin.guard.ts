import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TokenService } from '../services/token.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard  {
  constructor(private tokenService: TokenService, private router: Router) {}

  canActivate(): boolean {
    if (this.tokenService.hasRole('Admin')) return true;

    this.router.navigate(['/books']);
    return false;
  }
}
