<template>
  <v-app>
    <v-main>
      <Chat/>
    </v-main>
  </v-app>
</template>

<script>
import Chat from './components/Chat';

export default {
  name: 'App',

  components: {
    Chat,
  },
};
</script>
