import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { BooksRoutingModule } from './books-routing.module';
import { BookPageComponent } from './pages/book-page/book-page.component';
import { BookCardComponent } from './components/book-card/book-card.component';


@NgModule({
  declarations: [
    BookPageComponent,
    BookCardComponent
  ],
  imports: [
    CommonModule,
    BooksRoutingModule
  ]
})
export class BooksModule { }
