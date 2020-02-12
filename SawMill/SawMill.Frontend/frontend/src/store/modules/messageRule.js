/*
 messageRule = {
    id = 0,
    name = 'test',
    description = 'test',
    matcher = 'matcher';
    startAnchor = 'startAnchor',
    endAnchor = 'endAnchor',
    maxLength = -1,
}
*/
const state = {
  messageRules: [],
};

const getters = {
  messageRuleById: (state) => {
    return (messageRuleId) => {
      if (typeof messageRuleId === 'string' || messageRuleId instanceof String) {
        messageRuleId = parseInt(messageRuleId);
      }
      return state.messageRules.find(elem => elem.id === messageRuleId)
    }
  },

  allMessageRules: state => state.messageRules,
};

const actions = {
  async fetchMessageRules({ commit }) {
    try {
      const response = await this.$http.get("messageRule");
      commit("SET_MESSAGE_RULES", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchMessageRule({ commit }, id) {
    try {
      const response = await this.$http.get(`messageRule/${id}`);
      commit("REPLACE_MESSAGE_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async editMessageRule({ commit }, messageRuleToEdit) {
    try {
      const response = await this.$http.put(`messageRule/${messageRuleToEdit.id}`, messageRuleToEdit);
      commit("REPLACE_MESSAGE_RULE", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async deleteMessageRule({ commit }, messageRuleToDelete) {
    try {
      const response = await this.$http.delete(`messageRule/${messageRuleToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_MESSAGE_RULE", messageRuleToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async createMessageRule({ commit }, messageRuleToCreate) {
    console.log("create messageRule action");
    // try to create new
    try {
      const response = await this.$http.post("messageRule", messageRuleToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_MESSAGE_RULE", response.data);
      return response.data.id;
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  }
};

const mutations = {
  SET_MESSAGE_RULES: (state, messageRules) => state.messageRules = messageRules,
  ADD_MESSAGE_RULE: (state, messageRuleToAdd) => state.messageRules.push(messageRuleToAdd),
  REMOVE_MESSAGE_RULE: (state, messageRuleToRemove) => {
    const index = state.messageRules.findIndex(elem => elem.id == messageRuleToRemove.id);
    //if -1 then such messageRule does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.messageRules.splice(index, 1);
    }
  },
  REPLACE_MESSAGE_RULE: (state, messageRuleToReplace) => {
    const index = state.messageRules.findIndex(elem => elem.id == messageRuleToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.messageRules.splice(index, 1, messageRuleToReplace);
    } else {
      // else, add new messageRule
      state.messageRules.unshift(messageRuleToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
