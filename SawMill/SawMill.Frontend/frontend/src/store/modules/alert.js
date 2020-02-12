/*
 alert = {
    id = 0,
    name = 'test',
    description = 'test',
    value = 'matcher';
    treshold = 1,
    timespan = 1000,
    componentId = -1,
    generalRuleId = -1
}
*/
const state = {
  alerts: [],
};

const getters = {
  alertById: (state) => {
    return (alertId) => {
      if (typeof alertId === 'string' || alertId instanceof String) {
        alertId = parseInt(alertId);
      }
      return state.alerts.find(elem => elem.id === alertId)
    }
  },

  allAlerts: state => state.alerts,
  alertsByComponentId: (state) => {
    return (componentId) => {
      if (typeof componentId === 'string' || componentId instanceof String) {
        componentId = parseInt(componentId);
      }
      return state.alerts.filter(elem => elem.componentId === componentId)
    }
  },
};

const actions = {
  async fetchAlerts({ commit }) {
    try {
      const response = await this.$http.get("alert");
      console.log("alerts");
      console.log(response.data);
      commit("SET_ALERTS", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchAlert({ commit }, id) {
    try {
      const response = await this.$http.get(`alert/${id}`);
      commit("REPLACE_ALERT", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async editAlert({ commit }, alertToEdit) {
    try {
      const response = await this.$http.put(`alert/${alertToEdit.id}`, alertToEdit);
      commit("REPLACE_ALERT", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async deleteAlert({ commit }, alertToDelete) {
    try {
      const response = await this.$http.delete(`alert/${alertToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_ALERT", alertToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async createAlert({ commit }, alertToCreate) {
    console.log("create alert action");
    // try to create new
    try {
      const response = await this.$http.post("alert", alertToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_ALERT", response.data);
      return response.data.id;
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  }
};

const mutations = {
  SET_ALERTS: (state, alerts) => state.alerts = alerts,
  ADD_ALERT: (state, alertToAdd) => state.alerts.push(alertToAdd),
  REMOVE_ALERT: (state, alertToRemove) => {
    const index = state.alerts.findIndex(elem => elem.id == alertToRemove.id);
    //if -1 then such alert does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.alerts.splice(index, 1);
    }
  },
  REPLACE_ALERT: (state, alertToReplace) => {
    const index = state.alerts.findIndex(elem => elem.id == alertToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.alerts.splice(index, 1, alertToReplace);
    } else {
      // else, add new alert
      state.alerts.unshift(alertToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
