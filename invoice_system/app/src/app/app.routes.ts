import { Routes } from '@angular/router';
import { NotFound } from './not-found/not-found';
import { authGuard } from './core/auth/auth.guard';
import { Unauthorized } from './unauthorized/unauthorized';
import { Register } from './features/register/register';
import { Login } from './features/login/login';
import { Dashboard } from './features/dashboard/dashboard';
import { Home } from './home/home';
import { CustomerList } from './features/customer-list/customer-list';
import { AddCustomer } from './features/add-customer/add-customer';
import { CreateInvoice } from './features/create-invoice/create-invoice';
import { InvoiceList } from './features/invoice-list/invoice-list';
import { InvoiceDetails } from './features/invoice-details/invoice-details';
import { PaymentSuccess } from './features/payment-success/payment-success';
import { PaymentFailed } from './features/payment-failed/payment-failed';

export const routes: Routes = [
    {path:'',component:Dashboard},
    {path:'home',component:Dashboard},
    {path:'dashboard',component:Dashboard, canActivate: [authGuard]},
    {path:'login',component:Login},
    {path:'register',component:Register},
    {path:'customers',component:CustomerList, canActivate: [authGuard]},
    {path:'add-customer',component:AddCustomer, canActivate: [authGuard]},
    {path:'invoices',component:InvoiceList, canActivate: [authGuard]},
    {path:'create-invoice',component:CreateInvoice, canActivate: [authGuard]},
    {path: 'payment/success',component: PaymentSuccess},
    {path: 'payment/cancel',component: PaymentFailed},
    {path:'unauthorized',component:Unauthorized},
    {path:'invoice/:id',component:InvoiceDetails, canActivate: [authGuard]},
    {path:'**',component:NotFound}

];
