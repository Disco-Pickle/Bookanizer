import { defineStore } from 'pinia'
import { api } from 'boot/axios'

// Auth store. Persists the JWT + user payload in localStorage so a refresh
// keeps the session. Endpoint paths assume the C# REST server exposes
// /auth/register, /auth/login and /users/me — adjust to match your controllers.
export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: localStorage.getItem('bk_token') || null,
    user: JSON.parse(localStorage.getItem('bk_user') || 'null')
  }),

  getters: {
    isAuthenticated: (state) => !!state.token,
    username: (state) => (state.user ? state.user.username : '')
  },

  actions: {
    _persist () {
      if (this.token) localStorage.setItem('bk_token', this.token)
      else localStorage.removeItem('bk_token')
      if (this.user) localStorage.setItem('bk_user', JSON.stringify(this.user))
      else localStorage.removeItem('bk_user')
    },

    async register ({ username, email, password }) {
      const { data } = await api.post('/auth/register', { username, email, password })
      // Server may return a token on register, or just the user. Handle both.
      if (data.token) this.token = data.token
      this.user = data.user || { username, email }
      this._persist()
      return data
    },

    async login ({ username, password }) {
      const { data } = await api.post('/auth/login', { username, password })
      this.token = data.token
      this.user = data.user || { username }
      this._persist()
      return data
    },

    async fetchProfile () {
      const { data } = await api.get('/users/me')
      this.user = data
      this._persist()
      return data
    },

    logout () {
      this.token = null
      this.user = null
      this._persist()
    }
  }
})
