import { MODAL_IDS } from "./constants/modalConstants.ts";

import {
    refreshNewEventModal
} from "./components/modals/NewEventModal.vue";
import {
    refreshEditEventModal
} from "./components/modals/EditEventModal.vue";

function showElementWithId(id: string) {
    const element = document.getElementById(id);

    //#region Error Handling
    if (element == null) {
        console.error(`Could not find element with id '${id}'.`);
    }
    //#endregion

    element?.classList.add('show');
}

function hideElementWithId(id: string) {
    const element = document.getElementById(id);

    //#region Error Handling
    if (element == null) {
        console.error(`Could not find element with id '${id}'.`);
    }
    //#endregion

    element?.classList.remove('show');
}

export function hideAllModals() {
    for (const modalId of Object.values(MODAL_IDS)) {
        hideElementWithId(modalId);
    }
}


// INDIVIDUAL MODALS
// -----------------

// Zoom Settings.
export const showZoomSettingsModal = () => {
    showElementWithId(MODAL_IDS.ZOOM_SETTINGS_MODAL);
}

export const hideZoomSettingsModal = () => {
    hideElementWithId(MODAL_IDS.ZOOM_SETTINGS_MODAL);
}

// New Event.
export const showNewEventModal = () => {
    showElementWithId(MODAL_IDS.NEW_EVENT_MODAL);
    refreshNewEventModal();
}

export const hideNewEventModal = () => {
    hideElementWithId(MODAL_IDS.NEW_EVENT_MODAL);
}

// Edit Event.
export const showEditEventModal = () => {
    showElementWithId(MODAL_IDS.EDIT_EVENT_MODAL);
    refreshEditEventModal();
}

export const hideEditEventModal = () => {
    hideElementWithId(MODAL_IDS.EDIT_EVENT_MODAL);
}

// Year View.
export const showYearViewModal = () => {
    showElementWithId(MODAL_IDS.YEAR_VIEW_MODAL);
}

export const hideYearViewModal = () => {
    hideElementWithId(MODAL_IDS.YEAR_VIEW_MODAL);
}

// Tasks View.
export const showTasksModal = () => {
    showElementWithId(MODAL_IDS.TASKS_MODAL);
}

export const hideTasksModal = () => {
    hideElementWithId(MODAL_IDS.TASKS_MODAL);
}

// Schedule View.
export const showScheduleModal = () => {
    showElementWithId(MODAL_IDS.SCHEDULE_MODAL);
}

export const hideScheduleModal = () => {
    hideElementWithId(MODAL_IDS.SCHEDULE_MODAL);
}

// Sample Data View.
export const showSampleDataModal = () => {
    showElementWithId(MODAL_IDS.SAMPLE_DATA_MODAL);
}

export const hideSampleDataModal = () => {
    hideElementWithId(MODAL_IDS.SAMPLE_DATA_MODAL);
}
