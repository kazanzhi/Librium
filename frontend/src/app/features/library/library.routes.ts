import { Routes } from "@angular/router";

export const libraryRoutes: Routes = [
    {
        path: '',
        loadComponent: () =>
            import('./pages/my-library-page/my-library-page.component')
            .then(c => c.MyLibraryPageComponent)
    }
];