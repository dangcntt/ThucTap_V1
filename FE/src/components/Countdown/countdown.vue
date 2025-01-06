<template>
  <div class="countdown">
    <div class="countdown__block">
      <div v-if="check" class="countdown__digit">{{ minutes | twoDigits }}</div>
      <div v-else class="countdown__digit">00</div>
    </div>
    <div class="countdown__block">
      <div v-if="check" class="countdown__digit">{{ seconds | twoDigits }}</div>
      <div v-else class="countdown__digit">00</div>
    </div>
  </div>
</template>
<script>
import {mapState} from "vuex";
import moment from "moment";

export default {
  props: {
    date: null,
  },
  data () {
    return {
      now: Math.trunc((new Date()).getTime() / 1000),
      event: this.date,
      finish: false,
    }
  },
  created() {

    console.log(this.event)
  },
  mounted () {

    const _self = this
    if(!this.isFinish){
      window.setInterval(() => {
        this.now = Math.trunc((new Date()).getTime() / 1000)
        if (!this.finish && this.calculatedDate - this.now <= 0) {
          _self.finish = true
          _self.$emit('onFinish')
          console.log("kiet dep trai")
        }
      }, 1000)
    }

  },
  computed: {
    check(){
      return this.event != null? true: false;
    },
    secondCount () {
      if(this.isFinish){
        return Math.trunc(Date.parse(new Date()) / 1000)- this.now;
      }
      return this.calculatedDate - this.now
    },
    calculatedDate () {
      return Math.trunc(Date.parse(this.event) / 1000)
    },
    seconds () {
      if (this.secondCount < 0) return 0
      return this.secondCount % 60
    },
    minutes () {
      if (this.secondCount < 0) return 0
      return Math.trunc(this.secondCount / 60) % 60
    },
    hours () {
      if (this.secondCount < 0) return 0
      return Math.trunc(this.secondCount / 60 / 60) % 24
    },
    // ...mapState('pdfEditorStore', ['isFinish'])
  },
  filters: {
    twoDigits (value) {
      if (value.toString().length <= 1) {
        return '0' + value.toString()
      }
      return value.toString()
    }
  },
  watch: {
    // whenever question changes, this function will run
    date(newValue, oldValue) {
     this.event = newValue;
    }
  },
}
</script>
<style lang="scss">
.countdown{
  display: flex;
  justify-content: center;
  &__block {
    text-align: center;
    padding: 0px 15px;
    position: relative;
    &:first-child{
      padding-left: 0;
      .countdown__digit{
        &:before{
          display: none;
        }
      }
    }
    &:last-child{
      padding-right: 0;
    }
  }
  &__text {
    text-transform: uppercase;
    margin-bottom: 5px;
  }
  &__digit {
    font-size: 32px;
    font-weight: bold;
    line-height: 1;
    margin-bottom: 5px;
    color: #045E90;
    &:before{
      content: ":";
      position: absolute;
      left: -5px;
    }
  }
}
</style>
