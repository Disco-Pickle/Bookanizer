<template>
  <q-card flat class="bk-card bk-auth-card">
    <div class="font-display text-h5 q-mb-xs" style="font-weight: 600">Welcome back</div>
    <div class="bk-subtitle">Sign in to reach your collection.</div>

    <q-form @submit.prevent="onSubmit" class="q-gutter-md">
      <q-input
        v-model="form.username"
        label="Username"
        outlined
        dense
        class="bk-field"
        :rules="[(v) => !!v || 'Username is required']"
        autocomplete="username"
      />
      <q-input
        v-model="form.password"
        label="Password"
        type="password"
        outlined
        dense
        class="bk-field"
        :rules="[(v) => !!v || 'Password is required']"
        autocomplete="current-password"
      />

      <q-btn
        type="submit"
        no-caps
        unelevated
        color="primary"
        class="full-width"
        label="Sign in"
        :loading="loading"
      />
    </q-form>

    <div class="text-center q-mt-lg text-body2" style="color: var(--ink-soft)">
      New here?
      <router-link :to="{ name: 'register' }">Create an account</router-link>
    </div>
  </q-card>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useQuasar } from 'quasar'
import { useAuthStore } from 'stores/auth'

const router = useRouter()
const route = useRoute()
const $q = useQuasar()
const auth = useAuthStore()

const form = ref({ username: '', password: '' })
const loading = ref(false)

async function onSubmit () {
  loading.value = true
  try {
    await auth.login(form.value)
    $q.notify({ type: 'positive', message: 'Signed in.' })
    const redirect = route.query.redirect || { name: 'search' }
    router.push(redirect)
  } catch (err) {
    $q.notify({
      type: 'negative',
      message: err.response?.data?.message || 'Invalid credentials.'
    })
  } finally {
    loading.value = false
  }
}
</script>
