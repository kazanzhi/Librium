import { Component } from '@angular/core';
import { NavigationEnd, Router, RouterOutlet } from '@angular/router';
import { NavbarComponent } from './core/components/navbar/navbar.component';
import { filter } from 'rxjs';
import { TokenService } from './core/services/token.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavbarComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent {
  constructor(
    private router: Router,
    private tokenService: TokenService
  ) {}

  showNavbar = true;

  ngOnInit(): void {
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd)
      )
      .subscribe(() => {
        this.showNavbar = !this.router.url.startsWith('/auth');
      });
  }

  handleLogout(): void {
    this.tokenService.clear();
    this.router.navigate(['/auth']);
  }

  get authenticated(): boolean {
    return this.tokenService.isAuthenticated();
  }

  get isAdmin(): boolean {
    return this.tokenService.hasRole('Admin');
  }
}