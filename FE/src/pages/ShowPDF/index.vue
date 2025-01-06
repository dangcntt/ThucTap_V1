<template>
  <div v-if="pdfSrc">
    <PDFViewer
        :source="pdfSrc"
        style="height: 100vh; width: 100vw"
        @download="handleDownload"
    />
  </div>
</template>

<script>
import PDFViewer from 'pdf-viewer-vue/dist/vue2-pdf-viewer'
export default {
  components:{PDFViewer},
  data() {
    return {
      title: "Xem",
      apiUrl: process.env.VUE_APP_API_URL,
      files: null,
      pdfSrc: null
    };
  },
  created(){
    this.pdfSrc =this.apiUrl + "filesminio/view/" + this.$route.params.id;
    //  this.viewFile();
  },
  methods:{
    handleDownload(source, filename){
      const url =this.apiUrl +"filesminio/view/" +this.$route.params.id;
      window.location.href = url;
    },

  }
}
</script>

<style scoped>

</style>
