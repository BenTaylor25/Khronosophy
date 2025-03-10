import { TaskboardTaskModel } from "../../models/TaskboardTaskModel";
import { apiGetDefaultUserId } from "../Users/getDefaultUserId";
import { API_TASK_ROUTE } from "../../constants/api";

interface ApiTaskResponse {
    name: string,
    expectedDuration: string,
    importance: number | null,
    intensity: number | null
}

export async function apiGetAllTasks(): Promise<TaskboardTaskModel[]> {
    const userId = await apiGetDefaultUserId();

    //#region Error Handling
    if (userId === '') {
        console.error("Couldn't get default user.");
        return [];
    }
    //#endregion

    const route = `${API_TASK_ROUTE}/${userId}`;

    const tasksFromApi = [] as TaskboardTaskModel[];

    await fetch(route)
        .then(res => {
            return res.json();
        })
        .then(body => {
            for (const apiTask of body as ApiTaskResponse[]) {
                const taskFromApi = new TaskboardTaskModel(apiTask.name);
                // expected duration
                taskFromApi.importance = apiTask.importance;
                taskFromApi.intensity = apiTask.intensity;

                tasksFromApi.push(taskFromApi);
            }
        })
        .catch(err => {
            console.error(`Could not reach server; ${err}`);
        })

    return tasksFromApi;
}
