<script setup>
import { useAuthStore } from './stores/auth';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();

const handleLogout = () => {
  authStore.logout();
  router.push('/login');
};
</script>

<template>
  <v-app>
    <v-app-bar
      v-if="authStore?.user"
      color="primary"
      elevation="2"
      class="px-4"
    >
      <v-container class="d-flex align-center">
        <v-app-bar-title class="text-h5 font-weight-bold">
          <v-icon icon="mdi-folder" class="mr-2"></v-icon>
          File Management
        </v-app-bar-title>
        <v-spacer></v-spacer>
        <v-btn
          @click="handleLogout"
          color="white"
          variant="text"
          class="text-none"
          prepend-icon="mdi-logout"
        >
          Logout
        </v-btn>
      </v-container>
    </v-app-bar>

    <v-main>
      <v-container fluid class="fill-height pa-4">
        <v-fade-transition mode="out-in">
          <router-view v-slot="{ Component }">
            <component :is="Component" />
          </router-view>
        </v-fade-transition>
      </v-container>
    </v-main>

    <v-footer app color="primary" class="text-center d-flex justify-center">
      <span class="text-white">
        © {{ new Date().getFullYear() }} Youssef Adel
      </span>
    </v-footer>
  </v-app>
</template>

<style scoped>
.v-app-bar {
  backdrop-filter: blur(10px);
  background-color: rgba(var(--v-theme-primary), 0.95) !important;
}

.v-main {
  background-color: var(--background);
}

.v-footer {
  font-size: 0.875rem;
  opacity: 0.9;
}

/* Smooth transitions */
.v-fade-transition-enter-active,
.v-fade-transition-leave-active {
  transition: opacity 0.3s ease;
}

.v-fade-transition-enter-from,
.v-fade-transition-leave-to {
  opacity: 0;
}

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
