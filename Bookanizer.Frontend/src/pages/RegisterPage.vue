<template>
  <q-card flat class="bk-card bk-auth-card">
    <div class="font-display text-h5 q-mb-xs" style="font-weight: 600">Create your account</div>
    <div class="bk-subtitle">Start building your shelf.</div>

    <q-form @submit.prevent="onSubmit" class="q-gutter-md">
      <q-input
        v-model="form.username"
        label="Username"
        outlined
        dense
        class="bk-field"
        :rules="[
          (v) => !!v || 'Username is required',
          (v) => v.length >= 3 || 'At least 3 characters'
        ]"
        autocomplete="username"
      />
      <q-input
        v-model="form.email"
        label="Email"
        type="email"
        outlined
        dense
        class="bk-field"
        :rules="[
          (v) => !!v || 'Email is required',
          (v) => /.+@.+\..+/.test(v) || 'Enter a valid email'
        ]"
        autocomplete="email"
      />
      <q-input
        v-model="form.password"
        label="Password"
        type="password"
        outlined
        dense
        class="bk-field"
        :rules="[
          (v) => !!v || 'Password is required',
          (v) => v.length >= 8 || 'At least 8 characters'
        ]"
        autocomplete="new-password"
      />
      <q-input
        v-model="form.confirm"
        label="Confirm password"
        type="password"
        outlined
        dense
        class="bk-field"
        :rules="[(v) => v === form.password || 'Passwords do not match']"
        autocomplete="new-password"
      />

      <q-btn
        type="submit"
        no-caps
        unelevated
        color="primary"
        class="full-width"
        label="Create account"
        :loading="loading"
      />
    </q-form>

    <div class="text-center q-mt-lg text-body2" style="color: var(--ink-soft)">
      Already have an account?
      <router-link :to="{ name: 'login' }">Sign in</router-link>
    </div>
  </q-card>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useQuasar } from 'quasar'
import { useAuthStore } from 'stores/auth'

const router = useRouter()
const $q = useQuasar()
const auth = useAuthStore()

const form = ref({ username: '', email: '', password: '', confirm: '' })
const loading = ref(false)

async function onSubmit () {
  loading.value = true
  try {
    await auth.register({
      username: form.value.username,
      email: form.value.email,
      password: form.value.password
    })
    // If the server returned a token, the user is logged in already.
    if (auth.isAuthenticated) {
      $q.notify({ type: 'positive', message: 'Account created.' })
      router.push({ name: 'search' })
    } else {
      $q.notify({ type: 'positive', message: 'Account created — please sign in.' })
      router.push({ name: 'login' })
    }
  } catch (err) {
    $q.notify({
      type: 'negative',
      message: err.response?.data?.message || 'Could not create account.'
    })
  } finally {
    loading.value = false
  }
}
</script>
