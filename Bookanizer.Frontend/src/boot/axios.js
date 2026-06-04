import { defineBoot } from '#q-app/wrappers'
import axios from 'axios'

// Single axios instance pointed at the REST server (via nginx /api in prod,
// or the dev proxy in development). Token is attached from the auth store.
const api = axios.create({
  baseURL: process.env.API_BASE_URL || '/api'
})

export default defineBoot(({ app, store, router }) => {
  // Attach bearer token if present.
  api.interceptors.request.use((config) => {
    const token = localStorage.getItem('bk_token')
    if (token) {
      config.headers.Authorization = `Bearer ${token}`
    }
    return config
  })

  // On 401, clear session and bounce to login.
  api.interceptors.response.use(
    (response) => response,
    (error) => {
      if (error.response && error.response.status === 401) {
        localStorage.removeItem('bk_token')
        localStorage.removeItem('bk_user')
        if (router.currentRoute.value.name !== 'login') {
          router.push({ name: 'login' })
        }
      }
      return Promise.reject(error)
    }
  )

  app.config.globalProperties.$api = api
})

export { api }
