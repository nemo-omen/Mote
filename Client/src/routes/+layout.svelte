<script lang="ts">
	import '../app.css';
    import { ModeWatcher } from "mode-watcher";
    import Header from "$lib/components/Header.svelte";
    import * as Sidebar from "$lib/components/ui/sidebar/index";
    import AppSidebar from "$lib/components/AppSidebar.svelte";
	let { children } = $props();
</script>

<ModeWatcher />
<Sidebar.Provider>
    <AppSidebar />
    <Sidebar.Inset>
        <Header>
            <Sidebar.Trigger />
            
        </Header>
        
    </Sidebar.Inset>
    {@render children()}
</Sidebar.Provider>
