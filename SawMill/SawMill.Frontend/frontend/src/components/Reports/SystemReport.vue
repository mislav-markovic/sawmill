<template>
  <v-container fluid>
    <v-data-iterator :items="alertGroupCount" hide-default-footer>
      <template v-slot:header>
        <v-toolbar class="mb-3" color="indigo darken-5" dark flat>
          <v-toolbar-title>Report for system {{system.name}}</v-toolbar-title>
        </v-toolbar>
      </template>

      <template v-slot:default="props">
        <v-row>
          <v-col cols="12" sm="12" md="12" lg="12">
            <v-list>
              <v-list-item v-for="(alertGroup, i) in props.items" :key="i">
                <v-list-item-content>
                  <v-list-item-title>{{alertGroup.name}}: {{alertGroup.count}}</v-list-item-title>
                </v-list-item-content>
              </v-list-item>
            </v-list>
          </v-col>
        </v-row>

        <v-divider></v-divider>

        <v-row>
          <v-col
            v-for="componentReport in report.componentReports"
            :key="componentReport.componentId"
            cols="12"
            sm="6"
            md="4"
            lg="3"
          >
            <v-card>
              <v-card-title class="subheading">{{ componentById(componentReport.componentId).name }}</v-card-title>

              <v-divider></v-divider>

              <v-card-text>
                <v-list dense>
                  <v-list-item v-for="(alert, i) in componentAlerts(componentReport)" :key="i">
                    <v-list-item-content>{{alert.name}}:</v-list-item-content>
                    <v-list-item-content class="align-end">{{ alert.count }}</v-list-item-content>
                  </v-list-item>
                </v-list>
              </v-card-text>
            </v-card>
          </v-col>
        </v-row>
      </template>

      <template v-slot:footer>
        <v-toolbar class="mt-2" color="indigo" dark flat>
          <v-toolbar-title class="subheading">Generated on {{report.timestamp.toLocaleString()}}</v-toolbar-title>
        </v-toolbar>
      </template>
    </v-data-iterator>
    <v-row>
      <v-col cols="12" sm="8" md="8" lg="8" class="text-left">
        <v-btn text @click="back">Back</v-btn>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "system-report",
  props: {
    reportId: {
      type: Number,
      required: true,
      default: 0
    }
  },
  methods: {
    ...mapActions(["fetchComponents"]),
    back: function() {
      this.$router.go(-1);
    },
    componentAlerts: function(componentReport) {
      let alerts = [];
      console.log(componentReport.alertCount);

      for (const [key, value] of Object.entries(componentReport.alertCount)) {
        console.log(`key: ${key}, value: ${value}`);
        const alert = this.alertById(key);
        alerts.push({ name: alert.name, count: value });
      }

      return alerts;
    }
  },
  computed: {
    ...mapGetters([
      "systemById",
      "componentById",
      "reportById",
      "alertGroupById",
      "alertById",
      "alertsByComponentId"
    ]),
    report: function() {
      return this.reportById(this.reportId);
    },
    system: function() {
      return this.systemById(this.report.systemId);
    },
    alertGroupCount: function() {
      let alertGroups = [];
      for (const [key, value] of Object.entries(this.report.alertGroupCount)) {
        const alertGroup = this.alertGroupById(key);
        alertGroups.push({ name: alertGroup.name, count: value });
      }
      console.log(alertGroups);

      return alertGroups;
    }
  },
  created() {
    this.fetchComponents();
  }
};
</script>

<style>
</style>