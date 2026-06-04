<template>
  <q-layout view="hHh lpR fFf">
    <q-header bordered class="bk-header">
      <q-toolbar class="bk-toolbar">
        <q-toolbar-title class="row items-center no-wrap">
          <span class="font-display bk-brand" @click="$router.push({ name: 'search' })">
            Bookanizer
          </span>
        </q-toolbar-title>

        <!-- Desktop nav -->
        <div class="gt-sm row items-center q-gutter-sm">
          <q-btn
            v-for="link in links"
            :key="link.name"
            flat
            no-caps
            :icon="link.icon"
            :label="link.label"
            class="bk-nav-btn"
            :class="{ 'bk-nav-active': $route.name === link.name }"
            @click="$router.push({ name: link.name })"
          />
          <q-btn flat round icon="account_circle" class="bk-nav-btn">
            <q-menu>
              <q-list style="min-width: 180px">
                <q-item>
                  <q-item-section>
                    <q-item-label caption>Signed in as</q-item-label>
                    <q-item-label class="font-display text-weight-medium">{{ username }}</q-item-label>
                  </q-item-section>
                </q-item>
                <q-separator />
                <q-item clickable v-close-popup @click="$router.push({ name: 'profile' })">
                  <q-item-section avatar><q-icon name="menu_book" /></q-item-section>
                  <q-item-section>My collection</q-item-section>
                </q-item>
                <q-item clickable v-close-popup @click="onLogout">
                  <q-item-section avatar><q-icon name="logout" /></q-item-section>
                  <q-item-section>Sign out</q-item-section>
                </q-item>
              </q-list>
            </q-menu>
          </q-btn>
        </div>

        <!-- Mobile menu -->
        <q-btn class="lt-md" flat round icon="menu">
          <q-menu>
            <q-list style="min-width: 200px">
              <q-item
                v-for="link in links"
                :key="link.name"
                clickable
                v-close-popup
                @click="$router.push({ name: link.name })"
              >
                <q-item-section avatar><q-icon :name="link.icon" /></q-item-section>
                <q-item-section>{{ link.label }}</q-item-section>
              </q-item>
              <q-separator />
              <q-item clickable v-close-popup @click="onLogout">
                <q-item-section avatar><q-icon name="logout" /></q-item-section>
                <q-item-section>Sign out ({{ username }})</q-item-section>
              </q-item>
            </q-list>
          </q-menu>
        </q-btn>
      </q-toolbar>
    </q-header>

    <q-page-container>
      <router-view />
    </q-page-container>
  </q-layout>
</template>

<script setup>
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from 'stores/auth'

const router = useRouter()
const auth = useAuthStore()

const username = computed(() => auth.username || 'reader')

const links = [
  { name: 'search', label: 'Search', icon: 'search' },
  { name: 'add-book', label: 'Add book', icon: 'add' },
  { name: 'recommend', label: 'Recommend', icon: 'auto_awesome' },
  { name: 'profile', label: 'Profile', icon: 'menu_book' }
]

function onLogout () {
  auth.logout()
  router.push({ name: 'login' })
}
</script>

<style scoped>
.bk-header {
  background: #fbf6ec;
  color: var(--ink);
  border-bottom: 1px solid var(--line) !important;
}
.bk-toolbar {
  max-width: 1080px;
  margin: 0 auto;
  width: 100%;
  padding: 0 1rem;
}
.bk-brand {
  font-weight: 700;
  font-size: 1.35rem;
  color: var(--oxblood);
  cursor: pointer;
  letter-spacing: -0.02em;
}
.bk-nav-btn {
  color: var(--ink-soft);
  font-weight: 500;
}
.bk-nav-active {
  color: var(--oxblood);
}
</style>
