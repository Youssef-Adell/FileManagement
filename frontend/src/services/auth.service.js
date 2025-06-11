import api from './api'

export const authService = {
  async login(username, password) {
    try {
      const response = await api.post('/auth/login', { username, password })
      const { token, role } = response.data
      
      localStorage.setItem('token', token)
      localStorage.setItem('userRole', role)
      
      return { role }
    } catch (error) {
      throw error.response?.data?.message || 'Login failed'
    }
  },

  logout() {
    localStorage.removeItem('token')
    localStorage.removeItem('userRole')
  },

  isAuthenticated() {
    return !!localStorage.getItem('token')
  },

  getUserRole() {
    return localStorage.getItem('userRole')
  }
} 