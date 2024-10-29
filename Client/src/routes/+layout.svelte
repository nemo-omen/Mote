<script lang="ts">
	import '../app.css';
    import { ModeWatcher } from "mode-watcher";
    import Header from "$lib/components/Header.svelte";
    import * as Sidebar from "$lib/components/ui/sidebar/index";
	let { children } = $props();
</script>

<ModeWatcher />
<Header />
<Sidebar.Provider>
    {@render children()}
</Sidebar.Provider>
