import { defineStore } from "pinia";

import { TaskboardTaskModel } from "../models/TaskboardTaskModel";
import { apiGetAllTasks } from "../api/Tasks/getAllTasks";

export const useTasksStore = defineStore('tasks', {
    state: () => ({
        tasks: [] as TaskboardTaskModel[]
    }),
    actions: {
        addTask(newTask: TaskboardTaskModel) {
            this.tasks.push(newTask);
        },
        removeTask(task: TaskboardTaskModel) {
            const idx = this.tasks.indexOf(task);

            // #region Error Handling
            if (idx === -1) {
                return;
            }
            // #endregion

            this.tasks.splice(idx, 1);
        }
    }
});

apiGetAllTasks()
    .then(tasks => {
        const taskStore = useTasksStore();

        tasks.forEach(task => {
            taskStore.addTask(task)
        });
    });
