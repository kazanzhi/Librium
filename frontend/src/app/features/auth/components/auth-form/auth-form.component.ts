import { Component, OnInit, OnChanges,Input, Output, EventEmitter } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-auth-form',
    templateUrl: './auth-form.component.html',
    styleUrls: ['./auth-form.component.scss'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule]
})
export class AuthFormComponent implements OnInit, OnChanges {
  @Input() mode!: 'login' | 'register';
  @Output() registered = new EventEmitter<void>();
  @Output() loggedIn = new EventEmitter<string>();
  @Output() modeChange = new EventEmitter<'login' | 'register'>();

  form!: UntypedFormGroup;
  errorMessage: string | null = null;

  constructor(
    private fb: UntypedFormBuilder,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });

    this.syncMode();
  }

  ngOnChanges(): void {
    this.syncMode();
  }

  syncMode(): void {
    if (!this.form) return;

    if (this.mode === 'register' && !this.form.contains('username')) {
      this.form.addControl('username', this.fb.control('', [Validators.required, Validators.minLength(6)]));
    }

    if (this.mode === 'login' && this.form.contains('username')) {
      this.form.removeControl('username');
    }
  }

  submit(): void {
    this.errorMessage = null;
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const data = this.form.value;

    if (this.mode === 'login') {
      this.authService.login(data).subscribe({
        next: response => {
          this.loggedIn.emit(response.token);
        },
        error: err => {
          if (err.status === 400) {
            if (typeof err.error === 'string') {
              this.errorMessage = err.error;
            } else if (err.error?.message) {
              this.errorMessage = err.error.message;
            } else {
              this.errorMessage = 'Invalid credentials.';
            }
          }
        }
      }); 
    }

    if (this.mode === 'register') {
      this.authService.register(data).subscribe({
        next: () => {
          this.registered.emit();
        },
        error: err => {
          if (err.status === 400) {

            if (typeof err.error === 'string') {
              this.errorMessage = err.error;
              return;
            }

            if (err.error?.errors?.Username) {
              this.form.controls['username']
                .setErrors({ server: err.error.errors.Username[0] });
                return;
            }

            this.errorMessage = 'Invalid input data.';
          } else {
            this.errorMessage = 'An unexpected error occurred.';
          }
        }
      });
    }
  }

  toggleMode(): void {
    const nextMode = this.mode === 'login' ? 'register' : 'login';
    this.modeChange.emit(nextMode);
  }
}
