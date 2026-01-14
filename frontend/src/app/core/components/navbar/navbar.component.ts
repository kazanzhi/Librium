import { CommonModule } from '@angular/common';
import { Component, Output, EventEmitter, Input } from '@angular/core';
import { Router } from '@angular/router';
import { SearchService } from 'src/app/core/services/search.service';

@Component({
  selector: 'app-navbar',
  imports: [CommonModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.scss',
  standalone: true
})
export class NavbarComponent {
  @Input() authenticated: boolean = false;
  @Output() logout = new EventEmitter<void>();
  @Input() isAdmin: boolean = false;

  constructor(private router: Router, private searchService: SearchService) {}

  onSearch(value: string): void {
    this.searchService.setSearch(value);
  }

  onBrowse(): void {
    this.router.navigate(['/books']);
  }

  onMyLibrary(): void {
    this.router.navigate(['/library']);
  }

  onLogin(): void {
    this.router.navigate(['/auth']);
  }

  onLogout(): void {
    this.logout.emit();
  }

  onAdmin(): void {
    this.router.navigate(['/admin']);
  }
}
