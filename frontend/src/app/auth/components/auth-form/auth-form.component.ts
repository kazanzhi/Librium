import { Component, OnInit, Input } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TokenService } from 'src/app/core/services/token.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-auth-form',
  templateUrl: './auth-form.component.html',
  styleUrls: ['./auth-form.component.scss']
})
export class AuthFormComponent implements OnInit {
  @Input() mode!: 'login' | 'register';

  form!: FormGroup;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private tokenService: TokenService
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
      console.log('LOGIN OK');
      });
    }

    if (this.mode === 'register') {
      this.authService.register(data).subscribe(() => {
        console.log('REGISTER OK');
      });
    }
  }
}
