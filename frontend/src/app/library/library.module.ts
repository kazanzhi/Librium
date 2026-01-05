import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LibraryRoutingModule } from './library-routing.module';
import { MyLibraryPageComponent } from './pages/my-library-page/my-library-page.component';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [
    MyLibraryPageComponent
  ],
  imports: [
    CommonModule,
    LibraryRoutingModule,
    RouterModule
  ]
})
export class LibraryModule { }
