<template>
  <div class="home">
    <Dashboard />
  </div>
</template>

<script>
// @ is an alias to /src
import Dashboard from "@/components/Dashboard.vue";

export default {
    name: "home",
    components: {
        Dashboard
    }
};
</script>

<style scoped lang="scss">
div.home {
    text-align: left;
}
</style>
