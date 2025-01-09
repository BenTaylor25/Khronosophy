import { defineStore } from "pinia";

import { CalendarEventModel } from "../models/CalendarEventModel.ts";
import { apiGetAllCalendarEvents } from "../api/CalendarEvents/getAllEvents.ts";

export const useEventStore = defineStore('events', {
    state: () => ({
        events: [
            new CalendarEventModel(
                "wed 7 aug 24 test",
                // This is 7th AUGUST because JavaScript is stupid.
                new Date(2024, 7, 7, 10, 0, 0, 0),
                new Date(2024, 7, 7, 12, 0, 0, 0)
            ),
            new CalendarEventModel(
                "fri 2 aug 2024 test",
                new Date(2024, 7, 2, 12, 0, 0, 0),
                new Date(2024, 7, 2, 16, 0, 0, 0)
            ),
            new CalendarEventModel(
                "first supervisor meeting",
                new Date(2024, 9, 4, 15, 0, 0),
                new Date(2024, 9, 4, 15, 30, 0)
            )
        ] as CalendarEventModel[],
        selectedEvent: null as CalendarEventModel | null
    }),
    actions: {
        addEvent(newEvent: CalendarEventModel) {
            this.events.push(newEvent);
        },
        deleteSelectedEvent() {
            //#region Error Handling
            if (!this.selectedEvent) {
                return;
            }
            //#endregion

            const idx = this.events.indexOf(this.selectedEvent);

            //#region Error Handling
            if (idx === -1) {
                return;
            }
            //#endregion

            this.events.splice(idx, 1);
        },
        getEventsForDate(date: Date): CalendarEventModel[] {
            const eventsForDate = [] as CalendarEventModel[];

            // Todo: Refactor to handle multi-day events.
            for (const event of this.events) {
                if (
                    event.startTime.getFullYear() === date.getFullYear() &&
                    event.startTime.getMonth() === date.getMonth() &&
                    event.endTime.getDate() === date.getDate()
                ) {
                    eventsForDate.push(event as CalendarEventModel);
                }
            }

            return eventsForDate;
        }
    }
});

apiGetAllCalendarEvents()
    .then(serverEvents => {
        const eventStore = useEventStore();

        serverEvents.forEach(event => {
            eventStore.events.push(event);
        });
    })
