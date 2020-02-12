/*
 alertValue = {
    id: 0,
    alertId: 0,
    timespanStart: date,
    timespanEnd: date,
}
*/
const state = {
  alertValues: [],
};

const getters = {
  alertValueById: (state) => {
    return (alertValueId) => {
      if (typeof alertValueId === 'string' || alertValueId instanceof String) {
        alertValueId = parseInt(alertValueId);
      }
      return state.alertValues.find(elem => elem.id === alertValueId)
    }
  },

  allAlertValues: state => state.alertValues,
  alertValuesByAlertId: (state) => {
    return (alertId) => {
      if (typeof alertId === 'string' || alertId instanceof String) {
        alertId = parseInt(alertId);
      }
      return state.alertValues.filter(elem => elem.alertId === alertId)
    }
  },
};

const actions = {
  async fetchAlertValuesByAlertId({ commit }, alertId) {
    try {
      const response = await this.$http.get(`alert/value/${alertId}`);
      commit("APPEND_ALERT_VALUES", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
};

const mutations = {
  APPEND_ALERT_VALUES: (state, alertValues) => {
    let temp = alertValues.map(elem => {
      elem.timespanStart = new Date(Date.parse(elem.timespanStart));
      elem.timespanEnd = new Date(Date.parse(elem.timespanEnd));
      return elem;
    });
    state.alertValues.push(...temp);
  },
  SET_ALERT_VALUES: (state, alertValues) => state.alertValues = alertValues.map(elem => {
    elem.timespanStart = new Date(Date.parse(elem.timespanStart));
    elem.timespanEnd = new Date(Date.parse(elem.timespanEnd));
    return elem;
  }),
  ADD_ALERT_VALUE: (state, alertValueToAdd) => {
    alertValueToAdd.timespanStart = new Date(Date.parse(alertValueToAdd.timespanStart));
    alertValueToAdd.timespanEnd = new Date(Date.parse(alertValueToAdd.timespanEnd));
    state.alertValues.push(alertValueToAdd);
  },
  REMOVE_ALERT_VALUE: (state, alertValueToRemove) => {
    const index = state.alertValues.findIndex(elem => elem.id == alertValueToRemove.id);
    //if -1 then such alertValue does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.alertValues.splice(index, 1);
    }
  },
  REPLACE_ALERT_VALUE: (state, alertValueToReplace) => {
    alertValueToReplace.timespanStart = new Date(Date.parse(alertValueToReplace.timespanStart));
    alertValueToReplace.timespanEnd = new Date(Date.parse(alertValueToReplace.timespanEnd));

    const index = state.alertValues.findIndex(elem => elem.id == alertValueToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.alertValues.splice(index, 1, alertValueToReplace);
    } else {
      // else, add new alertValue
      state.alertValues.unshift(alertValueToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
