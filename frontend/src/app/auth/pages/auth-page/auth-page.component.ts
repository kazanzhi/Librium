import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-auth-page',
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.scss']
})
export class AuthPageComponent {
  mode: 'login' | 'register' = 'login';

  constructor(private route: ActivatedRoute) {
    const mode = this.route.snapshot.queryParamMap.get('mode');
    if (mode === 'register') {
      this.mode = 'register';
    }
  }
}
