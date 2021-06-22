const Header = {
    data() {
        return {
            categories: [],
            showCategories: false,
            isSignedIn: false,
            userInfo: {}
        }
    },
    mounted() {
        axios.get('/Categories')
            .then((x) => {
                this.categories = x.data;
            });
        axios.get('/Account')
            .then(x => {
                this.isSignedIn = x.data.isSignedIn;
                this.userInfo = x.data.userInfo;
            });
    },
    methods: {
        getUrl(cat) {
            if (cat === null) {
                return "#";
            }
            return '/app/products/' + cat.categoryId;
        },
        logOut() {
            axios.post('/Account/LogOff')
                .then(() => {
                    window.location.reload();
                });
        }
    }
}

Vue.createApp(Header).mount('#app-header')
