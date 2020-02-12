
<template>
  <v-card v-if="currentDateTimeRule">
    <v-card-title>
      <h4>{{ currentDateTimeRule.name }}</h4>
    </v-card-title>
    <v-card-subtitle>{{ currentDateTimeRule.description }}</v-card-subtitle>
    <v-card-text>Matcher: {{ currentDateTimeRule.matcher }}</v-card-text>
    <v-card-text>Start Anchor: {{ currentDateTimeRule.startAnchor }}</v-card-text>
    <v-card-text>End Anchor: {{ currentDateTimeRule.endAnchor }}</v-card-text>
    <v-card-text>Date Format: {{ currentDateTimeRule.dateFormat }}</v-card-text>
    <v-card-actions></v-card-actions>
  </v-card>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "dateTime-rule",
  props: {
    dateTimeRuleId: {
      required: true,
      type: Number,
      default: -1
    }
  },
  methods: {
    ...mapActions(["fetchDateTimeRule"])
  },
  computed: {
    ...mapGetters(["dateTimeRuleById"]),
    currentDateTimeRule: function() {
      return this.dateTimeRuleById(this.dateTimeRuleId);
    }
  },
  created() {
    this.fetchDateTimeRule(this.dateTimeRuleId);
  }
};
</script>

<style>
</style>