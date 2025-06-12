<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title>Pending Approvals</v-card-title>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="pendingFiles"
              :loading="loading"
            >
              <template v-slot:item.actions="{ item }">
                <v-btn
                  v-if="item.raw"
                  color="success"
                  size="small"
                  class="me-2"
                  @click="handleApprove(item.raw)"
                  :loading="item.raw?.approving"
                >
                  Approve
                </v-btn>
                <v-btn
                  v-if="item.raw"
                  color="error"
                  size="small"
                  @click="handleReject(item.raw)"
                  :loading="item.raw?.rejecting"
                >
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
import { ref, onMounted, onBeforeUnmount } from 'vue';
import axios from '../utils/axios';

export default {
  name: 'ApproverDashboardView',
  setup() {
    const pendingFiles = ref([]);
    const loading = ref(false);
    let mounted = true;

    const headers = [
      { title: 'Subject', key: 'subject' },
      { title: 'Classification', key: 'classification' },
      { title: 'Responsible Employee', key: 'responsibleEmployee' },
      { title: 'Submitted Date', key: 'submittedDate' },
      { title: 'Actions', key: 'actions', sortable: false },
    ];

    const fetchPendingFiles = async () => {
      if (!mounted) return;

      loading.value = true;
      try {
        const response = await axios.get('/api/Files/pending-approvals');
        if (mounted) {
          pendingFiles.value = response.data.map((file) => ({
            ...file,
            approving: false,
            rejecting: false,
          }));
        }
      } catch (error) {
        console.error('Error fetching pending files:', error);
      } finally {
        if (mounted) {
          loading.value = false;
        }
      }
    };

    const handleApprove = async (file) => {
      if (!file) return;

      file.approving = true;
      try {
        await axios.post(`/api/Files/${file.id}/approve`);
        if (mounted) {
          await fetchPendingFiles();
        }
      } catch (error) {
        console.error('Error approving file:', error);
      } finally {
        if (mounted && file) {
          file.approving = false;
        }
      }
    };

    const handleReject = async (file) => {
      if (!file) return;

      file.rejecting = true;
      try {
        await axios.post(`/api/Files/${file.id}/reject`);
        if (mounted) {
          await fetchPendingFiles();
        }
      } catch (error) {
        console.error('Error rejecting file:', error);
      } finally {
        if (mounted && file) {
          file.rejecting = false;
        }
      }
    };

    onMounted(() => {
      mounted = true;
      fetchPendingFiles();
    });

    onBeforeUnmount(() => {
      mounted = false;
    });

    return {
      pendingFiles,
      loading,
      headers,
      handleApprove,
      handleReject,
    };
  },
};
</script>
