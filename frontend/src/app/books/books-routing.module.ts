import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookPageComponent } from './pages/book-page/book-page.component';
import { BookDetailsPageComponent } from './pages/book-details-page/book-details-page.component';

const routes: Routes = [
  {
    path: '',
    component: BookPageComponent
  },
  {
    path: ':id',
    component: BookDetailsPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class BooksRoutingModule { }
