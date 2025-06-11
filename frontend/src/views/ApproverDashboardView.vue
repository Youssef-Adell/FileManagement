<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title>Pending Approvals</v-card-title>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="pendingApprovals"
              :loading="loading"
            >
              <template v-slot:item.actions="{ item }">
                <v-btn
                  color="success"
                  size="small"
                  class="me-2"
                  @click="handleApprove(item.id)"
                  :loading="loading"
                >
                  Approve
                </v-btn>
                <v-btn
                  color="error"
                  size="small"
                  @click="handleReject(item.id)"
                  :loading="loading"
                >
                  Reject
                </v-btn>
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <v-snackbar v-model="showError" color="error" timeout="3000">
      {{ error }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useFilesStore } from '../stores/files';

const filesStore = useFilesStore();
const error = ref('');
const showError = ref(false);

const headers = [
  { title: 'File Number', key: 'fileNumber' },
  { title: 'Subject', key: 'subject' },
  { title: 'Submitter', key: 'submitterName' },
  { title: 'Date', key: 'fileDate' },
  { title: 'Classification', key: 'classificationName' },
  { title: 'Actions', key: 'actions', sortable: false },
];

const handleApprove = async (fileId) => {
  try {
    await filesStore.approveFile(fileId);
  } catch (err) {
    error.value = err;
    showError.value = true;
  }
};

const handleReject = async (fileId) => {
  try {
    await filesStore.rejectFile(fileId);
  } catch (err) {
    error.value = err;
    showError.value = true;
  }
};

onMounted(async () => {
  try {
    await filesStore.fetchPendingApprovals();
  } catch (err) {
    error.value = err;
    showError.value = true;
  }
});
</script>
