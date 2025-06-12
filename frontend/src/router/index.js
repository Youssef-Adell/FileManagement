import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from '../stores/auth'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    {
      path: '/',
      redirect: '/login'
    },
    {
      path: '/login',
      name: 'Login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/creator-dashboard',
      name: 'CreatorDashboard',
      component: () => import('../views/CreatorDashboardView.vue'),
      meta: { requiresAuth: true, role: 'Creator' }
    },
    {
      path: '/approver-dashboard',
      name: 'ApproverDashboard',
      component: () => import('../views/ApproverDashboardView.vue'),
      meta: { requiresAuth: true, role: 'Approver' }
    }
  ]
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  const authStore = useAuthStore()

  // If trying to access login page
  if (to.path === '/login') {
    if (token) {
      // If logged in, redirect to appropriate dashboard based on role
      return next(authStore.user?.role === 'Creator' ? '/creator-dashboard' : '/approver-dashboard')
    }
    return next()
  }

  // If route requires auth and no token, redirect to login
  if (to.meta.requiresAuth && !token) {
    return next('/login')
  }

  // Check role-based access for protected routes
  if (to.meta.requiresAuth && to.meta.role) {
    if (authStore.user?.role !== to.meta.role) {
      // If user's role doesn't match required role, redirect to appropriate dashboard
      return next(authStore.user?.role === 'Creator' ? '/creator-dashboard' : '/approver-dashboard')
    }
  }

  next()
})

export default router 