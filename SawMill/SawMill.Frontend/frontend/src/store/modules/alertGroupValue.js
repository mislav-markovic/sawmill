/*
 alertGroupValue = {
    id: 0,
    alertId: 0,
    timespanStart: date,
    timespanEnd: date,
}
*/
const state = {
  alertGroupValues: [],
};

const getters = {
  alertGroupValueById: (state) => {
    return (alertGroupValueId) => {
      if (typeof alertGroupValueId === 'string' || alertGroupValueId instanceof String) {
        alertGroupValueId = parseInt(alertGroupValueId);
      }
      return state.alertGroupValues.find(elem => elem.id === alertGroupValueId)
    }
  },

  allAlertGroupValues: state => state.alertGroupValues,
  alertGroupValuesByAlertGroupId: (state) => {
    return (alertGroupId) => {
      if (typeof alertGroupId === 'string' || alertGroupId instanceof String) {
        alealertGroupIdtId = parseInt(alertGroupId);
      }
      return state.alertGroupValues.filter(elem => elem.alertGroupId === alertGroupId)
    }
  },
};

const actions = {
  async fetchAlertGroupValuesByAlertGroupId({ commit }, alertGroupId) {
    try {
      const response = await this.$http.get(`alertGroup/value/${alertGroupId}`);
      commit("APPEND_ALERT_GROUP_VALUES", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
};

const mutations = {
  APPEND_ALERT_GROUP_VALUES: (state, alertGroupValues) => {
    let temp = alertGroupValues.map(elem => {
      elem.timespanStart = new Date(Date.parse(elem.timespanStart));
      elem.timespanEnd = new Date(Date.parse(elem.timespanEnd));
      return elem;
    })
    state.alertGroupValues.push(...temp);
  },
  SET_ALERT_GROUP_VALUES: (state, alertGroupValues) => state.alertGroupValues = alertGroupValues.map(elem => {
    elem.timespanStart = new Date(Date.parse(elem.timespanStart));
    elem.timespanEnd = new Date(Date.parse(elem.timespanEnd));
    return elem;
  }),
  ADD_ALERT_GROUP_VALUE: (state, alertGroupValueToAdd) => {
    alertGroupValueToAdd.timespanStart = new Date(Date.parse(alertGroupValueToAdd.timespanStart));
    alertGroupValueToAdd.timespanEnd = new Date(Date.parse(alertGroupValueToAdd.timespanEnd));
    state.alertGroupValues.push(alertGroupValueToAdd);
  },
  REMOVE_ALERT_GROUP_VALUE: (state, alertGroupValueToRemove) => {
    const index = state.alertGroupValues.findIndex(elem => elem.id == alertGroupValueToRemove.id);
    //if -1 then such alertGroupValue does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.alertGroupValues.splice(index, 1);
    }
  },
  REPLACE_ALERT_GROUP_VALUE: (state, alertGroupValueToReplace) => {
    alertGroupValueToReplace.timespanStart = new Date(Date.parse(alertGroupValueToReplace.timespanStart));
    alertGroupValueToReplace.timespanEnd = new Date(Date.parse(alertGroupValueToReplace.timespanEnd));

    const index = state.alertGroupValues.findIndex(elem => elem.id == alertGroupValueToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.alertGroupValues.splice(index, 1, alertGroupValueToReplace);
    } else {
      // else, add new alertGroupValue
      state.alertGroupValues.unshift(alertGroupValueToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
