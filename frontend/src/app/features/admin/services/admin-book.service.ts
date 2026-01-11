import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookDto } from 'src/app/shared/dto/bookDto';

@Injectable({
  providedIn: 'root'
})
export class AdminBookService {

  private readonly apiUrl = 'http://localhost:5106/api/book';

  constructor(private http: HttpClient) { }

  create(bookDto : BookDto) : Observable<any> {
    return this.http.post<void>(this.apiUrl, bookDto);
  }

  update(bookId : string, bookDto : BookDto) : Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${bookId}`, bookDto);
  }

  delete(bookId : string) : Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${bookId}`);
  }

  addCategory(bookId: string, categoryName: string) : Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/${bookId}/categories/`, { name: categoryName });
  }

  removeCategory(bookId: string, categoryName: string) : Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${bookId}/categories/${categoryName}`);
  }
}