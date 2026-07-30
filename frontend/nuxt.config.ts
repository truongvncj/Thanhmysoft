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
      apiBaseUrl: '/api'
    }
  },
  routeRules: {
    '/api/**': { 
      proxy: process.env.API_PROXY_TARGET || (process.env.NODE_ENV === 'production' 
        ? 'http://backend:8080/api/**' 
        : 'http://127.0.0.1:5121/api/**')
    }
  }
})