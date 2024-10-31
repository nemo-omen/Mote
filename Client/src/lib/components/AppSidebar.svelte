<script lang="ts">
    import * as Sidebar from "$lib/components/ui/sidebar";
    import {NotebookPen} from "lucide-svelte";
    import type {ComponentProps} from "svelte";
    let { ref = $bindable(null), ...restProps }: ComponentProps<typeof Sidebar.Root> = $props();
</script>

<Sidebar.Root bind:ref {...restProps}>
    <Sidebar.Header>
        <Sidebar.Menu>
            <Sidebar.MenuItem>
                <Sidebar.MenuButton size="md">
                    <NotebookPen class="h-5 w-5" />
                    <h1>Mote</h1>
                </Sidebar.MenuButton>
            </Sidebar.MenuItem>
        </Sidebar.Menu>
    </Sidebar.Header>
    <Sidebar.Content>
        <Sidebar.Group>
            <Sidebar.GroupContent>
                <Sidebar.Menu>
                    
                </Sidebar.Menu>
            </Sidebar.GroupContent>
        </Sidebar.Group>
        
    </Sidebar.Content>
</Sidebar.Root>

<style>
    
</style>