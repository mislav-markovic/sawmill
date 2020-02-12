
<template>
  <div>
    <v-card v-if="currentCustomAttributeRule">
      <v-card-title>
        <h4>{{ currentCustomAttributeRule.name }}</h4>
      </v-card-title>
      <v-card-subtitle>{{ currentCustomAttributeRule.description }}</v-card-subtitle>
      <v-card-text>Matcher: {{ currentCustomAttributeRule.matcher }}</v-card-text>
      <v-card-text>Start Anchor: {{ currentCustomAttributeRule.startAnchor }}</v-card-text>
      <v-card-text>End Anchor: {{ currentCustomAttributeRule.endAnchor }}</v-card-text>
      <v-card-actions></v-card-actions>
    </v-card>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "custom-attribute-rule",
  props: {
    customAttributeRuleId: {
      required: true,
      type: Number,
      default: -1
    }
  },
  methods: {
    ...mapActions(["fetchCustomAttributeRule"])
  },
  computed: {
    ...mapGetters(["customAttributeRuleById"]),
    currentCustomAttributeRule: function() {
      return this.customAttributeRuleById(this.customAttributeRuleId);
    }
  },
  created() {
    this.fetchCustomAttributeRule(this.customAttributeRuleId);
  }
};
</script>

<style>
</style>