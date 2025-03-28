import {
    API_CLEAR_SCHEDULED_EVENTS_ROUTE,
    JSON_HEADERS
} from "../../constants/api";
import { apiGetDefaultUserId } from "../Users/getDefaultUserId";

export async function apiClearScheduledEvents() {
    const userId = await apiGetDefaultUserId();

    //#region Error Handling
    if (userId === '') {
        console.error("Couldn't get default user.");
    }
    //#endregion

    const apiRoute = `${API_CLEAR_SCHEDULED_EVENTS_ROUTE}/${userId}`;

    await fetch(apiRoute, {
        method: "DELETE",
        headers: JSON_HEADERS
    })
        .catch(err => {
            console.error(
                "Could not reach server to clear scheduled events: " + err
            );
        });
}

