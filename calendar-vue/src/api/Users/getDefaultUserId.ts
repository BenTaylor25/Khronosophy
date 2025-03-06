import { API_USER_ROUTE, JSON_HEADERS } from "../../constants/api";

interface DefaultUserRespose {
    id: string
}

export async function apiGetDefaultUserId(): Promise<string> {
    const route = `${API_USER_ROUTE}/getDefaultUserId`;

    await fetch(route, {
        method: "GET",
        headers: JSON_HEADERS
    })
    .then(async res => {
        if (!res.ok) {
            const err = await res.json();
            throw new Error(err.detail);
        }
        return res.json();
    })
    .then((body: DefaultUserRespose) => {
        return body.id;
    })
    .catch(err => {
        console.error(
            "Failed to reach server - " + err
        );
    });

    return "";
}
