import { defineStore } from "pinia";

import { CalendarEventModel } from "../models/CalendarEventModel.ts";
import { apiGetAllCalendarEvents } from "../api/CalendarEvents/getAllEvents.ts";

export const useEventStore = defineStore('events', {
    state: () => ({
        events: [] as CalendarEventModel[],
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
        },
        removeAll() {
            this.events = [];
        }
    }
});

export function refreshEvents() {
    apiGetAllCalendarEvents()
    .then(serverEvents => {
        const eventStore = useEventStore();

        eventStore.removeAll();

        serverEvents.forEach(event => {
            eventStore.events.push(event);
        });
    })
}

refreshEvents();
