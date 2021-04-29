import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { BussyService } from '../services/bussy.service';
import { Observable } from 'rxjs';
import { finalize, delay } from 'rxjs/operators';
import { Injectable } from '@angular/core';

@Injectable()
export class LoadingInterceptor implements HttpInterceptor {
    constructor(private busyService:BussyService){}

    intercept(req:HttpRequest<any>, next:HttpHandler):Observable<HttpEvent<any>>{
        this.busyService.busy();
        return next.handle(req).pipe(
            delay(1000),
            finalize(()=>{
                this.busyService.idle();
            })
        );
    }
}