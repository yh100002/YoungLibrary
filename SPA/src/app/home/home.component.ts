import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { first } from 'rxjs/operators';
import { User } from '../_models';
import { JwtHelperService } from '@auth0/angular-jwt';
import { UserService, AuthenticationService } from '../_services';

@Component({ templateUrl: 'home.component.html' })
export class HomeComponent implements OnInit {
    currentUser: string;
    currentUserSubscription: Subscription;    
    jwtHelper = new JwtHelperService();

    constructor(private authenticationService: AuthenticationService) {
       
        
      }

    ngOnInit() {
      this.currentUserSubscription = this.authenticationService.currentUser.subscribe(user => {
        this.currentUser = this.jwtHelper.decodeToken(user).unique_name;
        console.log(this.jwtHelper.decodeToken(user).unique_name);
      });
    }
    
}