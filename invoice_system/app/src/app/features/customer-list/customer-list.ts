import { Component } from '@angular/core';
import { Footer } from "../../layout/footer/footer";
import { Header } from "../../layout/header/header";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-customer-list',
  imports: [Footer, Header, RouterLink],
  templateUrl: './customer-list.html',
  styleUrl: './customer-list.css',
})
export class CustomerList {

}
