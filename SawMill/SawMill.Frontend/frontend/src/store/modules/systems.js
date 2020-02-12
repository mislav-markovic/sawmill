/*
 system = {
    id = 0,
    name = 'test',
    description = 'test',
    componentIds = [0, 1, 2],
}
*/
const state = {
  systems: [],
};

const getters = {
  allSystems: state => state.systems,
  systemById: (state) => (id) => {
    if (typeof id === 'string' || id instanceof String) {
      id = parseInt(id);
    }
    return state.systems.find(elem => elem.id == id)
  },
};

const actions = {
  async fetchSystems({ commit }) {
    try {
      const response = await this.$http.get("system");
      commit("SET_SYSTEMS", response.data);
    }
    catch (error) {
      // TODO: how to handle
      console.log(error);
    }
  },
  async fetchSystem({ commit }, systemIdToFetch) {
    try {
      const response = await this.$http.get(`system/${systemIdToFetch}`);
      commit("REPLACE_SYSTEM", response.data);
    }
    catch (error) {
      // TODO: how to handle
      console.log(error);
    }
  },
  async createSystem({ commit }, systemToCreate) {
    // try to create new
    try {
      const response = await this.$http.post("system", systemToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_SYSTEM", response.data);
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  },
  async deleteSystem({ commit }, systemToDelete) {
    try {
      const response = await this.$http.delete(`system/${systemToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_SYSTEM", systemToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async editSystem({ commit }, systemToEdit) {
    try {
      console.log(`editing system id ${systemToEdit.id}`)
      const response = await this.$http.put(`system/${systemToEdit.id}`, systemToEdit);

      commit("REPLACE_SYSTEM", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  addComponentToSystem({ commit, getters }, componentId, systemId) {
    var systemExists = getters.systemById(systemId);

    if (!!systemExists) {
      // add component id if it is not already present
      if (!systemExists.componentIds.includes(parseInt(componentId))) {
        systemId.systemExists.componentIds.push(parseInt(componentId));

        commit('REPLACE_SYSTEM', systemExists);
        dispatch('linkComponentToSystem', componentId, systemId);
      }
    }
  },
  removeComponentFromSystem({ commit, getters, dispatch }, componentId, systemId) {
    const systemExists = getters.systemById(systemId);

    if (!!systemExists) {
      const comps = systemExists.componentIds.filter(elem => elem != componentId);
      systemExists.componentIds = comps;
      commit('REPLACE_SYSTEM', systemExists);
      dispatch('unlinkComponentsFromSystem', systemId);
    }
  }
};

const mutations = {
  SET_SYSTEMS: (state, systems) => (state.systems = systems.map(elem => mapIdFieldsToInt(elem))),
  ADD_SYSTEM: (state, systemToAdd) => state.systems.unshift(mapIdFieldsToInt(systemToAdd)),
  REMOVE_SYSTEM: (state, systemToRemove) => {
    systemToRemove = mapIdFieldsToInt(systemToRemove);
    const index = state.systems.findIndex(elem => elem.id == systemToRemove.id);
    //if -1 then such system does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.systems.splice(index, 1);
    }
  },
  REPLACE_SYSTEM: (state, systemToReplace) => {
    systemToReplace = mapIdFieldsToInt(systemToReplace);
    const index = state.systems.findIndex(elem => elem.id == systemToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.systems.splice(index, 1, systemToReplace);
    } else {
      // else, add new system
      state.systems.push(systemToReplace);
    }
  }
};

function mapIdFieldsToInt(system) {
  system.id = parseInt(system.id);
  system.componentIds = system.componentIds.map(elem => parseInt(elem));

  return system;
}

export default {
  state,
  getters,
  actions,
  mutations,
};
