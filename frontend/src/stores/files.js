import { defineStore } from 'pinia'
import { filesService } from '../services/files.service'

export const useFilesStore = defineStore('files', {
  state: () => ({
    files: [],
    pendingApprovals: [],
    classifications: [],
    error: null,
    loading: false
  }),

  actions: {
    async fetchFiles() {
      try {
        this.loading = true
        this.error = null
        this.files = await filesService.getFiles()
      } catch (error) {
        this.error = error
        throw error
      } finally {
        this.loading = false
      }
    },

    async fetchPendingApprovals() {
      try {
        this.loading = true
        this.error = null
        this.pendingApprovals = await filesService.getPendingApprovals()
      } catch (error) {
        this.error = error
        throw error
      } finally {
        this.loading = false
      }
    },

    async submitFile(formData) {
      try {
        this.loading = true
        this.error = null
        const newFile = await filesService.submitFile(formData)
        this.files.push(newFile)
      } catch (error) {
        this.error = error
        throw error
      } finally {
        this.loading = false
      }
    },

    async approveFile(fileId) {
      try {
        this.loading = true
        this.error = null
        await filesService.approveFile(fileId)
        this.pendingApprovals = this.pendingApprovals.filter(file => file.id !== fileId)
      } catch (error) {
        this.error = error
        throw error
      } finally {
        this.loading = false
      }
    },

    async rejectFile(fileId) {
      try {
        this.loading = true
        this.error = null
        await filesService.rejectFile(fileId)
        this.pendingApprovals = this.pendingApprovals.filter(file => file.id !== fileId)
      } catch (error) {
        this.error = error
        throw error
      } finally {
        this.loading = false
      }
    },

    async fetchClassifications() {
      try {
        this.loading = true
        this.error = null
        this.classifications = await filesService.getClassifications()
      } catch (error) {
        this.error = error
        throw error
      } finally {
        this.loading = false
      }
    }
  }
}) 