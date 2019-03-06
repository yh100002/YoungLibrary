import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

import { User } from '../_models';

@Injectable({ providedIn: 'root' })
export class UserService {
    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }  

    //https://localhost:5001/api/auth/register
    register(user: User) {
        return this.http.post(`${this.baseUrl}auth/register`, user);
    }
    
}