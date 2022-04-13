<template>
    <h1 v-if="link">{{ link }}</h1>
    <h1 v-if="id">{{ id }}</h1>
</template>

<script>
  export default {
    props: ['link', 'id']
  }
</script>