import { API_CALENDAR_EVENT_ROUTE, API_TASK_ROUTE, JSON_HEADERS, NOT_SYNCED } from "../../constants/api.ts";
import { TaskboardTaskModel } from "../../models/TaskboardTaskModel.ts";

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
    events: string[] // array of events
}

export async function apiCreateNewTask(
    task: TaskboardTaskModel
) {
    const userId = "";
    const expectedDurationMinutes = 0; // Oops, forgot about that...

    const body = {
        userId,
        name: task.name,
        expectedDurationMinutes,
        importance: task.importance,
        intensity: task.intensity
    } as CreateBody;

    console.log(body);

    await fetch(API_TASK_ROUTE, {
        method: "POST",
        headers: JSON_HEADERS,
        body: JSON.stringify(body)
    })
    .then(res => {
        return res.json();
    })
    .then((body: CreateResponse) => {
        console.debug(body);
    })
    .catch(err => {
        console.error(
            "Failed to create Task on server - " + err
        );
    });
}
