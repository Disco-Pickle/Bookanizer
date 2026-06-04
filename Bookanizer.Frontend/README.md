# Bookanizer — Frontend

A [Quasar](https://quasar.dev) (Vue 3) single-page application for the Bookanizer
book recommendation system, plus an nginx reverse proxy for talking to the C#
REST server.

## Pages

| Route          | Page             | Purpose                                              |
|----------------|------------------|------------------------------------------------------|
| `/auth/register` | Register       | Account creation                                     |
| `/auth/login`    | Login          | Sign in                                              |
| `/search`        | Search         | Search the catalogue; shows results                  |
| `/add-book`      | Add book       | Record a read book (rating + `ReadLocationEnum`)     |
| `/profile`       | Profile        | Username + the user's whole collection               |
| `/recommend`     | Recommend      | Request a recommendation (optional reading context)  |

Routes under `/` require authentication (guarded in `src/router/index.js`).
The JWT and user payload are persisted in `localStorage` and attached as a
bearer token by the axios interceptor in `src/boot/axios.js`.

## REST endpoints the SPA expects

These are the assumptions baked into the frontend. Adjust either the C#
controllers or the calls in the pages/store so they line up.

| Method & path             | Used by        | Notes                                            |
|---------------------------|----------------|--------------------------------------------------|
| `POST /auth/register`     | Register       | `{ username, email, password }` → `{ token?, user }` |
| `POST /auth/login`        | Login          | `{ username, password }` → `{ token, user }`     |
| `GET  /users/me`          | Profile        | Current user                                     |
| `GET  /books/search?q=`   | Search, Add    | Array of books (or `{ items }`/`{ results }`)    |
| `GET  /collection`        | Profile        | The user's collection items                      |
| `POST /collection`        | Add book       | `{ bookId, rating, readLocation, dateRead, review }` |
| `DELETE /collection/:id`  | Profile        | Remove an item                                   |
| `GET  /recommendations`   | Recommend      | Optional `?readLocation=`; returns a book/score  |

A book object can be flat (`title`, `authorName`, `averageRating`,
`ratingsCount`, `genres[]`) or nested (`author.name`, `authors[]`); the
`BookCard` component handles both shapes.

## How requests reach the REST server

The SPA calls a relative base URL of `/api`. In production, the nginx reverse
proxy (`nginx/default.conf`) serves the built SPA and forwards `/api/*` to the
REST server, stripping the `/api` prefix — so `GET /api/books/search` hits the
REST server as `GET /books/search`. No browser CORS is involved because
everything is one origin.

During `quasar dev`, the dev server proxies `/api` to `DEV_API_TARGET`
(default `http://localhost:5000`) — see `quasar.config.js`.

## Develop

```bash
npm install
npm run dev          # http://localhost:9000, proxies /api to localhost:5000
# override the backend target:
DEV_API_TARGET=http://localhost:5050 npm run dev
```

## Build

```bash
npm run build        # outputs to dist/spa
```

## Run with nginx (Docker)

```bash
docker build -t bookanizer-frontend .
docker run -p 8080:80 bookanizer-frontend
# open http://localhost:8080
```

The image is multi-stage: it builds the SPA with Node, then serves `dist/spa`
with nginx. The reverse proxy's upstream is `rest:8080` — the Docker Compose
service name of the REST server. See `docker-compose.example.yml` for how to
slot this into the wider stack.

## Notes

- The reading-location selector on **Add book** and **Recommend** mirrors the
  C# `ReadLocationEnum` (`Home` / `Transit` / `Public`), the contextual feature
  under study in the thesis. The Recommend page forwards it to the recommender.
- Brand colours and fonts live in `src/css/app.scss` and the `framework.config.brand`
  block of `quasar.config.js`.
