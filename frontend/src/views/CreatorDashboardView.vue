<template>
  <v-container>
    <v-row>
      <v-col cols="12">
        <v-card>
          <v-card-title class="d-flex align-center">
            Submitted Files
            <v-spacer></v-spacer>
            <v-btn
              color="primary"
              @click="showUploadDialog = true"
              class="d-none d-sm-flex"
            >
              Add File
            </v-btn>
            <v-btn
              color="primary"
              size="small"
              @click="showUploadDialog = true"
              class="d-flex d-sm-none"
              icon
            >
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </v-card-title>
          <v-card-text>
            <v-data-table
              :headers="headers"
              :items="files"
              :loading="loading"
              class="elevation-1"
            >
              <template v-slot:item.status="{ item }">
                <status-badge :status="item.status" />
              </template>
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
            </v-data-table>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>

    <!-- Upload Dialog -->
    <v-dialog v-model="showUploadDialog" max-width="600px">
      <v-card>
        <v-card-title>Add New File</v-card-title>
        <v-card-text>
          <v-form @submit.prevent="handleSubmit" ref="form">
            <v-text-field
              v-model="subject"
              label="Subject"
              required
              :rules="[(v) => !!v || 'Subject is required']"
            ></v-text-field>

            <v-select
              v-model="classificationId"
              :items="classifications"
              item-title="name"
              item-value="id"
              label="Classification"
              required
              :rules="[(v) => !!v || 'Classification is required']"
            ></v-select>

            <v-select
              v-model="responsibleEmployeeId"
              :items="employees"
              item-title="fullName"
              item-value="id"
              label="Responsible Employee"
              required
              :rules="[(v) => !!v || 'Responsible employee is required']"
            ></v-select>

            <v-file-input
              v-model="attachment"
              label="File"
              required
              :rules="[
                (v) => !!v || 'File is required',
                (v) => {
                  if (!v) return true;
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
                  const fileExtension =
                    '.' + v.name.split('.').pop().toLowerCase();
                  return (
                    allowedTypes.includes(fileExtension) ||
                    'Only PDF, JPG, PNG, DOC, DOCX, XLS, and XLSX files are allowed'
                  );
                },
              ]"
              accept=".pdf,.jpg,.jpeg,.png,.doc,.docx,.xls,.xlsx"
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
import { ref, onMounted, computed } from 'vue';
import axios from '../utils/axios';
import { formatDate } from '../utils/dateUtils';
import StatusBadge from '../components/StatusBadge.vue';

export default {
  name: 'CreatorDashboardView',
  components: {
    StatusBadge,
  },
  setup() {
    const files = ref([]);
    const classifications = ref([]);
    const employees = ref([]);
    const loading = ref(false);
    const submitting = ref(false);
    const showUploadDialog = ref(false);

    // Separate refs for form fields
    const subject = ref('');
    const classificationId = ref(null);
    const responsibleEmployeeId = ref(null);
    const attachment = ref(null);

    const headers = [
      { title: 'Subject', key: 'subject' },
      { title: 'Classification', key: 'classificationName' },
      { title: 'Responsible Employee', key: 'responsibleEmployeeName' },
      { title: 'Status', key: 'status' },
      { title: 'Submitted Date', key: 'formattedDate' },
      { title: 'File', key: 'actions', sortable: false },
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

    const resetForm = () => {
      subject.value = '';
      classificationId.value = null;
      responsibleEmployeeId.value = null;
      attachment.value = null;
    };

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
      formData.append('subject', subject.value);
      formData.append('classificationId', classificationId.value);
      formData.append('responsibleEmployeeId', responsibleEmployeeId.value);
      formData.append('attachment', attachment.value);

      submitting.value = true;
      try {
        await axios.post('/api/Files', formData, {
          headers: {
            'Content-Type': 'multipart/form-data',
          },
        });
        showUploadDialog.value = false;
        await fetchFiles();
        resetForm();
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
      files: formattedFiles,
      classifications,
      employees,
      loading,
      submitting,
      showUploadDialog,
      subject,
      classificationId,
      responsibleEmployeeId,
      attachment,
      headers,
      handleSubmit,
      openFile,
    };
  },
};
</script>
