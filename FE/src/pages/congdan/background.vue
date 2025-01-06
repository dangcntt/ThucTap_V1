<script>
import {Carousel, Slide} from "vue-carousel";
import Layout from "@/pages/congdan/layout2";
import {required} from "vuelidate/lib/validators";
import Multiselect from "vue-multiselect";
import {notifyModel} from "@/models/notifyModel";
import vue2Dropzone from "vue2-dropzone";
// import the component
import Treeselect from "@riophae/vue-treeselect";
// import the styles
import "@riophae/vue-treeselect/dist/vue-treeselect.css";
import Vue from "vue";
import {mapState} from "vuex";
import moment from "moment";
import 'vue-slick-carousel/dist/vue-slick-carousel.css'
import 'vue-slick-carousel/dist/vue-slick-carousel-theme.css'
import flatpickr from "flatpickr";
import flatPickr from "vue-flatpickr-component";

export default {
  components: {
    Layout,
  },
  data() {
    return {
      apiView: `${process.env.VUE_APP_API_URL}filesminio/view/`,
      url: `${process.env.VUE_APP_API_URL}filesminio/view`,
      ImagesShow: [],
      totalRows: 1,
      numberOfElement: 1,
      perPage: 92,
      currentPage: 1,
      sortBy: "age",
      sortDesc: false,
      filterOn: [],
      showModal: true,
      checked: false,
      submitted: false,
      currentPlace: null,
      showCurrentPlace: true,
      selection: "all",
      visible: false,
      hoa:[],
    };
  },

  computed: {
    ...mapState('authStore', ['token']),

    //Validations
  },
  validations() {
    return {
      model: this.rules
    }
  },
  created() {
    this.getHoa();
  },
  destroyed() {
    window.removeEventListener("scroll", this.windowScroll);
  },
  mounted() {
  },
  watch: {
    token: {
      deep: true,
      handler(val) {
        this.tokenCurrent = val;
      }
    },
    perPage: {
      deep: true,
      handler(val){
        this.getHoa();
      }
    },
    currentPage:{
      deep: true,
      handler(val){
     //   console.log("this.perpage", this.currentPage);
        this.getHoa();
      }
    }
  },

  methods: {
    async getHoa(){
      const params = {
        start: this.currentPage,
        limit: this.perPage,
        sortBy : "Sort",
        sortDesc: true,
      };
      await this.$store.dispatch("hoaStore/getPagingParams", params).then((res) => {
        this.hoa = res.data.data;
        this.totalRows = res.data.totalRows;
      })
    },

    toggleMenu() {
      document.getElementById("topnav-menu-content").classList.toggle("show");
    },
    nextSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getNextPage());
    },
    prevSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getPreviousPage());
    },
    onFiltered(filteredItems) {
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },
    openNewTab(event) {
      const selectedValue = event.target.value;
      if (selectedValue !== "") {
        window.open(selectedValue, '_blank');
      }
    },
  },
};
</script>
<template>
  <Layout>
    <section class="section pt-4" id="home" style="padding-bottom: 0px !important;">
      <div class="row header" style="margin-top: 26px;">
        <div class="col-12">
          <div class="card">
            <div class="">
              <img src="@/assets/images/BANNER.png" alt="" style="width: 100%;">
            </div>
          </div>
        </div>
      </div>
      <div class="container" style="padding: 0 50px;">
        <b-row>
          <div class="wrap-one">
            <b-col md="12">
              <div class="col-lg-12">
                <h1 style="font-weight: bold; color: #000;">DANH SÁCH CÁC LOẠI HOA</h1>
              </div>
              <div class="col-lg-4">
              </div>
            </b-col>
          </div>
          <div class="wrap-main">
            <div class="row">
              <div class="col-lg-12">
                  <section>
                    <div class="row">
                      <div class="col-lg-12 col-md-12 col-sm-12" v-for="(item, index) in hoa"
                            :key="index">
                        <div
                            class="row"
                        >
                          <div class="col-md-12 mb-4">
                            <b-card no-body class="card-box">
                              <b-row no-gutters class="align-items-center card-intro">
                                <b-col md="12" style="position: relative;">
                                  <router-link
                                        :to="{
                                            path: `/hoa/chi-tiet/${item.code}`,
                                            }"
                                    >
                                    <div v-if="!item.avatar">
                                      <div class="float-left">
                                        <img
                                            src="@/assets/images/logo-con.png"
                                            alt="Không có ảnh"
                                            class="w-100"
                                            style="height: 1100px; padding: 10px; border-top-right-radius: 5px !important; border-top-left-radius: 5px !important;"
                                        >
                                      </div>
                                    </div>
                                    <div v-else>
                                        <b-card-img
                                            :src="apiView + item.avatar.fileId"
                                            class="img-hoa"
                                            style="height: 1100px; border-top-right-radius: 5px !important; border-top-left-radius: 5px !important;"
                                        ></b-card-img>
                                    </div>
                                  </router-link>
                                  <b-card-img
                                      :src="apiView + item.qrCode2.fileId"
                                      class="img-qrcode"
                                      style="position: absolute; top: 0; width: 30%; opacity: 0.7; border-bottom-right-radius: 5px !important; "
                                  ></b-card-img>
                                </b-col>
                                <b-col md="12" class="title-category">
                                  <router-link
                                        :to="{
                                            path: `/hoa/chi-tiet/${item.code}`,
                                            }"
                                    >
                                    <b-card-body class="" :title="`${item.name}`" style="color: #000 !important;">
                                      <!-- <p class="card-text">
                                        <small class="fs-13" style="text-align: justify;">
                                          {{item.content}}
                                        </small>
                                      </p> -->
                                      <p>
                                        <span style="font-weight: bold; color: #000;">Tên khoa học:</span>
                                        <span style="margin-left: 10px;">{{ item.engName }}</span>
                                      </p>
                                      <p style="
                                        text-align: justify;
                                        color: #000;
                                        display: -webkit-box !important;
                                        -webkit-line-clamp: 3;
                                        -webkit-box-orient: vertical;
                                        overflow: hidden;">
                                        {{ item.moTa }}
                                      </p>

                                      <b-button
                                          pill
                                          class="px-4 fs-13"
                                          size=""
                                          style="
                                            background-color:#efc62c;
                                            color: #000 !important;
                                            border: none;
                                            "
                                      >
                                        Chi tiết
                                        <i class="mdi mdi-arrow-right ps-2"></i>
                                      </b-button>
                                    </b-card-body>
                                  </router-link>
                                </b-col>
                              </b-row>
                            </b-card>
                          </div>
                        </div>
                      </div>
                    </div>
                  </section>
                  <div class="row mb-3">
                    <div class="col">
                      <div
                          class="dataTables_paginate paging_simple_numbers float-end"
                      >
                        <ul class="pagination pagination-rounded mb-0">
                          <!-- pagination -->
                          <b-pagination
                              v-model="currentPage"
                              :total-rows="totalRows"
                              :per-page="perPage"
                              class="customPagination"
                          ></b-pagination>
                        </ul>
                      </div>
                    </div>
                  </div>
                </div>
            </div>
          </div>
        </b-row>

      </div>
    </section>
  </Layout>
</template>


<style>

.img-hoa{
  object-fit: cover;
}

.card-title{
  position: absolute;
  top:-128px;
  font-size: 90px;
  background-color: rgb(216, 38, 110, 0.7);
  color: #fff;
  padding: 10px;
  border-radius: 10px;
}
.custom-content {
    display: -webkit-box!important;
    -webkit-line-clamp: 4;
    -webkit-box-orient: vertical;
    overflow: hidden;
}

.fs-20{
  font-size: 20px;
}

.pd-10{
  padding: 10px;
}

.td-stt {
  width: 50px;
}

.td-content {
  width: 50px;
}

.card{
  margin-bottom: 0px !important;
}

.title-category{
  padding: 10px 25px;
}
.card-body{
  padding: 0px !important;
}
.card-body p{
  margin-bottom: 5px;
}

.mt-175{
  margin-top: 175px;
}

.wrap-one{
  padding-top: 20px;
  float: left;
}

.wrap-main{
  padding-top: 20px;
}

.link-pdf{
  margin-top: 5px;
}

.img-kq{
  margin: 10px 0;
}
.img-kq img{
  width: 100%;
}

.card-card{
  margin-top: 10px;
}

.card-title-cs{
  padding: 10px;
  color: #fff;
  text-align: center;
  margin: 0;
  text-transform: uppercase;
}


.border{
  border: 1px solid #ccc !important;
}

.sample img{
  width: 100%;
  height: 450px;
}

.sample{
  margin-bottom: 10px;
  border-radius: 10px;
  box-shadow: rgba(17, 17, 26, 0.1) 0px 0px 5px;
}

.card-title-news{
  text-align: center;
}

.news{
  padding: 10px;
}

.block-post{
  margin-top: 20px;
  text-align: justify;
}

.block-left{
  border-right: 1px solid #ccc;
}

.lib-img img{
  border-radius: 10px;
  width: 100%;
  height: 200px;
}
.lib-img{
  display: flex;
  justify-content: center;

}


.lib img{
	transform: scale(1);
	transition: .3s ease-in-out;
}

.lib:hover img{
  -webkit-transform: scale(1.1);
	transform: scale(1.1);
  opacity: 0.7;
}

.decs{
  position: absolute;
  color: #fff;
  font-size: 13px;
  display: flex;
  font-weight: bold;
  top:-25px;
}

.decs p{
  padding: 5px;
  text-align: center;
  transform: translateY(40px);
	transition: 0.5s;
	opacity: 0;
  z-index: 1;
  background-color: #ec232b;
  border-radius: 10px;
}
.lib:hover .decs p{
  transform: translateY(0px);
	opacity: 1;
}


.title-search{
  width: 100%;
  text-align: center;
}

.title-search h3{
  color: #ec232b;
}


@container card (min-width: 380px) {
  .article-wrapper {
    display: grid;
    grid-template-columns: 100px 1fr;
    gap: 16px;
  }
  .article-body {
    padding-left: 0;
  }
  figure {
    width: 100%;
    height: 100%;
    overflow: hidden;
  }
  figure img {
    height: 100%;
    aspect-ratio: 1;
    object-fit: cover;
  }
}

.sr-only:not(:focus):not(:active) {
  clip: rect(0 0 0 0);
  clip-path: inset(50%);
  height: 1px;
  overflow: hidden;
  position: absolute;
  white-space: nowrap;
  width: 1px;
}

.img-lienket img{
  width: 100%;
  height: 150px;
}

.img-tintuc{
  border-radius: 20px !important;
}

.title-news{
  display: flex;
  align-items: center;
}
.news-title{
  text-align: justify;
}

.card-noti{
  max-height: 200px;
  overflow: scroll;
  overflow-x: hidden !important;
}

.cate-list-noti{
  padding: 10px;
}

.cate-list-noti ul li{
  list-style: none;
  padding: 10px 0 10px 0;
}

@media (max-width: 768px){
    .lib{
      margin-bottom: 10px;
    }

    .img-qrcode{
      width: 70px !important;
      height: 70px !important;
    }
}

</style>
