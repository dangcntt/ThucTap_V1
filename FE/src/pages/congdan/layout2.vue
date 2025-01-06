<script>
import {mapState} from "vuex";
import {required} from "vuelidate/lib/validators";
import Vue from "vue";
import {VueRecaptcha} from 'vue-recaptcha';
import minLength from "vuelidate/lib/validators/minLength";
import maxLength from "vuelidate/lib/validators/maxLength";
import sameAs from "vuelidate/lib/validators/sameAs";
import LetterCube from "@/components/LetterCube";
import Countdown from "@/components/Countdown/countdown";
import axios from "axios";
import moment from "moment";

const defaultProps = {
  hex: '#2b569a',
};


/**
 * Crypto ICO-landing page
 */
export default {
  components: {},
  data() {
    return {
      showButton: false,
      start: "",
      end: "",
      interval: "",
      days: "",
      minutes: "",
      hours: "",
      seconds: "",
      starttime: "Nov 5, 2020 15:37:25",
      endtime: "Dec 31, 2021 16:37:25",
      showModal: false,
      showRegisterModal: false,
      showForgotModal: false,
      showNotify: false,
      email: "",
      password: "",
      submitted: false,
      authError: null,
      tryingToLogIn: false,
      isAuthError: false,
      capcha: null,
      modelRegister: {
        firstName: null,
        lastName: null,
        userName: null,
        soDienThoai: null,
        email: null,
        password: null,
        confirmPassword: null,
        phoneNumber: null,
        emailAddress: null
      },
      modelAuth: {
        isAuthError: false,
        message: null
      },
      model: {
        userName: "",
        password: ""
      },
      currentUserAuth: null,
      isShow: false,
      url : `${process.env.VUE_APP_API_URL}filesminio/view/`,
      showPDF: false,
      pdfID: '',
      toltip: null,
      file: {
        id: null,
        fileId: null,
        fileName: null,
        ext: ".pdf"
      },
      otpShow: false,
      verifyOpt: {
        sender: "du.tranphuoc@gmail.com",
        receiver: "0836980284",
        token: ""
      },
      sendSmsOtp: {
        sender: "du.tranphuoc@gmail.com",
        receiver: "0836980284",
        applicationTitle: "Test DGov"
      },
      accessToken: null,
      tempUser: null,
      showButtonSendOTP: true,
      event: null,
      treeView: [],
      lienHe: {},
      suKien:[],
      idMenu : "",
      urlHeader: ""
    };
  },
  validations: {
    model: {
      userName: {
        required,
      },
      password: {
        required
      }
    }
  },
  created() {
    let authUser = localStorage.getItem("auth-user");
    if (authUser) {
      let jsonUserCurrent = JSON.parse(authUser);
      this.currentUserAuth = jsonUserCurrent;
     console.log("CURRENT USER AUTH  created : ", this.currentUserAuth)
    }
  },
  destroyed() {
    window.removeEventListener('scroll', this.handleScroll);
  },
  mounted() {
    window.addEventListener('scroll', this.handleScroll);
  },
  computed: {
    ...mapState('snackBarStore', ['notify', 'registerModal'])
  },
  methods: {
    async getThongTinHeader(){
      let promise =  this.$store.dispatch("headerStore/getAll")
      return promise.then(resp => {
        if(resp.data == null){
          return []
        }else{
          if (resp.data != null )
          {
            this.urlHeader = this.url +resp.data.file.fileId;
          }
        }
      })
    },
    handleScroll() {
      if (window.scrollY > 500) {
        this.showButton = true;
      } else {
        this.showButton = false;
      }
    },
    scrollToTop() {
      window.scrollTo({
        top: 0,
        behavior: 'smooth'
      });
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
    async GetTreeList(){
       console.log("LOG TREEVIEW : ")
      await this.$store.dispatch("menuCongDanStore/getTreeList").then((res) => {
        this.treeView = res.data;
       // console.log("LOG TREEVIEW : ", this.treeView)
      })
    },
    async getTreeFlatten(){
      await this.$store.dispatch("menuCongDanStore/getTreeFlatten").then((res) => {
        localStorage.setItem('flatten-menu', JSON.stringify(res.data));

      })
    },
    async getLienHe(){
      await this.$store.dispatch("lienheStore/getAll").then((res) => {
        // console.log("LIEN HE: ", res.data)
        this.lienHe = res.data;
      })
    },
    async getSuKien(){
      await this.$store.dispatch("suKienStore/getAll").then((res) => {
        console.log("SU KIEN: ", res.data)
        this.suKien = res.data;
        if (res.data && res.data.length > 0) {
      // Lấy phần tử cuối cùng của mảng
          const lastIndex = res.data.length - 1;
          this.suKien = res.data[lastIndex];
        }
      })
    },
    handleGetIdMenu(item) {
      if (item.id != this.idMenu) {
        if (item.link.indexOf("/{id}") < 0  && item.level === 0)
        {
       //   console.log("LOG ROUTER IF LAYOUT 2 : ", item)
          this.idMenu = item.id;
          //  console.log("LOG ITEM : ", item.link)
          this.$router.push(item.link);
        } else if (item.link.indexOf("/{id}") > 0 && item.level === 0){
          this.idMenu = item.id;
        //  console.log("LOG ROUTER IF ELSE  LAYOUT 2 : ", item.link.replace("{id}",  item.id))
          this.$router.push(item.link.replace("{id}",  item.id));
        }else {
        //  console.log("LOG ROUTER ELSE LAYOUT 2 : ", item.link +   item.id)
          this.idMenu = item.id;
          this.$router.push(item.link +  "/" + item.id);
        }
      }
    },
    async Login(e) {
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.model.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        await this.$store.dispatch("authStore/login", this.model).then((res) => {
          if (res.code === 0) {
            this.showModal = false;
            if (res.data.menu != null)
            {
              window.location.href = '/tai-khoan'
            }
            localStorage.setItem('auth-user', JSON.stringify(res.data));
            localStorage.setItem("user-token", JSON.stringify(res.data.accessToken));
            Vue.prototype.$auth_token = res.data.token;
            this.model = {
              username: null,
              password: null
            }
            this.$notify({
              group: "foo",
              title: "Thông báo",
              type: "success",
              text: res.message,
            });
            let authUser = localStorage.getItem("auth-user");
            if (authUser) {

              let jsonUserCurrent = JSON.parse(authUser);
          //    console.log("LOG SUBMIT :  ", jsonUserCurrent)
              this.currentUserAuth = jsonUserCurrent;
         //     console.log("LOG SUBMIT 1231 :  ", this.currentUserAuth)
            }
       //     this.$store.dispatch("authStore/setCurrentUser", res.data.accessToken);
          } else {
            this.$notify({
              group: "foo",
              title: "Thông báo",
              type: "warn",
              text: res.message,
            });
          }
        });
        loader.hide();
      }
      this.submitted = false;
    },
    logout() {
      console.log("work")
      // eslint-disable-next-line no-unused-vars
      var userLocalStorage = localStorage.getItem("user-token");
      if (userLocalStorage) {
        localStorage.removeItem("user-token");
        localStorage.removeItem("auth-user");
        Vue.prototype.$auth_token = null;
        this.$store.dispatch("authStore/setCurrentUser", null);
        window.location.href = "/"
        return;
      }
    },
    handlePush(path) {
      let pathUrl = path ?? "/"
      if (pathUrl != window.location.pathname) {
        this.$router.push(pathUrl)
      }
    },
  },
  watch: {
    registerModal: {
      deep: true,
      handler(val) {

      }
    },
    modelRegister: {
      deep: true,
      handler(val){

      }
    }
  }
};
</script>
<template>
  <div class="position-relative">
    <div class="navbar-expand-lg fixed-top">
      <nav class="navbar pd-0 bg-menu" id="navbar"
           style="padding: 0px;">
        <div class="cs-navbar-header menu">
          <button
              type="button"
              class="btn btn-sm px-3 font-size-16 d-lg-none header-item r-guiphananh btn-collapse bd pd"
              data-toggle="collapse"
              data-target="#topnav-menu-content"
              @click="toggleMenu()"
          >
            <i class="mdi mdi-format-align-justify pd-5" style="color: #fff"></i>
          </button>
          <div class="collapse navbar-collapse" id="topnav-menu-content"
          >
          <span style="padding: 0px 20px;font-size: 20px; color: #fff;">
            <a href="https://hoasadec.com.vn/" style="color: #fff;">Festival Hoa Kiểng Sa Đéc</a></span>
            <div class="book-hoa">
              <router-link
                  :to="{
                    path: `/book`,
                    }"
                  target="_blank"
              >
                <span style="color: #fff;">SÁCH HOA</span>
              </router-link>
            </div>
          <ul
                class="navbar-nav"
                id="topnav-menu"
                v-scroll-spy-active="{ selector: 'a.nav-link' }"
            >
            <div v-for="(item, index) in treeView" :key="index" class="dropdown">
              <button
                                  class="btn"
                                  type="button"
                                  id="dropdownMenuButton"
                                  data-mdb-toggle="dropdown"
                                  aria-expanded="false"
                                  @click="handleGetIdMenu(item)"
                                >
                <a class="nav-link fs-14"><strong>{{item.label}}</strong></a>
              </button>
              <ul class="dropdown-menu" aria-labelledby="dropdownMenuButton" v-if="item.children">
                <div v-for="(item, index) in item.children" :key="index">
                  <button
                      @click="handleGetIdMenu(item)"
                  >
                    <a class="nav-link fs-14"><strong>{{item.label}}</strong></a>
                  </button>
                </div>
              </ul>
            </div>
            <div class="ms-lg-2 text-white" style="padding: 8px;">
              <b-dropdown
                  v-if="currentUserAuth"
                  right
                  variant="black"
                  toggle-class="header-item"
                  menu-class="dropdown-menu-end menu-congdan"
                  style="width: 100%"
              >
                <template v-slot:button-content>
                  <div style="display: flex; align-items: flex-start; justify-content: center">
                    <div class="d-flex align-items-center">

                    </div>
                    <div v-if="currentUserAuth.avatar != null">
                      <img
                          class="rounded-circle header-profile-user"
                          :src="url + `${currentUserAuth.avatar}`"
                          alt="Avatar"
                      />
                    </div>
                    <div v-else>
                      <img
                          class="rounded-circle header-profile-user"
                          src="@/assets/images/logo-con.png"
                          alt="Avatar"
                      />
                    </div>

                  </div>
                </template>
                <!-- item-->
                <b-dropdown-item style="width: 100%;">
                  <span class="d-xl-inline-block ms-1">
                        <div v-if="currentUserAuth">
                          <div
                              style="display: flex; flex-direction: column; justify-content: left; align-items: flex-start">
                              <div class="text-black font-weight-bold" style="white-space: initial;">{{ currentUserAuth.fullName }}</div>
                            <div style="font-size: 10px; color: black;">@{{ currentUserAuth.userName }}</div>
                          </div>
                        </div>
                        <div v-else class="text-black font-weight-bold">
                          Khách
                        </div>
                      </span>
                </b-dropdown-item>
                <hr class="my-1">
                <b-dropdown-item v-if="currentUserAuth != null && currentUserAuth.menu != null"
                    @click="handlePush('/tai-khoan')" style="width: 100%">
                  <i class="bx bx-user font-size-16 align-middle me-1"></i>
                  Vào trang quản trị
                </b-dropdown-item>
                <b-dropdown-item @click="handlePush('/thong-tin-ca-nhan')" style="width: 100%">
                  <i class="bx bx-user font-size-16 align-middle me-1"></i>
                  Thông tin cá nhân
                </b-dropdown-item>
                <a @click="logout" href="javascript:void(0)" class="dropdown-item font-weight-bold" style="color: #faf150;">
                  <i
                      class="bx bx-power-off font-size-16 align-middle me-1"
                      style="color: #faf150;"
                  ></i>
                  Đăng xuất
                </a>
              </b-dropdown>
              <div v-else class="dangnhap">
                <button type="button" style="padding: 8px 5px;"
                        v-on:click="showModal = true, showRegisterModal = false, showForgotModal= false"
                        class="btn w-xs btn-login">ĐĂNG NHẬP
                </button>
              </div>
              
            </div>
            <!-- <div style="padding: 10px;">
              <button type="button" style="float: right;padding: 8px 5px;"
                        v-on:click="showModal = true, showRegisterModal = false, showForgotModal= false"
                        class="btn w-xs btn-login">ĐĂNG NHẬP
              </button>
            </div> -->
          </ul>
          </div>
        </div>
      </nav>
      
    </div>
    <div v-scroll-spy>
      <slot/>
      <div style="padding: 0px !important;">
        <img src="@/assets/images/bg-footer.png" alt="" style="width: 100%;">
      </div>
      <footer class="landing-footer bg-footer bg-menu">
        <div class="container">
          <div style="color: #fff; text-align: center; font-size: 15px;">
              Copyright © 2023 Sở Thông tin và Truyền thông
            
          </div>
        </div>

        <!-- ĐĂNG NHẬP -->
        <b-modal
            v-model="showModal"
            title="Thông tin đăng nhập"
            title-class="text-black font-18"
            body-class="p-3"
            hide-footer
            hide-header
            centered
            no-close-on-backdrop
            size="md"
            style="padding: 0px"
        >
          <Transition name="fade" mode="out-in">
            <div v-if="!showRegisterModal && !showForgotModal" class="row justify-content-center">
              <div class="col-md-12">
                <div class="card overflow-hidden" style="padding: 0px; margin-bottom: 0px; box-shadow: none">
                  <div class="bg-soft bg-primary bg-login">
                  </div>
                  <div class="card-body pt-0" style="padding: 10px 5px">
                    <div>
                      <router-link to="/" style="display: flex; justify-content: center">
                        <div class="avatar-md profile-user-wid mb-4">
                        <span class="avatar-title rounded-circle bg-light">
                        <img src="@/assets/images/4.png" alt height="80" style="padding: 5px;"/>
                        </span>
                        </div>
                      </router-link>
                    </div>
                    <b-form class="p-0" @submit.prevent="Login" ref="formContainer">
                      <h4 style="text-align: center; margin-bottom: 20px"> Thông tin đăng nhập</h4>
                      <b-alert
                          v-model="modelAuth.isAuthError"
                          variant="danger"
                          class="mt-3"
                          dismissible
                      >{{ modelAuth.message }}
                      </b-alert
                      >
                      <b-form-group
                          class="mb-3"
                          id="input-group-1"
                          label="Tài khoản"
                          label-for="input-1"
                      >
                        <b-form-input
                            id="input-1"
                            v-model="model.userName"
                            type="text"
                            placeholder="Nhập tên đăng nhập"
                            :class="{ 'is-invalid': submitted && $v.model.userName.$error }"
                        ></b-form-input>
                        <div
                            v-if="submitted && $v.model.userName.$error"
                            class="invalid-feedback"
                        >
                          <span v-if="!$v.model.userName.required">Tài khoản không được trống.</span>
                        </div>
                      </b-form-group>

                      <b-form-group
                          class="mb-3"
                          id="input-group-2"
                          label="Mật khẩu"
                          label-for="input-2"
                      >
                        <b-form-input
                            id="input-2"
                            v-model="model.password"
                            type="password"
                            placeholder="Nhập mật khẩu"
                            :class="{ 'is-invalid': submitted && $v.model.password.$error }"
                        ></b-form-input>
                        <div
                            v-if="submitted && !$v.model.password.required"
                            class="invalid-feedback"
                        >
                          Mật khẩu không được để trống!
                        </div>
                      </b-form-group>
                      <div class="row">
                        <div class="mt-1  col-md-6">
                          <b-button type="button" variant="danger" class="btn-block w-100"
                                    v-on:click="showModal = false"
                          >Thoát
                          </b-button
                          >
                        </div>
                        <div class="mt-1 col-md-6">
                          <b-button type="submit" variant="success" class="btn-block w-100"
                          >Đăng nhập
                          </b-button
                          >
                        </div>
                      </div>

                    </b-form>
                  </div>
                  <!-- end card-body -->
                </div>
              </div>
              <!-- end col -->
            </div>
          </Transition>
        </b-modal>

      </footer>
      <!-- Footer end -->
    </div>
    <button v-if="showButton" @click="scrollToTop" id="backToTopBtn" class="btn-back">
      <i class="mdi mdi-chevron-double-up" aria-hidden="true" style="font-size: 20px;"></i>
    </button>
    <div class="btn-lk">
      <router-link
          :to="{
            path: `/book`,
            }"
          target="_blank"
      >
        <i class="fa fa-book" aria-hidden="true" style="font-size: 50px; color: rgb(216 38 110);"></i>
      </router-link>
    </div>
  </div>
</template>
<style lang="scss">
.book-hoa{
  margin-left: 50%;
  font-size: 16px;
  padding: 13px 0px;
}

@media only screen and (max-width: 992px) {
  .book-hoa{
    margin-left: 10px;
  }
}
.dropdown-menu-right{
  right: 0;
  top: 35px;
}

.header-profile-user{
  height: 50px !important;
}
#topnav-menu-content{
  justify-content:space-between!important;
}
@media (min-width: 992px){
  .navbar-expand-lg .navbar-collapse {
    flex-direction: row !important;
    justify-content: center !important;
  }

}

.btn-lk{
  width: 50px;
  position: fixed;
  bottom: 30px;
  left: 30px;
  z-index: 999;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

// .btn-lk i{
//   width: 100%;
//   margin-bottom: 10px;
// }

.btn-back{
  position: fixed;
  bottom: 30px;
  right: 30px;
  z-index: 99;
  background-color: #ec008b;
  color: white;
  padding: 10px 15px;
  border: none;
  border-radius: 5px;
  cursor: pointer;
  opacity: 0.7;
}

#dropdownMenuButton{
  text-transform: uppercase;
}
.menu{
  width: 100%;
  font-family: 'Montserrat', sans-serif !important;
  font-weight: bold;
}
.nav-link{
  padding: 0;
}
.btn{
  margin: 0 !important;
  padding: 5px !important;
}
.navbar-nav{
  display: flex;
  flex-wrap: wrap;
}

.menu-flex{
  display: flex;
}

.dropdown:hover>.dropdown-menu {
  display: block;
  background-color: #ED0678;
}

.dropdown-menu{
  width: 250px !important;
}

.dropdown-menu button{
  border: none;
  background-color: #ED0678;
  text-transform: uppercase;
  width: 100%;
  text-align: left !important;
}

.dropdown-menu button:hover{
  background-color: #fff;
}

.dropdown-menu button:hover a{
  color: #ED0678 !important;
}

.dropdown-menu .nav-item:hover{
  background-color: #de2323;
}

.dropdown>.dropdown-toggle:active {
  /*Without this, clicking will make it sticky*/
    pointer-events: none;
}

.pd{
  padding: 8px !important;
}

.bd{
  border: 1px solid #fff;
  border-radius: 10px;
}

.header-inner{
 // background-image: 'url(${this.urlHeader})'
  background-position: center;
  background-repeat: no-repeat;
  background-size: cover;
  height: 200px;
}

#topnav-menu .nav-item a{
  font-size: 13px;
}

.bg-footer {
  min-height: 100%;
  padding-top: 20px;
  padding-bottom: 20px;
}

.text-light p{
  margin-bottom: 0 !important;
  font-size: 18px;
}

.logo-footer{
  text-align: center;
}

.logo-footer img{
  width: 120px;
}

.floating-button {
  position: fixed;
  right: 20px;
  bottom: 20px;
  z-index: 999999999;
}

.glow-on-button {
  width: 50px;
  height: 50px;
  background: #2b569a;
  border-radius: 50px;
  display: flex;
  justify-content: center;
  align-items: center;
  margin-bottom: 10px;
  box-shadow: rgba(17, 17, 26, 0.1) 0px 1px 0px, rgba(17, 17, 26, 0.1) 0px 8px 24px, rgba(17, 17, 26, 0.1) 0px 16px 48px;
}

.toltip-gpa,
.toltip-hdsd {
  position: absolute;
  top: 20%;
  padding: 5px 10px;
  background: rgba(43, 86, 154, 0.2);
  color: #2b569a;
  border-radius: 5px;
}

.toltip-gpa {
  left: -110px;
}

.toltip-hdsd {
  left: -150px;
}

.glow-on-button i {
  color: #fff !important;
  font-size: 16px;
}

@keyframes glowing {
  0% {
    background-position: 0 0;
  }
  50% {
    background-position: 400% 0;
  }
  100% {
    background-position: 0 0;
  }
}
</style>

<style scoped>
#topnav-menu .nav-item a {
  font-size: 14px;
}

.dropdown-pakn:hover {
  border: none;
}

.dropdown-pakn:focus {
  border: none;
}

.dropdown-pakn .items-pakn {
  line-height: 28px;
}

.dropdown-pakn .items-pakn:hover {
  background-color: #d7e7e5;
}

</style>

<style>


.menu-congdan {
  width: 100% !important;
  min-width: 200px;
}

.bg-navbar-dark {
  background: rgba(204, 204, 204, 0.56);
}

.hero-section.bg-ico-hero {
  height: 500px;
}

li.router-link-exact-active {
  margin: 5px;
  border-radius: 5px;
}

#topnav-menu-content li.router-link-exact-active > a {
  color: #2a2a2a !important;
  padding-top: 0px !important;
  padding-bottom: 0px !important;
  background-color: #fff;
}

#topnav-menu-content a.nav-link {
  line-height: 38px !important;
  border-radius: 5px;
  color: #ffffff;
}

@media only screen and (max-width: 2560px) {
  .hero-section.bg-ico-hero {
    position: relative;
    height: 400px;
  }

  .banner-home .title-box {
    padding-top: 30px;
  }

  .hero-section.bg-ico-primary {
    height: 300px;
  }

  .btn-collapse {
    position: absolute;
    top: 10px;
    left: 10px;
    height: 40px;
  }

  .hero-title {
    font-size: 38px !important;
    font-weight: bold;
    color: #2b569a;
  }

  .hero-action {
    display: none;
  }
}


@media only screen and (max-width: 1440px) {
  .hero-section.bg-ico-hero {
    position: relative;
    height: 300px;
  }


  .r-guiphananh {
    position: absolute;
    padding: 0;
    bottom: 50px;
    left: 0px;
  }

  .hero-section.bg-ico-primary {
    height: 300px;
    padding-top: 140px;
  }

  .hero-section.bg-ico-primary.banner-home {
    padding-top: 100px;
  }

  .hero-section.bg-ico-primary.contact-section {
    padding-top: 80px;
  }

  .contact-section .title-box h1 {
    font-size: 40px;
  }


  .hero-title {
    font-size: 22px !important;
    font-weight: bold;
  }

  .hero-action {
    display: none;
  }
}

@media only screen and (max-width: 1200px) {
  .hero-section.bg-ico-hero {
    height: 200px;
  }

  .r-guiphananh {
    bottom: 50px;
    left: 0px;
  }
}

@media only screen and (max-width: 1024px) {
  .hero-section.bg-ico-hero {
    height: 210px;
  }

  .contact-section .title-box {
    margin-top: 0px;
    padding-top: 40px;
  }

  .hero-section .title-box {
    margin-top: -40px;
  }

  .r-guiphananh {
    padding: 5px;
    top: 5px;
    left: 12px;
  }

  .hero-section.bg-ico-primary {
    height: 220px;
  }

  .hero-title {
    font-size: 22px !important;
    font-weight: bold;
    padding: 0 !important;
  }

  .hero-action {
    display: none;
  }

  .wr-show {
    display: none;
  }

}


@media only screen and (max-width: 768px) {
  .hero-section.bg-ico-hero {
    height: 150px;
    margin-top: 30px;
  }

  .cs-navbar-header {
    justify-content: end !important;
  }

  .hero-section .title-box {
    margin-top: -5px;
  }

  .contact-section .title-box {
    margin-top: 0px;
    padding-top: 0px;
  }

  .r-guiphananh {
    bottom: 3px;
    left: 0px;
  }

  .hero-section.banner-home {
    height: 200px !important;
    margin-top: 20px !important;
    padding-top: 30px !important;
  }

  .hero-section.bg-ico-primary {
    height: 150px;
    margin-top: 60px;
    padding-top: 30px !important;
  }
}

@media only screen and (max-width: 617px) {
  .hero-section.bg-ico-hero {
    height: 150px;
  }

  .hero-title {
    font-size: 16px !important;
    font-weight: bold;
  }
}

@media only screen and (max-width: 570px) {
  .hero-section.bg-ico-hero {
    height: 150px;
  }

  .hero-title {
    font-size: 16px !important;
    font-weight: bold;
  }
}

@media only screen and (max-width: 425px) {
  .hero-section.bg-ico-hero {
    height: 120px;
    padding: 0px;
    margin-top: 60px;
  }

  .hero-title.title-contact {
    margin-top: 20px !important;
    font-size: 30px !important;
  }
}

@media only screen and (max-width: 425px) {
  .hero-section.bg-ico-hero {
    height: 120px;
  }

  .hero-section.bg-ico-primary {
    height: 130px;
    margin-top: 60px;
  }

  .r-guiphananh {
    bottom: 20px;
  }

  .btn-guiphananh {
    font-size: 10px;
    padding: 5px;
  }

  .btn-custom {
    height: 20px;
    width: 100px;
    font-size: 10px;
    padding: 3px;
  }
}

@media only screen and (max-width: 375px) {
  .hero-section.bg-ico-hero {
    height: 90px;
  }

  .contact-section .title-box {
    margin-top: 60px;
  }

  .notify-section .title-box,
  .search-section .title-box {
    margin-top: 40px;
  }

  .hero-section.banner-home {
    height: 170px !important;
  }

  section#about {
    padding: 0px 10px;
  }


  .btn-guiphananh {
    padding: 4px;
    font-size: 12px;
  }

  .hero-section.bg-ico-primary {
    height: 170px;
    margin-top: 10px;
  }

  .r-guiphananh {
    bottom: 10px;
  }
}

@media only screen and (max-width: 320px) {
  .hero-section.bg-ico-hero {
    height: 80px;
  }

  .hero-section .hero-title {
    padding: 0px 20px !important;
  }

  .btn-guiphananh {
    margin-top: -15px;
  }
}

.text-primary-dark {
  color: #6E1511;
}

.icon_question {
  animation-name: spin, depth;
  animation-timing-function: linear;
  animation-iteration-count: infinite;
  animation-duration: 3s;
}

@keyframes spin {
  0% {
    transform: rotateY(0deg);
  }
  100% {
    transform: rotateY(-360deg);
  }
}


@keyframes depth {
  0% {
    text-shadow: 0 0 black;
  }
  25% {
    text-shadow: 1px 0 black, 2px 0 black, 3px 0 black, 4px 0 black, 5px 0 black;
  }
  50% {
    text-shadow: 0 0 black;
  }
  75% {
    text-shadow: -1px 0 black, -2px 0 black, -3px 0 black, -4px 0 black,
    -5px 0 black;
  }
  100% {
    text-shadow: 0 0 black;
  }
}

.btn-modal {
  background: linear-gradient(45deg, #F13F55, #F6C6C6);
  border: none;
  color: #fff;
}
</style>
<style lang="scss">
.success-checkmark {
  width: 80px;
  height: 115px;
  margin: 0 auto;

  .check-icon {
    width: 80px;
    height: 80px;
    position: relative;
    border-radius: 50%;
    box-sizing: content-box;
    border: 4px solid #4CAF50;

    &::before {
      top: 3px;
      left: -2px;
      width: 30px;
      transform-origin: 100% 50%;
      border-radius: 100px 0 0 100px;
    }

    &::after {
      top: 0;
      left: 30px;
      width: 60px;
      transform-origin: 0 50%;
      border-radius: 0 100px 100px 0;
      animation: rotate-circle 4.25s ease-in;
    }

    &::before, &::after {
      content: '';
      height: 100px;
      position: absolute;
      background: #FFFFFF;
      transform: rotate(-45deg);
    }

    .icon-line {
      height: 5px;
      background-color: #4CAF50;
      display: block;
      border-radius: 2px;
      position: absolute;
      z-index: 10;

      &.line-tip {
        top: 46px;
        left: 14px;
        width: 25px;
        transform: rotate(45deg);
        animation: icon-line-tip 0.75s;
      }

      &.line-long {
        top: 38px;
        right: 8px;
        width: 47px;
        transform: rotate(-45deg);
        animation: icon-line-long 0.75s;
      }
    }

    .icon-circle {
      top: -4px;
      left: -4px;
      z-index: 10;
      width: 80px;
      height: 80px;
      border-radius: 50%;
      position: absolute;
      box-sizing: content-box;
      border: 4px solid rgba(76, 175, 80, .5);
    }

    .icon-fix {
      top: 8px;
      width: 5px;
      left: 26px;
      z-index: 1;
      height: 85px;
      position: absolute;
      transform: rotate(-45deg);
      background-color: #FFFFFF;
    }
  }
}

@keyframes rotate-circle {
  0% {
    transform: rotate(-45deg);
  }
  5% {
    transform: rotate(-45deg);
  }
  12% {
    transform: rotate(-405deg);
  }
  100% {
    transform: rotate(-405deg);
  }
}

@keyframes icon-line-tip {
  0% {
    width: 0;
    left: 1px;
    top: 19px;
  }
  54% {
    width: 0;
    left: 1px;
    top: 19px;
  }
  70% {
    width: 50px;
    left: -8px;
    top: 37px;
  }
  84% {
    width: 17px;
    left: 21px;
    top: 48px;
  }
  100% {
    width: 25px;
    left: 14px;
    top: 45px;
  }
}

@keyframes icon-line-long {
  0% {
    width: 0;
    right: 46px;
    top: 54px;
  }
  65% {
    width: 0;
    right: 46px;
    top: 54px;
  }
  84% {
    width: 55px;
    right: 0px;
    top: 35px;
  }
  100% {
    width: 47px;
    right: 8px;
    top: 38px;
  }
}
</style>
<style lang="scss">

.bg-home-primary {
  //background: url("../../assets/images/background_primary.png");
  background-size: cover;
  background-position: top;
  height: 200px;
  margin: auto;
  margin-top: 80px;
  @media (max-width: 575.98px) {
    height: 80px;
    & .content-title {
      padding: 0px 20px;
      font-size: 14px;
    }
  }
  @media (max-width: 575.98px) {
    height: 50px;
    & .content-title {
      padding: 0px 20px;
    }
  }

}

@media only screen and (max-width: 768px) {
  #topnav-menu-content {
    padding-bottom: 50px;
  }
}

.btn-login {
  background-color: #faf150 !important;
  color: #c71a16;
  text-shadow: 0px 0px 2px #fff;
}

.btn-login:focus,
.btn-login:hover {
  border: none !important;
  background: #faf150 !important;
  color: #c71a16;
  box-shadow: rgba(0, 0, 0, 0.4) 0px 2px 4px, rgba(0, 0, 0, 0.3) 0px 7px 13px -3px, rgba(0, 0, 0, 0.2) 0px -3px 0px inset !important;
}

.btn-shape:focus,
.btn-shape:hover {
  border: none !important;
  box-shadow: rgba(0, 0, 0, 0.4) 0px 2px 4px, rgba(0, 0, 0, 0.3) 0px 7px 13px -3px, rgba(0, 0, 0, 0.2) 0px -3px 0px inset !important;
}

.fs-15 {
  font-size: 15px !important;
}

.fs-13 {
  font-size: 13px !important;
}

.fs-warning {
  font-size: 13px;
  color: #ffa000;
  font-style: italic
}

.fs-cyan {
  font-size: 13px;
  color: #449d73;
  font-style: italic
}

.ellipsis {
  white-space: nowrap;
  text-overflow: ellipsis;
  overflow: hidden;
}

.title-block-ellipsis {
  display: block;
  display: -webkit-box;
  max-width: 100%;
  height: 43px;
  margin: 0 auto;
  font-size: 14px;
  line-height: 1;
  -webkit-line-clamp: 4;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
}

.block-ellipsis {
  display: block;
  display: -webkit-box;
  max-width: 100%;
  height: 80px;
  margin: 0 auto;
  font-size: 14px;
  line-height: 1;
  -webkit-line-clamp: 4;
  -webkit-box-orient: vertical;
  overflow: hidden;
  text-overflow: ellipsis;
}

.cs-title-header span {
  font-size: 14px;
}

@media only screen and (max-width: 2560px) {
  .navbar-logo {
    line-height: 18px !important;
  }
}

@media only screen and (max-width: 1366px) {
  .navbar-logo {
    line-height: 18px !important;
  }
}

@media only screen and (max-width: 1200px) {
  .navbar-logo {
    line-height: 18px !important;
  }
}


@media only screen and (max-width: 992px) {
  .cs-navbar-header {
    justify-content: end !important;
    min-height: 50px;
  }
  .navbar-logo {
    width: 200px !important;
  }

  .menu{
    margin-left: 50px;
    padding-top: 50px;
  }
}


@media only screen and (max-width: 1024px) {
  .cs-title-header {
    width: 200px;
  }
  .cs-title-header p {
    font-size: 14px;
  }
  .logo-dark {
    height: 40px;
  }
  .logo-light {
    height: 40px;
  }
  .navbar {
    padding: 10px 0px;
  }
  .navbar-logo {
    line-height: 18px !important;
  }
}

@media only screen and (max-width: 768px) {
  .btn-collapse {
    position: absolute;
    left: 10px;
    height: 40px;
  }
}

@media only screen and (max-width: 425px) {
  .logo-dark {
    margin-left: 30px;
  }
  .navbar-logo {
    width: 270px !important;
  }
  .cs-title-header {
    width: 150px;
  }

}

@media only screen and (max-width: 375px) {
  .cs-title-header {
    width: 100%;
  }
  .cs-title-header span {
    font-size: 14px;
  }
}

@media only screen and (max-width: 768px) {
  .btn-guiphananh {
    margin-bottom: 5px;
  }

}

.event{
  background-color: #f8dff4;
  white-space: nowrap;
  font-weight: bold;
  font-size: 16px;
  padding: 12px 0;
  height: 50px;
}

.bg-login {
    background-image: url("../congdan/fes.jpg");
    background-size: cover;
    background-repeat: no-repeat;
    background-position: center;
    opacity: 1;
    min-height: 230px;
}
</style>

