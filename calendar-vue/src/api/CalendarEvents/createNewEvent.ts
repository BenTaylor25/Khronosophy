import { API_CALENDAR_EVENT_ROUTE, JSON_HEADERS, NOT_SYNCED } from "../../constants/api.ts";
import { CalendarEventModel } from "../../models/CalendarEventModel.ts";
import { apiGetDefaultUserId } from "../Users/getDefaultUserId.ts";

interface CreateBody {
    userId: string,
    name: string,
    startDateTime: string,
    endDateTime: string
}

interface CreateResponse {
    // id: string
    name: string,
    startDateTime: string,
    endDateTime: string,
    duration: string
}

export async function apiCreateNewEvent(
    calendarEvent: CalendarEventModel
) {
    const userId = await apiGetDefaultUserId();

    //#region Error Handling
    if (userId === '') {
        console.error("Couldn't get default user.");
        return false;
    }
    //#endregion

    const body = {
        userId,
        name: calendarEvent.name,
        startDateTime: calendarEvent.startTime.toISOString(),
        endDateTime: calendarEvent.endTime.toISOString()
    } as CreateBody;

    await fetch(API_CALENDAR_EVENT_ROUTE, {
        method: "POST",
        headers: JSON_HEADERS,
        body: JSON.stringify(body)
    })
    .then(res => {
        return res.json();
    })
    .then((body: CreateResponse) => {
        //#region Error Handling
        if (calendarEvent.serverId !== NOT_SYNCED) {
            const message = "Attempted to create a server CalendarEvent " +
                "using a local event with an existing serverId.";

            console.error(message);
        }
        //#endregion

        // calendarEvent.serverId = body.id;
    })
    .catch(err => {
        console.error(
            "Failed to create Calendar Event on server - " + err
        );
    });
}
