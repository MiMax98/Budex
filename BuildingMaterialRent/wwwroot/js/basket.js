const AppBasket = {
    data() {
        return {
            order: {},
            downloaded: false
        }
    },
    mounted() {
        this.loadOrder()
    },
    methods: {
        getTotal() {
            if (!this.downloaded) {
                return 0;
            }
            var total = 0;
            for (var i = 0; i < this.order.orderItems.length; i++) {
                total += this.order.orderItems[i].product.price * this.order.orderItems[i].quantity;
            }
            return total;
        },
        deleteItem(id) {
            var that = this;
            axios.post('/Orders/DeleteItem/' + id)
                .then(that.loadOrder);
        },
        loadOrder() {
            var that = this;
            axios.get('/Orders/Basket')
                .then((x) => {
                    that.order = x.data;
                    that.downloaded = true;
                });
        },
        getLength() {
            if (!this.downloaded) {
                return 0;
            }
            return this.order.orderItems.length;
        },
        increment(i) {
            var item = this.order.orderItems[i];
            this.updateItem(item.quantity + 1, item.orderItemId);
        },
        decrement(i) {
            var item = this.order.orderItems[i];
            if (item.quantity <= 1) {
                return;
            }
            this.updateItem(item.quantity - 1, item.orderItemId);
        },
        updateItem(quantity, id) {
            var that = this;
            axios.post('/Orders/Update', { quantity: quantity, orderItemId: id })
                .then(that.loadOrder);
        },
        placeOrder() {
            axios.post('/Orders/PlaceOrder')
                .then(function () {
                    window.location.href = '/app/products';
                });
        }
    }
}

Vue.createApp(AppBasket).mount('#app-basket')
