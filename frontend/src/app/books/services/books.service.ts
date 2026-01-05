import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from 'src/app/shared/models/book';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private readonly apiUrl = 'http://localhost:5106/api/book';

  constructor(private http: HttpClient) { }

  getAll() : Observable<Book[]> {
    return this.http.get<Book[]>(`${this.apiUrl}`);
  }

  getById(id: string) : Observable<Book> {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }
}
