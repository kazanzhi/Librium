import { Component, OnInit } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { AdminService } from '../../services/admin.service';

@Component({
  selector: 'app-admin-creation',
  templateUrl: './admin-creation.component.html',
  styleUrls: ['./admin-creation.component.scss']
})
export class AdminCreationComponent implements OnInit {
  form!: UntypedFormGroup;
  loading = false;
  message = '';

  constructor(private fb: UntypedFormBuilder, private adminService: AdminService) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  submit(): void {
    if (this.form.invalid) return;

    this.loading = true;
    this.message = '';

    this.adminService.registerAdmin(this.form.value).subscribe({
      next: () => {
        this.loading = false;
        this.message = 'Admin created';
        this.form.reset();
      },
      error: err => {
        this.loading = false;
        this.message = 'Failed to create admin';
        console.error(err);
      }
    });
  }
}
