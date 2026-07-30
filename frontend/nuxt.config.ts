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
    // Nếu chạy trong môi trường Docker (có biến môi trường), proxy qua container 'backend', ngược lại proxy qua localhost cổng 5121
    '/api/**': { 
      proxy: process.env.DATABASE_URL || process.env.ConnectionStrings__DefaultConnection || process.env.IS_DOCKER
        ? 'http://backend:8080/api/**' 
        : 'http://127.0.0.1:5121/api/**'
    }
  }
})