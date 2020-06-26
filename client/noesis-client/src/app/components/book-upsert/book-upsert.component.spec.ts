import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BookUpsertComponent } from './book-upsert.component';

describe('BookUpsertComponent', () => {
  let component: BookUpsertComponent;
  let fixture: ComponentFixture<BookUpsertComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BookUpsertComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BookUpsertComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
