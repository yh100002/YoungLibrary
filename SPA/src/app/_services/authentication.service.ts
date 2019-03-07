import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { User } from '../_models';

@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    baseUrl = environment.apiUrl;
    private currentUserSubject: BehaviorSubject<string>;
    public currentUser: Observable<string>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<string>(localStorage.getItem('currentUser'));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): string {
        return this.currentUserSubject.value;
    }

    //https://localhost:5001/api/auth/login
    login(username: string, password: string) {
        return this.http.post<any>(`${this.baseUrl}auth/login`, { username, password })
            .pipe(map(token => {
                // login successful if there's a jwt token in the response                
                if (token) {
                    console.log(token.token);
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', token.token);
                    this.currentUserSubject.next(token.token);
                }

                return token.token;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }
}