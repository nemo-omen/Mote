<script>
    import {Notebook} from "lucide-svelte";
</script>

<main class="h-dvh grid place-content-center">
    <article class="flex flex-col items-center gap-12 p-8 border border-muted-950 rounded shadow-md w-96">
        <h1 class="flex flex-col gap-4 items-center justify-center font-extrabold text-2xl text-slate-500">
            <Notebook size="56"/> Mote 
        </h1>
        <div class="flex items-center gap-2">
            <a href="/auth/login" class="underline text-slate-400">Log in</a>
            <span class="text-slate-600">or</span>
            <a href="/auth/signup" class="underline text-slate-400">Sign up</a>
        </div>
    </article>
</main>
