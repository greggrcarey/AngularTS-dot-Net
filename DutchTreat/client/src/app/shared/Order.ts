export class Order {
    orderId!: number;
    orderDate: Date = new Date();
    ordernumber!: string;
    items: OrderItem[] = new Array(new OrderItem());

    get subtotal(): number {

        const result = this.items.reduce(
            (tot, val) => {
                if (val.unitPrice !== undefined && val.quantity !== undefined)
                    return tot + (val.unitPrice * val.quantity);
                else {
                    return 0;
                }
            }, 0);
        return result;
    }
}

//export class OrderItem {
//    id: number = -1;
//    quantity: number = -1;
//    unitPrice: number = -1;
//    productId: number = -1;
//    productCategory: string = "";
//    productSize: string = "";
//    productTitle: string = "";
//    productArtist: string = "";
//    productArtId: string = "";
//}

export class OrderItem {
    id?: number;
    quantity: number = 0;
    unitPrice: number = 0;
    productId?: number;
    productCategory?: string;
    productSize?: string;
    productTitle?: string;
    productArtist?: string;
    productArtId?: string;
}


