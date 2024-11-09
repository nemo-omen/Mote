import type {Note} from "$lib/types/Note";

export const currentNote = $state({
    note: {} as Note
});