
<template>
  <div>
    <v-card>
      <v-card-title>
        <h4>{{ currentAlertGroup.name }}</h4>
      </v-card-title>
      <div>
        <v-card-subtitle>{{ currentAlertGroup.description }}</v-card-subtitle>
        <v-card-text>Timespan (in seconds): {{ currentAlertGroup.timespan }}</v-card-text>
        <v-card-text>
          Alerts:
          <v-list>
            <v-list-item v-for="alert in currentAlertGroup.alerts" :key="alert.alertId">
              <v-list-item-content>
                <v-list-item-title>{{ alert.alertName }}</v-list-item-title>
              </v-list-item-content>
              <v-list-item-content>
                <v-list-item-title>Position: {{alert.position}}</v-list-item-title>
              </v-list-item-content>
              <v-list-item-content>
                <v-list-item-title>Negated?: {{alert.not}}</v-list-item-title>
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </v-card-text>
        <v-card-text>
          <v-list>
            <v-list-item v-if="alertGroupValues.length > 0">
              <v-list-item-content>
                <v-list-item-title>
                  <h3>Detected in following timespans:</h3>
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-divider></v-divider>
            <v-list-item v-for="alertGroupValue in alertGroupValues" :key="alertGroupValue.id">
              <v-list-item-content>
                <v-list-item-title>{{ alertGroupValue.timespanStart.toLocaleString() }} - {{ alertGroupValue.timespanEnd.toLocaleString()}}</v-list-item-title>
                <v-list-item-title
                  v-for="alertValue in alertGroupValue.alertValues"
                  :key="alertValue.id"
                >{{alertById(alertValue.alertId).name}}: {{ alertValue.timespanStart.toLocaleString() }} - {{ alertValue.timespanEnd.toLocaleString()}}</v-list-item-title>
                <v-list-item-action-text>
                  <v-dialog
                    v-model="dialog[alertGroupValue.id]"
                    fullscreen
                    hide-overlay
                    transition="dialog-bottom-transition"
                  >
                    <template v-slot:activator="{ on }">
                      <v-btn color="primary" dark v-on="on">Show Logs</v-btn>
                    </template>
                    <v-card>
                      <v-toolbar dark color="primary">
                        <v-btn icon dark @click="dialog[alertGroupValue.id] = false">
                          <v-icon>mdi-close</v-icon>
                        </v-btn>
                        <v-toolbar-title>Logs For Correlation Group {{ currentAlertGroup.name }}</v-toolbar-title>
                        <v-spacer></v-spacer>
                      </v-toolbar>
                      <system-log
                        :systemId="currentAlertGroup.systemId"
                        :dateTimeRangeFilterProp="[alertGroupValue.timespanStart, alertGroupValue.timespanEnd]"
                        :componentsProp="[]"
                      ></system-log>
                    </v-card>
                  </v-dialog>
                </v-list-item-action-text>
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </v-card-text>
      </div>
    </v-card>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import systemLog from "../NormalizedLog/SystemLog";
export default {
  name: "alert-group",
  data: () => {
    return {
      dialog: {}
    };
  },
  props: {
    alertGroupId: {
      required: true,
      type: Number,
      default: -1
    },
    systemId: {
      required: true,
      type: Number,
      default: -1
    }
  },
  methods: {
    ...mapActions([
      "fetchAlerts",
      "fetchAlertGroups",
      "fetchAlertGroupValuesByAlertGroupId"
    ])
  },
  computed: {
    ...mapGetters([
      "alertById",
      "alertGroupById",
      "alertGroupValuesByAlertGroupId"
    ]),
    currentAlertGroup: function() {
      return this.alertGroupById(this.alertGroupId);
    },
    alertGroupValues: function() {
      return this.alertGroupValuesByAlertGroupId(this.alertGroupId);
    }
  },
  created() {
    this.fetchAlerts();
    this.fetchAlertGroupValuesByAlertGroupId(this.alertGroupId);
    this.fetchAlertGroups();
  },
  components: {
    systemLog
  }
};
</script>

<style>
</style>