import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
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
export class AuthFormComponent implements OnInit {
  @Input() mode!: 'login' | 'register';
  @Output() registered = new EventEmitter<void>();
  @Output() loggedIn = new EventEmitter<string>();
  @Output() modeChange = new EventEmitter<'login' | 'register'>();

  form!: UntypedFormGroup;

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
      this.form.addControl('username', this.fb.control('', Validators.required));
    }

    if (this.mode === 'login' && this.form.contains('username')) {
      this.form.removeControl('username');
    }
  }

  submit(): void {
    if (this.form.invalid) return;

    const data = this.form.value;

    if (this.mode === 'login') {
      this.authService.login(data).subscribe(response => {
      this.loggedIn.emit(response.token);
    }); 
    }

    if (this.mode === 'register') {
      this.authService.register(data).subscribe(() => {
        this.registered.emit();
      });
    }
  }

  toggleMode(): void {
    const nextMode = this.mode === 'login' ? 'register' : 'login';
    this.modeChange.emit(nextMode);
  }
}
