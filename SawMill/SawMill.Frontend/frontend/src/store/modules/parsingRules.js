/*
 parsingRule = {
    id = 0,
    componentId = 0,
    dateTimeRuleId = 0,
    parsingRuleId = 0,
    severityRuleId = 0,
    messageRuleId = 0,
    customAttributeRuleIds = [0, 1]
}
*/
const state = {
  parsingRules: [],
};

const getters = {
  parsingRuleById: (state) => {
    return (parsingRuleId) => {

      console.log(`inside vuex getting parsing rule with id ${parsingRuleId}`);
      if (typeof parsingRuleId === 'string' || parsingRuleId instanceof String) {
        parsingRuleId = parseInt(parsingRuleId);
      }
      return state.parsingRules.find(elem => elem.id === parsingRuleId)
    }
  },
  parsingRuleByComponentId: (state) => {
    return (componentId) => {
      if (typeof componentId === 'string' || componentId instanceof String) {
        componentId = parseInt(componentId);
      }
      return state.parsingRules.find(elem => elem.componentId === componentId)
    }
  },

  allParsingRules: state => state.parsingRules,
};

const actions = {
  async fetchParsingRules({ commit }) {
    try {
      const response = await this.$http.get("parsingRules");
      commit("SET_PARSING_RULES", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchParsingRule({ commit }, id) {
    try {
      console.log(`inside vuex fetching parsing rule with id ${id}`);
      const response = await this.$http.get(`parsingRules/${id}`);
      commit("REPLACE_PARSING_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchParsingRuleForComponent({ commit }, componentId) {
    try {
      const response = await this.$http.get(`parsingRules/forcomponent/${componentId}`);
      commit("REPLACE_PARSING_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async editParsingRule({ commit }, parsingRuleToEdit) {
    console.log("vuex edit parsing rules")
    try {
      const response = await this.$http.put(`parsingRules/${parsingRuleToEdit.id}`, parsingRuleToEdit);
      commit("REPLACE_PARSING_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async deleteParsingRule({ commit }, parsingRuleToDelete) {
    try {
      const response = await this.$http.delete(`parsingRules/${parsingRuleToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_PARSING_RULE", parsingRuleToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async createParsingRule({ commit }, parsingRuleToCreate) {
    console.log("create parsingRule action");
    console.log("creating: ");
    console.log(parsingRuleToCreate);
    // try to create new
    try {
      const response = await this.$http.post("parsingRules", parsingRuleToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_PARSING_RULE", response.data);
      return response.data.id;
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  }
};

const mutations = {
  SET_PARSING_RULES: (state, parsingRules) => state.parsingRules = parsingRules,
  ADD_PARSING_RULE: (state, parsingRuleToAdd) => state.parsingRules.push(parsingRuleToAdd),
  REMOVE_PARSING_RULE: (state, parsingRuleToRemove) => {
    const index = state.parsingRules.findIndex(elem => elem.id == parsingRuleToRemove.id);
    //if -1 then such parsingRule does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.parsingRules.splice(index, 1);
    }
  },
  REPLACE_PARSING_RULE: (state, parsingRuleToReplace) => {
    const index = state.parsingRules.findIndex(elem => elem.id == parsingRuleToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.parsingRules.splice(index, 1, parsingRuleToReplace);
    } else {
      // else, add new parsingRule
      state.parsingRules.unshift(parsingRuleToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
