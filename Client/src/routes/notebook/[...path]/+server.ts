import type {Note} from "$lib/types/Note";
import {error, json} from "@sveltejs/kit";

export async function PUT(event) {
    const { fetch, request } = event;
    let noteData: Note;
    try {
        noteData = await request.json();
    } catch (e) {
        error(400, "Invalid JSON");
    }
    
    // Send the updated note to the API
    let response: Response;
    try {
        response = await fetch("http://localhost:3000/api/notes", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(noteData)
        });
    } catch (e) {
        error(500, "Failed to update note");
    }
    
    // Return the updated note
    let updatedNote: Note;
    try {
        updatedNote = await response.json();
    } catch (e) {
        error(500, "Failed to parse response");
    }
    
    return json(updatedNote);
}