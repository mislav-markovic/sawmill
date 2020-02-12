<template>
  <div>
    <v-container fluid>
      <v-row>
        <v-col cols="12" sm="12" md="12" lg="12">
          <system :systemId="systemId"></system>
        </v-col>
      </v-row>
      <v-row justify="center">
        <v-col cols="12" sm="12" md="12" lg="12">
          <v-expansion-panels multiple accordion>
            <v-expansion-panel>
              <v-expansion-panel-header>Correlation Groups</v-expansion-panel-header>
              <v-expansion-panel-content>
                <div v-for="alertGroup in alertGroups" :key="alertGroup.id">
                  <alert-group :alertGroupId="alertGroup.id" :systemId="systemId"></alert-group>
                </div>
              </v-expansion-panel-content>
              <v-expansion-panel-content>
                <v-btn @click="isCreateAlertGroup = true">Add New Correlation Group</v-btn>
              </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel>
              <v-expansion-panel-header>Log Input</v-expansion-panel-header>
              <v-expansion-panel-content>
                <v-tabs v-model="currentTab" background-color="transparent" grow>
                  <v-tab
                    v-for="component in systemComponents"
                    :key="component.id"
                  >{{ component.name }}</v-tab>
                </v-tabs>

                <v-tabs-items v-model="currentTab">
                  <v-tab-item v-for="component in systemComponents" :key="component.id">
                    <template>
                      <raw-log-form :componentId="component.id"></raw-log-form>
                    </template>
                  </v-tab-item>
                </v-tabs-items>
              </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel>
              <v-expansion-panel-header>Reports</v-expansion-panel-header>
              <v-expansion-panel-content>
                <v-list>
                  <v-list-item
                    v-for="report in reports"
                    :key="report.id"
                    :to="`/reports/${report.id}`"
                    link
                  >
                    <v-list-item-content>
                      <v-list-item-title>{{report.timestamp.toLocaleString()}}</v-list-item-title>
                    </v-list-item-content>
                  </v-list-item>
                </v-list>
              </v-expansion-panel-content>
              <v-expansion-panel-content>
                <v-btn @click="generateReport">Generate New Report</v-btn>
              </v-expansion-panel-content>
            </v-expansion-panel>
            <v-expansion-panel>
              <v-expansion-panel-header>Normalized Log</v-expansion-panel-header>
              <v-expansion-panel-content>
                <system-log :systemId="systemId"></system-log>
              </v-expansion-panel-content>
            </v-expansion-panel>
          </v-expansion-panels>
        </v-col>
      </v-row>
    </v-container>

    <v-dialog
      v-model="isCreateAlertGroup"
      fullscreen
      hide-overlay
      transition="dialog-bottom-transition"
    >
      <v-card>
        <v-card-text>
          <alertGroup-form :systemId="systemId" v-on:done="isCreateAlertGroup = false"></alertGroup-form>
        </v-card-text>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="snackbar">
      {{ text }}
      <v-btn color="primary" text @click="snackbar = false">Close</v-btn>
    </v-snackbar>
  </div>
</template>

<script>
import SystemLog from "../NormalizedLog/SystemLog";
import System from "./System";
import RawLogForm from "../RawLog/RawLogForm";
import AlertGroup from "../Alert/AlertGroup";
import AlertGroupForm from "../Alert/AlertGroupFrom";
import { mapGetters, mapActions } from "vuex";
export default {
  name: "system-detail",
  props: {
    systemId: {
      required: true,
      type: Number,
      default: -1
    }
  },
  data: () => {
    return {
      snackbar: false,
      text: "",
      currentTab: null,
      isCreateAlertGroup: false
    };
  },
  methods: {
    ...mapActions([
      "fetchComponentsForSystem",
      "fetchSystem",
      "deleteSystem",
      "fetchAlertGroups",
      "createReport",
      "fetchReportsForSystem",
      "fetchAlerts"
    ]),
    generateReport: function() {
      this.text = `Report for system ${this.system.name} is being generated`;
      this.snackbar = true;
      this.createReport(this.systemId);
    }
  },
  computed: {
    ...mapGetters([
      "componentsBySystemId",
      "systemById",
      "alertGroupsBySystemId",
      "reportsBySystemId"
    ]),
    system: function() {
      return this.systemById(this.systemId);
    },
    reports: function() {
      return this.reportsBySystemId(this.systemId);
    },
    systemComponents: function() {
      return this.componentsBySystemId(this.systemId);
    },
    alertGroups: function() {
      return this.alertGroupsBySystemId(this.systemId);
    }
  },
  components: {
    System,
    RawLogForm,
    SystemLog,
    AlertGroup,
    AlertGroupForm
  },
  created() {
    this.fetchAlertGroups();
    this.fetchAlerts();
    this.fetchSystem(this.systemId);
    this.fetchComponentsForSystem(this.systemId);
    this.fetchReportsForSystem(this.systemId);
  }
};
</script>

<style>
</style>