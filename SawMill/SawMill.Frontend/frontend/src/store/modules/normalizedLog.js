/*
 normalizedLog = {
      id: 0,
      componentId: 1,
      dateTime: new DateTime(),
      message: '',
      severityLevel: {level: 0, display: ''},
      customValues: [{id: 0, value: '', customAttributeRuleId: 0}],
      rawLogId: 0
}
*/
const state = {
  normalizedLogs: [],
};

const getters = {
  normalizedLogById: (state) => {
    return (normalizedLogId) => {
      if (typeof normalizedLogId === 'string' || normalizedLogId instanceof String) {
        normalizedLogId = parseInt(normalizedLogId);
      }
      return state.normalizedLogs.find(elem => elem.id === normalizedLogId)
    }
  },

  normalizedLogByComponentId: (state) => {
    return (componentId) => {
      if (typeof componentId === 'string' || componentId instanceof String) {
        componentId = parseInt(componentId);
      }
      return state.normalizedLogs.filter(elem => elem.componentId === componentId)
    }
  },

  allNormalizedLogs: state => state.normalizedLogs,
};

const actions = {
  async fetchNormalizedLogsForSystem({ commit }, systemId) {
    try {
      const response = await this.$http.get(`log/normalized/system/${systemId}`, { params: { paginated: 50 } });
      commit("SET_NORMALIZED_LOGS", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  }, async fetchNormalizedLogsForSystemInDateRange({ commit }, { systemId, start, end }) {
    try {
      if (!start || !end || !systemId) {
        return;
      }
      const response = await this.$http.get(`log/normalized/system/${systemId}`, { params: { paginated: 50, start: start, end: end } });
      commit("SET_NORMALIZED_LOGS", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async loadMoreNormalizedLogsForSystem({ commit }, { systemId, logId }) {
    try {
      const params = { paginated: 100, lastLogId: logId };
      const response = await this.$http.get(`log/normalized/system/paginated/${systemId}`, { params: params });
      console.log("response");
      console.log(response.data);
      commit("APPEND_NORMALIZED_LOGS", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  }
};

const mutations = {
  SET_NORMALIZED_LOGS: (state, normalizedLogs) => state.normalizedLogs = normalizedLogs.map(elem => { elem.dateTime = new Date(Date.parse(elem.dateTime)); return elem }),
  ADD_NORMALIZED_LOG: (state, normalizedLogToAdd) => {
    normalizedLogToAdd.dateTime = new Date(Date.parse(normalizedLogToAdd.dateTime));
    state.normalizedLogs.push(normalizedLogToAdd)
  },
  APPEND_NORMALIZED_LOGS: (state, normalizedLogs) => {
    console.log("normalized logs");
    console.log(normalizedLogs);
    console.log(typeof normalizedLogs);
    if (normalizedLogs.length === 0) {
      return;
    }
    normalizedLogs = normalizedLogs.map(elem => { elem.dateTime = new Date(Date.parse(elem.dateTime)); return elem })
    if (state.normalizedLogs.length === 0) {
      console.log("normalized logs len is 0");
      state.normalizedLogs = normalizedLogs;
    } else {
      console.log("normalized logs have same system id");
      state.normalizedLogs.push(...normalizedLogs);
    }
  },
  REMOVE_NORMALIZED_LOG: (state, normalizedLogToRemove) => {
    const index = state.normalizedLogs.findIndex(elem => elem.id == normalizedLogToRemove.id);
    //if -1 then such normalizedLog does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.normalizedLogs.splice(index, 1);
    }
  },
  REPLACE_NORMALIZED_LOG: (state, normalizedLogToReplace) => {
    normalizedLogToReplace.dateTime = new Date(Date.parse(normalizedLogToReplace.dateTime));
    const index = state.normalizedLogs.findIndex(elem => elem.id == normalizedLogToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.normalizedLogs.splice(index, 1, normalizedLogToReplace);
    } else {
      // else, add new normalizedLog
      state.normalizedLogs.unshift(normalizedLogToReplace);
    }
  },
};

export default {
  state,
  getters,
  actions,
  mutations,
};
