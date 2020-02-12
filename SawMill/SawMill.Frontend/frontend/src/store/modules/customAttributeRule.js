/*
 customAttributeRule = {
    id = 0,
    name = 'test',
    description = 'test',
    matcher = 'matcher';
    startAnchor = 'startAnchor',
    endAnchor = 'endAnchor'
}
*/
const state = {
  customAttributeRules: [],
};

const getters = {
  customAttributeRuleById: (state) => {
    return (customAttributeRuleId) => {
      if (typeof customAttributeRuleId === 'string' || customAttributeRuleId instanceof String) {
        customAttributeRuleId = parseInt(customAttributeRuleId);
      }
      return state.customAttributeRules.find(elem => elem.id === customAttributeRuleId)
    }
  },

  allCustomAttributeRules: state => state.customAttributeRules,

  customAttributeRuleByComponentId: (state, getters) => {
    return (componentId) => {
      if (typeof componentId === 'string' || componentId instanceof String) {
        componentId = parseInt(componentId);
      }
      const temp = getters.parsingRuleByComponentId(componentId);
      if (typeof temp === "undefined") {
        return [];
      }
      const componentsCustomAttributeRuleIds = temp.customAttributeRuleIds.map(elem => parseInt(elem));
      return state.customAttributeRules.filter(elem => componentsCustomAttributeRuleIds.includes(parseInt(elem.id)));
    }
  }
};

const actions = {
  async fetchCustomAttributeRules({ commit }) {
    try {
      const response = await this.$http.get("customAttributeRule");
      commit("SET_CUSTOM_ATTRIBUTE_RULES", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchCustomAttributeRule({ commit }, id) {
    try {
      const response = await this.$http.get(`customAttributeRule/${id}`);
      commit("REPLACE_CUSTOM_ATTRIBUTE_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async editCustomAttributeRule({ commit }, customAttributeRuleToEdit) {
    try {
      const response = await this.$http.put(`customAttributeRule/${customAttributeRuleToEdit.id}`, customAttributeRuleToEdit);
      commit("REPLACE_CUSTOM_ATTRIBUTE_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async deleteCustomAttributeRule({ commit }, customAttributeRuleToDelete) {
    try {
      const response = await this.$http.delete(`customAttributeRule/${customAttributeRuleToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_CUSTOM_ATTRIBUTE_RULE", customAttributeRuleToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async createCustomAttributeRule({ commit }, customAttributeRuleToCreate) {
    console.log("create customAttributeRule action");
    // try to create new
    try {
      const response = await this.$http.post("customAttributeRule", customAttributeRuleToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_CUSTOM_ATTRIBUTE_RULE", response.data);
      return response.data.id;
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  }
};

const mutations = {
  SET_CUSTOM_ATTRIBUTE_RULES: (state, customAttributeRules) => state.customAttributeRules = customAttributeRules,
  ADD_CUSTOM_ATTRIBUTE_RULE: (state, customAttributeRuleToAdd) => state.customAttributeRules.push(customAttributeRuleToAdd),
  REMOVE_CUSTOM_ATTRIBUTE_RULE: (state, customAttributeRuleToRemove) => {
    const index = state.customAttributeRules.findIndex(elem => elem.id == customAttributeRuleToRemove.id);
    //if -1 then such customAttributeRule does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.customAttributeRules.splice(index, 1);
    }
  },
  REPLACE_CUSTOM_ATTRIBUTE_RULE: (state, customAttributeRuleToReplace) => {
    const index = state.customAttributeRules.findIndex(elem => elem.id == customAttributeRuleToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.customAttributeRules.splice(index, 1, customAttributeRuleToReplace);
    } else {
      // else, add new customAttributeRule
      state.customAttributeRules.unshift(customAttributeRuleToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
