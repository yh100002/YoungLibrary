import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AddHeaderInterceptor } from './_intercepter/addheader';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatInputModule,
  MatPaginatorModule,
  MatProgressSpinnerModule,
  MatSortModule,
  MatTableModule,
  MatIconModule,
  MatButtonModule,
  MatCardModule,
  MatFormFieldModule } from '@angular/material';
import { PaginationModule } from 'ngx-bootstrap';
import { NgxJsonViewerModule } from 'ngx-json-viewer';
import { AlertComponent } from './_components';
import { JwtInterceptor, ErrorInterceptor } from './_intercepter';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AppComponent } from './app.component';
import { BooksComponent, HighlightSearch } from './books/books.component';
import { BookDetailComponent } from './book-detail/book-detail.component';
import { BookAddComponent } from './book-add/book-add.component';
import { BookEditComponent } from './book-edit/book-edit.component';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { StatisticComponent } from './statistic/statistic.component';

@NgModule({
  declarations: [
    AppComponent,
    BooksComponent,
    BookDetailComponent,
    BookAddComponent,
    BookEditComponent,
    AlertComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    StatisticComponent,
    HighlightSearch
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatInputModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatProgressSpinnerModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule,
    MatFormFieldModule,
    PaginationModule.forRoot(),
    NgxJsonViewerModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AddHeaderInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

