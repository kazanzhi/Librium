import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { AdminDashboardComponent } from './pages/admin-dashboard/admin-dashboard.component';
import { BookEditorComponent } from './components/book-editor/book-editor.component';
import { CategoryEditorComponent } from './components/category-editor/category-editor.component';


@NgModule({
  declarations: [
    AdminDashboardComponent,
    BookEditorComponent,
    CategoryEditorComponent
  ],
  imports: [
    CommonModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
