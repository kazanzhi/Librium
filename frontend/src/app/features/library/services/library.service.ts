import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { Book } from 'src/app/shared/models/book';
import { catchError, map, tap } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LibraryService {
  private readonly apiUrl = 'http://localhost:5106/api/userlibrary';
  private myLibrary: Book[] | null = null;

  constructor(private http: HttpClient) { }

  getMyLibrary(): Observable<Book[]> {
  return this.http.get<Book[]>(this.apiUrl).pipe(
    tap(books => this.myLibrary = books),
    catchError(err => {
      if (err.status === 401) {
        return of([]);
      }
      throw err;
    })
  );
}

  isInLibrary(bookId: string): Observable<boolean> {
    return this.getMyLibrary().pipe(
      map(books => books.some(b => b.id === bookId))
    );
  }

  addBook(bookId: string) {
    return this.http.post(`${this.apiUrl}/${bookId}`, {});
  }

  removeBook(bookId: string) {
    return this.http.delete(`${this.apiUrl}/${bookId}`);
  }

}
