import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BookDto } from 'src/app/shared/dto/bookDto';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private readonly apiUrl = 'http://localhost:5106/api/auth';

  constructor(private http: HttpClient) { }

  registerAdmin(dto: { email: string; username: string; password: string }): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/register-admin`, dto);
  }
}
