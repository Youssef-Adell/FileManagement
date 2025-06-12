import { defineStore } from 'pinia'
import axios from '../utils/axios'

export const useAuthStore = defineStore('auth', {
  state: () => ({
    user: JSON.parse(localStorage.getItem('user')) || null,
    token: localStorage.getItem('token') || null
  }),

  actions: {
    async login(username, password) {
      try {
        const response = await axios.post('/api/Auth/login', { username, password })
        const { token, user } = response.data
        
        this.token = token
        this.user = user
        localStorage.setItem('token', token)
        localStorage.setItem('user', JSON.stringify(user))
        
        return user
      } catch (error) {
        throw error
      }
    },

    logout() {
      // Clear state first
      this.user = null
      this.token = null
      
      // Then clear storage
      localStorage.removeItem('token')
      localStorage.removeItem('user')
      
      // Clear any axios default headers
      delete axios.defaults.headers.common['Authorization']
    },

    async restoreSession() {
      const token = localStorage.getItem('token')
      const user = JSON.parse(localStorage.getItem('user'))
      
      if (!token) return

      try {
        // You might want to add an endpoint to validate the token and get user info
        // For now, we'll just restore from localStorage
        this.token = token
        this.user = user
      } catch (error) {
        this.logout()
      }
    }
  }
}) 