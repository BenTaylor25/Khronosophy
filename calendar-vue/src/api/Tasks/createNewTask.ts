import { API_TASK_ROUTE, JSON_HEADERS } from "../../constants/api.ts";
import { TaskboardTaskModel } from "../../models/TaskboardTaskModel.ts";
import { apiGetDefaultUserId } from "../Users/getDefaultUserId.ts";

interface CreateBody {
    userId: string,
    name: string,
    expectedDurationMinutes: number,
    importance: number | null,
    intensity: number | null
}

interface CreateResponse {
    name: string,
    expectedDuration: string,
    importance: number,
    intensity: number,
    events: string[] // TODO ARRAY OF EVENTS
}

export async function apiCreateNewTask(
    task: TaskboardTaskModel
): Promise<boolean> {
    const userId = await apiGetDefaultUserId();

    //#region Error Handling
    if (userId === '') {
        console.error("Couldn't get default user.");
        return false;
    }
    //#endregion

    const expectedDurationMinutes = 0; // Oops, forgot about that...

    let success = false;

    const body = {
        userId,
        name: task.name,
        expectedDurationMinutes,
        importance: task.importance,
        intensity: task.intensity
    } as CreateBody;

    await fetch(API_TASK_ROUTE, {
        method: "POST",
        headers: JSON_HEADERS,
        body: JSON.stringify(body)
    })
    .then(res => {
        return res.json();
    })
    .then((body: CreateResponse) => {
        success = true;
        console.debug(body);
    })
    .catch(err => {
        console.error(
            "Failed to create Task on server - " + err
        );
    });

    return success;
}
