<template>
  <div>
    <v-data-table
      :headers="headers"
      :items="logs"
      :loading="loading"
      :search="search"
      item-key="id"
      hide-default-footer
      disable-sort
      disable-pagination
      :show-expand="!selectable"
      :show-select="selectable"
      single-select
      v-model="selectedLog"
      class="elevation-1"
    >
      <template v-slot:expanded-item="{ item, headers }">
        <td :colspan="headers.length">
          <v-simple-table dense>
            <template v-slot:default>
              <thead>
                <tr>
                  <th class="text-left">Custom Attribute Rule</th>
                  <th class="text-left">Custom Attribute Value</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="customAttribute in item.customValues" :key="customAttribute.id">
                  <td>{{ customAttribute.customAttributeRuleName }}</td>
                  <td>{{ customAttribute.value }}</td>
                </tr>
              </tbody>
            </template>
          </v-simple-table>
        </td>
      </template>

      <template v-slot:body.prepend>
        <tr>
          <td></td>
          <td>
            <v-select
              multiple
              v-model="components"
              :items="availableComponents"
              item-text="name"
              item-value="id"
              label="Components"
            >
              <template v-slot:selection="{ item, index }">
                <v-chip v-if="index === 0">
                  <span>{{ item.name }}</span>
                </v-chip>
                <span
                  v-if="index === 1"
                  class="grey--text caption"
                >(+{{ components.length - 1 }} others)</span>
              </template>
            </v-select>
          </td>
          <td>
            <date-picker
              v-model="dateTimeRangeFilter"
              type="datetime"
              range
              value-type="date"
              placeholder="Select datetime range"
            ></date-picker>
          </td>
          <td>
            <v-select
              multiple
              v-model="severityLevelFilters"
              :items="severityLevels"
              item-value="level"
              item-text="display"
              label="Severity"
            >
              <template v-slot:selection="{ item, index }">
                <v-chip v-if="item.level === Math.min(...severityLevelFilters)">
                  <span>{{ item.display }}</span>
                </v-chip>
                <span
                  v-if="index === 1"
                  class="grey--text caption"
                >(+{{ severityLevelFilters.length - 1 }} others)</span>
              </template>
            </v-select>
          </td>
          <td>
            <v-text-field v-model="search" type="text" label="Search Message"></v-text-field>
          </td>
          <td></td>
        </tr>
      </template>

      <template v-slot:footer v-if="!isFixedLogMode">
        <v-btn text block justify-center align-center @click="loadMore">Load More</v-btn>
      </template>

      <template v-slot:item.dateTime="{ item }">{{item.dateTime.toLocaleString()}}</template>
    </v-data-table>
  </div>
</template>

<script>
import { mapGetters, mapActions } from "vuex";
import DatePicker from "vue2-datepicker";
import "vue2-datepicker/index.css";
export default {
  name: "system-log",
  props: {
    systemId: {
      required: true,
      default: 0,
      type: Number
    },
    dateTimeRangeFilterProp: {
      required: false,
      type: Array,
      default: () => []
    },
    componentsProp: {
      required: false,
      type: Array,
      default: () => []
    },
    fixedLogsId: {
      required: false,
      type: Array,
      default: () => []
    }
  },
  data() {
    return {
      selectable: false,
      selectedLog: [],
      menus: {},
      dialog: {},
      severityLevels: [
        { level: 0, display: "Debug" },
        { level: 1, display: "Trace" },
        { level: 2, display: "Info" },
        { level: 3, display: "Warning" },
        { level: 4, display: "Error" },
        { level: 5, display: "Fatal" }
      ],
      search: "",
      severityLevelFilters: [],
      dateTimeRangeFilter: [],
      components: [],
      loading: true,

      headers: [
        {
          text: "Component",
          align: "left",
          sortable: false,
          value: "componentName"
        },
        {
          text: "Timestamp",
          align: "left",
          sortable: false,
          value: "dateTime"
        },
        {
          text: "Severity",
          align: "left",
          sortable: false,
          value: "severityLevel.display"
        },
        { text: "Message", align: "left", sortable: false, value: "message" }
      ]
    };
  },
  methods: {
    ...mapActions([
      "fetchComponentsForSystem",
      "fetchNormalizedLogsForSystem",
      "loadMoreNormalizedLogsForSystem",
      "fetchNormalizedLogsForSystemInDateRange"
    ]),
    loadMore: function() {
      this.loading = true;
      const logId = this.logs[this.dataLength - 1].id;
      this.loadMoreNormalizedLogsForSystem({
        systemId: this.systemId,
        logId: logId
      });
      this.loading = false;
    }
  },
  computed: {
    ...mapGetters([
      "componentsBySystemId",
      "allNormalizedLogs",
      "componentById"
    ]),
    isFixedLogMode: function() {
      return this.fixedLogsId.length > 0;
    },
    logs: function() {
      let temp = this.allNormalizedLogs.map(elem => {
        elem.componentName = this.componentById(elem.componentId).name;
        return elem;
      });

      if (this.fixedLogsId.length > 0) {
        temp = temp.filter(elem => this.fixedLogsId.includes(elem.id));
      }
      if (this.dateTimeRangeFilter && this.dateTimeRangeFilter.length === 2) {
        const startDate = this.dateTimeRangeFilter[0];
        const endDate = this.dateTimeRangeFilter[1];
        temp = temp.filter(
          elem => elem.dateTime >= startDate && elem.dateTime <= endDate
        );

        if (temp.length === 0) {
          this.fetchNormalizedLogsForSystemInDateRange(
            this.systemId,
            this.dateTimeRangeFilter[0],
            this.dateTimeRangeFilter[1]
          );

          temp = this.allNormalizedLogs.map(elem => {
            elem.componentName = this.componentById(elem.componentId).name;
            return elem;
          });
        }
      }

      if (this.components) {
        temp = temp.filter(elem =>
          this.components.includes(parseInt(elem.componentId))
        );
      }
      if (this.severityLevelFilters) {
        temp = temp.filter(elem =>
          this.severityLevelFilters.includes(parseInt(elem.severityLevel.level))
        );
      }
      return temp;
    },
    dataLength: function() {
      if (typeof this.logs === "undefined") {
        return 0;
      }
      return this.logs.length;
    },
    availableComponents: function() {
      return this.componentsBySystemId(this.systemId);
    }
  },
  created() {
    this.loading = true;
    this.fetchComponentsForSystem(this.systemId);

    if (
      !this.dateTimeRangeFilterProp ||
      this.dateTimeRangeFilterProp.length != 2
    ) {
      this.fetchNormalizedLogsForSystem(this.systemId);
    } else {
      console.log("assign");
      Object.assign(this.dateTimeRangeFilter, this.dateTimeRangeFilterProp);
      this.fetchNormalizedLogsForSystemInDateRange({
        systemId: this.systemId,
        start: this.dateTimeRangeFilter[0],
        end: this.dateTimeRangeFilter[1]
      });
    }
    console.log(this.dateTimeRangeFilter);

    if (!this.severityLevelFilters || this.severityLevelFilters.length === 0) {
      Object.assign(
        this.severityLevelFilters,
        this.severityLevels.map(elem => parseInt(elem.level))
      );
    }

    if (!this.componentsProp || this.componentsProp.length === 0) {
      Object.assign(
        this.components,
        this.componentsBySystemId(this.systemId).map(elem => parseInt(elem.id))
      );
    } else {
      const loadedComponents = this.componentsProp.map(elem => parseInt(elem));
      Object.assign(this.components, loadedComponents);
    }
    this.loading = false;
  },
  components: {
    DatePicker
  }
};
</script>

<style>
</style>