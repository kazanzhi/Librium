import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/books.service';
import { Book } from 'src/app/shared/models/book';
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
  errorMessage: string | null = null;

  constructor(
    private booksService: BookService
  ) { }

  ngOnInit(): void {
    this.booksService.getAll().subscribe({
      next: books => {
        this.books = books;
        this.loading = false;
      }
    })
  }
}