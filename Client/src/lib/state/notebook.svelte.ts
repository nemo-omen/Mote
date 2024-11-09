import type {Note} from "$lib/types/Note";

export class Notebook {
    notes = $state([] as Note[]);
    currentNote = $state({} as Note);
}