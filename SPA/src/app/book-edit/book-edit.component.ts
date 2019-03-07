import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ApiService } from '../_services/api.service';
import { FormControl, FormGroupDirective, FormBuilder, FormGroup, NgForm, Validators } from '@angular/forms';
import { Book } from '../_models';

@Component({
  selector: 'app-book-edit',
  templateUrl: './book-edit.component.html',
  styleUrls: ['./book-edit.component.scss']
})
export class BookEditComponent implements OnInit {

  bookForm: FormGroup;
  id:string='';
  name:string='';
  desc:string='';
  price:number=null;
  isLoadingResults = false;

  constructor(private router: Router, private route: ActivatedRoute, private api: ApiService, private formBuilder: FormBuilder) { }

  ngOnInit() {
    this.getBook(this.route.snapshot.params['id']);
    this.bookForm = this.formBuilder.group({
      'name' : [null, Validators.required],
      'desc' : [null, Validators.required],
      'price' : [null, Validators.required]
    });
  }

  getBook(id:string) {
    this.api.getBook(id).subscribe(data => {
      this.id = data.id;
      this.bookForm.setValue({        
        name: data.name,
        desc: data.desc,
        price: data.price
      });
    });
  }

  onFormSubmit(form:NgForm) {
    this.isLoadingResults = true;        
    this.api.updateBook(this.id, form)
      .subscribe(res => {          
          this.isLoadingResults = false;
          this.router.navigate(['/book-detail', this.id]);
        }, (err) => {
          console.log(err);
          this.isLoadingResults = false;
        }
      );
  }

  bookDetails() {
    this.router.navigate(['/book-detail', this.id]);
  }

}
