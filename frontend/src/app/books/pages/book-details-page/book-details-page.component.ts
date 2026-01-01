import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/shared/models/book';
import { BooksService } from '../../services/books.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-book-details-page',
  templateUrl: './book-details-page.component.html',
  styleUrls: ['./book-details-page.component.scss']
})
export class BookDetailsPageComponent implements OnInit {
  book?: Book;
  loading = true;

  constructor(
    private route: ActivatedRoute,
    private bookService: BooksService
  ) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if (!id) {
      this.loading = false;
      return;
    }

    this.bookService.getById(id).subscribe({
      next: book => {
        this.book = book;
        this.loading = false;
      },
      error: err => {
        console.error(err);
        this.loading = false;
      }
    });
  }

}
