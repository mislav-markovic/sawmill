<template>
  <v-form v-model="valid">
    <v-text-field v-model="dateTimeRule.name" label="Name" required></v-text-field>
    <v-textarea v-model="dateTimeRule.description" label="Description" required auto-grow></v-textarea>
    <v-text-field v-model="dateTimeRule.matcher" label="Matcher" required></v-text-field>
    <v-text-field v-model="dateTimeRule.startAnchor" label="Start anchor" required></v-text-field>
    <v-text-field v-model="dateTimeRule.endAnchor" label="End Anchor" required></v-text-field>
    <v-text-field v-model="dateTimeRule.dateFormat" label="Date Format" hide-details single-line></v-text-field>
    <v-btn :disabled="!valid" color="success" class="mr-4" @click="submit">submit</v-btn>
    <v-btn color="error" class="mr-4" @click="cancel">cancel</v-btn>
  </v-form>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "datetimerule-form",
  data: () => {
    return {
      valid: false,
      dateTimeRuleBeforeEdit: {
        id: 0,
        name: "",
        description: "",
        matcher: "",
        startAnchor: "",
        endAnchor: "",
        dateFormat: ""
      }
    };
  },
  props: {
    isEdit: {
      required: false,
      type: Boolean,
      default: false
    },
    dateTimeRule: {
      default: () => {
        return {
          id: 0,
          name: "",
          description: "",
          matcher: "",
          startAnchor: "",
          endAnchor: "",
          dateFormat: ""
        };
      },
      required: false,
      type: Object
    }
  },
  methods: {
    ...mapActions(["createDateTimeRule", "editDateTimeRule"]),
    submit: async function() {
      var idForReturn = 0;
      if (this.isEdit) {
        await this.editDateTimeRule(this.dateTimeRule);
        idForReturn = this.dateTimeRule.id;
      } else {
        const newId = await this.createDateTimeRule(this.dateTimeRule);
        idForReturn = newId;
      }

      this.$emit("done", { isEdit: this.isEdit, id: idForReturn });
    },
    cancel: function() {
      Object.assign(this.dateTimeRule, this.dateTimeRuleBeforeEdit);
      this.$emit("done", { isEdit: this.isEdit, id: this.dateTimeRule.id });
    }
  },
  computed: {},
  created() {
    console.log(`is date time form edit: ${this.isEdit}`);
    if (this.isEdit) {
      this.valid = true;
      console.log("edit date time rule");
      console.log(this.dateTimeRule);
      Object.assign(this.dateTimeRuleBeforeEdit, this.dateTimeRule);
    } else {
      Object.assign(this.dateTimeRule, this.dateTimeRuleBeforeEdit);
    }
  }
};
</script>