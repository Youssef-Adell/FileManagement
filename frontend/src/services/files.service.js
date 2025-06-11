import api from './api'

export const filesService = {
  async getFiles() {
    try {
      const response = await api.get('/files')
      return response.data
    } catch (error) {
      throw error.response?.data?.message || 'Failed to fetch files'
    }
  },

  async getPendingApprovals() {
    try {
      const response = await api.get('/files/pending-approvals')
      return response.data
    } catch (error) {
      throw error.response?.data?.message || 'Failed to fetch pending approvals'
    }
  },

  async submitFile(formData) {
    try {
      const response = await api.post('/files', formData, {
        headers: {
          'Content-Type': 'multipart/form-data'
        }
      })
      return response.data
    } catch (error) {
      throw error.response?.data?.message || 'Failed to submit file'
    }
  },

  async approveFile(fileId) {
    try {
      await api.post(`/files/${fileId}/approve`)
    } catch (error) {
      throw error.response?.data?.message || 'Failed to approve file'
    }
  },

  async rejectFile(fileId) {
    try {
      await api.post(`/files/${fileId}/reject`)
    } catch (error) {
      throw error.response?.data?.message || 'Failed to reject file'
    }
  },

  async getClassifications() {
    try {
      const response = await api.get('/classifications')
      return response.data
    } catch (error) {
      throw error.response?.data?.message || 'Failed to fetch classifications'
    }
  }
} 