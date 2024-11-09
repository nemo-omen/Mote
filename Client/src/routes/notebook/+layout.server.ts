import type { PageServerLoad } from "./$types";
import type { Note } from "$lib/types/Note";
import {error} from "@sveltejs/kit";

export const load: PageServerLoad = async (event) => {
    const { fetch } = event;
    let response;
    try {
        response = await fetch('http://localhost:5050/api/notes');
    } catch (err) {
        error(500, err.message);
    }

    if(!response.ok) {
        error(response.status, response.statusText);
    }

    let data: Note[];
    try {
        data = await response.json();
    }catch(err) {
        error(500, err.message);
    }

    return {
        notes: data,
    };
};