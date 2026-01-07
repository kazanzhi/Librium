import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { CategoryService } from 'src/app/categories/services/category.service';
import { Category } from 'src/app/shared/models/category';

@Component({
  selector: 'app-category-editor',
  templateUrl: './category-editor.component.html',
  styleUrls: ['./category-editor.component.scss']
})
export class CategoryEditorComponent implements OnInit {
  @Input() categories: Category[] = [];
  @Output() saved = new EventEmitter<void>();

  form!: UntypedFormGroup;
  selectedCategory?: Category;
  loading = false;

  constructor(private categoryService: CategoryService, private fb: UntypedFormBuilder) { }

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required]]
    });
  }

  select(category: Category): void {
    this.selectedCategory = category;
    this.form.patchValue({ name: category.name });
  }  

  create(): void {
    if (this.form.invalid) return;

    this.loading = true;

    this.categoryService.create(this.form.value).subscribe({
      next: () => {
        this.loading = false;
        this.saved.emit();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  update(): void {
    if (!this.selectedCategory || this.form.invalid) return;

    this.loading = true;

    this.categoryService.update(this.selectedCategory.id, this.form.value).subscribe({
      next: () => {
        this.reset();
        this.saved.emit();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  delete(category: Category): void {
    this.loading = true;

    this.categoryService.delete(category.id).subscribe({
      next: () => {
        this.reset();
        this.saved.emit();
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  reset(): void {
    this.loading = false;
    this.selectedCategory = undefined;
    this.form.reset();
  }
}