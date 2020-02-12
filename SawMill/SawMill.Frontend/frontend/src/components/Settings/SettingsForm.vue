<template>
  <v-row>
    <v-col cols="12" sm="12" md="12" lg="12">
      <v-text-field
        v-model="settings.frequency"
        label="Analyzer Frequency in seconds"
        type="number"
      ></v-text-field>
    </v-col>
    <v-btn color="success" class="mr-4" @click="submit">submit</v-btn>
  </v-row>
</template>

<script>
export default {
  name: "settings-form",
  data: () => {
    return {
      settings: {
        name: "",
        id: 0,
        frequency: 1
      }
    };
  },
  methods: {
    submit: function() {
      this.$http.put("settings", this.settings);
    }
  },
  created() {
    this.$http
      .get("settings")
      .then(response => (this.settings = response.data));
  }
};
</script>

<style>
</style>