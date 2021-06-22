const AppProducts = {
    data() {
        return {
            productList: [],
            categoryName: null
        }
    },
    mounted() {
        var ps = window.location.pathname.split('/');
        var id = ps[ps.length - 1];
        if (ps.length === 4 && Number.isInteger(id * 1)) {
            axios.get('/Categories/' + id)
                .then((x) => {
                    this.productList = x.data.products;
                    this.categoryName = x.data.name;
                });
        } else {
            axios.get('/Products')
                .then((x) => {
                    this.productList = x.data;
                });
        }

    },
    methods: {
        getUrl(prod) {
            if (prod == null) {
                return "#";
            }
            return '/app/product/' + prod.productId;
        }
    }
}

Vue.createApp(AppProducts).mount('#app-products')
