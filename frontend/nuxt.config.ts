// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: '2025-07-15',
  devtools: { enabled: true },
  ssr: true,
  vite: {
    server: {
      allowedHosts: true
    }
  },
  modules: ['@nuxtjs/tailwindcss'],
  tailwindcss: {
    config: {
      future: {
        hoverOnlyWhenSupported: true
      },
      content: [
        './app/**/*.{vue,js,ts,jsx,tsx}',
        './components/**/*.{vue,js,ts,jsx,tsx}',
        './layouts/**/*.{vue,js,ts,jsx,tsx}',
        './pages/**/*.{vue,js,ts,jsx,tsx}'
      ]
    }
  },
  runtimeConfig: {
    public: {
      apiBaseUrl: process.env.API_BASE_URL || '/api'
    }
  },
  routeRules: {
    '/api/**': { proxy: 'http://127.0.0.1:5121/api/**' }
  }
})