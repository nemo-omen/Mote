<script lang="ts">
    import { currentNote } from '$lib/state/currentNote.svelte';
    import { afterNavigate } from "$app/navigation";
    import { EditorState, Compartment } from "@codemirror/state";
    import { EditorView, basicSetup } from "codemirror";
    import { barf, coolGlow } from 'thememirror';
    import { defaultKeymap, indentWithTab } from "@codemirror/commands";
    
    import { markdown, markdownLanguage } from "@codemirror/lang-markdown";
    import {onMount} from "svelte";
    import {javascript} from "@codemirror/lang-javascript";
    import {keymap} from "@codemirror/view";

    let { data } = $props();
    
    let headerState = $state({editing: false});
    let editorElement: HTMLDivElement;
    let editor: EditorView;
    
    onMount(() => {
        let theme = EditorView.theme({
            "&": {
                color: "--color-slate-200",
                backgroundColor: "black"
            },
        });
        
    });

    afterNavigate(() => {
        editorElement.innerHTML = "";
        currentNote.note = data.currentNote;
        let startState = EditorState.create({
            doc: currentNote.note.content,
            extensions: [
                basicSetup,
                keymap.of([indentWithTab]),
                markdown({base: markdownLanguage}),
                barf,
                EditorView.updateListener.of((update) => {
                    if (update.docChanged) {
                        currentNote.note.content = update.state.doc.toString();
                    }
                }),
            ]
        });

        editor = new EditorView({
            state: startState,
            parent: editorElement,
        });
    });
    
    function toggleTitleEditing() {
        headerState.editing = !headerState.editing;
        if (!headerState.editing) {
            saveCurrentNote();
        }
    }
    
    function saveCurrentNote() {
        console.log(`Saving note: `, currentNote.note);
    }
</script>

<div class="w-full">
    {#if !headerState.editing}
        <button onclick={() => toggleTitleEditing()} class="border-0 bg-transparent p-0 w-full text-start">
            <h1 class="text-2xl font-bold text-cyan-600 cursor-pointer">
                {currentNote.note.title}
            </h1>
        </button>
    {:else}
    <input 
            type="text" class="w-full text-2xl font-bold text-cyan-600 border-slate-500"
            bind:value={currentNote.note.title} onblur={() => toggleTitleEditing()}
            autofocus>
    {/if}
</div>
<div class="editor w-full h-full bg-black" bind:this={editorElement}></div>

<style>
    :root {
        --slate-200: #e2e8f0;
        --slate-500: #64748b;
        --slate-800: #1e293b;
    }
    :global(.cm-editor) {
        border: 1px solid var(--slate-800);
        background-color: var(--background);
        min-height: 100%;
        border-radius: 4px;
    }
    
    :global(.cm-editor.cm-focused) {
        border-color: var(--slate-500);
    }
    
    :global(.cm-editor .cm-roller) {
        background-color: var(--background);
        min-height: 100%;
    }
    
    :global(.cm-content) {
        font-size: 1.2rem;
        /*color: var(--slate-500);*/
    }

    :global(.cm-focused .cm-content) {
        /*color: var(--slate-200);*/
    }
</style>