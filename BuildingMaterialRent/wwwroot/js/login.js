const AppLogin = {
    data() {
        return {
            user: {},
            validation: {}
        }
    },
    mounted() {
    },
    methods: {
        login(e) {
            this.clearErrors();
            var that = this;
            axios.post('/Account/Login', this.user)
                .then(function (resp) {
                    window.location.href = '/app/products';
                })
                .catch(function (err) {
                    if (err.response.status === 400) {
                        that.validation = err.response.data.errors;
                    } else {
                        that.validation = { General: ["Błąd serwera"] };
                    }
                });
            e.preventDefault();
        },
        clearErrors() {
            this.validation = {};
        }

    }
}

Vue.createApp(AppLogin).mount('#app-login')
