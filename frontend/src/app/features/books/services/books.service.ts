import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Book } from 'src/app/shared/models/book';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private readonly apiUrl = 'http://localhost:5106/api/book';

  constructor(private http: HttpClient) { }

  getAll(search?: string) {
    let params = new HttpParams();

    if (search) {
      params = params.set('search', search);
    }
    return this.http.get<Book[]>(this.apiUrl, { params });
  }

  getById(id: string) {
    return this.http.get<Book>(`${this.apiUrl}/${id}`);
  }
}
