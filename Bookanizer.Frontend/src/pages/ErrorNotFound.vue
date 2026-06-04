<template>
  <q-page class="bk-auth-shell">
    <div class="text-center">
      <div class="font-display" style="font-size: 5rem; color: var(--oxblood); font-weight: 700; line-height: 1;">
        404
      </div>
      <div class="bk-subtitle">This page seems to be missing from the shelf.</div>
      <q-btn no-caps unelevated color="primary" label="Back to search"
             @click="$router.push({ name: 'search' })" />
    </div>
  </q-page>
</template>

<script setup>
</script>
