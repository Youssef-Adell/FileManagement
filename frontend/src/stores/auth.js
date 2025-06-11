import { defineStore } from 'pinia'
import { authService } from '../services/auth.service'
import router from '../router'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    userRole: localStorage.getItem('userRole') || null,
    error: null
  }),

  actions: {
    async login(username, password) {
      try {
        this.error = null
        const { role } = await authService.login(username, password)
        this.userRole = role
        
        // Redirect based on role
        if (role === 'Creator') {
          router.push('/creator')
        } else if (role === 'Approver') {
          router.push('/approver')
        }
      } catch (error) {
        this.error = error
        throw error
      }
    },

    logout() {
      authService.logout()
      this.userRole = null
      router.push('/login')
    }
  }
}) 