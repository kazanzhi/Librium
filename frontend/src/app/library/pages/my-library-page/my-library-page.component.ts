import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/shared/models/book';
import { LibraryService } from '../../services/library.service';

@Component({
    selector: 'app-my-library-page',
    templateUrl: './my-library-page.component.html',
    styleUrls: ['./my-library-page.component.scss'],
    standalone: false
})
export class MyLibraryPageComponent implements OnInit {
  books: Book[] = [];
  loading = false;

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
      error: (error) => {
        console.error('Error loading library:', error);
        this.loading = false;
      }
    });
  }

  remove(bookId: string): void {
    this.loading = true;

    this.libraryService.removeBook(bookId).subscribe({
      next: () => this.loadMyLibrary(),
      error: error => {
        console.error(error);
        this.loading = false;
      }
    });
  }
}