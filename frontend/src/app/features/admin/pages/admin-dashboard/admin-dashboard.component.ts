import { Component, OnInit, SimpleChanges } from '@angular/core';
import { Book } from 'src/app/shared/models/book';
import { Category } from 'src/app/shared/models/category';
import { CategoryService } from 'src/app/features/categories/services/category.service';
import { BookService} from 'src/app/features/books/services/books.service';
import { TokenService } from 'src/app/core/services/token.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CategoryEditorComponent } from '../../components/category-editor/category-editor.component';
import { BookEditorComponent } from '../../components/book-editor/book-editor.component';
import { AdminCreationComponent } from '../../components/admin-creation/admin-creation.component';

@Component({
    selector: 'app-admin-dashboard',
    templateUrl: './admin-dashboard.component.html',
    styleUrls: ['./admin-dashboard.component.scss'],
    standalone: true,
    imports: [CommonModule, CategoryEditorComponent, BookEditorComponent, AdminCreationComponent]
})
export class AdminDashboardComponent implements OnInit {
  books: Book[] = [];
  categories: Category[] = [];

  selectedBook?: Book;
  loading = true;

  showManageCategories = false;
  showAdminCreation = false;
  showManageBooks = false;
  errorCategoryMessage: string | null = null;
  errorBookMessage: string | null = null;
  
  constructor(
    private bookService: BookService,
    private categoryService: CategoryService,
    private tokenService: TokenService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadAll();
  }

  loadAll(): void {
    this.errorBookMessage = null;
    this.errorCategoryMessage = null;

    this.loading = true;

    this.bookService.getAll().subscribe({
      next: (books) => {
        this.books = books;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        if(err.status === 400){
          this.errorBookMessage = err.error;
        } else {
          this.errorBookMessage = 'An unexpected error occurred.';
        }
      }
    });

    this.categoryService.getAll().subscribe({
      next: (categories) => {
        this.categories = categories;
      }
    });
  }

  edit(book: Book): void {
    this.selectedBook = book;
    this.showManageBooks = true;
  }

  delete(book: Book): void {
    this.selectedBook = book;
  }

  createNew(): void {
    this.selectedBook = undefined;
    this.showManageBooks = true;
  }
  onBookSaved(): void {
    this.loadAll();
    this.showManageBooks = false;
    this.selectedBook = undefined;
  }
  
  logout(): void {
    this.tokenService.clear();
    this.router.navigate(['/auth']);
  }
}