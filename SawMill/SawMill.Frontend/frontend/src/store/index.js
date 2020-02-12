import Vuex from "vuex";
import Vue from "vue";
import systems from "./modules/systems";
import components from "./modules/component";
import messageRules from "./modules/messageRule";
import severityRules from "./modules/severityRule";
import dateTimeRules from "./modules/dateTimeRule";
import parsingRules from "./modules/parsingRules";
import customAttributeRules from "./modules/customAttributeRule";
import normalizedLogs from "./modules/normalizedLog";
import alert from "./modules/alert";
import alertValue from "./modules/alertValue";
import alertGroup from "./modules/alertGroup";
import alertGroupValue from "./modules/alertGroupValue";
import reports from "./modules/reports"
import axios from "axios";

// Load Vuex
Vue.use(Vuex);

const base = axios.create({
  baseURL: 'http://localhost:52361/api/',
})
const store = new Vuex.Store({
  modules: {
    systems,
    components,
    messageRules,
    severityRules,
    dateTimeRules,
    customAttributeRules,
    parsingRules,
    normalizedLogs,
    alert,
    alertValue,
    alertGroup,
    alertGroupValue,
    reports
  },
})

store.$http = base;

// Create store
export default store;
