<template>
  <v-container>
    <v-row>
      <v-col>
        <v-card>
          <v-card-title>Pending Files</v-card-title>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="files"
              :loading="loading"
              class="elevation-1"
            >
              <template v-slot:item.actions="{ item }">
                <v-btn
                  color="primary"
                  variant="text"
                  @click="openFile(item.actions)"
                  :disabled="!item.actions"
                >
                  <v-icon>mdi-file-download</v-icon>
                  Download
                </v-btn>
              </template>
              <template v-slot:item.approvalActions="{ item }">
                <v-btn
                  color="success"
                  variant="text"
                  @click="handleApprove(item)"
                  :loading="item.approving"
                  :disabled="item.approving || item.rejecting"
                >
                  <v-icon>mdi-check</v-icon>
                  Approve
                </v-btn>
                <v-btn
                  color="error"
                  variant="text"
                  @click="handleReject(item)"
                  :loading="item.rejecting"
                  :disabled="item.approving || item.rejecting"
                >
                  <v-icon>mdi-close</v-icon>
                  Reject
                </v-btn>
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import { ref, onMounted, computed } from 'vue';
import axios from '../utils/axios';
import { formatDate } from '../utils/dateUtils';

export default {
  name: 'ApproverDashboardView',
  setup() {
    const files = ref([]);
    const loading = ref(false);

    const headers = [
      { title: 'Subject', key: 'subject' },
      { title: 'Classification', key: 'classificationName' },
      { title: 'Submitter', key: 'submitterName' },
      { title: 'Submitted Date', key: 'formattedDate' },
      { title: 'File', key: 'actions', sortable: false },
      { title: 'Actions', key: 'approvalActions', sortable: false },
    ];

    const openFile = (url) => {
      window.open(url, '_blank');
    };

    const formattedFiles = computed(() => {
      return files.value.map((file) => ({
        ...file,
        formattedDate: formatDate(file.fileDate),
        actions: file.attachmentUrl,
      }));
    });

    const fetchPendingFiles = async () => {
      loading.value = true;
      try {
        const response = await axios.get('/api/Files/pending-approvals');
        files.value = response.data;
      } catch (error) {
        console.error('Error fetching pending files:', error);
      } finally {
        loading.value = false;
      }
    };

    const handleApprove = async (file) => {
      if (!file) return;
      file.approving = true;
      try {
        await axios.post(`/api/Files/${file.id}/approve`);
        await fetchPendingFiles();
      } catch (error) {
        console.error('Error approving file:', error);
      } finally {
        file.approving = false;
      }
    };

    const handleReject = async (file) => {
      if (!file) return;
      file.rejecting = true;
      try {
        await axios.post(`/api/Files/${file.id}/reject`);
        await fetchPendingFiles();
      } catch (error) {
        console.error('Error rejecting file:', error);
      } finally {
        file.rejecting = false;
      }
    };

    onMounted(fetchPendingFiles);

    return {
      files: formattedFiles,
      loading,
      headers,
      handleApprove,
      handleReject,
      openFile,
    };
  },
};
</script>
