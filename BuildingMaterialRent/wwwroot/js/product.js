const AppProduct = {
    data() {
        return {
            product: {
                stock:1
            },
            quantity: 1,
            isSignedIn: false
        }
    },
    mounted() {
        this.loadProduct();
    },
    methods: {
        decrement() {
            if (this.quantity <= 1) {
                this.quantity = 1;
                return;
            }
            this.quantity--;
        },
        increment() {
            if (this.quantity >= this.product.stock) {
                this.quantity = this.product.stock;
                return;
            }
            this.quantity++;
        },
        addToBasket() {
            axios.post('/Orders/Add', { quantity: this.quantity, productId: this.product.productId})
                .then(function () {
                    window.location.href = '/app/basket';
                });
        },
        loadProduct() {
            var that = this;
            var ps = window.location.pathname.split('/');
            var id = ps[ps.length - 1];
            if (!Number.isInteger(id * 1)) {
                window.location.href = '/app/products';
            }
            this.quantity = 1;
            axios.get('/Products/' + id)
                .then((x) => {
                    that.product = x.data;
                });
            axios.get('/Account')
                .then(x => {
                    this.isSignedIn = x.data.isSignedIn;
                });
        }
    }
}

Vue.createApp(AppProduct).mount('#app-product')
