import { Routes } from "@angular/router";

export const booksRoutes: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./pages/books-page/books-page.component')
            .then(c => c.BooksPageComponent)
    },
    {
        path: ':id',
        loadComponent: () =>
            import('./pages/book-details-page/book-details-page.component')
            .then(c => c.BookDetailsPageComponent)
    }
];