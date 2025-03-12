<script setup lang="ts">
import {
  showZoomSettingsModal,
  showYearViewModal,
  showTasksModal,
  showScheduleModal
} from '../modalController.ts';
import { useDateStore } from '../stores/DateStore.ts';

const dateStore = useDateStore();
</script>

<template>
  <div id="header">

    <div id="controls">

      <div
        id="today"
        @click="dateStore.jumpToToday()"
      >
        <p>Today</p>
      </div>

      <div
        id="year-view"
        @click="showYearViewModal()"
      >
        <p>Year</p>
      </div>

    </div>

    <div id="central">

      <div
        id="tasks"
        @click="showTasksModal()"
      >
        <p>Tasks</p>
      </div>

      <div
        id="schedule"
        @click="showScheduleModal()"
      >
        <p>Schedule</p>
      </div>

    </div>

    <div id="settings">

      <div
        id="zoom-setting"
        @click="showZoomSettingsModal"
      >
        <p>Zoom</p>
      </div>

    </div>

  </div>
</template>

<style lang="scss">

#header {
  display: flex;
  align-items: center;
  justify-content: space-between;

  #controls, #central, #settings {
    display: flex;

    * {
      height: 50%;
      background: green;
      padding: 0.1rem 0.3rem;
      margin: 1rem;
      cursor: pointer;
    }
  }

  #controls {
    margin-left: 1rem;
  }

  #settings {
    margin-right: 1rem;
  }
}

</style>
