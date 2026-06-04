<template>
  <q-page>
    <div class="bk-page">
      <!-- Profile header -->
      <div class="row items-center q-col-gutter-md q-mb-lg">
        <div class="col-auto">
          <q-avatar size="72px" color="primary" text-color="white" class="font-display">
            {{ initials }}
          </q-avatar>
        </div>
        <div class="col">
          <h1 class="bk-title" style="margin-bottom: 0">{{ username }}</h1>
          <div class="bk-subtitle" style="margin-bottom: 0">
            {{ collection.length }} book{{ collection.length === 1 ? '' : 's' }} in your collection
          </div>
        </div>
        <div class="col-auto">
          <q-btn no-caps outline color="primary" icon="add" label="Add book"
                 @click="$router.push({ name: 'add-book' })" />
        </div>
      </div>

      <q-separator class="q-mb-lg" style="background: var(--line)" />

      <div v-if="loading" class="row justify-center q-py-xl">
        <q-spinner-dots size="40px" color="primary" />
      </div>

      <div v-else-if="!collection.length" class="bk-card q-pa-xl text-center" style="color: var(--ink-soft)">
        <q-icon name="menu_book" size="42px" class="q-mb-sm" style="color: var(--gold)" />
        <div class="font-display text-h6" style="color: var(--ink)">Your shelf is empty</div>
        <div class="q-mb-md">Add the books you've read to start getting recommendations.</div>
        <q-btn no-caps unelevated color="primary" label="Add your first book"
               @click="$router.push({ name: 'add-book' })" />
      </div>

      <div v-else class="bk-grid">
        <BookCard
          v-for="item in collection"
          :key="item.id || item.bookId"
          :book="bookOf(item)"
        >
          <template #action>
            <div class="row items-center justify-between full-width">
              <span v-if="item.readLocation" class="bk-tag">{{ item.readLocation }}</span>
              <q-btn flat round dense size="sm" icon="delete_outline" color="negative"
                     @click="onRemove(item)">
                <q-tooltip>Remove</q-tooltip>
              </q-btn>
            </div>
          </template>
        </BookCard>
      </div>
    </div>
  </q-page>
</template>

<script setup>
import { ref, computed, onMounted } from 'vue'
import { useQuasar } from 'quasar'
import { api } from 'boot/axios'
import { useAuthStore } from 'stores/auth'
import BookCard from 'components/BookCard.vue'

const $q = useQuasar()
const auth = useAuthStore()

const collection = ref([])
const loading = ref(true)

const username = computed(() => auth.username || 'reader')
const initials = computed(() => (username.value || '?').slice(0, 2).toUpperCase())

// A collection item may nest the book object or carry flat fields.
function bookOf (item) {
  return item.book || item
}

async function load () {
  loading.value = true
  try {
    // Refresh username in case it changed server-side.
    try { await auth.fetchProfile() } catch { /* non-fatal */ }
    const { data } = await api.get('/collection')
    collection.value = Array.isArray(data) ? data : (data.items || [])
  } catch (err) {
    $q.notify({
      type: 'negative',
      message: err.response?.data?.message || 'Could not load your collection.'
    })
  } finally {
    loading.value = false
  }
}

function onRemove (item) {
  $q.dialog({
    title: 'Remove book',
    message: `Remove “${bookOf(item).title}” from your collection?`,
    cancel: true,
    persistent: true
  }).onOk(async () => {
    try {
      await api.delete(`/collection/${item.id || item.bookId}`)
      collection.value = collection.value.filter((c) => c !== item)
      $q.notify({ type: 'positive', message: 'Removed.' })
    } catch (err) {
      $q.notify({
        type: 'negative',
        message: err.response?.data?.message || 'Could not remove the book.'
      })
    }
  })
}

onMounted(load)
</script>
