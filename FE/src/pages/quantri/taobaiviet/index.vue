<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {numeric, required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import Multiselect from "vue-multiselect";
import DatePicker from "vue2-datepicker";
import {VMoney} from "v-money";
import {pagingModel} from "@/models/pagingModel";
import Treeselect from "@riophae/vue-treeselect";
import {menuModel} from "@/models/menuModel";
import vue2Dropzone from "vue2-dropzone";
import {notifyModel} from "@/models/notifyModel";
import {contentModel, hoaModel} from "@/models/contentModel";
import axios from "axios";
import 'vue2-dropzone/dist/vue2Dropzone.min.css'
import {itemTreeModel} from "@/models/itemTreeModel";



export default {
  page: {
    title: "Tạo bài viết",
    meta: [{name: "description", content: appConfig.description}],
  },
  // eslint-disable-next-line vue/no-unused-components
  components: {Multiselect, Layout, PageHeader, DatePicker,Treeselect, vueDropzone: vue2Dropzone,
    'ckeditor-nuxt': () => {
      return import('@blowstack/ckeditor-nuxt')
    },
    //ckeditor: CKEditor.component

  },
  directives: {money: VMoney},
  data() {
    return {
      data: [],
      title: "Tạo bài viết",
      items: [
        {
          text: "TẠO BÀI VIẾT",
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
      model: hoaModel.baseJson(),
      modelContent: hoaModel.baseJson(),
      itemTree: itemTreeModel.baseJson(),
      pagination: pagingModel.baseJson(),
      treeView: [],
      listParent: [],
      //editor: ClassicEditor,
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
      url : `${process.env.VUE_APP_API_URL}filesminio/view/`,
      dropzoneOptionsChamSoc: {
        url: `${process.env.VUE_APP_API_URL}filesminio/upload`,
        acceptedFiles: ".jpg,.jpeg,.png,.gif",
        thumbnailHeight: 100,
        thumbnailWidth: 300,
        maxFiles: 1,
        maxFilesize: 30,
        headers: {"My-Awesome-Header": "header value"},
        addRemoveLinks: true
      },
      image: "",
      file: "",
      listMenu : [],
      itemsBaiViet:[{
        name: null,
        file: null
      }],
      targetIndex: 0,
    };
  },
  created() {
  },
  methods: {
    onClickFile(index) {
      this.targetIndex = index;
    },
    uploadFileChamSoc(files, response) {
      let fileSuccess = response.data;
      console.log('log success', response.data);

      if (response.code === 0) {
        this.itemsBaiViet[this.targetIndex].file = {
          fileId: fileSuccess.fileId,
          fileName: fileSuccess.fileName,
          ext: fileSuccess.ext,
          path: fileSuccess.path,
        };

        console.log('log model file item', this.itemsBaiViet[this.targetIndex].file);
      }
      //this.targetIndex ++;
    },
    getColorWithExtFile(ext) {
      if (ext == '.png' || ext == '.jpg'|| ext == '.jpeg' )
        return 'text-danger';

    },

    getIconWithExtFile(ext) {
      if (ext == '.png' || ext == '.jpg'|| ext == '.jpeg')
        return 'mdi mdi-file-image-outline';
    },
    deleteFile(index){
      if (this.itemsBaiViet != null && this.itemsBaiViet[index].file != null)
      {
        axios.post(`${process.env.VUE_APP_API_URL}filesminio/delete/${this.itemsBaiViet[index].file.fileId}`).then((response) => {
          this.itemsBaiViet[index].file = null;
          console.log('log model file remove', this.itemsBaiViet[index].file);
        }).catch((error) => {
          // Handle error here
          console.error('Error deleting file:', error);
        });
      }
    },
    onAccept(file) {
      this.image = file.name;
      this.file = file;
    },
    normalizer(node){
      if(node.children == null || node.children == 'null'){
        delete node.children;
      }
    },
    async GetTreeList(){
      await this.$store.dispatch("menuCongDanStore/getTreeListTBV").then((res) => {
        this.treeView = res.data;
      })
    },
    async handleSubmit(e) {
      e.preventDefault();
      this.submitted = true;
      let loader = this.$loading.show({
        container: this.$refs.formContainer,
      });
      console.log('loggggggg', this.itemsBaiViet)
      this.modelContent.chamSoc=this.itemsBaiViet
      await this.$store.dispatch("contentStore/create", hoaModel.toJson(this.modelContent)).then((res) => {
        if (res.code === 0) {
          this.GetTreeList();
          this.showModal = false;
          this.modelContent = hoaModel.baseJson();
          this.$refs.myVueDropzone.removeAllFiles();
          this.modelContent.noiDung = '';
        }
        this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        this.modelContent.ngayKy = null
      });
      loader.hide();
      this.submitted = false;
    },
    uploadFile(files,response) {
      let fileSuccess = response.data;
      console.log('log suscess', response.data)
      if (response.code == 0){
        this.modelContent.file.push({
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
    removeThisFile(files, error, xhr, index) {
      let fileCongViec = JSON.parse(files.xhr.response);
      if (fileCongViec.data && fileCongViec.data.fileId) {
        let idFile = fileCongViec.data.fileId;
        let resultData = this.itemsBaiViet[index].file.filter(x => {
          return x.fileId != idFile;
        })
        this.itemsBaiViet[index].file = resultData;
        //  console.log('log model file remove', this.modelContent.files)
      }
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
      if (this.modelContent != null && this.modelContent.avatar != null)
      {
        //  console.log("LOG this.modelContent : ", this.modelContent)
        axios.post(`${process.env.VUE_APP_API_URL}filesminio/delete/${this.modelContent.avatar.fileId}`).then((response) => {
          this.modelContent.files = null;
          //     console.log('log model file remove', this.modelContent.files);
        }).catch((error) => {
          // Handle error here
          //       console.error('Error deleting file:', error);
        });
      }
      if ( event.target &&  event.target.files.length > 0 ) {
        const formData = new FormData()
        formData.append('files', event.target.files[0])
        axios.post(`${process.env.VUE_APP_API_URL}filesminio/Upload`,formData).then((response) => {
          let resultData = response.data
          if (response.data.code == 0){
            this.modelContent.avatar={
              fileId: resultData.data.fileId,
              fileName: resultData.data.fileName,
              ext: resultData.data.ext,
              path: resultData.data.path
            };
          }
        }).catch((error) => {
          // Handle error here
          console.error('Error deleting file:', error);
        });

      }
    },
    addItem: function () {
      this.itemsBaiViet.push({
        name: null,
        file: null
      });
      this.targetIndex ++;
    },
    removeItem: function () {
      this.itemsBaiViet.pop({
        name: null,
        file: null
      });
      this.targetIndex --;
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
            <div class="col-6">
              <div class="mb-2">
                <label class="text-left mb-0">Tên loài hoa</label>
                <span style="color: red">&nbsp;*</span>
                <input
                    v-model="modelContent.name"
                    id="percent"
                    type="text"
                    class="form-control"
                    placeholder="Nhập tên loài hoa"
                />
              </div>
            </div>
            <div class="col-6">
              <div class="mb-2">
                <label class="text-left mb-0">Tên khoa học</label>
                <span style="color: red">&nbsp;*</span>
                <input
                    v-model="modelContent.engName"
                    id="percent"
                    type="text"
                    class="form-control"
                    placeholder="Nhập tên khoa học"
                />
              </div>
            </div>

            <div class="col-6">
              <div class="mb-2 ">
                <label for="formFileSm" class="text-left mb-0">Ảnh đại diện</label>
                <input
                    id="formFileSm"
                    name="file-input"
                    ref="fileInput"
                    type="file"
                    class="form-control"
                    @change="upload($event)"
                />
              </div>
            </div>
            <div class="col-6">
              <div class="mb-2">
                <label class="text-left mb-0">Sort</label>
                <span style="color: red">&nbsp;*</span>
                <input
                    v-model="modelContent.sort"
                    id="percent"
                    type="number"
                    class="form-control"
                    placeholder="Nhập vị trí"
                />
              </div>
            </div>
            <!-- <div class="col-12">
              <div class="form-check form-switch mb-2">
                <label class="text-left mb-2">Xuất bản</label>
                <input
                    class="form-check-input"
                    type="checkbox"
                    id="flexSwitchCheckChecked"
                    checked=""
                />
              </div>
            </div> -->
              <div class="col-md-12">
                <div class="mb-2 ">
                  <label class="text-left mb-0">Mô tả</label>
                  <span style="color: red">&nbsp;*</span>
                  <textarea
                      v-model="modelContent.moTa"
                      id="percent"
                      type="text"
                      class="form-control mota"
                      placeholder="Nhập mô tả"

                  >
                  </textarea>
                </div>
              </div>
              <div class="content-lis col-10" >
                <div class="cs-title-box">
                  <div class="cs-title py-2">
                    <span class="fw-normal font-size-13">QUÁ TRÌNH CANH TÁC</span>
                  </div>
                </div>
              </div>
              <div class="col-6">
                <div class="mb-2">
                  <label class="text-left mb-0">Giá thể</label>
                  <span style="color: red">&nbsp;*</span>
                  <textarea
                      v-model="modelContent.giaThe"
                      id="percent"
                      type="text"
                      class="form-control"
                      placeholder="Nhập giá thể"
                  >
                  </textarea>
                </div>
              </div>
              <div class="col-6">
                <div class="mb-2">
                  <label class="text-left mb-0">Trồng chậu</label>
                  <span style="color: red">&nbsp;*</span>
                  <textarea
                      v-model="modelContent.trongChau"
                      id="percent"
                      type="text"
                      class="form-control"
                      placeholder="Nhập trồng chậu"
                  >
                  </textarea>
                </div>
              </div>
              <div class="col-6">
                <div class="mb-2">
                  <label class="text-left mb-0">Tưới nước</label>
                  <span style="color: red">&nbsp;*</span>
                  <textarea
                      v-model="modelContent.tuoiNuoc"
                      id="percent"
                      type="text"
                      class="form-control"
                      placeholder="Nhập tưới nước"
                  >
                  </textarea>
                </div>
              </div>
              <div class="col-6">
                <div class="mb-2">
                  <label class="text-left mb-0">Bón phân</label>
                  <span style="color: red">&nbsp;*</span>
                  <textarea
                      v-model="modelContent.bonPhan"
                      id="percent"
                      type="text"
                      class="form-control"
                      placeholder="Nhập bón phân"
                  >
                  </textarea>
                </div>
              </div>
              <div class="col-6">
                <div class="mb-2">
                  <label class="text-left mb-0">Cắt tỉa</label>
                  <span style="color: red">&nbsp;*</span>
                  <textarea
                      v-model="modelContent.catTia"
                      id="percent"
                      type="text"
                      class="form-control"
                      placeholder="Nhập cắt tỉa"
                  >
                  </textarea>
                </div>
              </div>

              <div class="content-lis col-10" >
                <div class="cs-title-box">
                  <div class="cs-title py-2">
                    <span class="fw-normal font-size-13">QUÁ TRÌNH CHĂM SÓC CỦA CÂY</span>
                  </div>
                </div>
              </div>
            <div class="row" v-for="(item,index) in itemsBaiViet" :key="index">
              <div class="col-12">
                <div class="mb-2">
                  <label class="text-left mb-0">Mô tả về cây</label>
                  <span style="color: red">&nbsp;*</span>
                  <input
                      v-model="item.name"
                      id="percent"
                      type="text"
                      class="form-control"
                      placeholder="Nhập mô tả về cây"
                  />
                </div>
              </div>
              <div class="col-12" >
                <label class="text-left">Chọn ảnh (Kích thước 350 x 150) </label>
                <template  v-if="item.file" >
                  <a
                      class="ml-25"
                      :href="`${url}/${item.file.fileId}`"
                  ><i
                      :class="`${getColorWithExtFile(item.file.ext)} me-2 ${getIconWithExtFile(item.file.ext)}`"
                  ></i>{{item.file.fileName }}</a>
                  <b-button
                      variant="flat-danger"
                      class="btn-icon"
                      @click="deleteFile(index)"
                  >
                    <i class="mdi mdi-trash-can-outline"></i>
                  </b-button>
                </template>
                <vue-dropzone
                    id="index"
                    ref="myVueDropzone"
                    :use-custom-slot="true"
                    :options="dropzoneOptionsChamSoc"
                    @vdropzone-file-added="onClickFile(index)"
                    v-on:vdropzone-sending="sendingEvent"
                    v-on:vdropzone-removed-file="removeThisFile(index)"
                    v-on:vdropzone-success="uploadFileChamSoc"
                >
                  <div class="dropzone-custom-content">
                    <div class="mb-1">
                      <i class="display-4 text-muted bx bxs-cloud-upload"></i>
                    </div>
                    <h4>Kéo thả tệp hoặc click vào đây để tải tệp tin.</h4>
                  </div>
                </vue-dropzone>
              </div>
            </div>
            <div class="row col-md-4 mt-3">
              <b-button
                  style="width: auto; padding: 0; margin-left: 10px;"
                  variant="flat-danger"
                  class="btn-icon"
                  @click="addItem"
              >
                <i class="bx bx-plus"
                  style="background-color: #1976d2; font-size: 16px; color: #fff; padding: 5px; border-radius: 50%;"
                >
                </i>
              </b-button>
              <b-button
                  style="width: auto; padding: 0; margin-left: 10px;"
                  variant="flat-danger"
                  class="btn-icon"
                  @click="removeItem"
              >
                <i class="bx bx-trash"
                  style="background-color: #ff5252; font-size: 16px; color: #fff; padding: 5px; border-radius: 50%;"
                >
                </i>
              </b-button>
            </div>



              <div class="content-lis col-10" >
                <div class="cs-title-box">
                  <div class="cs-title py-2">
                    <span class="fw-normal font-size-13">CÂY GIỐNG</span>
                  </div>
                </div>
              </div>
              <div>
                <ckeditor-nuxt v-model="modelContent.cayGiong"
                              :config="editorConfig"  >
                </ckeditor-nuxt>
              </div>

              <div class="content-lis col-10" >
                <div class="cs-title-box">
                  <div class="cs-title py-2">
                    <span class="fw-normal font-size-13">SÂU BỆNH CHÍNH</span>
                  </div>
                </div>
              </div>
              <div>
                <ckeditor-nuxt v-model="modelContent.sauBenh"
                              :config="editorConfig"  >
                </ckeditor-nuxt>
              </div>

            <div class="content-lis col-10" >
              <div class="cs-title-box">
                <div class="cs-title py-2">
                  <span class="fw-normal font-size-13">NỘI DUNG BÀI VIẾT</span>
                </div>
              </div>
            </div>
            <div>
              <ckeditor-nuxt v-model="modelContent.noiDung"
                             :config="editorConfig"  >

              </ckeditor-nuxt>

            </div>
            <template >
              <div style="margin-top: 20px;">
                <vue-dropzone
                    id="dropzone"
                    ref="myVueDropzone"
                    :use-custom-slot="true"
                    :options="dropzoneOptions"
                    v-on:vdropzone-sending="sendingEvent"
                    v-on:vdropzone-removed-file="removeThisFile"
                    v-on:vdropzone-success="uploadFile"
                >
                  <div class="dropzone-custom-content">
                    <div class="mb-1">
                      <i class="display-4 text-muted bx bxs-cloud-upload"></i>
                    </div>
                    <h4>Kéo thả tệp hoặc click vào đây để tải tệp tin.</h4>
                  </div>
                </vue-dropzone>
              </div>
            </template>

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
