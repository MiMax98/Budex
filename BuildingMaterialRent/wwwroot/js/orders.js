const AppOrders = {
    data() {
        return {
            orders: []
        }
    },
    mounted() {
        this.loadOrders();
    },
    methods: {
        formatDate(value) {
            if (value) {
                return moment(String(value)).format('YYYY-MM-DD HH:mm:ss')
            } else {
                return '-';
            }
        },
        getStatus(s) {
            switch (s) {
                case 2: return 'Złożone';
                case 3: return 'Odebrane';
                case 4: return 'Zwócone';
                default: return '-';
            }
        },
        loadOrders() {
            var that = this;
            axios.get('/Orders')
                .then((x) => {
                    that.orders = x.data;
                });
        },
        pickupOrder(id) {
            var that = this;
            axios.post('/Orders/PickupOrder/'+id)
                .then(that.loadOrders);
        },
        returnOrder(id) {
            var that = this;
            axios.post('/Orders/ReturnOrder/' + id)
                .then(that.loadOrders);
        }
    }
}

Vue.createApp(AppOrders).mount('#app-orders')
