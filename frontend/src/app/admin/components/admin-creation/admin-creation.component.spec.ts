import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminCreationComponent } from './admin-creation.component';

describe('AdminCreationComponent', () => {
  let component: AdminCreationComponent;
  let fixture: ComponentFixture<AdminCreationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminCreationComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminCreationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
