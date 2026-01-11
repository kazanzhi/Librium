import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Book } from 'src/app/shared/models/book';

@Component({
    selector: 'app-book-card',
    templateUrl: './book-card.component.html',
    styleUrls: ['./book-card.component.scss'],
    standalone: true,
    imports: [CommonModule, RouterLink]
})
export class BookCardComponent{
  @Input() book!: Book;
}