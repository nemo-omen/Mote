<script lang="ts">
    import SidebarPage from "$lib/components/SidebarPage.svelte";
    let { children, data } = $props();
    const { notes } = data;
</script>

<SidebarPage {notes}>
    {@render children()}
</SidebarPage>
