<template>
  <v-app>
    <v-navigation-drawer temporary app v-model="drawer">
      <v-list nav dense>
        <v-list-item to="/system" link>
          <v-list-item-title>Systems Overview</v-list-item-title>
        </v-list-item>
        <v-list-item to="/settings" link>
          <v-list-item-title>Analyzer Settings</v-list-item-title>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-app-bar clipped-left color="deep-purple accent-4" dark app>
      <v-app-bar-nav-icon @click="drawer = !drawer"></v-app-bar-nav-icon>

      <v-toolbar-title>Sawmill</v-toolbar-title>
      <v-spacer></v-spacer>

      <v-spacer></v-spacer>
    </v-app-bar>
    <!-- Sizes your content based upon application components -->
    <v-content>
      <!-- Provides the application the proper gutter -->
      <v-container fluid>
        <!-- If using vue-router -->
        <router-view></router-view>
      </v-container>
    </v-content>

    <v-footer height="auto" color="indigo" dark>
      <v-layout justify-center row wrap>
        <v-flex
          color="indigo"
          dark
          py-0
          text-xs-center
          white--text
          xs12
        >Information system log analyser</v-flex>
      </v-layout>
    </v-footer>
  </v-app>
</template>

<script>
export default {
  name: "App",
  data: () => {
    return {
      drawer: false
    };
  }
};
</script>
