import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { Book } from './book';

const httpOptions = {
  headers: new HttpHeaders({'Content-Type': 'application/json'})
};
const apiUrl = "http://localhost:3000/api/v1/books";

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  constructor(private http: HttpClient) { }

  getBooks (): Observable<Book[]> {
    return this.http.get<Book[]>(apiUrl)
      .pipe(
        tap(books => console.log('Fetch books')),
        catchError(this.handleError('getBooks', []))
      );
  }

  getBook(id: number): Observable<Book> {
    const url = `${apiUrl}/${id}`;
    return this.http.get<Book>(url).pipe(
      tap(_ => console.log(`fetched book id=${id}`)),
      catchError(this.handleError<Book>(`getBook id=${id}`))
    );
  }

  addBook (book): Observable<Book> {
    return this.http.post<Book>(apiUrl, book, httpOptions).pipe(
      // tslint:disable-next-line:no-shadowed-variable
      tap((book: Book) => console.log(`added book w/ id=${book.id}`)),
      catchError(this.handleError<Book>('addBook'))
    );
  }

  updateBook (id, book): Observable<any> {
    const url = `${apiUrl}/${id}`;
    return this.http.put(url, book, httpOptions).pipe(
      tap(_ => console.log(`updated book id=${id}`)),
      catchError(this.handleError<any>('updateBook'))
    );
  }

  deleteBook (id): Observable<Book> {
    const url = `${apiUrl}/${id}`;

    return this.http.delete<Book>(url, httpOptions).pipe(
      tap(_ => console.log(`deleted book id=${id}`)),
      catchError(this.handleError<Book>('deleteBook'))
    );
  }

  private handleError<T> (operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
