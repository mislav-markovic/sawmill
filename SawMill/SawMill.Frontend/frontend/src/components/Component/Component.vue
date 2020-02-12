<template>
  <div>
    <v-card v-if="currentComponent">
      <v-card-title>
        <h4>{{ currentComponent.name }}</h4>
      </v-card-title>
      <v-card-text>{{ currentComponent.description }}</v-card-text>
      <v-card-text v-if="currentComponent.parsingRulesId">
        <v-expansion-panels :flat="true" multiple>
          <v-expansion-panel>
            <v-expansion-panel-header>Parsing Rules</v-expansion-panel-header>
            <v-expansion-panel-content>
              <parsing-rules
                v-if="currentComponent.parsingRulesId"
                :parsingRuleId="currentComponent.parsingRulesId"
              ></parsing-rules>
            </v-expansion-panel-content>
          </v-expansion-panel>
          <v-expansion-panel>
            <v-expansion-panel-header>Alerts</v-expansion-panel-header>
            <v-expansion-panel-content>
              <div v-for="alert in alerts" :key="alert.id">
                <alert :alertId="alert.id" :componentId="componentId"></alert>
              </div>
            </v-expansion-panel-content>
            <v-expansion-panel-content>
              <v-btn @click="isCreateAlert = true">Add New Alert</v-btn>
            </v-expansion-panel-content>
          </v-expansion-panel>
        </v-expansion-panels>
      </v-card-text>
      <v-card-actions>
        <v-btn
          v-if="!currentComponent.parsingRulesId"
          :to="{path: `/parsingRules/${currentComponent.id}`}"
        >Parsing Rules Setup</v-btn>
        <v-btn
          v-if="currentComponent.parsingRulesId"
          :to="{path: `/parsingRules/${currentComponent.id}/${currentComponent.parsingRulesId}`}"
        >Edit Parsing Rules</v-btn>
        <v-btn :to="{path: '/system'}">Back</v-btn>
      </v-card-actions>
    </v-card>

    <v-dialog v-model="isCreateAlert" max-width="50%" max-height="auto">
      <v-card>
        <v-card-text>
          <alert-form :isEdit="false" :componentId="componentId" v-on:done="alertFormDone"></alert-form>
        </v-card-text>
      </v-card>
    </v-dialog>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import parsingRules from "../ParsingRules/ParsingRules";
import alertForm from "../Alert/AlertForm";
import alert from "../Alert/Alert";
export default {
  name: "app-component",
  data: () => {
    return {
      isCreateAlert: false
    };
  },
  props: {
    componentId: {
      required: true,
      type: Number,
      default: -1
    }
  },
  methods: {
    ...mapActions(["fetchComponent", "fetchAlerts"]),
    alertFormDone: function() {
      this.isCreateAlert = false;
    }
  },
  computed: {
    ...mapGetters(["componentById", "allComponents", "alertsByComponentId"]),
    alerts: function() {
      let temp = this.alertsByComponentId(this.componentId);
      if (typeof temp === "undefined") {
        return [];
      } else {
        return temp;
      }
    },
    currentComponent: function() {
      return this.componentById(this.componentId);
    }
  },
  created() {
    this.fetchAlerts();
    this.fetchComponent(this.componentId);
  },
  components: {
    parsingRules,
    alertForm,
    alert
  }
};
</script>

<style scoped>
</style>
