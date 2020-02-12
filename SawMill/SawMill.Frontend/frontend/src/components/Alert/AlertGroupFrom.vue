<template>
  <v-form>
    <v-text-field v-model="alertGroup.name" label="Name" :required="true"></v-text-field>
    <v-textarea v-model="alertGroup.description" label="Description" required auto-grow></v-textarea>
    <v-row>
      <v-col cols="12" sm="4" md="4" lg="4">
        <v-text-field
          v-model="alertGroup.timespan"
          label="Seconds"
          type="number"
          :rules="[rules.required]"
        ></v-text-field>
      </v-col>
    </v-row>
    <v-select
      v-model="selectedAlertId"
      :items="selectableAlerts"
      item-text="name"
      item-value="id"
      label="Correlation alerts"
    ></v-select>
    <v-list v-if="selectedAlerts.length > 0">
      <v-list-item v-for="alert in selectedAlerts" :key="alert.alertId">
        <v-list-item-content>
          <v-list-item-title>{{ alert.alertName }}</v-list-item-title>
        </v-list-item-content>
        <v-list-item-content>
          <v-list-item-title>
            <v-text-field v-model="alert.position" label="Position" type="number"></v-text-field>
          </v-list-item-title>
        </v-list-item-content>
        <v-list-item-content>
          <v-list-item-title>
            <v-checkbox v-model="alert.not" :label="'Is Alert Negated?'"></v-checkbox>
          </v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>
    <v-btn color="success" class="mr-4" @click="addAlert">Add Alert</v-btn>
    <v-btn color="success" class="mr-4" @click="submit">submit</v-btn>
    <v-btn color="error" class="mr-4" @click="cancel">cancel</v-btn>
  </v-form>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
export default {
  name: "alertGroup-form",
  data: () => {
    return {
      selectedAlerts: [],
      alertGroup: {
        id: 0,
        name: "",
        description: "",
        timespan: 0,
        systemId: 0,
        alerts: []
      },
      selectedAlertId: [],
      rules: {
        required: value => !!value || "Required."
      }
    };
  },
  props: {
    systemId: {
      required: true,
      type: Number
    }
  },
  methods: {
    ...mapActions(["fetchAlerts", "createAlertGroup"]),
    addAlert: function() {
      const fetched = this.alertById(this.selectedAlertId);
      console.log(`selected alert id ${this.selectedAlertId}`);
      let alert = {
        alertId: this.selectedAlertId,
        alertName: fetched.name,
        position: 0,
        not: false
      };

      this.selectedAlerts.push(alert);
    },
    submit: async function() {
      var idForReturn = 0;
      this.alertGroup.alerts = this.selectedAlerts;
      console.log("alertGroup to create");
      console.log(this.alertGroup);
      const newId = await this.createAlertGroup(this.alertGroup);
      idForReturn = newId;
      this.$emit("done", { isEdit: false, id: idForReturn });
    },
    cancel: function() {
      this.$emit("done", { isEdit: false, id: this.alertGroup.id });
    }
  },
  computed: {
    ...mapGetters(["alertById", "componentsBySystemId", "alertsByComponentId"]),
    selectableAlerts: function() {
      let components = [...this.componentsBySystemId(this.systemId)];
      console.log("components");
      console.log(components);
      let alerts = [];
      components.forEach(elem => {
        alerts.push(...this.alertsByComponentId(elem.id));
      });
      return alerts;
    }
  },
  created() {
    this.fetchAlerts();
    this.alertGroup.systemId = this.systemId;
  }
};
</script>