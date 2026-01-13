import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/shared/models/book';
import { LibraryService } from '../../services/library.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-my-library-page',
    templateUrl: './my-library-page.component.html',
    styleUrls: ['./my-library-page.component.scss'],
    standalone: true,
    imports: [CommonModule, RouterLink]

})
export class MyLibraryPageComponent implements OnInit {
  books: Book[] = [];
  loading = false;
  removeError: string | null = null;

  constructor(private libraryService: LibraryService) { }

  ngOnInit(): void {
    this.loadMyLibrary();
  }

  loadMyLibrary(): void {
    this.loading = true;

    this.libraryService.getMyLibrary().subscribe({
      next: (books) => {
        this.books = books;
        this.loading = false;
      },
      error: err => {
        this.loading = false;
      }
    });
  }

  remove(bookId: string): void {
    this.removeError = null;
    this.loading = true;

    this.libraryService.removeBook(bookId).subscribe({
      next: () => this.loadMyLibrary(),
      error: err => {
        this.loading = false;
        if (err.status === 400 && typeof err.error === 'string') {
          this.removeError = err.error;
        } else {
          this.removeError = 'Failed to remove book.';
        }
      }
    });
  }
}