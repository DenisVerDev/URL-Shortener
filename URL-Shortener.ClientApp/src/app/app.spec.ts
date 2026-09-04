import { provideHttpClient } from '@angular/common/http';
import {
  provideHttpClientTesting
} from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';

import { App } from './app';

describe('App', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        App
      ],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    }).compileComponents();
  });

  it('should create the app', () => {
    const fixture = TestBed.createComponent(App);
    const app = fixture.componentInstance;

    expect(app).toBeTruthy();
  });

  it('should render the URL form', async () => {
    const fixture = TestBed.createComponent(App);

    await fixture.whenStable();

    const compiled =
      fixture.nativeElement as HTMLElement;

    expect(
      compiled.querySelector('h2')?.textContent
    ).toContain('Shorten a URL');

    expect(
      compiled.querySelector(
        'input[type="url"]')
    ).not.toBeNull();

    expect(
      compiled.querySelector(
        'button[type="submit"]')
    ).not.toBeNull();
  });
});
