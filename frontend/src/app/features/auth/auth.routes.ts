import { Routes } from "@angular/router";

export const authRoutes: Routes = [
    {
        path: '',
        loadComponent: () => 
            import('./pages/auth-page/auth-page.component')
            .then(c => c.AuthPageComponent),
    }
];