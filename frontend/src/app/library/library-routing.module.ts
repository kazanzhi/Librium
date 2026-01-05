import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyLibraryPageComponent } from './pages/my-library-page/my-library-page.component';

const routes: Routes = [
  {
    path: '',
    component: MyLibraryPageComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class LibraryRoutingModule { }
