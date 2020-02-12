<template>
  <v-form v-model="valid">
    <v-text-field v-model="component.name" label="Name" required></v-text-field>
    <v-textarea v-model="component.description" label="Description" required auto-grow></v-textarea>
    <v-select
      v-show="isEdit"
      v-model="component.systemId"
      :items="selectItems"
      :hint="selectHint"
      persistent-hint
      item-text="name"
      item-value="id"
      label="System"
    ></v-select>
    <v-btn :disabled="!valid" color="success" class="mr-4" @click="submit">submit</v-btn>
    <v-btn color="error" class="mr-4" @click="cancel">cancel</v-btn>
  </v-form>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "component-form",
  data: () => {
    return {
      valid: false,
      componentBeforeEdit: {}
    };
  },
  props: {
    isEdit: {
      required: false,
      type: Boolean,
      default: false
    },
    forSystemId: {
      required: false,
      default: 0,
      type: Number
    },
    component: {
      default: () => {
        return {
          id: 0,
          name: "",
          description: "",
          systemId: 0
        };
      },
      required: false,
      type: Object
    }
  },
  methods: {
    ...mapActions([
      "fetchSystems",
      "addComponentToSystem",
      "removeComponentFromSystem",
      "createComponent",
      "editComponent"
    ]),
    submit: async function() {
      var idForReturn = 0;
      if (this.isEdit) {
        await this.editComponent(this.component);
        this.removeComponentFromSystem(
          this.component.id,
          this.component.systemId
        );
        this.addComponentToSystem(this.component.id, this.component.systemId);
        idForReturn = this.component.id;
      } else {
        const newId = await this.createComponent(this.component);
        this.addComponentToSystem(newId, this.component.systemId);
        idForReturn = newId;
      }

      this.$emit("done", { isEdit: this.isEdit, id: idForReturn });
    },
    cancel: function() {
      Object.assign(this.component, this.componentBeforeEdit);
      this.$emit("done", { isEdit: this.isEdit, id: this.component.id });
    }
  },
  computed: {
    ...mapGetters(["allSystems", "systemById"]),
    selectItems: function() {
      return this.allSystems;
    },
    selectHint: function() {
      var hint = "";
      if (this.component.systemId > 0) {
        const system = this.systemById(this.component.systemId);

        if (!!system) {
          hint = system.description;
        }
      }
      return hint;
    }
  },
  created() {
    this.fetchSystems();
    if (this.isEdit) {
      this.valid = true;
      console.log("edit comp");
      console.log(this.component);
      Object.assign(this.componentBeforeEdit, this.component);
    } else {
      this.component.systemId = this.forSystemId;
    }
  }
};
</script>