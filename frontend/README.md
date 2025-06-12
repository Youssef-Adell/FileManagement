# File Management System Frontend

A Vue 3 frontend application for managing file submissions and approvals.

## Features

- User authentication with JWT
- Role-based access control (Creator and Approver roles)
- File submission with metadata
- File approval workflow
- Modern UI with Vuetify components

## Setup

1. Install dependencies:
```bash
npm install
```

2. Create a `.env` file in the root directory with the following content:
```
VITE_API_URL=http://localhost:5000
```

3. Start the development server:
```bash
npm run dev
```

## Project Structure

- `src/views/` - Page components
  - `LoginView.vue` - Login page
  - `CreatorDashboardView.vue` - File submission dashboard
  - `ApproverDashboardView.vue` - File approval dashboard
- `src/stores/` - Pinia stores
  - `auth.js` - Authentication state management
- `src/utils/` - Utility functions
  - `axios.js` - Axios instance with interceptors
- `src/router/` - Vue Router configuration
  - `index.js` - Route definitions and guards

## API Integration

The application integrates with the following API endpoints:

- `POST /api/Auth/login` - User authentication
- `GET /api/Files` - List all files
- `POST /api/Files` - Submit new file
- `GET /api/Files/pending-approvals` - List pending approvals
- `POST /api/Files/{fileId}/approve` - Approve file
- `POST /api/Files/{fileId}/reject` - Reject file
- `GET /api/Classifications` - List classifications
- `GET /api/Employees` - List employees
