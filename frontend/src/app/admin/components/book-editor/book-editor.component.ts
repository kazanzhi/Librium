import { Component, OnInit, Input, Output, EventEmitter, SimpleChanges } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Book } from 'src/app/shared/models/book';
import { AdminBookService } from '../../services/admin-book.service';
import { Category } from 'src/app/shared/models/category';

@Component({
  selector: 'app-book-editor',
  templateUrl: './book-editor.component.html',
  styleUrls: ['./book-editor.component.scss']
})
export class BookEditorComponent implements OnInit {
  @Input() book?: Book;
  @Input() categories: Category[] = [];
  @Output() saved = new EventEmitter<void>();

  form!: FormGroup;
  loading = false;

  constructor(private fb: FormBuilder, private adminBookService: AdminBookService) { }

  ngOnInit(): void {
    this.buildForm();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['book']) {
      this.buildForm();
    }
  }

  private buildForm(): void {
    this.form = this.fb.group({
      title: [this.book?.title ?? '', Validators.required],
      author: [this.book?.author ?? '', Validators.required],
      content: [this.book?.content ?? '', Validators.required],
      publishedYear: [this.book?.publishedYear ?? '', Validators.required]
    });
  }

  submit(): void {
    if(this.form.invalid) return;
    
    this.loading = true;

    const request = this.book
      ? this.adminBookService.update(this.book.id, this.form.value)
      : this.adminBookService.create(this.form.value);
    
      request.subscribe({
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

  delete(): void {
    if(!this.book) return;

    this.loading = true;

    this.adminBookService.delete(this.book.id).subscribe({
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

  addCategory(categoryName: string): void {
    if (!this.book) return;

    this.adminBookService.addCategory(this.book.id, categoryName).subscribe({
      next: () => this.saved.emit(),
      error: err => console.error(err)
    });
  }

  removeCategory(categoryName: string): void {
    if (!this.book) return;

    this.adminBookService.removeCategory(this.book.id, categoryName).subscribe({
      next: () => this.saved.emit(),
      error: err => console.error(err)
    });
  }
}