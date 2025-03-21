import { API_UTMTK_SCHEDULE_ROUTE } from "../../constants/api";
import { apiGetDefaultUserId } from "../Users/getDefaultUserId";

export async function apiUTMTKSchedule() {
    const userId = await apiGetDefaultUserId();

    //#region Error Handling
    if (userId === '') {
        console.error("Couldn't get default user.");
    }
    //#endregion

    const route = API_UTMTK_SCHEDULE_ROUTE + `/${userId}`;

    await fetch(route, {
        method: "POST"
    })
    .catch(err => {
        console.error(err);
    });
}
