import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Category } from 'src/app/shared/models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  constructor(private http: HttpClient) { }

  private readonly apiUrl = 'http://localhost:5106/api/category';

  getAll(): Observable<Category[]> {
    return this.http.get<Category[]>(this.apiUrl);
  }

  create(categoryName: string): Observable<void> {
    return this.http.post<void>(this.apiUrl, categoryName);
  }

  update(categoryId: string, categoryName: string): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${categoryId}`, categoryName);
  }

  delete(categoryId: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${categoryId}`);
  }
}