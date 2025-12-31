import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LibraryBookCardComponent } from './library-book-card.component';

describe('LibraryBookCardComponent', () => {
  let component: LibraryBookCardComponent;
  let fixture: ComponentFixture<LibraryBookCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LibraryBookCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LibraryBookCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
