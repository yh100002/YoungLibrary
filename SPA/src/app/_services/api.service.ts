import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { catchError, tap, map } from 'rxjs/operators';
import { Book } from '../_models/book';
import { PaginatedResult } from '../_models/pagination';

const httpOptions = {
  headers: new HttpHeaders({'Content-Type': 'application/json'})
};
const apiUrl = "https://localhost:5001/api/book";

@Injectable({
  providedIn: 'root'
})
export class ApiService {    
  constructor(private http: HttpClient) { }

  getBooks (page = 0): Observable<PaginatedResult<Book[]>> {
    let paginatedResult: PaginatedResult<Book[]> = new PaginatedResult<Book[]>();
    const url = `${apiUrl}/getBooks?page=${page}&size=2`;
    return this.http.get<PaginatedResult<Book[]>>(url)
      .pipe(
        
        map(response => {
          paginatedResult = response;  
          console.log(response);
          return paginatedResult;
        })     
      );
  }

  getBook(id: string): Observable<Book> {
    const url = `${apiUrl}/getBook/${id}`;
    return this.http.get<Book>(url).pipe(
      tap(_ => console.log(`fetched book id=${id}`)),
      catchError(this.handleError<Book>(`getBook id=${id}`))
    );
  }

  addBook (book): Observable<Book> {
    const url = `${apiUrl}/addBook/`;
    return this.http.post<Book>(url, book, httpOptions).pipe(
      // tslint:disable-next-line:no-shadowed-variable
      tap((book: Book) => console.log(`added book w/ id=${book.id}`)),
      catchError(this.handleError<Book>('addBook'))
    );
  }

  updateBook (id, book): Observable<any> {    
    const url = `${apiUrl}/update/`;
    book.id = id;
    return this.http.put<Book>(url, book, httpOptions).pipe(
      tap(_ => console.log(`updated book id=${book.desc}`)),
      catchError(this.handleError<Book>('updateBook'))
    );
  }

  deleteBook (id: string): Observable<Book> {
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
