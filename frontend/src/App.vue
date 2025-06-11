<script setup>
import { computed } from 'vue';
import { useAuthStore } from './stores/auth';
</script>

<template>
  <v-app>
    <v-app-bar v-if="isAuthenticated" color="primary" dark>
      <v-app-bar-title>File Management System</v-app-bar-title>
      <v-spacer />
      <v-btn @click="handleLogout" text>Logout</v-btn>
    </v-app-bar>

    <v-main>
      <router-view />
    </v-main>
  </v-app>
</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
  transition: filter 300ms;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>
