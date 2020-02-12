/*
 dateTimeRule = {
    id = 0,
    name = 'test',
    description = 'test',
    matcher = 'matcher',
    startAnchor = 'startAnchor',
    endAnchor = 'endAnchor',
    dateFormat = 'dd/mm/yyyy'
}
*/
const state = {
  dateTimeRules: [],
};

const getters = {
  dateTimeRuleById: (state) => {
    return (dateTimeRuleId) => {
      if (typeof dateTimeRuleId === 'string' || dateTimeRuleId instanceof String) {
        dateTimeRuleId = parseInt(dateTimeRuleId);
      }
      return state.dateTimeRules.find(elem => elem.id === dateTimeRuleId)
    }
  },

  allDateTimeRules: state => state.dateTimeRules,
};

const actions = {
  async fetchDateTimeRules({ commit }) {
    try {
      const response = await this.$http.get("dateTimeRule");
      commit("SET_DATE_TIME_RULES", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchDateTimeRule({ commit }, id) {
    try {
      const response = await this.$http.get(`dateTimeRule/${id}`);
      commit("REPLACE_DATE_TIME_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async editDateTimeRule({ commit }, dateTimeRuleToEdit) {
    try {
      const response = await this.$http.put(`dateTimeRule/${dateTimeRuleToEdit.id}`, dateTimeRuleToEdit);
      commit("REPLACE_DATE_TIME_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async deleteDateTimeRule({ commit }, dateTimeRuleToDelete) {
    try {
      const response = await this.$http.delete(`dateTimeRule/${dateTimeRuleToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_DATE_TIME_RULE", dateTimeRuleToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async createDateTimeRule({ commit }, dateTimeRuleToCreate) {
    console.log("create dateTimeRule action");
    // try to create new
    try {
      const response = await this.$http.post("dateTimeRule", dateTimeRuleToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_DATE_TIME_RULE", response.data);
      return response.data.id;
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  }
};

const mutations = {
  SET_DATE_TIME_RULES: (state, dateTimeRules) => state.dateTimeRules = dateTimeRules,
  ADD_DATE_TIME_RULE: (state, dateTimeRuleToAdd) => state.dateTimeRules.push(dateTimeRuleToAdd),
  REMOVE_DATE_TIME_RULE: (state, dateTimeRuleToRemove) => {
    const index = state.dateTimeRules.findIndex(elem => elem.id == dateTimeRuleToRemove.id);
    //if -1 then such dateTimeRule does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.dateTimeRules.splice(index, 1);
    }
  },
  REPLACE_DATE_TIME_RULE: (state, dateTimeRuleToReplace) => {
    const index = state.dateTimeRules.findIndex(elem => elem.id == dateTimeRuleToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.dateTimeRules.splice(index, 1, dateTimeRuleToReplace);
    } else {
      // else, add new dateTimeRule
      state.dateTimeRules.unshift(dateTimeRuleToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
