<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import {numeric, required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import Switches from "vue-switches";

export default {
  page: {
    title: "THÔNG TIN",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader,Switches },
  data() {
    return {
      data: [],
      title: "THÔNG TIN",
      items: [
        {
          text: "THÔNG TIN",
          active: true,
        }
      ],
      model: null,
      showModal : false,
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}files/upload`,
        thumbnailWidth: 300,
        thumbnailHeight: 160,
        maxFiles: 4,
        maxFilesize: 30,
        headers: { "My-Awesome-Header": "header value" },
        addRemoveLinks: true,
        acceptedFiles: ".pdf",
      },
      url :  process.env.VUE_APP_API_URL + 'files/view/' ,
      submitted: false,
    };
  },
  computed: {
    //Validations
    rules(){
      return{
        invoiceType:{required},
        templateCode: {required},
        invoiceSeries: {required},
        currencyCode: {required},
        adjustmentType: {required}
      }
    }
  },
  validations: {
    model: {
      invoiceType:{required},
      templateCode: {required},
      invoiceSeries: {required},
      currencyCode: {required},
      adjustmentType: {required}
    },
  },
  created() {
    this.getThongTin()
  },
  methods: {
    async handleSubmitThongTin(e) {
      console.log("LOG THONG TIN ", this.model)
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      console.log("LOG INVALID 36165 : ", this.$v);
      if (this.$v.$invalid) {
        console.log("LOG INVALID  : ")
        return;
      } else {
        console.log("LOG ELSE  INVALID  : ")
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });

        // await this.$store.dispatch("hoaDonStore/getFile").then((res) => {
        //   var blob = new Blob([this.base64ToArrayBuffer(res.data)], {type: "application/pdf"});
        //   var link = document.createElement('a');
        //   link.href = window.URL.createObjectURL(blob);
        //   var fileName = "hoadon";
        //   link.download = fileName;
        //   link.click();
        // })
      await this.$store.dispatch("thongtinPhatHanhStore/create", this.model).then((res) => {
        if (res.code === 0) {
          this.model = res.data
          this.showModal = false;
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
      });
        loader.hide();
        this.submitted = false;

    }
    this.submitted = false;
},

    async getThongTin(){
      let promise =  this.$store.dispatch("thongtinPhatHanhStore/getAll")
      return promise.then(resp => {
        if(resp.data == null){
          return []
        }else{
        //  console.log("LOG NGUYEN TUAN KIET ",resp.data[0] )
          if (resp.data != null && resp.data.length > 0 )
          {
           this.model = resp.data[0]
          }

        }
      })
    },

  },
  watch: {},
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>

    <div class="card">
      <div class="card-body">
        <form ref="formContainer">
          <!-- API Key -->
          <div class="row">
            <div class="col-md-3">
              <label for="">Mã loại hóa đơn </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    v-model="model.invoiceType"
                    type="text"
                    class="form-control"
                    placeholder="Nhập tên"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.invoiceType.$error,
                    }"
                >
                <div
                  v-if="submitted && !$v.model.invoiceType.required"
                    class="invalid-feedback"
                >
                  InvoiceType không được trống.
                </div>
              </div>
            </div>
          </div>
          <!--Is Ative -->



          <div class="row">
            <div class="col-md-3">
              <label for="">Kí hiệu mẫu hóa đơn </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    v-model="model.templateCode"
                    type="text"
                    class="form-control "
                    placeholder="Nhập templateCode"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.templateCode.$error,
                    }"
                  />
                  <div
                      v-if="submitted && !$v.model.templateCode.required"
                      class="invalid-feedback"
                  >
                    TemplateCode không được trống.
                  </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Kí hiệu hóa đơn </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="text"
                    class="form-control "
                    v-model="model.invoiceSeries"
                    placeholder="Nhập địa chỉ của bên bán"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.invoiceSeries.$error,
                    }"
                  />
                  <div
                      v-if="submitted && !$v.model.invoiceSeries.required"
                      class="invalid-feedback"
                  >
                    invoiceSeries không được trống.
                  </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Mã tiền tệ </label><span class="text-danger">*</span>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="text"
                    class="form-control "
                    v-model="model.currencyCode"
                    placeholder="Nhập currencyCode"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.currencyCode.$error,
                    }"
                  />
                  <div
                      v-if="submitted && !$v.model.currencyCode.required"
                      class="invalid-feedback"
                  >
                    Mã tiền tệ không được trống.
                  </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Trạng thái điều chỉnh hóa đơn</label>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <input
                    type="text"
                    class="form-control "
                    placeholder="Nhập adjustmentType"
                    v-model="model.adjustmentType"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.adjustmentType.$error,
                    }"
                >
                <div
                    v-if="submitted && !$v.model.adjustmentType.required"
                    class="invalid-feedback"
                >
                  Trạng thái điều chỉnh hóa đơn không được trống.
                </div>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Trạng thái thanh toán của hóa đơn</label>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <switches class="mt-2 mb-0"  v-model="model.paymentStatus" type-bold="false" color="success" ></switches>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-md-3">
              <label for="">Cho phép người dùng tra cứu hóa đơn</label>
            </div>
            <div class="col-md-9">
              <div class="form-group mb-3">
                <switches class="mt-2 mb-0"  v-model="model.cusGetInvoiceRight" type-bold="false" color="success" ></switches>
              </div>
            </div>
          </div>

          <div class="row">
            <div class="col-md-12 text-center" >
              <b-button
                  class="btn cs-btn-primary btn-rounded mb-2 me-2"
                  variant="success"
                  v-on:click="handleSubmitThongTin"
              >
                <i class="bx bx-save "></i>
                Lưu thông tin
              </b-button>
            </div>
          </div>
        </form>
      </div>
    </div>

  </Layout>
</template>
<style>
.cs-edit-file{
  background: rgba(241, 102, 92, 0.1);
  padding: 0px 10px;
  border-radius: 5px;
  width: fit-content;
}
</style>
