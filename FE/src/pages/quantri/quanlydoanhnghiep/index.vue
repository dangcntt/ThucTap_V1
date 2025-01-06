<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import Multiselect from "vue-multiselect";
import DatePicker from "vue2-datepicker";
import {VMoney} from "v-money";
import {pagingModel} from "@/models/pagingModel";
import Treeselect from "@riophae/vue-treeselect";
import vue2Dropzone from "vue2-dropzone";
import {notifyModel} from "@/models/notifyModel";
import axios from "axios";
import 'vue2-dropzone/dist/vue2Dropzone.min.css'
import{doanhNghiepModel} from "@/models/doanhNghiepModel";


export default {
  page: {
    title: "Quản lý doanh nghiệp",
    meta: [{name: "description", content: appConfig.description}],
  },
  // eslint-disable-next-line vue/no-unused-components
  components: {Multiselect, Layout, PageHeader, DatePicker,Treeselect, vueDropzone: vue2Dropzone,
    //ckeditor: CKEditor.component

  },
  directives: {money: VMoney},
  data() {
    return {
      data: [],
      title: "Quản lý doanh nghiệp",
      items: [
        {
          text: "Quản lý doanh nghiệp",
          active: true,
        }
      ],
      currentPage: 1,
      pageOptions: [5, 10, 25, 50, 100],
      sortBy: "age",
      filter: null,
      sortDesc: false,
      isShow : false,
      filterOn: [],
      numberOfElement: 1,
      totalRows: 1,
      perPage : 10,
      stt: 1,
      submitted: false,
      model: doanhNghiepModel.baseJson(),
      pagination: pagingModel.baseJson(),
      editorConfig: {
        toolbar: {
          items: [
            'heading', '|','style',
            'bold',
            'italic',
            'link',
            'bulletedList',
            'numberedList',
            '|',
            'outdent',
            'indent',
            '|',
            'imageUpload',
            'blockQuote',
            'insertTable',
            'mediaEmbed',
            'codeBlock',
            'alignment',
            'ckbox',
            'fontColor',
            'fontBackgroundColor',
            'fontFamily',
            'fontSize',
            'formatPainter',
            'highlight',
            'htmlEmbed',
            'tableOfContents',
            'undo',
            'redo'
          ],
          shouldNotGroupWhenFull: false
        },
        removePlugins: ['Title', 'ImageCaption'],
        simpleUpload: {
          uploadUrl: process.env.VUE_APP_API_URL + "filesminio/uploadCK",
          headers: {
            'Authorization': 'optional_token'
          },
        },

      },
      dropzoneOptions: {
        url: `${process.env.VUE_APP_API_URL}filesminio/upload`,
        acceptedFiles: ".jpg,.jpeg,.png,.gif,.pdf,.doc,.docx,.xls,.xlsx, .zip",
        thumbnailHeight: 100,
        thumbnailWidth: 300,
        maxFiles: 30,
        maxFilesize: 50,
        maxFileSizeInMB:50,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true
      },
      image: "",
      file: "",
    };
  },
  // validations: {
  //   modelContent: {
  //     name: {required},
  //     noiDung: {required},
  //     menu:{required},
  //   },
  // },
  validations() {
    return {
      modelContent : this.rules,
    }
  },
  computed: {
 
  },
  created() {
 
  },
  methods: {
    onAccept(file) {
      this.image = file.name;
      this.file = file;
    },

   
    normalizer(node){
      if(node.children == null || node.children == 'null'){
        delete node.children;
      }
    },
    

    async handleSubmit(e) {
      console.log("LOG 1: ", this.model)
      e.preventDefault();
      this.submitted = true;
    
        await this.$store.dispatch("doanhNghiepStore/create", doanhNghiepModel.toJson(this.model)).then((res) => {
          if (res.code === 0) {
            this.model = null;
        
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
       
    },
    uploadFile(files,response) {
      let fileSuccess = response.data;
      console.log('log suscess', response.data)
      if (response.code == 0){
        this.modelContent.files.push({
          fileId: fileSuccess.fileId,
          fileName: fileSuccess.fileName,
          ext: fileSuccess.ext,
          path: fileSuccess.path
        });
      }
      console.log('log model file', this.modelContent.files)

    },

    clearFile() {
      console.log("LOG NGUYEN TUAN KIET CLEAR ")
    },
    removeThisFile(files, error, xhr) {
      let fileCongViec = JSON.parse(files.xhr.response);
      if (fileCongViec.data && fileCongViec.data.fileId) {
        let idFile = fileCongViec.data.fileId;
        let resultData = this.modelContent.files.filter(x => {
          return x.fileId != idFile;
        })
        this.modelContent.files = resultData;
      //  console.log('log model file remove', this.modelContent.files)
      }

      // let fileCongViec = JSON.parse(files.xhr.response);
      // if (fileCongViec.data && fileCongViec.data.fileId) {
      //   let idFile = fileCongViec.data.fileId;
      //   // Call your delete API here
      //   axios.post(`${process.env.VUE_APP_API_URL}filesminio/delete/${idFile}`).then((response) => {
      //     let resultData = this.modelContent.files.filter((x) => {
      //       return x.fileId != idFile;
      //     });
      //     this.modelContent.files = resultData;
      //     console.log('log model file remove', this.modelContent.files);
      //   }).catch((error) => {
      //     // Handle error here
      //     console.error('Error deleting file:', error);
      //   });
      // }
    },
    sendingEvent(files, xhr, formData) {
      console.log('log file', files)
      formData.append("files", files);
    },
    formatDate(dateString) {
      // Chuyển đổi định dạng ngày từ DD/MM/YYYY sang YYYY-MM-dd
      const [day, month, year] = dateString.split('/');
      const formattedDate = `${year}-${month}-${day}`;
      return formattedDate;
    },
    async upload() {
      if (this.modelContent != null && this.modelContent.fileImage != null)
      {
        console.log("LOG this.modelContent : ", this.modelContent)
        axios.post(`${process.env.VUE_APP_API_URL}filesminio/delete/${this.modelContent.fileImage.fileId}`).then((response) => {
              this.modelContent.files = null;
              console.log('log model file remove', this.modelContent.files);
            }).catch((error) => {
              // Handle error here
              console.error('Error deleting file:', error);
            });
      }
      if ( event.target &&  event.target.files.length > 0 ) {
        const formData = new FormData()
        formData.append('files', event.target.files[0])
          axios.post(`${process.env.VUE_APP_API_URL}filesminio/UploadFileImage`,formData).then((response) => {
            // console.log("LOG UPDATE : ", response);
            let resultData = response.data
            if (response.data.code == 0){
              this.modelContent.fileImage={
                fileId: resultData.data.fileId,
                fileName: resultData.data.fileName,
                ext: resultData.data.ext,
                path: resultData.data.path
              };
               console.log("LOG UPDATE : ", this.modelContent);
            }
          }).catch((error) => {
            // Handle error here
            console.error('Error deleting file:', error);
          });

      }
    },
  },
  watch: {
    model: {
      deep: true,
      handler(val){
      }
    },
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="card">
      <div class="card-body">
        <form @submit.prevent="handleSubmit"
              ref="formContainer">
          <div class="row" ref="formContainer">
            <div class="content-lis col-md-6">
              <div class="cs-title-box">
                <div class="cs-title py-2" style="padding: 10px;">
                  <span class="fw-normal font-size-13">THÔNG TIN BÀI VIẾT</span>
                </div>
              </div>
            </div>

            <div class="text-end mt-2 col-md-6">
              <b-button type="submit" variant="success" class="ms-1 px-3 btn-luu"
              >Đăng bài viết
              </b-button>
            </div>
              <div class="col-12" >
              <div class="mb-2">
                <label class="text-left mb-0">Tên doanh nghiệp</label>
                <span style="color: red">&nbsp;*</span>
                <input
                    v-model="model.name"
                    id="percent"
                    type="text"
                    class="form-control"
                    placeholder="Nhập tên doanh nghiệp"
                    :class="{
                      'is-invalid':
                        submitted && $v.model.name != null && $v.model.name.$error,
                    }"
                />
                <div
                    v-if="submitted && $v.model.name != null   && !$v.model.name.required"
                    class="invalid-feedback"
                >
                  Tên doanh nghiệp không được trống.
                </div>
              </div>
            </div>
              <div class="col-md-12">
                <div class="mb-2 ">
                  <label class="text-left mb-0">Địa chỉ</label>
                  <textarea
                      v-model="model.diaChi"
                      id="percent"
                      type="text"
                      class="form-control diachi"
                      placeholder="Nhập địa chỉ"
                      :class="{
                      'is-invalid':
                        submitted && $v.model.diaChi != null && $v.model.diaChi.$error,
                    }"
                  >
                  </textarea>
                  <div
                      v-if="submitted && $v.model.diaChi != null   && !$v.model.diaChi.required"
                      class="invalid-feedback"
                  >
                   Địa chỉ không được trống.
                  </div>
                </div>
              </div>
              <div class="col-md-12">
                <div class="mb-2 ">
                  <label class="text-left mb-0">Số điên thoại</label>
                  <input
                      v-model="model.sdt"
                      id="percent"
                      type="text"
                      class="form-control sdt"
                      placeholder="Nhập số điện thoại"
                      :class="{
                        'is-invalid':
                          submitted && $v.model.sdt != null && $v.model.sdt.$error,
                      }"
                  >
                  
                  <div
                      v-if="submitted && $v.model.sdt != null   && !$v.model.sdt.required"
                      class="invalid-feedback"
                  >
                    Số điện thoại không được trống.
                  </div>
                </div>
              </div>
              <div class="col-12">
                <div class="mb-3">
                  <label class="text-left">Liên kết</label>
                  <span style="color: red">&nbsp;*</span>
                  <input
                      id="content"
                      v-model="model.link"
                      type="link"
                      class="form-control"
                      placeholder="Nhập liên kết"
                      :class="{
                      'is-invalid':
                        submitted && $v.model.link.$error,
                    }"
                  />
                  <div
                      v-if="submitted && !$v.model.link.required"
                      class="invalid-feedback"
                  >
                    Liên kết không được trống.
                  </div>
                </div>
              </div>
          </div>
        </form>

        
      </div>
    </div>

  </Layout>
</template>
<style>

.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
.vue-treeselect__menu{
  max-height: 165px !important;
}
.btn-luu{
  background-color: rgb(181, 0, 39);
  border: none;
}
.btn-luu:hover{
  background-color: rgb(170, 23, 54);
}
.mx-table .mx-table-date .table thead th, thead, th {
  background-color: rgb(255, 255, 255)!important;
  accent-color: #0e0e0e !important;
  border: none;
}
.mota{
  height: 100px;
}
.mx-calendar-content .cell.active {
  color: #fff;
  background-color: #1284e7 !important;
}
.page-content{
  min-height: 800px;
}
</style>
