import { Component, OnInit } from '@angular/core';
import { BookService } from '../../services/books.service';
import { Book } from 'src/app/shared/models/book';
import { CommonModule } from '@angular/common';
import { BookCardComponent } from '../../components/book-card/book-card.component';
import { SearchService } from 'src/app/core/services/search.service';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs';

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
    private searchService: SearchService
  ) { }

  ngOnInit(): void {
    this.searchService.search$
      .pipe(
        debounceTime(300),
        distinctUntilChanged(),
        switchMap(search => {
          this.loading = true;
          return this.booksService.getAll(search);
        })
      )
      .subscribe({
        next: books => {
          this.books = books;
          this.loading = false;
        },
        error: () => {
          this.loading = false;
        }
      });
  }
}
