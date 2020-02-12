<template>
  <div>
    <v-container fluid>
      <v-row>
        <v-col v-for="system in allSystems" :key="system.id" cols="12" sm="6" md="6" lg="4">
          <system :systemId="system.id"></system>
        </v-col>

        <v-row justify="center">
          <v-dialog v-model="createDialog" persistent max-width="auto">
            <system-form :isEdit="false" v-on:done="createDialog = !createDialog"></system-form>
          </v-dialog>
        </v-row>
      </v-row>
      <v-btn fab color="cyan accent-2" bottom left @click="createDialog = !createDialog">
        <v-icon>mdi-plus</v-icon>
      </v-btn>
    </v-container>
  </div>
</template>

<script>
import systemForm from "./SystemForm";
import system from "./System";
import { mapGetters, mapActions } from "vuex";
export default {
  name: "system-index",
  data: () => {
    return {
      createDialog: false
    };
  },
  methods: {
    ...mapActions(["fetchSystems"])
  },
  computed: {
    ...mapGetters(["allSystems"])
  },
  components: {
    system,
    systemForm
  },
  created() {
    this.fetchSystems();
  }
};
</script>
