/*
 severityRule = {
    id = 0,
    name = 'test',
    description = 'test',
    matcher = 'matcher',
    startAnchor = 'startAnchor',
    endAnchor = 'endAnchor',
    Trace = 'trace',
    Debug = 'debug',
    Info = 'info',
    Warning = 'warning',
    Error = 'error',
    Fatal = 'fatal',
}
*/
const state = {
  severityRules: [],
};

const getters = {
  severityRuleById: (state) => {
    return (severityRuleId) => {
      if (typeof severityRuleId === 'string' || severityRuleId instanceof String) {
        severityRuleId = parseInt(severityRuleId);
      }
      return state.severityRules.find(elem => elem.id === severityRuleId)
    }
  },

  allSeverityRules: state => state.severityRules,
};

const actions = {
  async fetchSeverityRules({ commit }) {
    try {
      const response = await this.$http.get("severityRule");
      commit("SET_SEVERITY_RULES", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchSeverityRule({ commit }, id) {
    try {
      const response = await this.$http.get(`severityRule/${id}`);
      commit("REPLACE_SEVERITY_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async editSeverityRule({ commit }, severityRuleToEdit) {
    try {
      const response = await this.$http.put(`severityRule/${severityRuleToEdit.id}`, severityRuleToEdit);
      commit("REPLACE_SEVERITY_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async deleteSeverityRule({ commit }, severityRuleToDelete) {
    try {
      const response = await this.$http.delete(`severityRule/${severityRuleToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_SEVERITY_RULE", severityRuleToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async createSeverityRule({ commit }, severityRuleToCreate) {
    console.log("create severityRule action");
    // try to create new
    try {
      const response = await this.$http.post("severityRule", severityRuleToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_SEVERITY_RULE", response.data);
      return response.data.id;
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  }
};

const mutations = {
  SET_SEVERITY_RULES: (state, severityRules) => state.severityRules = severityRules,
  ADD_SEVERITY_RULE: (state, severityRuleToAdd) => state.severityRules.push(severityRuleToAdd),
  REMOVE_SEVERITY_RULE: (state, severityRuleToRemove) => {
    const index = state.severityRules.findIndex(elem => elem.id == severityRuleToRemove.id);
    //if -1 then such severityRule does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.severityRules.splice(index, 1);
    }
  },
  REPLACE_SEVERITY_RULE: (state, severityRuleToReplace) => {
    const index = state.severityRules.findIndex(elem => elem.id == severityRuleToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.severityRules.splice(index, 1, severityRuleToReplace);
    } else {
      // else, add new severityRule
      state.severityRules.unshift(severityRuleToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
