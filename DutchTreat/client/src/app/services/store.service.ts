import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { Order, OrderItem } from "../shared/Order";
import { Product } from "../shared/Product";


@Injectable()
export class Store {

    constructor(private http: HttpClient) {

    }


    public products: Product[] = [];

    public order: Order = new Order();

    loadProducts(): Observable<void> {
        return this.http.get<Product[]>("/api/products")
            .pipe(map(data => { this.products = data }));
    }

    addToOrder(product: Product) {

        let possibleItem = this.order.items.find(o => o.productArtId === product.artId);

        if (possibleItem !== undefined && possibleItem.quantity !== undefined) {
            possibleItem.quantity++;
        }
        else
        {
            let item = new OrderItem();
            
            item.productId = product.id;
            item.productTitle = product.title;
            item.productArtId = product.artId;
            item.productArtist = product.artist;
            item.productCategory = product.category;
            item.productSize = product.size;
            item.unitPrice = product.price;
            item.quantity = 1;
            this.order.items.push(item);
        }

    }


}
