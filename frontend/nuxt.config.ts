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
      // Ở local (chế độ dev) sẽ gọi cổng 5121, ở server (production) sẽ gọi cổng 8080 qua IP server
      apiBaseUrl: process.env.NODE_ENV === 'development' 
        ? 'http://localhost:5121' 
        : 'http://103.249.158.42:8080'
    }
  },
  routeRules: {}
})