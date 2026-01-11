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

  isAuthenticated(): boolean {
    return !!this.get();
  }

  getPayload(): any | null {
  const token = this.get();
  if (!token) return null;

  const parts = token.split('.');
  if (parts.length !== 3) return null;

  try {
    const base64 = parts[1].replace(/-/g, '+').replace(/_/g, '/');
    const json = decodeURIComponent(
      atob(base64).split('').map(c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)).join('')
    );
    return JSON.parse(json);
  } catch {
    return null;
  }
}

  getRoles(): string[] {
    const payload = this.getPayload();
    if (!payload) return [];

    const role =
      payload['role'] ??
      payload['roles'] ??
      payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];

    if (!role) return [];
    return Array.isArray(role) ? role : [String(role)];
  }

  hasRole(role: string): boolean {
    return this.getRoles().includes(role);
  }
}