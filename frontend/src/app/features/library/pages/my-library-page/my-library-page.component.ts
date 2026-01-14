import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/shared/models/book';
import { LibraryService } from '../../services/library.service';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { SearchService } from 'src/app/core/services/search.service';
import { debounceTime, distinctUntilChanged } from 'rxjs';

@Component({
    selector: 'app-my-library-page',
    templateUrl: './my-library-page.component.html',
    styleUrls: ['./my-library-page.component.scss'],
    standalone: true,
    imports: [CommonModule, RouterLink]

})
export class MyLibraryPageComponent implements OnInit {
  allBooks: Book[] = [];
  books: Book[] = [];
  loading = false;
  removeError: string | null = null;

  constructor(
    private libraryService: LibraryService, 
    private searchService: SearchService
  ) { }

  ngOnInit(): void {
    this.loadMyLibrary();

    this.searchService.search$
      .pipe(
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(search => {
        this.applyFilter(search);
      });
  }

  private applyFilter(search: string): void {
    if (!search) {
      this.books = this.allBooks;
      return;
    }

    const value = search.toLowerCase();

    this.books = this.allBooks.filter(book =>
      book.title.toLowerCase().includes(value) ||
      book.author.toLowerCase().includes(value)
    );
  }

  loadMyLibrary(): void {
    this.loading = true;

    this.libraryService.getMyLibrary().subscribe({
      next: (books) => {
        this.allBooks = books;
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