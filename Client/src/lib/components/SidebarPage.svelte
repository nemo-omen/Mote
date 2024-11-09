<script lang="ts">
	import AppSidebar from "$lib/components/AppSidebar.svelte";
	import * as Breadcrumb from "$lib/components/ui/breadcrumb/index.js";
	import { Separator } from "$lib/components/ui/separator/index.js";
	import * as Sidebar from "$lib/components/ui/sidebar/index.js";
	import { currentNote } from "$lib/state/currentNote.svelte";
	import { afterNavigate } from "$app/navigation";
	import type { Note } from "$lib/types/Note";
	import {flattenNotes} from "$lib/utils/flattenNotes";

	let { children, notes } = $props();
	let breadCrumbs = $state(getParentPaths(currentNote.note));
	
	afterNavigate(() => {
		breadCrumbs = getParentPaths(currentNote.note);
	});
	function getParentPaths(note: Note, pathObjects = []) {
		const flatNotes = flattenNotes(notes);
		console.log(flatNotes);
		pathObjects.unshift({
			title: note.title,
			slug: note.slug,
			path: note.path,
		});
		
		if (note.parentId) {
			console.log(note.parentId);
			const parentNote = flatNotes.find((n) => (n.id === note.parentId));
			pathObjects = getParentPaths(parentNote, pathObjects);
		}
		return pathObjects;
	}
	$inspect(currentNote);
</script>

<Sidebar.Provider>
	<AppSidebar {notes}/>
	<Sidebar.Inset>
		<header class="flex h-16 shrink-0 items-center gap-2 border-b px-4">
			<Sidebar.Trigger class="-ml-1" />
			<Separator orientation="vertical" class="mr-2 h-4" />
			<Breadcrumb.Root>
				<Breadcrumb.List>
					<Breadcrumb.Item class="hidden md:block">
						<Breadcrumb.Link href="/notebook">Notebook</Breadcrumb.Link>
					</Breadcrumb.Item>
					<Breadcrumb.Separator class="hidden md:block" />
					{#each breadCrumbs as breadCrumb, index (index)}
						<Breadcrumb.Item class="hidden md:block">
						<Breadcrumb.Link href="/notebook/{breadCrumb.path}">{breadCrumb.title}</Breadcrumb.Link>
					</Breadcrumb.Item>
						{#if index < breadCrumbs.length - 1}
						<Breadcrumb.Separator class="hidden md:block" />
						{/if}
					{/each}
				</Breadcrumb.List>
			</Breadcrumb.Root>
		</header>
		<div class="flex flex-1 flex-col gap-4 p-4">
			{@render children()}
		</div>
	</Sidebar.Inset>
</Sidebar.Provider>
