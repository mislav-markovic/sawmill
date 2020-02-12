/*
 report = {
    id = 0,
    name = 'test',
    description = 'test',
    componentIds = [0, 1, 2],
}
*/
const state = {
  reports: [],
};

const getters = {
  allReports: state => state.reports,
  reportById: (state) => (id) => {
    if (typeof id === 'string' || id instanceof String) {
      id = parseInt(id);
    }
    return state.reports.find(elem => elem.id == id)
  },
  reportsBySystemId: (state) => (systemId) => {
    if (typeof systemId === 'string' || systemId instanceof String) {
      id = parseInt(systemId);
    }
    return state.reports.filter(elem => elem.systemId == systemId)
  },
};

const actions = {
  async fetchReportsForSystem({ commit }, systemId) {
    try {
      const response = await this.$http.get(`reports/system/${systemId}`);
      commit("SET_REPORTS", response.data);
    }
    catch (error) {
      // TODO: how to handle
      console.log(error);
    }
  },
  async fetchReport({ commit }, reportIdToFetch) {
    try {
      const response = await this.$http.get(`reports/${reportIdToFetch}`);
      commit("REPLACE_REPORT", response.data);
    }
    catch (error) {
      // TODO: how to handle
      console.log(error);
    }
  },
  async createReport({ commit }, systemId) {
    // try to create new
    try {
      const response = await this.$http.post(`reports/${systemId}`);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_REPORT", response.data);
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  },
};

const mutations = {
  SET_REPORTS: (state, reports) => (state.reports = reports.map(elem => mapIdFieldsToInt(elem))),
  ADD_REPORT: (state, reportToAdd) => state.reports.unshift(mapIdFieldsToInt(reportToAdd)),
  REMOVE_REPORT: (state, reportToRemove) => {
    reportToRemove = mapIdFieldsToInt(reportToRemove);
    const index = state.reports.findIndex(elem => elem.id == reportToRemove.id);
    //if -1 then such report does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.reports.splice(index, 1);
    }
  },
  REPLACE_REPORT: (state, reportToReplace) => {
    reportToReplace = mapIdFieldsToInt(reportToReplace);
    const index = state.reports.findIndex(elem => elem.id == reportToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.reports.splice(index, 1, reportToReplace);
    } else {
      // else, add new report
      state.reports.push(reportToReplace);
    }
  }
};

function mapIdFieldsToInt(report) {
  report.id = parseInt(report.id);
  report.systemId = parseInt(report.systemId);

  return report;
}

export default {
  state,
  getters,
  actions,
  mutations,
};
