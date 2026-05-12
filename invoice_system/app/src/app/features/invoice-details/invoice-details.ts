import { Component } from '@angular/core';
import { Header } from "../../layout/header/header";
import { Footer } from "../../layout/footer/footer";
import { PaymentService } from '../../core/services/payment.service';

@Component({
  selector: 'app-invoice-details',
  imports: [Header, Footer],
  templateUrl: './invoice-details.html',
  styleUrl: './invoice-details.css',
})
export class InvoiceDetails {
invoice: {id: number, customerName: string, amount: number} = {
  id: 123,
  customerName: 'John Doe',
  amount: 250.00
};
constructor(
  private paymentService: PaymentService
) {
}

payNow() {

  this.paymentService
    .createCheckoutSession(this.invoice.id)
    .subscribe({

      next: response => {

        window.location.href = response.url;

      }

    });

}
}
