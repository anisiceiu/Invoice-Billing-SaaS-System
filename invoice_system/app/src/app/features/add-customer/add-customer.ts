import { Component } from '@angular/core';
import { Header } from "../../layout/header/header";
import { Footer } from "../../layout/footer/footer";
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-add-customer',
  imports: [Header, Footer,RouterLink],
  templateUrl: './add-customer.html',
  styleUrl: './add-customer.css',
})
export class AddCustomer {

}
