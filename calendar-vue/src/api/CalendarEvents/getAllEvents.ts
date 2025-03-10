import { CalendarEventModel } from "../../models/CalendarEventModel.ts";
import { apiGetDefaultUserId } from "../Users/getDefaultUserId.ts";
import { API_CALENDAR_EVENTS_ROUTE } from "../../constants/api.ts";

interface ApiEventResponse {
    id: string,
    name: string,
    startDateTime: string,
    endDateTime: string
}

export async function apiGetAllCalendarEvents():
    Promise<CalendarEventModel[]>
{
    const userId = await apiGetDefaultUserId();

    //#region Error Handling
    if (userId === '') {
        console.error("Couldn't get default user.");
        return [];
    }
    //#endregion

    const route = `${API_CALENDAR_EVENTS_ROUTE}/${userId}`;

    const eventsFromApi = [] as CalendarEventModel[];

    await fetch(route)
        .then(res => {
            return res.json();
        })
        .then(body => {
            for (const apiEvent of body as ApiEventResponse[]) {
                const startDateTime = new Date(apiEvent.startDateTime);
                const endDateTime = new Date(apiEvent.endDateTime);

                const eventFromApi = new CalendarEventModel(
                    apiEvent.name,
                    startDateTime,
                    endDateTime
                );
                eventFromApi.serverId = apiEvent.id;

                eventsFromApi.push(eventFromApi);
            }
        })
        .catch(err => {
            console.error(`Could not reach server; ${err}`);
        });

    return eventsFromApi;
}
