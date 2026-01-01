import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LibraryRoutingModule } from './library-routing.module';
import { MyLibraryPageComponent } from './pages/my-library-page/my-library-page.component';
import { LibraryBookCardComponent } from './components/library-book-card/library-book-card.component';


@NgModule({
  declarations: [
    MyLibraryPageComponent,
    LibraryBookCardComponent
  ],
  imports: [
    CommonModule,
    LibraryRoutingModule
  ]
})
export class LibraryModule { }
