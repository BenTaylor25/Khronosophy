<script setup lang="ts">
import { MODAL_IDS } from '../../constants/modalConstants.ts';
import { hideScheduleModal } from '../../modalController.ts';
import { refreshEvents } from '../../stores/CalendarStore.ts';
import { apiUTMTKSchedule } from '../../api/Scheduling/UTMTKSchedule.ts';
import { apiETFSchedule } from '../../api/Scheduling/ETFSchedule.ts';

import ModalShadow from './ModalShadow.vue';
import { apiClearScheduledEvents } from '../../api/Scheduling/clearScheduledEvents.ts';
</script>

<template>
  <modal-shadow
    :id="MODAL_IDS.SCHEDULE_MODAL"
    @click="hideScheduleModal()"
  >

    <div
      id="schedule-modal-content"
      @click.stop
    >

      <h1>Schedule</h1>

      <button
        id="reset-button"
        @click="clearScheduledEvents()"
      >
        Reset Tasks
      </button>

      <div id="algorithms">

        <!-- There is little value adding this right now. -->
        <!-- <button
          id="dumb-scheduler"
        >
          Dumb Scheduler
        </button> -->

        <button
          id="etf-scheduler"
          @click="scheduleETF()"
        >
          ETF Scheduler
        </button>

        <button
          id="utmtk-scheduler"
          @click="scheduleUTMTK()"
        >
          UTMTK Scheduler
        </button>

      </div>

    </div>

  </modal-shadow>
</template>

<script lang="ts">
async function clearScheduledEvents() {
  await apiClearScheduledEvents();
  refreshEvents();
  hideScheduleModal();
}

async function scheduleETF() {
  await apiETFSchedule();
  refreshEvents();
  hideScheduleModal();
}

async function scheduleUTMTK() {
  await apiUTMTKSchedule();
  refreshEvents();
  hideScheduleModal();
}
</script>

<style scoped lang="scss">
#schedule-modal-content {
  position: absolute;
  flex-direction: column;
  background-color: bisque;
  border-radius: 8px;
  justify-content: center;
  align-items: center;
  top: 30%;
  left: 20%;
  bottom:30%;
  right:20%;
  min-height: 10rem;

  &.show {
    display: flex;
  }

  h1, label {
    color: black;
  }

  button {
    padding: 1rem;
    margin: 1rem;
    background-color: darkcyan;
    cursor: pointer;

    &#reset-button {
      background-color: darkred;
    }
  }
}
</style>
