<script setup lang="ts">
import { ref } from 'vue';
import { MODAL_IDS } from '../../constants/modalConstants';
import { hideTasksModal } from '../../modalController';
import { useTasksStore } from '../../stores/TasksStore';

import ModalShadow from './ModalShadow.vue';
import { TaskboardTaskModel } from '../../models/TaskboardTaskModel';
import { apiCreateNewTask } from '../../api/Tasks/createNewTask';

const tasksStore = useTasksStore();
</script>

<template>
  <modal-shadow
    :id="MODAL_IDS.TASKS_MODAL"
    @click="hideTasksModal()"
  >

    <div
      id="tasks-modal-content"
      @click.stop
    >

      <h1>Tasks</h1>

      <!-- Tasks Header -->
       <div id="task-header" class="task">
          <p>Task Name</p>
          <p>Importance</p>
          <p>Intensity</p>
          <p>Delete</p>
       </div>

       <!-- Existing Tasks -->
      <div id="existing-tasks">
        <div
          class="task"
          v-for="task in tasksStore.tasks"
        >
          <input :value="task.name" />
          <input
            type="number"
            :value="task.importance || ''"
            min="0"
            max="10"
          />
          <input
            type="number"
            :value="task.intensity || ''"
            min="0"
            max="10"
          />
          <button @click="removeTask(task as TaskboardTaskModel)">
            Delete
          </button>
        </div>
      </div>

      <!-- New Task -->
      <form
        id="new-task"
        class="task"
        @submit.prevent="createNewTask()"
      >
        <input
          id="new-task-name"
          type="text"
          v-model="newTaskName"
        />

        <input
          id="new-task-importance"
          type="number"
          v-model="newTaskImportance"
          min="0"
          max="10"
        />

        <input
          id="new-task-intensity"
          type="number"
          v-model="newTaskIntensity"
          min="0"
          max="10"
        />

        <button type="submit">Create New</button>

      </form>

    </div>

  </modal-shadow>

</template>

<script lang="ts">
const newTaskName = ref('');
const newTaskImportance = ref(0);
const newTaskIntensity = ref(0);

function createNewTask() {
  if (newTaskIsValid()) {
    const tasksStore = useTasksStore();

    const newTask = new TaskboardTaskModel(
      newTaskName.value
    );
    newTask.importance = newTaskImportance.value;
    newTask.intensity = newTaskIntensity.value;

    apiCreateNewTask(newTask);
    tasksStore.addTask(newTask);

    clearForm();
  }
}

function newTaskIsValid(): boolean {
  return newTaskName.value.length > 0 &&
    newTaskImportance.value >= 0 &&
    newTaskImportance.value <= 10 &&
    newTaskIntensity.value >= 0 &&
    newTaskIntensity.value <= 10;
}

function clearForm() {
  newTaskName.value = "";
  newTaskImportance.value = 0;
  newTaskIntensity.value = 0;
}

function removeTask(task: TaskboardTaskModel) {
  const tasksStore = useTasksStore();

  tasksStore.removeTask(task);
}

</script>

<style scoped lang="scss">
#tasks-modal-content {
  position: absolute;
  flex-direction: column;
  background-color: bisque;
  border-radius: 8px;
  justify-content: center;
  align-items: center;
  top: 20%;
  left: 30%;
  bottom: 20%;
  right: 30%;
  min-height: 10rem;
  overflow: hidden;

  &.show {
    display: flex;
  }

  h1, label {
    color: black;
  }

  .task {
    display: flex;
    justify-content: space-evenly;
    border: 1px solid black;
    color: black;
    margin: 0 1rem;

    * {
      width: 25%;
    }
  }

  #existing-tasks {
    height: 60%;
    overflow-y: auto;
  }

  #new-task {
    margin-top: 2rem;
    margin-right: 1rem;
    border: none;
  }
}
</style>
