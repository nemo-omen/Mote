<script lang="ts">
	import * as Collapsible from "$lib/components/ui/collapsible/index.js";
	import * as Sidebar from "$lib/components/ui/sidebar/index.js";
	import ChevronRight from "lucide-svelte/icons/chevron-right";
	import FileText from "lucide-svelte/icons/file-text";
	import File from "lucide-svelte/icons/file";
	import FileStack from "lucide-svelte/icons/file-stack";
	import Folder from "lucide-svelte/icons/folder";
	import type { ComponentProps } from "svelte";
	import type { Note } from "$lib/types/Note";
	import {ChevronDown, Notebook} from "lucide-svelte";
	import {MenuButton, MenuSub} from "$lib/components/ui/sidebar/index.js";
	import { currentNote } from '$lib/state/currentNote.svelte'
	import { flattenNotes } from "$lib/utils/flattenNotes";
	import { afterNavigate } from "$app/navigation";

	let { notes, ref = $bindable(null), ...restProps }: ComponentProps<typeof Sidebar.Root> = $props();
	let filteredNotes = filterNulls(notes);
	let hasCurrentChild = $state(false)
	function filterNulls(notes: Note[]) {
		return notes.filter((note) => {
			if(note !== null) {
				if(note.children) {
					note.children = filterNulls(note.children);
				}
				return note;
			}
		});
	}
	
	function setHasCurrentChild(note: Note) {
		if(note.children) {
			hasCurrentChild = flattenNotes(note.children).some((child) => child.id === currentNote.note.id);
		}
		hasCurrentChild = false;
	}
	
	afterNavigate(() => {
		setHasCurrentChild(notes);
	});
	
	$inspect(hasCurrentChild);
</script>

{#snippet childNoteSnippet(note: Note)}
	{#if note}
		{#if currentNote.note.id !== note.id}
		<Sidebar.MenuButton class="w-full flex-nowrap">
			<FileText />
			<a class="text-nowrap" href="/notebook/{note.path}">
				{note.title?? "Untitled"}
			</a>
		</Sidebar.MenuButton>
		{:else}
		<Sidebar.MenuButton class="w-full flex-nowrap" isActive>
			<FileText />
			<a class="text-nowrap" href="/notebook/{note.path}">
				{note.title?? "Untitled"}
			</a>
		</Sidebar.MenuButton>
		{/if}
	{/if}
{/snippet}

{#snippet noteSnippet(note: Note)}
	{#if note}
		{#if (!note.children || (note.children && !note.children.length))}
			{@render childNoteSnippet(note)}
		{:else if note.children}
			<Sidebar.Menu>
				<Collapsible.Root 
						class="group/collapsible [&[data-state=open]>button>svg:first-child]:rotate-90"
						expanded={hasCurrentChild}
				>
					<Sidebar.MenuItem>
						<Collapsible.Trigger class="w-full">
						{#if currentNote.note.id !== note.id}
							<Sidebar.MenuButton class="w-full flex-nowrap">
								<ChevronRight size="16" strokeWidth="3" className="transition-transform" />
								<div class="flex flex-row flex-nowrap gap-2 items-center">
									<FileStack size="16"/>
									<a class="text-nowrap" href="/notebook/{note.path}">
										{note.title?? "Untitled"}
									</a>
								</div>
							</Sidebar.MenuButton>
							{:else}
							<Sidebar.MenuButton class="w-full flex-nowrap" isActive>
								<ChevronRight size="16" strokeWidth="3" className="transition-transform" />
								<div class="flex flex-row flex-nowrap gap-2 items-center">
									<FileStack size="16"/>
									<a class="text-nowrap" href="/notebook/{note.path}">
										{note.title?? "Untitled"}
									</a>
								</div>
							</Sidebar.MenuButton>
							{/if}
						</Collapsible.Trigger>
						<Collapsible.Content>
							<Sidebar.MenuSub>
								{#each note.children as childNote, index (index)}
									{@render noteSnippet(childNote)}
								{/each}
							</Sidebar.MenuSub>
						</Collapsible.Content>
					</Sidebar.MenuItem>
				</Collapsible.Root>
			</Sidebar.Menu>
		{/if}
	{/if}
{/snippet}

<Sidebar.Root bind:ref {...restProps}>
	<Sidebar.Header>
		<Sidebar.Menu>
			<Sidebar.MenuItem>
				<Sidebar.MenuButton size="md">
					<a href="/" class="flex items-center gap-1">
						<Notebook size={16} strokeWidth={3} />
						<span class="font-semibold text-lg">Mote</span>
					</a>
				</Sidebar.MenuButton>
			</Sidebar.MenuItem>
		</Sidebar.Menu>
	</Sidebar.Header>
	<Sidebar.Content>
		<Sidebar.Group>
			<Sidebar.GroupLabel>Notes</Sidebar.GroupLabel>
			<Sidebar.GroupContent>
					{#each filteredNotes as note, index (index)}
						{@render noteSnippet(note)}
					{/each}
			</Sidebar.GroupContent>
		</Sidebar.Group>
	</Sidebar.Content>
	<Sidebar.Rail />
</Sidebar.Root>
