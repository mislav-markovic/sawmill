
<template>
  <div>
    <v-card v-if="currentSeverityRule">
      <v-card-title>
        <h4>{{ currentSeverityRule.name }}</h4>
      </v-card-title>
      <v-card-subtitle>{{ currentSeverityRule.description }}</v-card-subtitle>
      <v-card-text>Matcher: {{ currentSeverityRule.matcher }}</v-card-text>
      <v-card-text>Start Anchor: {{ currentSeverityRule.startAnchor }}</v-card-text>
      <v-card-text>End Anchor: {{ currentSeverityRule.endAnchor }}</v-card-text>
      <v-card-text>Debug: {{ currentSeverityRule.debug }}</v-card-text>
      <v-card-text>Trace: {{ currentSeverityRule.trace }}</v-card-text>
      <v-card-text>Info: {{ currentSeverityRule.info }}</v-card-text>
      <v-card-text>Warning: {{ currentSeverityRule.warning }}</v-card-text>
      <v-card-text>Error: {{ currentSeverityRule.error }}</v-card-text>
      <v-card-text>Fatal: {{ currentSeverityRule.fatal }}</v-card-text>
      <v-card-actions></v-card-actions>
    </v-card>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "severity-rule",
  props: {
    severityRuleId: {
      required: true,
      type: Number,
      default: -1
    }
  },
  methods: {
    ...mapActions(["fetchSeverityRule"])
  },
  computed: {
    ...mapGetters(["severityRuleById", "allSeverityRules"]),
    currentSeverityRule: function() {
      return this.severityRuleById(this.severityRuleId);
    }
  },
  created() {
    this.fetchSeverityRule(this.severityRuleId);
  }
};
</script>

<style>
</style>