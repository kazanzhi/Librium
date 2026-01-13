import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AdminService } from '../../services/admin.service';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-admin-creation',
    templateUrl: './admin-creation.component.html',
    styleUrls: ['./admin-creation.component.scss'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule]
})
export class AdminCreationComponent implements OnInit {
  form!: UntypedFormGroup;
  loading = false;
  message = '';
  errorMessage: string | null = null;

  constructor(private fb: UntypedFormBuilder, private adminService: AdminService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', [Validators.required, Validators.minLength(6)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  submit(): void {
    this.errorMessage = null;

    if (this.form.invalid){
      this.form.markAllAsTouched()
      return;
    }

    this.loading = true;
    this.message = '';

    this.adminService.registerAdmin(this.form.value).subscribe({
      next: () => {
        this.loading = false;
        this.errorMessage = null;
        this.message = 'Admin created';
        this.form.reset();
      },
      error: err => {
        this.loading = false;

        if(err.status === 400)
        {
          if(typeof err.error === 'string')
          {
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
