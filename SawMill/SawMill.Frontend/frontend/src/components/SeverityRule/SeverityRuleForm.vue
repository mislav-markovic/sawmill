<template>
  <v-form v-model="valid">
    <v-text-field v-model="severityRule.name" label="Name" required></v-text-field>
    <v-textarea v-model="severityRule.description" label="Description" required auto-grow></v-textarea>
    <v-text-field v-model="severityRule.matcher" label="Matcher" required></v-text-field>
    <v-text-field v-model="severityRule.startAnchor" label="Start anchor" required></v-text-field>
    <v-text-field v-model="severityRule.endAnchor" label="End Anchor" required></v-text-field>
    <v-text-field v-model="severityRule.debug" label="Debug" hide-details single-line></v-text-field>
    <v-text-field v-model="severityRule.trace" label="Trace" hide-details single-line></v-text-field>
    <v-text-field v-model="severityRule.info" label="Info" hide-details single-line></v-text-field>
    <v-text-field v-model="severityRule.warning" label="Warning" hide-details single-line></v-text-field>
    <v-text-field v-model="severityRule.error" label="Error" hide-details single-line></v-text-field>
    <v-text-field v-model="severityRule.fatal" label="Fatal" hide-details single-line></v-text-field>
    <v-btn :disabled="!valid" color="success" class="mr-4" @click="submit">submit</v-btn>
    <v-btn color="error" class="mr-4" @click="cancel">cancel</v-btn>
  </v-form>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "severityRule-form",
  data: () => {
    return {
      valid: false,
      severityRuleBeforeEdit: {
        id: 0,
        name: "",
        description: "",
        matcher: "",
        startAnchor: "",
        endAnchor: "",
        debug: "",
        trace: "",
        info: "",
        warning: "",
        error: "",
        fatal: ""
      }
    };
  },
  props: {
    isEdit: {
      required: false,
      type: Boolean,
      default: false
    },
    severityRule: {
      default: () => {
        return {
          id: 0,
          name: "",
          description: "",
          matcher: "",
          startAnchor: "",
          endAnchor: "",
          debug: "",
          trace: "",
          info: "",
          warning: "",
          error: "",
          fatal: ""
        };
      },
      required: false,
      type: Object
    }
  },
  methods: {
    ...mapActions(["createSeverityRule", "editSeverityRule"]),
    submit: async function() {
      var idForReturn = 0;
      if (this.isEdit) {
        await this.editSeverityRule(this.severityRule);
        idForReturn = this.severityRule.id;
      } else {
        const newId = await this.createSeverityRule(this.severityRule);
        idForReturn = newId;
      }

      this.$emit("done", { isEdit: this.isEdit, id: idForReturn });
    },
    cancel: function() {
      Object.assign(this.severityRule, this.severityRuleBeforeEdit);
      this.$emit("done", { isEdit: this.isEdit, id: this.severityRule.id });
    }
  },
  computed: {},
  created() {
    if (this.isEdit) {
      this.valid = true;
      console.log("edit date time rule");
      console.log(this.severityRule);
      Object.assign(this.severityRuleBeforeEdit, this.severityRule);
    } else {
      Object.assign(this.severityRule, this.severityRuleBeforeEdit);
    }
  }
};
</script>