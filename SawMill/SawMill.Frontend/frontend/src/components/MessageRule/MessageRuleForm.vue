<template>
  <v-form v-model="valid">
    <v-text-field v-model="messageRule.name" label="Name" required></v-text-field>
    <v-textarea v-model="messageRule.description" label="Description" required auto-grow></v-textarea>
    <v-text-field v-model="messageRule.matcher" label="Matcher" required></v-text-field>
    <v-text-field v-model="messageRule.startAnchor" label="Start anchor" required></v-text-field>
    <v-text-field v-model="messageRule.endAnchor" label="End Anchor" required></v-text-field>
    <v-text-field
      v-model="messageRule.maxLength"
      label="Max Length"
      hide-details
      single-line
      type="number"
    ></v-text-field>
    <v-btn :disabled="!valid" color="success" class="mr-4" @click="submit">submit</v-btn>
    <v-btn color="error" class="mr-4" @click="cancel">cancel</v-btn>
  </v-form>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "messageRule-form",
  data: () => {
    return {
      valid: false,
      messageRuleBeforeEdit: {
        id: 0,
        name: "",
        description: "",
        matcher: "",
        startAnchor: "",
        endAnchor: "",
        maxLength: 0
      }
    };
  },
  props: {
    isEdit: {
      required: false,
      type: Boolean,
      default: false
    },
    messageRule: {
      default: () => {
        return {
          id: 0,
          name: "",
          description: "",
          matcher: "",
          startAnchor: "",
          endAnchor: "",
          maxLength: 0
        };
      },
      required: false,
      type: Object
    }
  },
  methods: {
    ...mapActions(["createMessageRule", "editMessageRule"]),
    submit: async function() {
      var idForReturn = 0;
      if (this.isEdit) {
        await this.editMessageRule(this.messageRule);
        idForReturn = this.messageRule.id;
      } else {
        const newId = await this.createMessageRule(this.messageRule);
        idForReturn = newId;
      }

      this.$emit("done", { isEdit: this.isEdit, id: idForReturn });
    },
    cancel: function() {
      Object.assign(this.messageRule, this.messageRuleBeforeEdit);
      this.$emit("done", { isEdit: this.isEdit, id: this.messageRule.id });
    }
  },
  computed: {},
  created() {
    if (this.isEdit) {
      this.valid = true;
      Object.assign(this.messageRuleBeforeEdit, this.messageRule);
    } else {
      Object.assign(this.messageRule, this.messageRuleBeforeEdit);
    }
  }
};
</script>