<script>
import Layout from "@/pages/congdan/layout2";
import Multiselect from "vue-multiselect";
import VueEasyLightbox from "vue-easy-lightbox";
import Carousel from 'vue-slick-carousel';

/**
 * Crypto ICO-landing page
 */
export default {
  components: {Layout, VueEasyLightbox},
  data() {
    return {
      data: [],
      start: 2,
      // url: `${process.env.VUE_APP_API_URL}files/view`,
      totalRows: 1,
      numberOfElement: 1,
      perPage: 9,
      currentPage: 1,
      sortBy: "age",
      sortDesc: false,
      model:[],
      filterOn: [],
      pageOptions: [5,10,25,50,100],
      url: `${process.env.VUE_APP_API_URL}filesminio/view`,
      apiView: `${process.env.VUE_APP_API_URL}filesminio/view/`,
      hoa:[],
      keyFile: 0,
      relink:[],
      doanhnghiep:[],
      indexImage: 0,
      visible: false,
      listPicture: [],
      ImagesShow: [],
      index: 0,
      slickOptions: {
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 1000,
      },
    };
  },
  computed: {

  },
  watch: {

  },
  created() {
    console.log("LOG CREATED LIEN HE ")
    window.addEventListener("scroll", this.windowScroll);
    this.getHoa();
    this.getDoanhNghiep();
  },
  destroyed() {
    window.removeEventListener("scroll", this.windowScroll);
  },
  mounted() {
    this.start = new Date(this.starttime).getTime();
    this.end = new Date(this.endtime).getTime();
    // Update the count down every 1 second
  },
  methods: {
    handleShowRegisterModal(){
      this.$store.dispatch("snackBarStore/showRegisterModal", !this.$store.state.snackBarStore.registerModal)
      console.log("abc")
    },
    onFiltered(filteredItems) {
      // Trigger pagination to update the number of buttons/pages due to filtering
      this.totalRows = filteredItems.length;
      this.currentPage = 1;
    },
    /**
     * Window scroll method
     */
    windowScroll() {
      const navbar = document.getElementById("navbar");
      if (
          document.body.scrollTop >= 50 ||
          document.documentElement.scrollTop >= 50
      ) {
        navbar.classList.add("nav-sticky");
      } else {
        navbar.classList.remove("nav-sticky");
      }
    },
    /**
     * Toggle menu
     */
    toggleMenu() {
      document.getElementById("topnav-menu-content").classList.toggle("show");
    },
    nextSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getNextPage());
    },
    prevSlide() {
      this.$refs.carousel.goToPage(this.$refs.carousel.getPreviousPage());
    },
    normalizer(node) {
      if (node.children == null || node.children == 'null') {
        delete node.children;
      }
    },
    async getHoa(){
      await this.$store.dispatch("hoaStore/getByCode", this.$route.params.code).then((res) => {
        this.hoa = res.data;
        res.data.files.forEach((elements) => {
          this.listPicture.push(elements)
          this.ImagesShow.push(this.apiView + elements?.fileId)
            }
        )
      })
    },
    showImg(indexImage) {
      this.keyFile = indexImage;
      this.visible = true;
    },
    handleHide() {
      this.visible = false;
    },
    getId(id){
      this.relink.push(process.env.VUE_APP_API_URL + 'filesminio/view/' + id)
      this.showPreview();
    },
    showPreview() {
      const preview = this.$imagePreview({
        initIndex: 0,
        images:this.relink,
        isEnableBlurBackground: false,
        isEnableLoopToggle: true,
        initViewMode: "contain",
        containScale: 1,
        shirnkAndEnlargeDeltaRatio: 0.2,
        wheelScrollDeltaRatio: 0.2,
        isEnableImagePageIndicator: true,
        maskBackgroundColor: "rgba(0,0,0,0.6)",
        zIndex: 4000,
        isEnableDownloadImage: true
      })
      this.relink=[]
    },
    async getDoanhNghiep(){
      const params = {
        start: this.currentPage,
        limit: this.perPage,
        sortDesc: true,
      };
      await this.$store.dispatch("doanhNghiepStore/getPagingParams", params).then((res) => {
        console.log("DOANH NGHIEP: ", res.data.data)
        this.doanhnghiep = res.data.data;
      })
    },

  },
};
</script>

<template>
  <Layout>
    <section class="section p-2 bg-white" id="about">
        <div class="row align-items-center">
          <div>
            <div class="row">
              <div class="col-12">
                <div class="row">
                  <div class="col-lg-12">
                    <div class="container">
                      <div class="row">
                        <div class="col-md-12" style="margin-top: 70px;">
                          <div class="row">
                            <div class="col-md-6 col-sm-6 mt-4">
                              <div class="popup-gallery d-flex flex-wrap justify-content-center">
                                <div
                                    class="img-box w-100"
                                    @click="() => showImg(indexImage)"
                                >
                                  <b-card-img
                                      :src="apiView + listPicture[indexImage]?.fileId"
                                      class="img-thumbnail mx-auto d-block cs-img-thumbnail"
                                      style="background-color: #fff; border: none; border-radius: 20px; max-height: 600px;"
                                  >
                                  </b-card-img>
                                  <div class="row mt-2">
                                    <div class="carousel-img col-md-3 col-sm-3" v-for="(src, index) in listPicture" :key="index">
                                      <VueSlickCarousel>
                                          <b-card-img
                                              :src="apiView + src?.fileId"
                                              class="cs-vueslickcarousel"
                                              style="height: 150px; border-radius: 15px;"
                                          >
                                          </b-card-img>
                                      </VueSlickCarousel>
                                    </div>
                                  </div>
                                </div>
                                <vue-easy-lightbox
                                    :visible="visible" :imgs="ImagesShow" :index="keyFile"
                                    @hide="handleHide">
                                </vue-easy-lightbox>
                              </div>
                              </div>

                            <div class="col-md-6 col-sm-6" style="position: relative;">
                              <div class="mt-4 mb-2">
                                <h3 style="color: rgb(216 38 110);">
                                  <i class="bx bx-book"></i>
                                    Mô tả chi tiết
                                </h3>
                              </div>
                              <div class="chitiet" style="border-bottom: 1px dotted #ccc;">
                                <div class="item-name" style="font-size: 15px;">
                                  <span>Tên gọi:</span>
                                  <span style="font-weight: bold; margin-left: 10px;">{{ this.hoa.name }}</span>
                                </div>
                                <div class="item-name" style="font-size: 15px;">
                                  <span>Tên khoa học:</span>
                                  <span style="font-weight: bold; margin-left: 10px;">{{ this.hoa.engName }}</span>
                                </div>
                                <!-- <div class="item-name" style="font-size: 15px;">
                                  <span>Mô tả:</span>
                                  <span style="font-weight: bold; margin-left: 10px;">...</span>
                                </div> -->
                              </div>
                              <div class="mt-4 mb-2">
                                <h3 style="color: rgb(216 38 110);">
                                  <i class="bx bxs-tree"></i>
                                    Cây giống
                                </h3>
                              </div>
                              <div class="chitiet" style="border-bottom: 1px dotted #ccc;">
                                <div v-html="hoa.cayGiong"></div>
                              </div>
                              <div class="mt-4 mb-2">
                                <h3 style="color: rgb(216 38 110);">
                                  <i class="bx bx-bug-alt"></i>
                                    Sâu bệnh chính
                                </h3>
                              </div>
                              <div class="chitiet" style="border-bottom: 1px dotted #ccc;">
                                <div v-html="hoa.sauBenh"></div>
                              </div>
                              <b-card-img
                                    :src="apiView + hoa.qrCode2?.fileId"
                                    class="rounded-0"
                                    style="position: absolute;right: 10px; top: 10px; width: 100px; height: 100px;"
                                ></b-card-img>
                              <!-- <img src="@/assets/images/qrCode.jpg" alt="" width="100px" style="position: absolute;right: 10px; top: 10px;"> -->
                            </div>
                          </div>
                        </div>

                        <div class="col-md-12">
                          <div class="row mt-2">
                            <div class="col-md-6">
                              <div class="card-body card-box">
                                  <h4 style="padding: 20px;">Quá trình canh tác</h4>

                                  <div>
                                    <ul class="verti-timeline list-unstyled" style="padding-left: 28px; margin: 0 15px !important;">
                                      <li class="event-list" style="padding: 0px !important;">
                                        <div class="event-timeline-dot">
                                          <i class="bx bx-right-arrow-circle"></i>
                                        </div>
                                        <div class="d-flex">
                                          <div class="me-3">
                                            <i class="bx bxs-tree h2 text-primary"></i>
                                          </div>
                                          <div class="flex-grow-1">
                                            <div>
                                              <h5 class="font-size-14" style="color: #000;">GIÁ THỂ</h5>
                                              <p class="text-muted">
                                                {{ this.hoa.giaThe }}
                                              </p>
                                            </div>
                                          </div>
                                        </div>
                                      </li>
                                      <li class="event-list" style="padding: 0px !important;">
                                        <div class="event-timeline-dot">
                                          <i class="bx bx-right-arrow-circle"></i>
                                        </div>
                                        <div class="d-flex">
                                          <div class="me-3">
                                            <i class="bx bx-copy-alt h2 text-primary"></i>
                                          </div>
                                          <div class="flex-grow-1">
                                            <div>
                                              <h5 class="font-size-14" style="color: #000;">TRỒNG CHẬU</h5>
                                              <p class="text-muted">
                                                {{ this.hoa.trongChau }}
                                              </p>
                                            </div>
                                          </div>
                                        </div>
                                      </li>

                                      <li class="event-list" style="padding: 0px !important;">
                                        <div class="event-timeline-dot">
                                          <i class="bx bx-right-arrow-circle"></i>
                                        </div>
                                        <div class="d-flex">
                                          <div class="me-3">
                                            <i class="bx bxs-color-fill h2 text-primary"></i>
                                          </div>
                                          <div class="flex-grow-1">
                                            <div>
                                              <h5 class="font-size-14" style="color: #000;">TƯỚI NƯỚC</h5>
                                              <p class="text-muted">
                                                {{ this.hoa.tuoiNuoc }}
                                              </p>
                                            </div>
                                          </div>
                                        </div>
                                      </li>
                                      <li class="event-list" style="padding: 0px !important;">
                                        <div class="event-timeline-dot">
                                          <i class="bx bx-right-arrow-circle"></i>
                                        </div>
                                        <div class="d-flex">
                                          <div class="me-3">
                                            <i class="bx bx-shape-square h2 text-primary"></i>
                                          </div>
                                          <div class="flex-grow-1">
                                            <div>
                                              <h5 class="font-size-14" style="color: #000;">BÓN PHÂN</h5>
                                              <p class="text-muted">
                                                {{ this.hoa.bonPhan }}
                                              </p>
                                            </div>
                                          </div>
                                        </div>
                                      </li>
                                      <li class="event-list" style="padding: 0px !important;">
                                        <div class="event-timeline-dot">
                                          <i class="bx bx-right-arrow-circle"></i>
                                        </div>
                                        <div class="d-flex">
                                          <div class="me-3">
                                            <i class="bx bx-cut h2 text-primary"></i>
                                          </div>
                                          <div class="flex-grow-1">
                                            <div>
                                              <h5 class="font-size-14" style="color: #000;">CẮT TỈA</h5>
                                              <p class="text-muted">
                                                {{ this.hoa.catTia }}
                                              </p>
                                            </div>
                                          </div>
                                        </div>
                                      </li>
                                    </ul>
                                  </div>
                                </div>
                            </div>
                            <div class="col-md-6 mb-3">
                              <div class="card-body row" style="box-shadow: 0 0.75rem 1.5rem rgb(18 38 63 / 17%) !important;">
                                <div class="col-md-12" style="height: 100%;">
                                  <h4 style="padding: 10px; color: rgb(216 38 110);">Quá trình chăm sóc của cây</h4>
                                </div>
                                <div class="col-md-6 mb-2" v-for="(item, index) in this.hoa.chamSoc" :key="index">
                                  <b-card no-body class="mb-2" style="box-shadow: 0 0.75rem 1.5rem rgb(18 38 63 / 17%) !important;">
                                    <b-row no-gutters class="align-items-center card-intro">
                                      <b-col md="12" style="position: relative;">
                                          <div v-if="!item.file?.fileId">
                                            <div class="float-left">
                                              <img
                                                  src="@/assets/images/logo-con.png"
                                                  alt="Không có ảnh"
                                                  class="w-100"
                                                  style="object-fit:contain; height: 175px; padding: 10px; border-top-right-radius: 5px !important; border-top-left-radius: 5px !important;"
                                              >
                                            </div>
                                          </div>
                                          <div v-else>
                                              <b-card-img
                                                  :src="apiView + item.file?.fileId"
                                                  class=""
                                                  style="object-fit:contain; height: 175px; border-top-right-radius: 5px !important; border-top-left-radius: 5px !important;"
                                              ></b-card-img>
                                          </div>
                                      </b-col>
                                      <b-col md="12" style="padding: 5px 25px;" class="title-category">
                                          <b-card-body>
                                            <p style="text-align: center; margin-bottom: 0px;">
                                              <span style="font-weight: bold; color: #000;">{{ item.name }}</span>
                                            </p>
                                          </b-card-body>
                                      </b-col>
                                    </b-row>
                                  </b-card>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                        <!-- <div>
                          <Carousel :options="slickOptions">
                            <div class="items" v-for="(item, index) in this.hoa.files" :key="index">
                              <img :src="apiView + item.fileId" />
                            </div>
                          </Carousel>
                        </div> -->
                        <div class="col-md-12 mb-3">
                          <div class="title mt-4 mb-4">
                            <h2>
                                {{ this.hoa.name }}
                            </h2>
                          </div>
                          <div v-html="hoa.noiDung" class="noidung" style="font-size: 14px; text-align: justify;">
                          </div>
                        </div>
                      </div>

                      <div class="row d-flex align-items-baseline mb-3">
                        <div class="col-6 position-relative pt-4">
                          <a href="https://festivalhoa.dongthap.gov.vn/"
                              class="back"
                          >
                            <i class="bx bx-left-arrow-alt me-2 text-white"></i>
                            Quay lại
                          </a>
                        </div>
                        <div class="col-6 position-relative pt-4" style="font-size: 16px; text-align: right;">
                          <span style="font-weight: bold; color: #000;">Số lượt xem:</span>
                          <span style="margin-left: 10px;">{{ this.hoa.view }}</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <div class="container">

              <b-row>
                <div class="wrap-one mt-2">
                  <b-col md="12">
                    <div class="col-lg-12">
                      <h3 style="font-weight: bold; color: #000;">DANH SÁCH CỞ SỞ MUA BÁN HOA KIỂNG</h3>
                    </div>
                    <div class="col-lg-4">
                    </div>
                  </b-col>
                  <div class="row">
                    <div class="col-lg-4 col-md-6 mb-3" v-for="(item,index) in doanhnghiep" :key="index">
                      <a :href="item.link" target="_blank">
                        <div class="card-bg">
                          <div class="card-addres" style="color: #fff;">
                            <i class="mdi mdi-arrow-right-bold">Xem bản đồ</i>
                          </div>
                          <div class="card-dn">
                            <div class="name-dn" style="font-weight: bold; font-size: 15px;">
                              {{ item.name }}
                            </div>
                            <div class="add-dn" style="font-size: 13px;">
                              {{ item.diaChi }}
                            </div>
                            <div class="sdt-dn" style="font-size: 13px;">
                              Điện thoại: {{ item.sdt }}
                            </div>
                          </div>
                        </div>
                      </a>
                    </div>
                  </div>
                </div>
              </b-row>
            </div>
          </div>
        </div>
    </section>
  </Layout>
</template>
<style>

/* .chitiet p{
  margin-bottom: 5px !important;
} */
.items{
  width: 350px !important;
  height: 250px !important;
  margin: 50px !important;
}
.items img{
  width: 350px !important;
  height: 300px !important;
}
.slick-prev{
  color: #000 !important;
  left: -25px !important;
  z-index: 99;
}
.slick-next{
  color: #000 !important;
  right: -25px !important;
  z-index: 99;
}

.slick-slide{
  width: 500px !important;
}
.slick-slide img{
  width: 100% !important;
  height: 100% !important;
}

.card-bg{
  background-color: #00b29a;
  position: relative;
  height: 150px;
  border-radius: 20px;
  margin: 20px;
}

.card-dn{
  position: absolute;
  background-color: #f2f2f2;
  border-radius: 10% 13% 0% 34% / 0% 34% 20% 60%;
  padding: 25px;
  right: -20px;
  top: -20px;
  width: 100%;
  height: 150px;
  box-shadow: 0 0.75rem 1.5rem rgb(18 38 63 / 17%) !important
}

.card-addres{
  position: absolute;
  right: 20px;
  bottom: 0;
}

.name-dn, .add-dn, .sdt-dn{
  text-align: right;
  color: #000;
}

.chitiet{
  font-size: 15px;
  color: #000;
  text-align: justify;
}
.event-timeline-dot{
  left: -40px !important;
  font-size: 20px !important;
}
.img-chitiet{
  border-radius: 10px;
}

.carousel-img img{
  object-fit: cover;
}

.img-box img{
  object-fit: cover;
}
@media only screen and (max-width: 576px){
  .img-con{
    width: 100px;
  }
  .img-chitiet{
    width: 90px !important;
  }
}

@media only screen and (max-width: 768px){
  .card-img{
    height: 300px;
  }
  .img-chitiet{
    height: 100px;
    width: 65px;
  }
}

@media only screen and (max-width: 1200px){
  .carousel-img{
    width: 25% !important;
    height: 100px;
    padding: 5px;
  }
  .carousel-img img{
    height: 100px !important;
  }
}

table tbody tr th{
  width: 200px;
}

.category{
  background-color: #f9f9f9;
}

.cate-title{
  background-color: #990000;
  color: #fff;
  padding: 10px;
  font-size: 20px;
}

.cate-list ul li{
  list-style: none;
  border-bottom: 1px dashed #d0cfcf;
  padding: 10px 0 10px 0;
}

.cate-list ul li a{
  margin-left: 10px;
  font-size: 14px;
  color: #78797C;
}

/* .cs-title-box .cs-title .ic-item {
  background-color: #fff;
  color: #d60604;
  padding: 5px 7px;
  border-radius: 50px;
  margin-right: 10px;
}

.cs-title-box .cs-title {
  background-color: #d60604;
  color: #fff;
  width: fit-content;
  padding: 5px;
  padding-right: 20px;
  border-radius: 50px;
  position: relative;
  z-index: 99;
  margin: 10px 0px;

}

.cs-title-box:before {
  display: block;
  height: 1px;
  width: 100%;
  background: linear-gradient(90deg, #d60604, rgba(199, 26, 22, 0));
  position: absolute;
  top: 50%;
  z-index: 1;
} */
.btn-yellow{
  background-color: #EFC62C;
  border: none;
  border-radius: 0 !important;
  color: #000 !important;
}

.btn-yellow:hover{
  background-color: #ffc800;
  border: none;
}

.color-primary {
  /*color: #28883b;*/
  color: #2b569a;
}

.bg-primary {
  /*background-color: #28883b !important;*/
  background-color: #2b569a !important;
}

.w-10 {
  width: 10%;
}
.w-80 {
  width: 80%;
}
.w-90 {
  width: 90%;
}

.block-ellipsis {
  display: block;
  display: -webkit-box;
  max-width: 100%;
  font-size: 14px;
  line-height: 1.4;
  -webkit-line-clamp: 3;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
}

tr {
  vertical-align: middle !important;
  box-shadow: 0 0 1rem rgb(18 38 63 / 3%) !important;
}

.bg-ico-primary {
  height: 340px;
}
.ribbons {
  /*background: linear-gradient(45deg, #940012, #F6C6C6);*/
  position: absolute;
  padding: 10px;
  font-weight: bold;
  color: #fff;
  border-radius: 5px;
  top: -18px;
  left: 20px;
  background-color: #2b569a;
  box-shadow: rgba(255, 255, 255, 0) 0px 4px 6px -1px, rgba(255, 255, 255, 0.5) 0px 2px 4px -1px;
}

@media only screen and (max-width: 425px){
  .create-at{
    text-align: start !important;
    margin-bottom: 5px;
  }
}


section.bg-ico-primary {
  padding-top: 100px;
}
.btn-detail {
  background:#2b569a;
  border: none;
}

.btn-secondary {
  --bs-btn-bg: #2b569a;
  --bs-btn-hover-bg: #537961;
}

.custom-content{
  display: -webkit-box !important;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
}
.image-tracuu-pakn {
  border-bottom-left-radius: 10px !important;
  border-top-left-radius: 10px !important;
  width: -webkit-fill-available;
}

.card-body{
    padding: 0px
}

.title h2{
    border-bottom: 3px solid #ffc800;
    padding-bottom: 20px;
    text-transform: uppercase;
    color: #000;
}

.detail{
    font-size: 14px;
}

.detail .image img{
    width: 80%;
}

.detail .image{
    text-align: center;
}


</style>
