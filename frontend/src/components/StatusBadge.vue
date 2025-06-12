<template>
  <v-chip
    :color="statusColor"
    :text-color="textColor"
    size="small"
    class="font-weight-medium"
  >
    {{ status }}
  </v-chip>
</template>

<script>
export default {
  name: 'StatusBadge',
  props: {
    status: {
      type: String,
      required: true,
      validator: (value) => ['Pending', 'Approved', 'Rejected'].includes(value),
    },
  },
  computed: {
    statusColor() {
      const colors = {
        Pending: 'warning',
        Approved: 'success',
        Rejected: 'error',
      };
      return colors[this.status] || 'grey';
    },
    textColor() {
      return 'white';
    },
  },
};
</script>

<style scoped>
.v-chip {
  text-transform: capitalize;
}
</style>
