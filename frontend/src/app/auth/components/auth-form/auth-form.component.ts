import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { TokenService } from 'src/app/core/services/token.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss']
})
export class AuthFormComponent implements OnInit {
  @Input() mode!: 'login' | 'register';
  @Output() registered = new EventEmitter<void>();

  form!: UntypedFormGroup;

  constructor(
    private fb: UntypedFormBuilder,
    private authService: AuthService,
    private tokenService: TokenService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });

    if (this.mode === 'register') {
      this.form.addControl(
        'username',
        this.fb.control('', Validators.required)
      );
    }
  }

  submit(): void {
    if (this.form.invalid) return;

    const data = this.form.value;

    if (this.mode === 'login') {
      this.authService.login(data).subscribe(response => {
      this.tokenService.save(response.token);
      this.router.navigate(['/books']);
    }); 
    }

    if (this.mode === 'register') {
      this.authService.register(data).subscribe(() => {
        this.registered.emit();
      });
    }
  }
}
