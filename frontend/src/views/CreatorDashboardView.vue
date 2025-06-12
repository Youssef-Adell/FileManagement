<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title class="d-flex align-center">
            Submitted Files
            <v-spacer></v-spacer>
            <v-btn color="primary" @click="showUploadDialog = true">
              Upload New File
            </v-btn>
          </v-card-title>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="files"
              :loading="loading"
            ></v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Upload Dialog -->
    <v-dialog v-model="showUploadDialog" max-width="600px">
      <v-card>
        <v-card-title>Upload New File</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="handleSubmit" ref="form">
            <v-text-field
              v-model="form.subject"
              label="Subject"
              required
              :rules="[(v) => !!v || 'Subject is required']"
            ></v-text-field>

            <v-select
              v-model="form.classificationId"
              :items="classifications"
              item-title="name"
              item-value="id"
              label="Classification"
              required
              :rules="[(v) => !!v || 'Classification is required']"
            ></v-select>

            <v-select
              v-model="form.responsibleEmployeeId"
              :items="employees"
              item-title="fullName"
              item-value="id"
              label="Responsible Employee"
              required
              :rules="[(v) => !!v || 'Responsible employee is required']"
            ></v-select>

            <v-file-input
              v-model="form.attachment"
              label="File"
              required
              :rules="[(v) => !!v || 'File is required']"
            ></v-file-input>
          </v-form>
        </v-card-text>
        <v-card-actions>
          <v-spacer></v-spacer>
          <v-btn color="grey" @click="showUploadDialog = false">Cancel</v-btn>
          <v-btn color="primary" @click="handleSubmit" :loading="submitting">
            Submit
          </v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </v-container>
</template>

<script>
import { ref, onMounted } from 'vue';
import axios from '../utils/axios';

export default {
  name: 'CreatorDashboardView',
  setup() {
    const files = ref([]);
    const classifications = ref([]);
    const employees = ref([]);
    const loading = ref(false);
    const submitting = ref(false);
    const showUploadDialog = ref(false);
    const form = ref({
      subject: '',
      classificationId: null,
      responsibleEmployeeId: null,
      attachment: null,
    });

    const headers = [
      { title: 'Subject', key: 'subject' },
      { title: 'Classification', key: 'classification' },
      { title: 'Responsible Employee', key: 'responsibleEmployee' },
      { title: 'Status', key: 'status' },
      { title: 'Submitted Date', key: 'submittedDate' },
    ];

    const fetchFiles = async () => {
      loading.value = true;
      try {
        const response = await axios.get('/api/Files');
        files.value = response.data;
      } catch (error) {
        console.error('Error fetching files:', error);
      } finally {
        loading.value = false;
      }
    };

    const fetchClassifications = async () => {
      try {
        const response = await axios.get('/api/Classifications');
        classifications.value = response.data;
      } catch (error) {
        console.error('Error fetching classifications:', error);
      }
    };

    const fetchEmployees = async () => {
      try {
        const response = await axios.get('/api/Employees');
        employees.value = response.data;
      } catch (error) {
        console.error('Error fetching employees:', error);
      }
    };

    const handleSubmit = async () => {
      const formData = new FormData();
      formData.append('subject', form.value.subject);
      formData.append('classificationId', form.value.classificationId);
      formData.append(
        'responsibleEmployeeId',
        form.value.responsibleEmployeeId
      );
      formData.append('attachment', form.value.attachment);

      submitting.value = true;
      try {
        await axios.post('/api/Files', formData, {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        });
        showUploadDialog.value = false;
        await fetchFiles();
        form.value = {
          subject: '',
          classificationId: null,
          responsibleEmployeeId: null,
          attachment: null,
        };
      } catch (error) {
        console.error('Error submitting file:', error);
      } finally {
        submitting.value = false;
      }
    };

    onMounted(() => {
      fetchFiles();
      fetchClassifications();
      fetchEmployees();
    });

    return {
      files,
      classifications,
      employees,
      loading,
      submitting,
      showUploadDialog,
      form,
      headers,
      handleSubmit,
    };
  },
};
</script>
