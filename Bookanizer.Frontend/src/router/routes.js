const routes = [
  // Auth pages use a bare shell (no app chrome).
  {
    path: '/auth',
    component: () => import('layouts/AuthLayout.vue'),
    children: [
      { path: 'login', name: 'login', component: () => import('pages/LoginPage.vue') },
      { path: 'register', name: 'register', component: () => import('pages/RegisterPage.vue') }
    ]
  },

  // Authenticated app.
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      { path: '', redirect: { name: 'search' } },
      { path: 'search', name: 'search', component: () => import('pages/SearchPage.vue') },
      { path: 'add-book', name: 'add-book', component: () => import('pages/AddBookPage.vue') },
      { path: 'profile', name: 'profile', component: () => import('pages/ProfilePage.vue') },
      { path: 'recommend', name: 'recommend', component: () => import('pages/RecommendPage.vue') }
    ]
  },

  { path: '/:catchAll(.*)*', component: () => import('pages/ErrorNotFound.vue') }
]

export default routes
