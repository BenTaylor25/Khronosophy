<script setup lang="ts">
import { MODAL_IDS } from '../../constants/modalConstants';
import { hideTasksModal } from '../../modalController';
import { useTasksStore } from '../../stores/TasksStore';

import ModalShadow from './ModalShadow.vue';

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
            :value="task.importance || 0"
            min="0"
            max="10"
          />
          <input
            type="number"
            :value="task.intensity || 0"
            min="0"
            max="10"
          />
          <button>Delete</button>
        </div>
      </div>

      <!-- New Task -->
      <div id="new-task" class="task">
        <input type="text" />
        <input type="number" min="0" max="10" />
        <input type="number" min="0" max="10" />
        <button>Create New</button>
      </div>

    </div>

  </modal-shadow>

</template>

<script lang="ts">
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
