import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/books.service';
import { Book } from 'src/app/shared/models/book';

@Component({
  selector: 'app-book-page',
  templateUrl: './book-page.component.html',
  styleUrls: ['./book-page.component.scss']
})
export class BookPageComponent implements OnInit {
  books: Book[] = [];
  loading = true;

  constructor(private booksService: BookService) { }

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
}