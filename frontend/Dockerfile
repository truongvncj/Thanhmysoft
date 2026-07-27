FROM node:20-alpine

WORKDIR /app

# Copy package info and install dependencies
COPY package*.json ./
RUN npm install

# Copy source code and build
COPY . .
RUN npm run build

EXPOSE 3000
ENV PORT=3000
ENV HOST=0.0.0.0

# Start Nuxt server
CMD ["node", ".output/server/index.mjs"]
