
<template>
  <div>
    <v-card>
      <v-card-title>
        <h4>{{ currentAlert.name }}</h4>
      </v-card-title>
      <div v-if="!isEditData">
        <v-card-subtitle>{{ currentAlert.description }}</v-card-subtitle>
        <v-card-text>Value: {{ currentAlert.value }}</v-card-text>
        <v-card-text>Threshold: {{ currentAlert.threshold }}</v-card-text>
        <v-card-text>Timespan (in seconds): {{ currentAlert.timespan }}</v-card-text>
        <v-card-text>Is Constant Value: {{ currentAlert.hasConstantValue }}</v-card-text>
        <v-card-text>
          Custom Attribute:
          <custom-attribute-rule :customAttributeRuleId="currentAlert.generalRuleId"></custom-attribute-rule>
        </v-card-text>
        <v-card-actions v-if="!isEditData">
          <v-btn text justify-end align-end @click="switchEdit">Edit</v-btn>
          <v-btn text justify-start align-start @click="deleteAlertById">Delete</v-btn>
        </v-card-actions>
        <v-card-text>
          <v-list>
            <v-list-item v-if="alertValues.length > 0">
              <v-list-item-content>
                <v-list-item-title>
                  <h3>Detected in following timespans:</h3>
                </v-list-item-title>
              </v-list-item-content>
            </v-list-item>
            <v-divider></v-divider>
            <v-list-item v-for="alertValue in alertValues" :key="alertValue.id">
              <v-list-item-content>
                <v-list-item-title>{{ alertValue.timespanStart.toLocaleString() }} - {{ alertValue.timespanEnd.toLocaleString()}}</v-list-item-title>
                <v-list-item-action-text>
                  <v-dialog
                    v-model="dialog[alertValue.id]"
                    fullscreen
                    hide-overlay
                    transition="dialog-bottom-transition"
                  >
                    <template v-slot:activator="{ on }">
                      <v-btn color="primary" dark v-on="on">Show Logs</v-btn>
                    </template>
                    <v-card>
                      <v-toolbar dark color="primary">
                        <v-btn icon dark @click="dialog[alertValue.id] = false">
                          <v-icon>mdi-close</v-icon>
                        </v-btn>
                        <v-toolbar-title>Logs For Alert {{ currentAlert.name }}</v-toolbar-title>
                        <v-spacer></v-spacer>
                      </v-toolbar>
                      <system-log
                        :systemId="systemIdOfAlert"
                        :dateTimeRangeFilterProp="[alertValue.timespanStart, alertValue.timespanEnd]"
                        :componentsProp="[componentId]"
                      ></system-log>
                    </v-card>
                  </v-dialog>
                </v-list-item-action-text>
              </v-list-item-content>
            </v-list-item>
          </v-list>
        </v-card-text>
      </div>
      <v-card-text v-if="isEditData">
        <alert-form
          :isEdit="true"
          :alert="currentAlert"
          :componentId="componentId"
          v-on:done="switchEdit"
        ></alert-form>
      </v-card-text>
    </v-card>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import alertForm from "./AlertForm";
import customAttributeRule from "../CustomAttributeRule/CustomAttributeRule";
import systemLog from "../NormalizedLog/SystemLog";
export default {
  name: "alert",
  data: () => {
    return {
      dialog: {},
      isEditData: false
    };
  },
  props: {
    alertId: {
      required: true,
      type: Number,
      default: -1
    },
    componentId: {
      required: true,
      type: Number,
      default: -1
    }
  },
  methods: {
    ...mapActions([
      "fetchAlert",
      "deleteAlert",
      "fetchAlertValuesByAlertId",
      "fetchComponent"
    ]),
    switchEdit: function() {
      this.isEditData = !this.isEditData;
    },
    deleteAlertById: async function() {
      await this.deleteAlert(this.currentAlert);
    }
  },
  computed: {
    ...mapGetters(["alertById", "alertValuesByAlertId", "componentById"]),
    currentAlert: function() {
      return this.alertById(this.alertId);
    },
    alertValues: function() {
      return this.alertValuesByAlertId(this.alertId);
    },
    systemIdOfAlert: function() {
      return this.componentById(this.componentId).systemId;
    }
  },
  created() {
    this.fetchAlert(this.alertId);
    this.fetchAlertValuesByAlertId(this.alertId);
    this.fetchComponent(this.componentId);
  },
  components: {
    alertForm,
    customAttributeRule,
    systemLog
  }
};
</script>

<style>
</style>