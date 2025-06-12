<template>
  <v-container class="fill-height">
    <v-row justify="center" align="center">
      <v-col cols="12" sm="8" md="6" lg="4">
        <v-card class="elevation-12">
          <v-toolbar color="primary" dark flat>
            <v-toolbar-title>Login</v-toolbar-title>
          </v-toolbar>
          <v-card-text>
            <v-form @submit.prevent="handleLogin" ref="form">
              <v-text-field
                v-model="username"
                label="Username"
                prepend-icon="mdi-account"
                type="text"
                required
                :rules="[(v) => !!v || 'Username is required']"
              ></v-text-field>
              <v-text-field
                v-model="password"
                label="Password"
                prepend-icon="mdi-lock"
                type="password"
                required
                :rules="[(v) => !!v || 'Password is required']"
              ></v-text-field>
            </v-form>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="primary" @click="handleLogin" :loading="loading">
              Login
            </v-btn>
          </v-card-actions>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/auth';

export default {
  name: 'LoginView',
  setup() {
    const router = useRouter();
    const authStore = useAuthStore();
    const form = ref(null);
    const username = ref('');
    const password = ref('');
    const loading = ref(false);

    const handleLogin = async () => {
      const { valid } = await form.value.validate();
      if (!valid) return;

      loading.value = true;
      try {
        const user = await authStore.login(username.value, password.value);
        router.push(
          user.role === 'Creator' ? '/creator-dashboard' : '/approver-dashboard'
        );
      } catch (error) {
        console.error('Login failed:', error);
      } finally {
        loading.value = false;
      }
    };

    return {
      form,
      username,
      password,
      loading,
      handleLogin,
    };
  },
};
</script>
