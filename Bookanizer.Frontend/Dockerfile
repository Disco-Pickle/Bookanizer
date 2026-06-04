# Multi-stage build: compile the Quasar SPA, then serve it with nginx
# acting as the reverse proxy to the REST server.

# ---- Stage 1: build the SPA -------------------------------------------------
FROM node:22-alpine AS build
WORKDIR /app

# Install deps (cache-friendly).
COPY package.json ./
RUN npm install

# Build.
COPY . .
RUN npx quasar build

# ---- Stage 2: serve with nginx ---------------------------------------------
FROM nginx:1.27-alpine
# SPA static output.
COPY --from=build /app/dist/spa /usr/share/nginx/html
# Reverse-proxy config (replaces the default server block).
COPY nginx/default.conf /etc/nginx/conf.d/default.conf

EXPOSE 80
# Simple healthcheck using wget (available in nginx:alpine).
HEALTHCHECK --interval=30s --timeout=5s --start-period=10s --retries=3 \
  CMD wget -q -O /dev/null http://localhost/ || exit 1

CMD ["nginx", "-g", "daemon off;"]
