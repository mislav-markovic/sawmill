/*
 alertGroup = {
    id = 0,
    name = 'test',
    description = 'test',
    value = 'matcher';
    timespan = 1000,
    systemId = -1,
}
*/
const state = {
  alertGroups: [],
};

const getters = {
  alertGroupById: (state) => {
    return (alertGroupId) => {
      if (typeof alertGroupId === 'string' || alertGroupId instanceof String) {
        alertGroupId = parseInt(alertGroupId);
      }
      return state.alertGroups.find(elem => parseInt(elem.id) === alertGroupId)
    }
  },

  allAlertGroups: state => state.alertGroups,
  alertGroupsBySystemId: (state) => {
    return (systemId) => {
      if (typeof systemId === 'string' || systemId instanceof String) {
        systemId = parseInt(systemId);
      }
      return state.alertGroups.filter(elem => elem.systemId === systemId)
    }
  },
};

const actions = {
  async fetchAlertGroups({ commit }) {
    try {
      const response = await this.$http.get("alertGroup");
      console.log("alertGroups");
      console.log(response.data);
      commit("SET_ALERT_GROUPS", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchAlertGroup({ commit }, id) {
    try {
      const response = await this.$http.get(`alertGroup/${id}`);
      commit("REPLACE_ALERT_GROUP", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async editAlertGroup({ commit }, alertGroupToEdit) {
    try {
      const response = await this.$http.put(`alertGroup/${alertGroupToEdit.id}`, alertGroupToEdit);
      commit("REPLACE_ALERT_GROUP", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async deleteAlertGroup({ commit }, alertGroupToDelete) {
    try {
      const response = await this.$http.delete(`alertGroup/${alertGroupToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_ALERT_GROUP", alertGroupToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async createAlertGroup({ commit }, alertGroupToCreate) {
    console.log("create alertGroup action");
    // try to create new
    try {
      const response = await this.$http.post("alertGroup", alertGroupToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_ALERT_GROUP", response.data);
      return response.data.id;
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  }
};

const mutations = {
  SET_ALERT_GROUPS: (state, alertGroups) => state.alertGroups = alertGroups,
  ADD_ALERT_GROUP: (state, alertGroupToAdd) => state.alertGroups.push(alertGroupToAdd),
  REMOVE_ALERT_GROUP: (state, alertGroupToRemove) => {
    const index = state.alertGroups.findIndex(elem => elem.id == alertGroupToRemove.id);
    //if -1 then such alertGroup does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.alertGroups.splice(index, 1);
    }
  },
  REPLACE_ALERT_GROUP: (state, alertGroupToReplace) => {
    const index = state.alertGroups.findIndex(elem => elem.id == alertGroupToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.alertGroups.splice(index, 1, alertGroupToReplace);
    } else {
      // else, add new alertGroup
      state.alertGroups.unshift(alertGroupToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
