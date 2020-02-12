/*
 component = {
    id = 0,
    name = 'test',
    description = "desc",
    systemId = 1,
    parsingRulesId = 1
}
*/
const state = {
  components: [],
};

const getters = {
  componentById: (state) => {
    return (componentId) => {
      if (typeof componentId === 'string' || componentId instanceof String) {
        componentId = parseInt(componentId);
      }
      return state.components.find(elem => elem.id === componentId)
    }
  },

  allComponents: state => state.components,

  componentsBySystemId: (state) => (systemIdParam) => {
    if (typeof systemIdParam === 'string' || systemIdParam instanceof String) {
      systemIdParam = parseInt(systemIdParam);
    }
    return state.components.filter(elem => parseInt(elem.systemId) == systemIdParam);
  },

  componentsWithoutSystem: (state, getters) => getters.allComponents.filter(comp => comp.systemId <= 0),
};

const actions = {
  async fetchComponents({ commit }) {
    try {
      const response = await this.$http.get("component");
      commit("SET_COMPONENTS", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchComponent({ commit }, id) {
    try {
      const response = await this.$http.get(`component/${id}`);
      commit("REPLACE_COMPONENT", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async fetchComponentsForSystem({ commit }, systemId) {
    try {
      const response = await this.$http.get(`component`, { params: { systemId: systemId } });
      response.data.forEach(componentElem => commit("REPLACE_COMPONENT", componentElem));
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async editComponent({ commit }, componentToEdit) {
    try {
      const response = await this.$http.put(`component/${componentToEdit.id}`, componentToEdit);
      commit("REPLACE_COMPONENT", response.data);
    }
    catch (error) {
      // TODO
      console.log(error);
    }
  },
  async deleteComponent({ commit }, componentToDelete) {
    try {
      const response = await this.$http.delete(`component/${componentToDelete.id}`);
      if (!!response.data === true) {
        commit("REMOVE_COMPONENT", componentToDelete);
      }
    }
    catch (error) {
      // TODO: how to handle
      console.error(error);
    }
  },
  async createComponent({ commit }, componentToCreate) {
    console.log("create component action");
    // try to create new
    try {
      const response = await this.$http.post("component", componentToCreate);

      // when created add it to store, note: add returned value because it will have id 
      commit("ADD_COMPONENT", response.data);
      return response.data.id;
    }
    catch (error) {
      // TODO: how to handle error, show to user?
      console.log(error);
    }
  },
  unlinkComponentsFromSystem({ commit, getters }, systemId) {
    console.log(`unlinking from system ${systemId}`);
    getters.allComponents.filter(comp => comp.systemId == systemId).forEach(elem => {
      console.log(`unlinking component ${elem.id}`);
      elem.systemId = 0;
      commit('REPLACE_COMPONENT', elem);
    })
  },
  linkComponentToSystem({ commit, getters }, componentId, systemId) {
    console.log(`linking component ${componentId} to system ${systemId}`);
    var component = getters.componentById(componentId);
    component.systemId = parseInt(systemId);
    commit('REPLACE_COMPONENT', component);
  }
};

const mutations = {
  SET_COMPONENTS: (state, components) => state.components = components.map(elem => mapIdFieldsToInt(elem)),
  ADD_COMPONENT: (state, componentToAdd) => state.components.push(mapIdFieldsToInt(componentToAdd)),
  REMOVE_COMPONENT: (state, componentToRemove) => {
    componentToRemove = mapIdFieldsToInt(componentToRemove);
    const index = state.components.findIndex(elem => elem.id == componentToRemove.id);
    //if -1 then such component does not exists
    if (index !== -1) {
      // if it exists remove element at its index
      state.components.splice(index, 1);
    }
  },
  REPLACE_COMPONENT: (state, componentToReplace) => {
    componentToReplace = mapIdFieldsToInt(componentToReplace);
    const index = state.components.findIndex(elem => elem.id == componentToReplace.id);
    if (index !== -1) {
      // if it exists replace element at its index
      state.components.splice(index, 1, componentToReplace);
    } else {
      // else, add new component
      state.components.unshift(componentToReplace);
    }
  },
};

function mapIdFieldsToInt(component) {
  component.id = parseInt(component.id);
  component.systemId = parseInt(component.systemId);
  component.parsingRulesId = parseInt(component.parsingRulesId);

  return component;
}
export default {
  state,
  getters,
  actions,
  mutations,
};
