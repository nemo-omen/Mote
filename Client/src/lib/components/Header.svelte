<script lang="ts">
    import { toggleMode } from 'mode-watcher';
    import Sun from 'lucide-svelte/icons/sun';
    import Moon from 'lucide-svelte/icons/moon';
    import { Button } from '$lib/components/ui/button/index';
</script>

<header class="flex flex-row justify-between items-center p-2 border-b-2 border-muted">
    <h1>
        <a href="/" class="font-bold">
            Mote
        </a>
    </h1>
    <nav>
        <ul class="flex flex-row gap-4 items-center">
            <li><a href="/">Home</a></li>
            <li>
                <Button onclick={toggleMode} variant="outline" size="icon">
                    <Sun
                            class="h-[1.2rem] w-[1.2rem] rotate-0 scale-100 transition-all dark:-rotate-90 dark:scale-0"
                    />
                    <Moon
                            class="absolute h-[1.2rem] w-[1.2rem] rotate-90 scale-0 transition-all dark:rotate-0 dark:scale-100"
                    />
                    <span class="sr-only">Toggle theme</span>
                </Button>
            </li>
        </ul>
    </nav>
</header>

<style>
    header {
        grid-area: header;
    }
</style>