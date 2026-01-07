import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/shared/models/book';
import { BookService } from '../../services/books.service';
import { ActivatedRoute } from '@angular/router';
import { LibraryService } from 'src/app/library/services/library.service';

@Component({
    selector: 'app-book-details-page',
    templateUrl: './book-details-page.component.html',
    styleUrls: ['./book-details-page.component.scss'],
    standalone: false
})
export class BookDetailsPageComponent implements OnInit {
  book?: Book;
  loading = true;
  isInMyLibrary = false;

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private libraryService: LibraryService
  ) { }

  ngOnInit(): void {
    this.loadBook();
  }

  loadBook(): void {
  const id = this.route.snapshot.paramMap.get('id');
  if (!id) {
    this.loading = false;
    return;
  }

  this.bookService.getById(id).subscribe({
    next: book => {
      this.book = book;

      this.libraryService.isInLibrary(book.id).subscribe(isIn => {
        this.isInMyLibrary = isIn;
        this.loading = false;
      });
    },
    error: err => {
      console.error(err);
      this.loading = false;
    }
  });
  }


  addToLibrary(): void {
    this.libraryService.addBook(this.book!.id).subscribe({
      next: () => {
        console.log('Book added to library');
        this.isInMyLibrary = true;
      },
      error: err => {
        console.error('Error adding book to library:', err);
      }
    });
  }

  removeFromLibrary(): void {
    this.libraryService.removeBook(this.book!.id).subscribe({
      next: () => {
        console.log('Book removed from library');
        this.isInMyLibrary = false;
      },
      error: err => {
        console.error('Error removing book from library:', err);
      }
    });
  }
}
