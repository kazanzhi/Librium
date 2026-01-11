import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/books.service';
import { Book } from 'src/app/shared/models/book';
import { TokenService } from 'src/app/core/services/token.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BookCardComponent } from '../../components/book-card/book-card.component';

@Component({
    selector: 'app-books-page',
    templateUrl: './books-page.component.html',
    styleUrls: ['./books-page.component.scss'],
    standalone: true,
    imports: [CommonModule, BookCardComponent]
})
export class BooksPageComponent implements OnInit {
  books: Book[] = [];
  loading = true;

  constructor(
    private booksService: BookService, 
    private tokenService: TokenService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.booksService.getAll().subscribe({
      next: books => {
        this.books = books;
        this.loading = false;
      },
      error: err => {
        console.error('Error fetching books:', err);
        this.loading = false;
      }
    })
  }

  logout(): void {
    this.tokenService.clear();
    this.router.navigate(['/auth']);
  }
}