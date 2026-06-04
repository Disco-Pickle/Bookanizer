<template>
  <q-page>
    <div class="bk-page">
      <h1 class="bk-title">Find a book</h1>
      <p class="bk-subtitle">Search the catalogue by title, author, or genre.</p>

      <q-form @submit.prevent="onSearch" class="row q-gutter-sm items-stretch q-mb-lg">
        <q-input
          v-model="query"
          outlined
          dense
          class="bk-field col"
          placeholder="e.g. The Name of the Wind"
          clearable
          @clear="results = []"
        >
          <template #prepend><q-icon name="search" /></template>
        </q-input>
        <q-btn
          type="submit"
          no-caps
          unelevated
          color="primary"
          label="Search"
          :loading="loading"
        />
      </q-form>

      <div v-if="loading" class="row justify-center q-py-xl">
        <q-spinner-dots size="40px" color="primary" />
      </div>

      <div v-else-if="searched && !results.length" class="text-center q-py-xl" style="color: var(--ink-soft)">
        <q-icon name="search_off" size="42px" class="q-mb-sm" />
        <div>No books matched “{{ lastQuery }}”.</div>
      </div>

      <div v-else-if="results.length">
        <div class="text-caption q-mb-sm" style="color: var(--ink-soft)">
          {{ results.length }} result{{ results.length === 1 ? '' : 's' }}
        </div>
        <div class="bk-grid">
          <BookCard v-for="book in results" :key="book.id || book.bookId" :book="book">
            <template #action>
              <q-btn
                no-caps
                outline
                size="sm"
                color="primary"
                icon="add"
                label="Add to collection"
                @click="goAdd(book)"
              />
            </template>
          </BookCard>
        </div>
      </div>

      <div v-else class="bk-card q-pa-xl text-center" style="color: var(--ink-soft)">
        <q-icon name="auto_stories" size="42px" class="q-mb-sm" style="color: var(--gold)" />
        <div class="font-display text-h6" style="color: var(--ink)">Search to begin</div>
        <div>Type a title or author above to explore the catalogue.</div>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios'
import BookCard from 'components/BookCard.vue'

const router = useRouter()
const $q = useQuasar()

const query = ref('')
const lastQuery = ref('')
const results = ref([])
const loading = ref(false)
const searched = ref(false)

async function onSearch () {
  if (!query.value || !query.value.trim()) return
  loading.value = true
  searched.value = true
  lastQuery.value = query.value
  try {
    // Assumes GET /books/search?q=...; adjust to your REST endpoint.
    const { data } = await api.get('/books/search', { params: { q: query.value.trim() } })
    results.value = Array.isArray(data) ? data : (data.items || data.results || [])
  } catch (err) {
    $q.notify({
      type: 'negative',
      message: err.response?.data?.message || 'Search failed.'
    })
    results.value = []
  } finally {
    loading.value = false
  }
}

function goAdd (book) {
  // Carry the selected book to the Add page via route state.
  router.push({
    name: 'add-book',
    query: {
      bookId: book.id || book.bookId,
      title: book.title
    }
  })
}
</script>
