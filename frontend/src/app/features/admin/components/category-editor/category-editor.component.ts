import { CommonModule } from '@angular/common';
import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { ReactiveFormsModule, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { CategoryService } from 'src/app/features/categories/services/category.service';
import { Category } from 'src/app/shared/models/category';

@Component({
    selector: 'app-category-editor',
    templateUrl: './category-editor.component.html',
    styleUrls: ['./category-editor.component.scss'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule]
})
export class CategoryEditorComponent implements OnInit {
  @Input() categories: Category[] = [];
  @Output() saved = new EventEmitter<void>();

  form!: UntypedFormGroup;
  selectedCategory?: Category;
  loading = false;
  errorMessage: string | null = null;

  constructor(private categoryService: CategoryService, private fb: UntypedFormBuilder) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required]]
    });
  }

  select(category: Category): void {
    this.errorMessage = null;
    this.selectedCategory = category;
    this.form.patchValue({ name: category.name });
  }  

  create(): void {
    this.errorMessage = null;

    if (this.form.invalid) return;

    this.loading = true;

    this.categoryService.create(this.form.value).subscribe({
      next: () => {
        this.loading = false;
        this.reset();
        this.saved.emit();
      },
      error: (err) => {
        this.loading = false;
        if(err.status === 400){
          this.errorMessage = err.error;
        }else {
          this.errorMessage = 'An unexpected error occurred.';
        }
      }
    });
  }

  update(): void {
    this.errorMessage = null;

    if (!this.selectedCategory || this.form.invalid) return;

    this.loading = true;

    this.categoryService.update(this.selectedCategory.id, this.form.value).subscribe({
      next: () => {
        this.reset();
        this.saved.emit();
      },
      error: (err) => {
        this.loading = false;
        if (err.status === 400) {
          this.errorMessage = err.error;
        } else {
          this.errorMessage = 'An unexpected error occurred.';
        }
      }
    });
  }

  delete(category: Category): void {
    this.errorMessage = null;
    this.loading = true;

    this.categoryService.delete(category.id).subscribe({
      next: () => {
        this.reset();
        this.saved.emit();
      },
      error: (err) => {
        this.loading = false;
        if (err.status === 400) {
          this.errorMessage = err.error;
        } else {
          this.errorMessage = 'An unexpected error occurred.';
        }
      }
    });
  }

  reset(): void {
    this.errorMessage = null;
    this.loading = false;
    this.selectedCategory = undefined;
    this.form.reset();
  }
}