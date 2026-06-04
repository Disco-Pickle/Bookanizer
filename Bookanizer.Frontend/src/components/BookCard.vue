<template>
  <div class="bk-book">
    <div class="bk-book__title">{{ book.title || 'Untitled' }}</div>
    <div class="bk-book__author">{{ authorLabel }}</div>

    <div v-if="book.averageRating" class="row items-center q-gutter-xs q-mb-xs">
      <q-icon name="star" size="14px" style="color: var(--gold)" />
      <span class="text-caption">{{ Number(book.averageRating).toFixed(2) }}</span>
      <span v-if="book.ratingsCount" class="text-caption" style="color: var(--ink-soft)">
        ({{ book.ratingsCount.toLocaleString() }})
      </span>
    </div>

    <div v-if="genres.length" class="q-mt-xs">
      <span v-for="g in genres" :key="g" class="bk-tag">{{ g }}</span>
    </div>

    <div v-if="$slots.action" class="q-mt-sm">
      <slot name="action" />
    </div>
  </div>
</template>

<script setup>
import { computed } from 'vue'

const props = defineProps({
  book: { type: Object, required: true }
})

const authorLabel = computed(() => {
  const b = props.book
  if (b.authorName) return b.authorName
  if (b.author && b.author.name) return b.author.name
  if (Array.isArray(b.authors)) return b.authors.map((a) => a.name || a).join(', ')
  return 'Unknown author'
})

const genres = computed(() => {
  const b = props.book
  if (Array.isArray(b.genres)) {
    return b.genres.map((g) => (typeof g === 'string' ? g : g.name)).slice(0, 4)
  }
  return []
})
</script>
