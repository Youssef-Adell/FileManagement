import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../views/LoginView.vue'
import CreatorDashboardView from '../views/CreatorDashboardView.vue'
import ApproverDashboardView from '../views/ApproverDashboardView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      redirect: to => {
        const token = localStorage.getItem('token')
        const userRole = localStorage.getItem('userRole')
        
        if (!token) return '/login'
        return userRole === 'Creator' ? '/creator' : '/approver'
      }
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
      meta: { requiresGuest: true }
    },
    {
      path: '/creator',
      name: 'creator',
      component: CreatorDashboardView,
      meta: { requiresAuth: true, role: 'Creator' }
    },
    {
      path: '/approver',
      name: 'approver',
      component: ApproverDashboardView,
      meta: { requiresAuth: true, role: 'Approver' }
    }
  ]
})

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem('token')
  const userRole = localStorage.getItem('userRole')

  // If route requires guest (login page) and user is authenticated
  if (to.meta.requiresGuest && token) {
    // Redirect to appropriate dashboard based on role
    return next(userRole === 'Creator' ? '/creator' : '/approver')
  }

  // If route requires auth and user is not authenticated
  if (to.meta.requiresAuth && !token) {
    return next('/login')
  }

  // If route requires specific role and user doesn't have it
  if (to.meta.role && to.meta.role !== userRole) {
    // Redirect to appropriate dashboard based on role
    return next(userRole === 'Creator' ? '/creator' : '/approver')
  }

  next()
})

export default router 