import { API_CALENDAR_EVENT_ROUTE, NOT_SYNCED } from "../../constants/api.ts";
import { CalendarEventModel } from "../../models/CalendarEventModel.ts";

// THIS IS STILL USING THE OLD API SO IT WILL NOT WORK PROPERLY.

export async function apiDeleteEvent(calendarEvent: CalendarEventModel) {
    //#region Error Handling
    if (calendarEvent === null || calendarEvent.serverId === NOT_SYNCED) {
        console.error("Attempted to update invalid event.");
    }
    //#endregion

    const route = `${API_CALENDAR_EVENT_ROUTE}/${calendarEvent.serverId}`;

    await fetch(route, {
        method: "DELETE"
    })
    .catch(err => {
        console.error("Delete event request failed: " + err);
    });
}
