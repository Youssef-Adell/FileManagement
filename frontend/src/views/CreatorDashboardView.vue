<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title class="d-flex align-center">
            Submitted Files
            <v-spacer />
            <v-btn color="primary" @click="showSubmitDialog = true">
              Submit New File
            </v-btn>
          </v-card-title>
          <v-card-text>
            <v-data-table :headers="headers" :items="files" :loading="loading">
              <template v-slot:item.status="{ item }">
                <v-chip :color="getStatusColor(item.status)" small>
                  {{ item.status }}
                </v-chip>
              </template>
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Submit File Dialog -->
    <v-dialog v-model="showSubmitDialog" max-width="600px">
      <v-card>
        <v-card-title>Submit New File</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="handleSubmit" ref="form">
            <v-text-field v-model="form.subject" label="Subject" required />
            <v-select
              v-model="form.classificationId"
              :items="classifications"
              item-title="name"
              item-value="id"
              label="Classification"
              required
            />
            <v-file-input
              v-model="form.file"
              label="File"
              accept=".pdf,.jpg,.jpeg,.png,.doc,.docx,.xls,.xlsx"
              required
              :rules="fileRules"
            />
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn color="grey darken-1" text @click="showSubmitDialog = false">
            Cancel
          </v-btn>
          <v-btn
            color="primary"
            @click="handleSubmit"
            :loading="submitting"
            :disabled="submitting"
          >
            Submit
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <v-snackbar v-model="showError" color="error" timeout="3000">
      {{ error }}
    </v-snackbar>
  </v-container>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import { useFilesStore } from '../stores/files';

const filesStore = useFilesStore();
const showSubmitDialog = ref(false);
const submitting = ref(false);
const error = ref('');
const showError = ref(false);

const form = ref({
  subject: '',
  classificationId: null,
  file: null,
});

const headers = [
  { title: 'File Number', key: 'fileNumber' },
  { title: 'Subject', key: 'subject' },
  { title: 'Status', key: 'status' },
  { title: 'Date', key: 'fileDate' },
  { title: 'Classification', key: 'classificationName' },
];

const fileRules = [
  (v) => !!v || 'File is required',
  (v) => {
    const allowedTypes = [
      '.pdf',
      '.jpg',
      '.jpeg',
      '.png',
      '.doc',
      '.docx',
      '.xls',
      '.xlsx',
    ];
    const fileExtension = '.' + v?.name?.split('.').pop().toLowerCase();
    return allowedTypes.includes(fileExtension) || 'Invalid file type';
  },
];

const getStatusColor = (status) => {
  switch (status.toLowerCase()) {
    case 'approved':
      return 'success';
    case 'rejected':
      return 'error';
    default:
      return 'warning';
  }
};

const handleSubmit = async () => {
  try {
    submitting.value = true;
    const formData = new FormData();
    formData.append('subject', form.value.subject);
    formData.append('classificationId', form.value.classificationId);
    formData.append('file', form.value.file);

    await filesStore.submitFile(formData);
    showSubmitDialog.value = false;
    form.value = {
      subject: '',
      classificationId: null,
      file: null,
    };
  } catch (err) {
    error.value = err;
    showError.value = true;
  } finally {
    submitting.value = false;
  }
};

onMounted(async () => {
  try {
    await Promise.all([
      filesStore.fetchFiles(),
      filesStore.fetchClassifications(),
    ]);
  } catch (err) {
    error.value = err;
    showError.value = true;
  }
});
</script>
