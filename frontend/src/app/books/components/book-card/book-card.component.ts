import { Component, Input } from '@angular/core';
import { Book } from 'src/app/shared/models/book';

@Component({
    selector: 'app-book-card',
    templateUrl: './book-card.component.html',
    styleUrls: ['./book-card.component.scss'],
    standalone: false
})
export class BookCardComponent{
  @Input() book!: Book;
}