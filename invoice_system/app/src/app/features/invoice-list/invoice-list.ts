import { Component } from '@angular/core';
import { Header } from "../../layout/header/header";
import { Footer } from "../../layout/footer/footer";
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-invoice-list',
  imports: [Header, Footer, RouterLink],
  templateUrl: './invoice-list.html',
  styleUrl: './invoice-list.css',
})
export class InvoiceList {

}
