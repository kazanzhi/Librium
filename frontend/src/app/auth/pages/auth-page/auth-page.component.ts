import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TokenService } from 'src/app/core/services/token.service';

@Component({
  selector: 'app-auth-page',
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.scss']
})
export class AuthPageComponent {
  mode: 'login' | 'register' = 'login';

  constructor(
    private route: ActivatedRoute,
    private tokenService: TokenService,
    private router: Router
  ) {}
    
  ngOnInit(): void {
    if (this.tokenService.isAuthenticated()) {
      this.router.navigate(['/books']);
      return;
    }

    const mode = this.route.snapshot.queryParamMap.get('mode');
    if (mode === 'register') {
      this.mode = 'register';
    }
  }

  onRegistered(): void {
    this.mode = 'login';
  }
}
