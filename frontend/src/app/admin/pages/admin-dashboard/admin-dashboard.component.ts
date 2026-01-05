import { Component, OnInit, SimpleChanges } from '@angular/core';
import { Book } from 'src/app/shared/models/book';
import { Category } from 'src/app/shared/models/category';
import { CategoryService } from 'src/app/categories/services/category.service';
import { BookService} from 'src/app/books/services/books.service';
import { TokenService } from 'src/app/core/services/token.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.scss']
})
export class AdminDashboardComponent implements OnInit {
  books: Book[] = [];
  categories: Category[] = [];

  selectedBook?: Book;
  loading = true;

  showManageCategories = false;
  showAdminCreation = false;
  showManageBooks = false;
  
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
    this.loading = true;

    this.bookService.getAll().subscribe({
      next: (books) => {
        this.books = books;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error fetching books:', err);
        this.loading = false;
      }
    });

    this.categoryService.getAll().subscribe({
      next: (categories) => {
        this.categories = categories;
      },
      error: (err) => {
        console.error('Error fetching categories:', err);
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