
<template>
  <div>
    <v-card v-if="currentMessageRule">
      <v-card-title>
        <h4>{{ currentMessageRule.name }}</h4>
      </v-card-title>
      <v-card-subtitle>{{ currentMessageRule.description }}</v-card-subtitle>
      <v-card-text>Matcher: {{ currentMessageRule.matcher }}</v-card-text>
      <v-card-text>Start Anchor: {{ currentMessageRule.startAnchor }}</v-card-text>
      <v-card-text>End Anchor: {{ currentMessageRule.endAnchor }}</v-card-text>
      <v-card-text>Max Length: {{ currentMessageRule.maxLength }}</v-card-text>
      <v-card-actions></v-card-actions>
    </v-card>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "message-rule",
  props: {
    messageRuleId: {
      required: true,
      type: Number,
      default: -1
    }
  },
  methods: {
    ...mapActions(["fetchMessageRule"])
  },
  computed: {
    ...mapGetters(["messageRuleById", "allMessageRules"]),
    currentMessageRule: function() {
      return this.messageRuleById(this.messageRuleId);
    }
  },
  created() {
    this.fetchMessageRule(this.messageRuleId);
  }
};
</script>

<style>
</style>