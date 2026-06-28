<template>
  <q-page>
    <div class="bk-page" style="max-width: 640px">
      <h1 class="bk-title">Add a read book</h1>
      <p class="bk-subtitle">Record a book you've read, your rating, and where you read it.</p>

      <q-card flat class="bk-card q-pa-lg">
        <q-form @submit.prevent="onSubmit" class="q-gutter-md">
          <!-- Book selection: either prefilled from search, or look one up. -->
          <q-select
            v-model="selectedBook"
            outlined
            dense
            class="bk-field"
            label="Book"
            use-input
            input-debounce="350"
            :options="bookOptions"
            option-label="title"
            option-value="id"
            map-options
            emit-value
            hint="Type to search the catalogue"
            @filter="onFilter"
            :rules="[(v) => !!v || 'Select a book']"
          >
            <template #no-option>
              <q-item><q-item-section class="text-grey">No matches</q-item-section></q-item>
            </template>
          </q-select>

          <q-rating
            v-model="form.rating"
            :max="5"
            size="2em"
            color="primary"
            icon="star_border"
            icon-selected="star"
          />
          <div class="text-caption" style="color: var(--ink-soft)">Your rating (optional)</div>

          <!-- ReadLocationEnum — the contextual feature under study. -->
          <q-select
            v-model="form.readLocation"
            outlined
            dense
            class="bk-field"
            label="Where did you read it?"
            :options="readLocations"
            emit-value
            map-options
            :rules="[(v) => !!v || 'Select a reading location']"
          />

          <q-input
            v-model="form.dateRead"
            outlined
            dense
            class="bk-field"
            label="Date read (optional)"
            type="date"
            stack-label
          />

          <q-input
            v-model="form.review"
            outlined
            dense
            type="textarea"
            class="bk-field"
            label="Notes / review (optional)"
            autogrow
          />

          <q-btn
            type="submit"
            no-caps
            unelevated
            color="primary"
            label="Add to my collection"
            :loading="loading"
          />
        </q-form>
      </q-card>
    </div>
  </q-page>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios'

const route = useRoute()
const router = useRouter()
const $q = useQuasar()

const selectedBook = ref(null)
const bookOptions = ref([])
const loading = ref(false)

// Mirrors the C# ReadLocationEnum (Home / Transit / Public).
const readLocations = [
  { label: 'Home', value: 'Home' },
  { label: 'Transit', value: 'Transit' },
  { label: 'Public', value: 'Public' }
]

const form = ref({
  rating: 0,
  readLocation: null,
  dateRead: '',
  review: ''
})

onMounted(() => {
  // Prefill if arriving from the search page.
  if (route.query.bookId) {
    const prefill = { id: Number(route.query.bookId), title: route.query.title || 'Selected book' }
    bookOptions.value = [prefill]
    selectedBook.value = prefill.id
  }
})

async function onFilter (val, update, abort) {
  if (!val || val.length < 2) {
    update(() => { bookOptions.value = bookOptions.value })
    return
  }
  try {
    const { data } = await api.get('/books/search', { params: { q: val } })
    const items = Array.isArray(data) ? data : (data.items || data.results || [])
    update(() => {
      bookOptions.value = items.map((b) => ({ id: b.id || b.bookId, title: b.title }))
    })
  } catch {
    abort()
  }
}

async function onSubmit () {
  loading.value = true
  try {
    // Assumes PUT /collection updates an existing interaction for the current user.
    await api.put('/collection', {
      bookId: selectedBook.value,
      rating: form.value.rating || null,
      readLocation: form.value.readLocation,
      dateRead: form.value.dateRead || null,
      review: form.value.review || null
    })
    $q.notify({ type: 'positive', message: 'Added to your collection.' })
    router.push({ name: 'profile' })
  } catch (err) {
    $q.notify({
      type: 'negative',
      message: err.response?.data?.message || 'Could not add the book.'
    })
  } finally {
    loading.value = false
  }
}
</script>
