import Vue from "vue";
import vuetify from './plugins/vuetify';
import App from "./App.vue";
import store from "./store";
import router from "./router";
import axios from "axios";

Vue.config.productionTip = false;

// setup base url for http requests
const base = axios.create({
  baseURL: 'http://localhost:52361/api/'
})
Vue.prototype.$http = base;

new Vue({
  vuetify,
  router,
  store,
  render: h => h(App),
}).$mount("#app");
