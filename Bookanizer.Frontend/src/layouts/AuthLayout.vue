<template>
  <q-layout view="hHh lpR fFf">
    <q-page-container>
      <div class="bk-auth-shell">
        <div class="full-width" style="max-width: 460px">
          <div class="text-center q-mb-lg">
            <div class="font-display text-h4" style="color: var(--oxblood); font-weight: 700;">
              Bookanizer
            </div>
            <div class="bk-subtitle q-mt-xs" style="margin-bottom: 0">
              Your shelf, organized. Your next read, discovered.
            </div>
          </div>
          <router-view />
        </div>
      </div>
    </q-page-container>
  </q-layout>
</template>

<script setup>
// Bare shell for login / register.
</script>
