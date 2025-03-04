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

      <div id="existing-tasks">
        <div
          class="task"
          v-for="task in tasksStore.tasks"
        >
          <p>{{ task.name }}</p>
          <p>{{ task.importance || 0 }}</p>
          <p>{{ task.intensity || 0 }}</p>
          <p>(delete button)</p>
        </div>
      </div>

      <p style="color: red">todo: Add new task</p>

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

    margin: 0 0 0 1rem;

    &#task-header {
      margin: 0 1rem;
    }

    * {
      width: 25%;
    }
  }

  #existing-tasks {
    height: 60%;
    overflow-y: auto;
  }
}
</style>
