<script setup lang="ts">
import { CalendarEventModel } from '../../models/CalendarEventModel.ts';
import { calcEventStyle } from '../../helpers/dynCalendarEventOffset.ts';
import { showEditEventModal } from '../../modalController.ts';
import { useEventStore } from '../../stores/CalendarStore.ts';
</script>

<template>
  <div
    class="calendar-event"
    :style="calcEventStyle($props.calendarEventModel)"
    @click.stop="eventClicked()"
  >
    <p>{{calendarEventModel.name}}</p>
  </div>
</template>

<script lang="ts">
export default {
  props: {
    calendarEventModel: {
      type: CalendarEventModel,
      required: true
    }
  },
  methods: {
    eventClicked() {
      const calendarStore = useEventStore();
      calendarStore.selectedEvent = this.calendarEventModel;

      showEditEventModal();
    }
  }
}
</script>

<style lang="scss">
.calendar-event {
  position: absolute;
  height: 5rem;
  background-color: lime;
  overflow: hidden;
  overflow-wrap: break-word;
  cursor: pointer;

  p {
    color: black;
    font-size: 0.75rem;
    margin: 0;
  }
}
</style>
