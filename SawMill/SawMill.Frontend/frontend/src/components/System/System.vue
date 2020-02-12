<template>
  <v-card>
    <v-card-title>
      <v-toolbar flat>
        <v-toolbar-title class="grey--text">
          <h4>{{ system.name }}</h4>
        </v-toolbar-title>

        <v-spacer></v-spacer>

        <v-btn icon :to="`/system/${system.id}`">
          <v-icon>mdi-clipboard-arrow-right-outline</v-icon>
        </v-btn>
        <v-btn icon @click="switchEdit">
          <v-icon>mdi-pencil</v-icon>
        </v-btn>

        <v-btn icon @click="buttonDeleteSystem">
          <v-icon>mdi-trash-can-outline</v-icon>
        </v-btn>
      </v-toolbar>
    </v-card-title>

    <v-card-text>
      <!-- show data in non edit form -->
      <v-list three-line v-show="!isEditing">
        <v-list-item>
          <v-list-item-content>
            <v-textarea auto-grow readonly label="Description" :value="system.description"></v-textarea>
          </v-list-item-content>
        </v-list-item>
        <v-list-group dense>
          <template v-slot:activator>
            <v-list-item-content>
              <v-list-item-title>Components</v-list-item-title>
            </v-list-item-content>
          </template>

          <v-list-item v-for="component in systemComponents" :key="component.id">
            <v-list-item-content>
              <v-list-item-title v-text="component.name"></v-list-item-title>
              <v-list-item-subtitle v-text="component.description"></v-list-item-subtitle>
            </v-list-item-content>
            <v-list-item-action>
              <v-btn icon @click="switchComponentEdit(component)">
                <v-icon>mdi-pencil</v-icon>
              </v-btn>
              <v-btn icon :to="`/component/${component.id}`">
                <v-icon>mdi-clipboard-arrow-right-outline</v-icon>
              </v-btn>
            </v-list-item-action>

            <v-dialog v-model="isComponentEditing" persistent max-width="600px">
              <v-card>
                <v-card-text>
                  <component-form
                    :isEdit="true"
                    :forSystemId="componentForEdit.systemId"
                    :component="componentForEdit"
                    v-on:done="doneComponentEditing"
                  ></component-form>
                </v-card-text>
              </v-card>
            </v-dialog>
          </v-list-item>
          <v-list-item></v-list-item>
        </v-list-group>
      </v-list>

      <!-- show form in edit form -->
      <system-form v-show="!!isEditing" :isEdit="true" :system="system" v-on:done="switchEdit"></system-form>
    </v-card-text>
  </v-card>
</template>

<script>
import systemForm from "./SystemForm";
import componentForm from "../Component/ComponentForm";
import { mapGetters, mapActions } from "vuex";
export default {
  name: "system",
  props: {
    systemId: {
      type: Number,
      required: true,
      default: -1
    }
  },
  data: () => {
    return {
      isEditing: false,
      isComponentEditing: false,
      componentForEdit: {}
    };
  },
  components: {
    systemForm,
    componentForm
  },
  methods: {
    ...mapActions(["fetchComponentsForSystem", "fetchSystem", "deleteSystem"]),
    buttonDeleteSystem: function() {
      console.log(`buttonDeleteSystem ${this.system.name}:${this.system.id}`);
      this.deleteSystem(this.system);
    },
    switchEdit: function() {
      console.log("trace system switch edit");
      this.isEditing = !this.isEditing;
    },
    switchComponentEdit: function(comp) {
      Object.assign(this.componentForEdit, comp);
      this.isComponentEditing = true;
    },
    doneComponentEditing: function() {
      this.componentForEdit = {};
      this.isComponentEditing = false;
    }
  },
  computed: {
    ...mapGetters(["componentsBySystemId", "systemById"]),
    system: function() {
      return this.systemById(this.systemId);
    },
    systemComponents: function() {
      return this.componentsBySystemId(this.systemId);
    }
  },
  created() {
    this.fetchSystem(this.systemId);
    this.fetchComponentsForSystem(this.systemId);
  }
};
</script>