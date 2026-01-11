import { Routes } from '@angular/router';
import { AdminGuard } from './core/guards/admin.guard';
import { AuthGuard } from './core/guards/auth.guard';

export const appRoutes: Routes = [
  {
    path: '',
    redirectTo: 'books',
    pathMatch: 'full'
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./features/auth/auth.routes').then(r => r.authRoutes)
  },
  {
    path: 'books',
    loadChildren: () =>
      import('./features/books/books.routes').then(r => r.booksRoutes)
  },
  {
    path: 'admin',
    loadChildren: () =>
      import('./features/admin/admin.routes').then(r => r.adminRoutes),
    canActivate: [AdminGuard, AuthGuard]
  },
  {
    path: 'library',
    loadChildren: () =>
      import('./features/library/library.routes').then(r => r.libraryRoutes),
    canActivate: [AuthGuard]
  }
];
