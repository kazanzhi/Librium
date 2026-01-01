import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  private readonly key = 'librium_token';

  save(token: string): void {
    localStorage.setItem(this.key, token);
  }

  get(): string | null {
    return localStorage.getItem(this.key);
  }

  clear(): void {
    localStorage.removeItem(this.key);
  }
}
