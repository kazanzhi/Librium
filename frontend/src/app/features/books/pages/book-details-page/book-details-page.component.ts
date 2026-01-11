import { Component, OnInit } from '@angular/core';
import { Book } from 'src/app/shared/models/book';
import { BookService } from '../../services/books.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { LibraryService } from 'src/app/features/library/services/library.service';
import { CommonModule } from '@angular/common';
import { TokenService } from 'src/app/core/services/token.service';

@Component({
    selector: 'app-book-details-page',
    templateUrl: './book-details-page.component.html',
    styleUrls: ['./book-details-page.component.scss'],
    standalone: true,
    imports: [CommonModule, RouterLink]
})
export class BookDetailsPageComponent implements OnInit {
  book?: Book;
  loading = true;

  isAuthenticated = false;  
  isInMyLibrary = false;

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private libraryService: LibraryService,
    private tokenService: TokenService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadBook();
  }

  loadBook(): void {
    this.isAuthenticated = this.tokenService.isAuthenticated();

    const id = this.route.snapshot.paramMap.get('id');
    if (!id) {
      this.loading = false;
      return;
    }

     this.bookService.getById(id).subscribe({
      next: book => {
        this.book = book;
        this.loading = false;
        
        if (this.isAuthenticated) {
          this.libraryService.isInLibrary(book.id).subscribe(isIn => {
            this.isInMyLibrary = isIn;
            this.loading = false;
          });
        } else {
          this.loading = false;
        }
      },
      error: () => {
        this.loading = false;
      }
    });
  }

  onLibraryAction(): void {
    if (!this.isAuthenticated) {
      this.router.navigate(['/auth']);
      return;
    }

    if (this.isInMyLibrary) {
      this.libraryService.removeBook(this.book!.id).subscribe(() => {
        this.isInMyLibrary = false;
      });
    } else {
      this.libraryService.addBook(this.book!.id).subscribe(() => {
        this.isInMyLibrary = true;
      });
    }
  }
}