import { Component, OnInit } from '@angular/core';
import { ApiService } from '../api.service';
import { Book } from '../_models/book';
import { Pagination, PaginatedResult } from '../_models/pagination';

@Component({
  selector: 'app-books',
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.scss']
})
export class BooksComponent implements OnInit {
  pagination: Pagination;
  displayedColumns: string[] = ['name', 'price'];
  data: Book[] = [];
  isLoadingResults = true;

  constructor(private api: ApiService) { }

  ngOnInit() {
    this.api.getBooks()
      .subscribe(res => {
        this.data = res.items;
        //this.pagination.from = res.from;
        console.log(res);
        console.log(res.from);
        console.log(this.data);
        console.log(this.pagination);
        this.isLoadingResults = false;
      }, err => {
        console.log(err);
        this.isLoadingResults = false;
      });
  }

}
