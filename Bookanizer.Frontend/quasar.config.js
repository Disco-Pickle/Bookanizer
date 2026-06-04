import { defineConfig } from '#q-app/wrappers'

export default defineConfig((/* ctx */) => {
  return {
    boot: ['axios'],

    css: ['app.scss'],

    extras: ['roboto-font', 'material-icons'],

    build: {
      target: {
        browser: ['es2022', 'firefox115', 'chrome115', 'safari14'],
        node: 'node20'
      },
      vueRouterMode: 'history',
      env: {
        // Base URL the SPA uses to reach the REST API. In production the nginx
        // reverse proxy serves the SPA and forwards /api to the REST server,
        // so a relative path keeps it origin-agnostic.
        API_BASE_URL: process.env.API_BASE_URL || '/api'
      }
    },

    devServer: {
      open: false,
      port: 9000,
      proxy: {
        // During `quasar dev`, forward API calls to the REST server directly
        // so the dev experience mirrors the nginx production routing.
        '/api': {
          target: process.env.DEV_API_TARGET || 'http://localhost:5000',
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/api/, '')
        }
      }
    },

    framework: {
      config: {
        brand: {
          primary: '#7a1f2b',
          secondary: '#3a3530',
          accent: '#c08a3e',
          dark: '#1d1a17',
          'dark-page': '#161310',
          positive: '#4f7a52',
          negative: '#a23b2d',
          info: '#5a7d8c',
          warning: '#c08a3e'
        }
      },
      plugins: ['Notify', 'Loading', 'Dialog']
    },

    animations: [],

    ssr: { pwa: false },

    framework_components: []
  }
})
