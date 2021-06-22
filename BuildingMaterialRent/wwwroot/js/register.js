const AppRegister = {
    data() {
        return {
            user: {},
            validation: {}
        }
    },
    mounted() {
    },
    methods: {
        register(e) {
            this.clearErrors();
            var that = this;
            axios.post('/Account/Register', this.user)
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

Vue.createApp(AppRegister).mount('#app-register')
