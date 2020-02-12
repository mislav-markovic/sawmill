<template>
  <v-form v-model="valid">
    <v-text-field v-model="customAttributeRule.name" label="Name" required></v-text-field>
    <v-textarea v-model="customAttributeRule.description" label="Description" required auto-grow></v-textarea>
    <v-text-field v-model="customAttributeRule.matcher" label="Matcher" required></v-text-field>
    <v-text-field v-model="customAttributeRule.startAnchor" label="Start anchor" required></v-text-field>
    <v-text-field v-model="customAttributeRule.endAnchor" label="End Anchor" required></v-text-field>
    <v-btn :disabled="!valid" color="success" class="mr-4" @click="submit">submit</v-btn>
    <v-btn color="error" class="mr-4" @click="cancel">cancel</v-btn>
  </v-form>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "customAttributeRule-form",
  data: () => {
    return {
      valid: false,
      customAttributeRuleBeforeEdit: {
        id: 0,
        name: "",
        description: "",
        matcher: "",
        startAnchor: "",
        endAnchor: ""
      }
    };
  },
  props: {
    isEdit: {
      required: false,
      type: Boolean,
      default: false
    },
    customAttributeRule: {
      default: () => {
        return {
          id: 0,
          name: "",
          description: "",
          matcher: "",
          startAnchor: "",
          endAnchor: ""
        };
      },
      required: false,
      type: Object
    }
  },
  methods: {
    ...mapActions(["createCustomAttributeRule", "editCustomAttributeRule"]),
    submit: async function() {
      var idForReturn = 0;
      if (this.isEdit) {
        await this.editCustomAttributeRule(this.customAttributeRule);
        idForReturn = this.customAttributeRule.id;
      } else {
        const newId = await this.createCustomAttributeRule(
          this.customAttributeRule
        );
        idForReturn = newId;
      }

      this.$emit("done", { isEdit: this.isEdit, id: idForReturn });
    },
    cancel: function() {
      Object.assign(
        this.customAttributeRule,
        this.customAttributeRuleBeforeEdit
      );
      this.$emit("done", {
        isEdit: this.isEdit,
        id: this.customAttributeRule.id
      });
    }
  },
  computed: {},
  created() {
    if (this.isEdit) {
      this.valid = true;
      Object.assign(
        this.customAttributeRuleBeforeEdit,
        this.customAttributeRule
      );
    } else {
      Object.assign(
        this.customAttributeRule,
        this.customAttributeRuleBeforeEdit
      );
    }
  }
};
</script>