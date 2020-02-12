<template>
  <v-row v-if="currentParsingRule">
    <v-col v-if="currentParsingRule.dateTimeRuleId > 0" cols="12" sm="4" md="4" lg="4">
      <dateTime-rule :dateTimeRuleId="currentParsingRule.dateTimeRuleId"></dateTime-rule>
    </v-col>
    <v-col v-if="currentParsingRule.severityRuleId > 0" cols="12" sm="4" md="4" lg="4">
      <severity-rule :severityRuleId="currentParsingRule.severityRuleId"></severity-rule>
    </v-col>
    <v-col v-if="currentParsingRule.messageRuleId > 0" cols="12" sm="4" md="4" lg="4">
      <message-rule :messageRuleId="currentParsingRule.messageRuleId"></message-rule>
    </v-col>

    <v-col
      v-for="customAttributeRuleId in currentParsingRule.customAttributeRuleIds"
      :key="customAttributeRuleId"
      cols="12"
      sm="4"
      md="4"
      lg="4"
    >
      <custom-attribute-rule :customAttributeRuleId="customAttributeRuleId"></custom-attribute-rule>
    </v-col>
  </v-row>
</template>

<script>
import dateTimeRule from "../DateTimeRule/DateTimeRule";
import messageRule from "../MessageRule/MessageRule";
import severityRule from "../SeverityRule/SeverityRule";
import customAttributeRule from "../CustomAttributeRule/CustomAttributeRule";
import { mapGetters, mapActions } from "vuex";
export default {
  components: {
    dateTimeRule,
    messageRule,
    severityRule,
    customAttributeRule
  },
  name: "parsing-rule",
  props: {
    parsingRuleId: {
      type: Number,
      required: true,
      default: -1
    }
  },
  methods: {
    ...mapActions(["fetchParsingRule"])
  },
  computed: {
    ...mapGetters([
      "dateTimeRuleById",
      "messageRuleById",
      "severityRuleById",
      "customAttributeRuleById",
      "parsingRuleById"
    ]),
    currentParsingRule: function() {
      return this.parsingRuleById(this.parsingRuleId);
    }
  },
  created() {
    this.fetchParsingRule(this.parsingRuleId);
  }
};
</script>