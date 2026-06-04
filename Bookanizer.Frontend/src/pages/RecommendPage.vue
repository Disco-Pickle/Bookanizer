<template>
  <q-page>
    <div class="bk-page" style="max-width: 760px">
      <h1 class="bk-title">Your next read</h1>
      <p class="bk-subtitle">
        Ask the recommender for a fresh suggestion. Optionally tell it where you'll be reading —
        the model factors reading location into its prediction.
      </p>

      <q-card flat class="bk-card q-pa-lg q-mb-lg">
        <div class="row q-col-gutter-md items-end">
          <div class="col-12 col-sm">
            <div class="text-caption q-mb-xs" style="color: var(--ink-soft)">
              Reading context (optional)
            </div>
            <q-select
              v-model="readLocation"
              outlined
              dense
              class="bk-field"
              label="Where will you read?"
              :options="readLocations"
              emit-value
              map-options
              clearable
            />
          </div>
          <div class="col-12 col-sm-auto">
            <q-btn
              no-caps
              unelevated
              color="primary"
              icon="auto_awesome"
              label="Recommend a book"
              :loading="loading"
              @click="getRecommendation"
            />
          </div>
        </div>
      </q-card>

      <div v-if="loading" class="row justify-center q-py-xl">
        <q-spinner-dots size="44px" color="primary" />
      </div>

      <transition appear enter-active-class="animated fadeInUp">
        <div v-if="!loading && recommendation" class="bk-rec">
          <div class="text-caption q-mb-sm" style="color: var(--oxblood); letter-spacing: 0.08em; text-transform: uppercase;">
            Recommended for you
          </div>
          <div class="font-display text-h4" style="font-weight: 600; line-height: 1.1;">
            {{ recommendation.title }}
          </div>
          <div class="text-subtitle1 q-mt-xs" style="color: var(--ink-soft)">
            {{ authorLabel }}
          </div>

          <div v-if="recommendation.averageRating" class="row items-center q-gutter-xs q-mt-sm">
            <q-icon name="star" size="16px" style="color: var(--gold)" />
            <span>{{ Number(recommendation.averageRating).toFixed(2) }}</span>
            <span v-if="recommendation.ratingsCount" style="color: var(--ink-soft)">
              · {{ recommendation.ratingsCount.toLocaleString() }} ratings
            </span>
          </div>

          <div v-if="recommendation.score != null" class="q-mt-sm text-caption" style="color: var(--ink-soft)">
            Predicted preference score: {{ Number(recommendation.score).toFixed(3) }}
            <span v-if="usedLocation"> · context: {{ usedLocation }}</span>
          </div>

          <p v-if="recommendation.description" class="q-mt-md" style="color: var(--ink-soft)">
            {{ recommendation.description }}
          </p>

          <div class="row q-gutter-sm q-mt-md">
            <q-btn no-caps unelevated color="primary" icon="add" label="Add to collection"
                   @click="addRecommended" />
            <q-btn no-caps flat color="primary" icon="refresh" label="Try another"
                   @click="getRecommendation" />
          </div>
        </div>
      </transition>

      <div v-if="!loading && !recommendation" class="bk-card q-pa-xl text-center" style="color: var(--ink-soft)">
        <q-icon name="auto_awesome" size="42px" class="q-mb-sm" style="color: var(--gold)" />
        <div class="font-display text-h6" style="color: var(--ink)">No recommendation yet</div>
        <div>Press the button above to get a suggestion from the recommender.</div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios'

const router = useRouter()
const $q = useQuasar()

const readLocation = ref(null)
const usedLocation = ref(null)
const recommendation = ref(null)
const loading = ref(false)

const readLocations = [
  { label: 'Home', value: 'Home' },
  { label: 'Transit', value: 'Transit' },
  { label: 'Public', value: 'Public' }
]

const authorLabel = computed(() => {
  const r = recommendation.value
  if (!r) return ''
  if (r.authorName) return r.authorName
  if (r.author && r.author.name) return r.author.name
  if (Array.isArray(r.authors)) return r.authors.map((a) => a.name || a).join(', ')
  return 'Unknown author'
})

async function getRecommendation () {
  loading.value = true
  try {
    // Assumes GET /recommendations returns one or more books; the read
    // location is forwarded so the NFM can use it as a contextual feature.
    const { data } = await api.get('/recommendations', {
      params: readLocation.value ? { readLocation: readLocation.value } : {}
    })
    const rec = Array.isArray(data) ? data[0] : (data.recommendation || data)
    recommendation.value = rec || null
    usedLocation.value = readLocation.value
    if (!rec) {
      $q.notify({ type: 'warning', message: 'No recommendation available yet — add more books first.' })
    }
  } catch (err) {
    $q.notify({
      type: 'negative',
      message: err.response?.data?.message || 'Could not fetch a recommendation.'
    })
  } finally {
    loading.value = false
  }
}

async function addRecommended () {
  const r = recommendation.value
  if (!r) return
  router.push({
    name: 'add-book',
    query: { bookId: r.id || r.bookId, title: r.title }
  })
}
</script>
