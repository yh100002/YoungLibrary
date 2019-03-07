import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BooksComponent } from './books/books.component';
import { BookDetailComponent } from './book-detail/book-detail.component';
import { BookAddComponent } from './book-add/book-add.component';
import { BookEditComponent } from './book-edit/book-edit.component';
import { AuthGuard } from './_guards';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';
import { StatisticComponent } from './statistic/statistic.component';

const routes: Routes = [
  { path: '', component: HomeComponent, canActivate: [AuthGuard] },
  {
    path: 'books',
    component: BooksComponent,
    data: { title: 'List of Books' }
  },
  {
    path: 'book-detail/:id',
    component: BookDetailComponent,
    data: { title: 'Book Details' }
  },
  {
    path: 'book-add',
    component: BookAddComponent,
    data: { title: 'Add Book' }
  },
  {
    path: 'book-edit/:id',
    component: BookEditComponent,
    data: { title: 'Edit Book' }
  },
  { 
    path: 'login', 
    component: LoginComponent 
  },
  { 
    path: 'register', 
    component: RegisterComponent 
  },
  { 
    path: 'statistic', 
    component: StatisticComponent 
  },
  // otherwise redirect to home
  { path: '**', redirectTo: '' }
  //{ path: '', redirectTo: '/books',  pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
