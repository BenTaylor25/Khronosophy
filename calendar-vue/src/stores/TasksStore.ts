import { defineStore } from "pinia";

import { TaskboardTaskModel } from "../models/TaskboardTaskModel";

export const useTasksStore = defineStore('tasks', {
    state: () => ({
        tasks: [
            new TaskboardTaskModel(
                "test task"
            )
        ] as TaskboardTaskModel[]
    }),
    actions: {
        addTask(newTask: TaskboardTaskModel) {
            this.tasks.push(newTask);
        }
    }
});

// TODO:
// apiGetAllTasks().then(useEventStore().tasks.push(t for t in response)).
