import {error} from "@sveltejs/kit";

export async function load(event) {
    const { fetch, params, parent } = event;
    const { path } = params;
    let response;

    try {
        response = await fetch(`http://localhost:5050/api/notes/path?path=${path}`);
    } catch (err) {
        error(500, err.message);
    }

    if (!response.ok) {
        error(response.status, response.statusText);
    }

    let data;

    try {
        data = await response.json();
    } catch (err) {
        error(500, err.message);
    }
    
    console.log(data.path);

    return {
        currentNote: data
    };
}