import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class PaymentService {

  constructor(private http: HttpClient) {
  }

  createCheckoutSession(invoiceId: number) {
    return this.http.post<any>(
      'https://localhost:7207/api/payment/create-checkout-session',
      {
        invoiceId
      }
    );
  }
}