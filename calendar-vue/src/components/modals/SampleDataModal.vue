<script setup lang="ts">
import { MODAL_IDS } from '../../constants/modalConstants.ts';
import { hideSampleDataModal } from '../../modalController.ts';

import ModalShadow from './ModalShadow.vue';
</script>

<template>
  <modal-shadow
    :id="MODAL_IDS.SAMPLE_DATA_MODAL"
    @click="hideSampleDataModal()"
  >
    <div
      id="sample-data-modal-content"
      @click.stop
    >

      <button
      >
        Load Data
      </button>

      <button
        id="reset-button"
      >
        Reset Data
      </button>

    </div>
  </modal-shadow>
</template>

<script lang="ts">

</script>

<style scoped lang="scss">
#sample-data-modal-content {
  position: absolute;
  flex-direction: column;
  background-color: bisque;
  border-radius: 8px;
  display: flex;
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
