import type {Note} from "$lib/types/Note";

export function flattenNotes(notes: Note[]): Note[] {
    let flatNotes: Note[] = [];
    notes.forEach(note => {
        flatNotes.push(note);
        if(note && note.children) {
            flatNotes = [...flatNotes, ...flattenNotes(note.children)];
        }
    });
    return flatNotes;
}