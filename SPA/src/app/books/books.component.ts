import {Component, NgModule, VERSION, Pipe, PipeTransform, OnInit} from '@angular/core';
import {BrowserModule, DomSanitizer} from '@angular/platform-browser';
import { ApiService } from '../_services/api.service';
import { Book } from '../_models/book';
import { Pagination, PaginatedResult } from '../_models/pagination';
import { PaginationComponent } from 'ngx-bootstrap';


@Pipe({
  name: 'highlight'
})
export class HighlightSearch implements PipeTransform {
constructor(private sanitizer: DomSanitizer){}

transform(value: any, args: any): any {
  if (!args) {
    return value;
  }
  // Match in a case insensitive maneer
  const re = new RegExp(args, 'gi');
  const match = value.match(re);

  // If there's no match, just return the original value.
  if (!match) {
    return value;
  }

  const replacedValue = value.replace(re, "<mark>" + match[0] + "</mark>")
  return this.sanitizer.bypassSecurityTrustHtml(replacedValue);
}
}


@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.scss']
})
export class BooksComponent implements OnInit {
  pagination : Pagination = { from: 0, count:0, index:0, pages:0, size:0, hasPrevious:false, hasNext:false};
  displayedColumns: string[] = ['name', 'desc', 'price', 'updated_at'];
  data: Book[] = [];
  isLoadingResults = true;
  searchTerm: string;

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.api.getBooks(1)
      .subscribe(res => {
        this.data = res.items;       
        this.pagination.from = res.from;
        this.pagination.count = res.count;
        this.pagination.index = res.index;
        this.pagination.pages = res.pages;
        this.pagination.size = res.size;       
        this.isLoadingResults = false;
      }, err => {
        console.log(err);
        this.isLoadingResults = false;
      });
  }

  pageChanged(event: any): void {
    this.pagination.index = event.page;
    this.loadAll();   
  }

  loadAll()
  {
    console.log(this.pagination.index);
    this.api.getBooks(this.pagination.index)
    .subscribe(res => {
      this.data = res.items;
      this.isLoadingResults = false;
      this.pagination.from = res.from;
      this.pagination.count = res.count;
      this.pagination.index = res.index;
      this.pagination.pages = res.pages;
      this.pagination.size = res.size;       
    }, err => {
      console.log(err);
      this.isLoadingResults = false;
    });    
  }
  
  updateSearch(e) {
    this.searchTerm = e.target.value;
  }

}
