import { defineStore } from "pinia";

import { TaskboardTaskModel } from "../models/TaskboardTaskModel";
import { apiGetAllTasks } from "../api/Tasks/getAllTasks";

export const useTasksStore = defineStore('tasks', {
    state: () => ({
        tasks: [
            new TaskboardTaskModel(
                "test task"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
            new TaskboardTaskModel(
                "test task 2"
            ),
        ] as TaskboardTaskModel[]
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

// TODO:
// apiGetAllTasks().then(useEventStore().tasks.push(t for t in response)).
apiGetAllTasks()
    .then(tasks => {
        const taskStore = useTasksStore();

        tasks.forEach(task => {
            taskStore.addTask(task)
        });
    })
