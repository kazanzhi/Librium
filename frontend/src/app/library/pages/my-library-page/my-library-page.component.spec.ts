import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyLibraryPageComponent } from './my-library-page.component';

describe('MyLibraryPageComponent', () => {
  let component: MyLibraryPageComponent;
  let fixture: ComponentFixture<MyLibraryPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyLibraryPageComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyLibraryPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
