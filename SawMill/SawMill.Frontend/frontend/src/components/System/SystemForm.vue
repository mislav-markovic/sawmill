<template>
  <div>
    <v-card>
      <v-card-text>
        <v-text-field v-model="system.name" label="Name" required></v-text-field>
        <v-textarea v-model="system.description" label="Description" required auto-grow></v-textarea>
        <v-select
          v-model="system.componentIds"
          :items="selectItems"
          item-text="name"
          item-value="id"
          label="Components"
          multiple
        >
          <template v-slot:append-item>
            <v-divider class="mb-2"></v-divider>
            <v-list-item @click="isCreateComponent = true">
              <v-list-item-avatar color="success">
                <v-icon>mdi-plus-thick</v-icon>
              </v-list-item-avatar>

              <v-list-item-content>
                <v-list-item-subtitle>Create Component</v-list-item-subtitle>
              </v-list-item-content>
            </v-list-item>
          </template>
        </v-select>
      </v-card-text>
      <v-card-actions>
        <v-spacer></v-spacer>
        <v-btn color="success" class="mr-4" @click="submit">submit</v-btn>
        <v-btn color="error" class="mr-4" @click="cancel">cancel</v-btn>
      </v-card-actions>
    </v-card>

    <v-dialog v-model="isCreateComponent" max-width="50%" max-height="auto">
      <v-card>
        <v-card-text>
          <component-form :isEdit="false" :forSystemId="system.id" v-on:done="componentFormDone"></component-form>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import componentForm from "../Component/ComponentForm.vue";
import { mapGetters, mapActions } from "vuex";
export default {
  name: "system-form",
  props: {
    isEdit: {
      default: false,
      type: Boolean,
      required: false
    },
    system: {
      default: () => {
        return {
          id: 0,
          name: "",
          description: "",
          componentIds: []
        };
      },
      type: Object,
      required: false
    }
  },
  components: {
    componentForm
  },
  data: () => {
    return {
      valid: false,
      isCreateComponent: false,
      systemBeforeEdit: {}
    };
  },
  methods: {
    ...mapActions([
      "editSystem",
      "createSystem",
      "fetchComponents",
      "addComponentToSystem",
      "removeComponentFromSystem"
    ]),
    submit: function() {
      console.log(`trace system form submit start, edit mode: ${this.isEdit}`);
      if (this.isEdit) {
        const componentsToUnlink = this.systemBeforeEdit.componentIds.filter(
          e => !this.system.componentIds.includes(e)
        );
        const componentsToLink = this.system.componentIds.filter(
          e => !this.systemBeforeEdit.componentIds.includes(e)
        );

        console.log("link:");
        console.log(componentsToLink);
        console.log("unlink:");
        console.log(componentsToUnlink);
        componentsToUnlink.forEach(e =>
          this.removeComponentFromSystem(e, this.system.id)
        );
        componentsToLink.forEach(e =>
          this.addComponentToSystem(e, this.system.id)
        );
        this.editSystem(this.system);
      } else {
        this.createSystem(this.system);
      }

      console.log("trace system form submit end, before event emit");
      this.$emit("done");
    },
    cancel: function() {
      Object.assign(this.system, this.systemBeforeEdit);
      this.$emit("done");
    },
    componentFormDone: function({ isEdit, id }) {
      console.log(`component form done ${isEdit}, ${id}`);
      if (!isEdit && typeof id != undefined) {
        this.system.componentIds.push(id);
      }
      this.isCreateComponent = false;
    }
  },
  created() {
    this.fetchComponents();
    console.log("created system form");
    console.log(this.componentsWithoutSystem);
    if (this.isEdit) {
      this.valid = true;
      console.log(this.system.componentIds);
      Object.assign(this.systemBeforeEdit, this.system);
    }
  },
  computed: {
    ...mapGetters(["componentsBySystemId", "componentsWithoutSystem"]),
    selectItems: function() {
      var allSelectableComponents = this.componentsWithoutSystem;
      if (this.isEdit) {
        return allSelectableComponents.concat(
          this.componentsBySystemId(this.system.id)
        );
      } else {
        return allSelectableComponents;
      }
    }
  }
};
</script>