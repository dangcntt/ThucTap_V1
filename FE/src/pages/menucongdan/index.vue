<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {menuModel} from "@/models/menuModel";
import Treeselect from "@riophae/vue-treeselect";

export default {
  page: {
    title: "Danh mục Menu Công Dân",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader, Treeselect},
  data() {
    return {
      data: [],
      title: "Menu",
      items: [
        {
          text: "Menu",
          active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      showModal: false,
      showDeleteModal: false,
      submitted: false,
      model: menuModel.baseJson(),
      listParent: [],
      treeView: [],
      itemEvents: {
        mouseover: function () {
        },
        contextmenu: function () {
          arguments[2].preventDefault()
        }
      },
    };
  },
  validations: {
    model: {
      name: {required},
    },
  },
  created() {
 //   this.GetPagingParam();
 //    this.GetListParent();
     this.GetTreeList();
  },
  methods: {
    addCoQuanToModel(node, instanceId ){
      if(node.id){
        this.model.parentId = node.id;
      }
    },
    normalizer(node){
      if(node.children == null || node.children == 'null'){
        delete node.children;
      }
    },
    async GetTreeList(){
      await this.$store.dispatch("menuCongDanStore/getTreeList").then((res) => {
        this.treeView = res.data;
      //  console.log("LOG TREE VIEW : " , this.treeView)
      })
    },
    async handleDelete() {
      if (this.model._id != 0 && this.model._id != null && this.model._id) {
        await this.$store.dispatch("menuCongDanStore/delete", {_id :this.model._id}).then((res) => {
          if (res.code === 0) {
            this.GetTreeList();
            this.model = menuModel.baseJson();
            this.showDeleteModal = false;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        });
      }
    },
    handleResetForm() {
      this.model = menuModel.baseJson()
    },
    handleShowDeleteModal(id) {
      this.model._id = id;
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      console.log("LOG HANLE SUBMIT : " )
      e.preventDefault();
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formContainer,
        });
        if (
            this.model._id != 0 &&
            this.model._id != null &&
            this.model._id
        ) {
          // Update model
          console.log("LOG UPDATE MENU :" ,this.model)
          await this.$store.dispatch("menuCongDanStore/update", this.model).then((res) => {
            if (res.code === 0) {
              this.GetTreeList();
              this.showModal = false;
              this.model = {};
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        } else {
          console.log("LOG CREATE MENU :" ,this.model)
          // Create model
          await this.$store.dispatch("menuCongDanStore/create", this.model).then((res) => {
            if (res.code === 0) {
              this.GetTreeList();
              this.showModal = false;
              this.model = menuModel.baseJson();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
    async handleUpdate(id) {
      await this.$store.dispatch("menuCongDanStore/getById", {_id : id}).then((res) => {
        if (res.code === 0) {
          this.model = menuModel.toJson(res.data);
          this.showModal = true;
          this.listParent = this.listParent.filter(x => x._id != id);
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
   async itemClick (node) {
     await this.handleUpdate(node.model.id);
    }

  },
  computed: {

  },
  watch: {
    showModal(status) {
      if (status == false) this.model = menuModel.baseJson();
    },
    model() {
      return this.model;
    },
    showDeleteModal(val) {
      if (val == false) {
        this.model._id = null;
      }
    }
  },
};
</script>

<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-6">
        <div class="card">
          <div class="card-body">
            <v-jstree :data="treeView"   @item-click="itemClick"   :item-events="itemEvents"
                      text-field-name="label"
            ></v-jstree>

          </div>
        </div>
      </div>
      <div class="col-6">
        <div class="card">
          <div class="card-body">
            <form @submit.prevent="handleSubmit"
                  ref="formContainer"
            >
              <div class="row">
                <div class="col-12">
                  <input type="hidden" v-model="model._id"/>
                  <div class="mb-3">
                    <label class="text-left">Tên menu</label>
                    <span style="color: red">&nbsp;*</span>
                    <input
                        id="ten"
                        v-model="model.name"
                        type="text"
                        class="form-control"
                        placeholder="Nhập tên menu"
                                            :class="{
                                                      'is-invalid':
                                                        submitted && $v.model.name.$error,
                                                    }"
                    />
                    <div
                        v-if="submitted && !$v.model.name.required"
                        class="invalid-feedback"
                    >
                      Tên menu không được trống.
                    </div>
                  </div>
                  <div class="mb-3">
                    <label class="text-left">Path</label>
                    <input
                        id="path"
                        v-model="model.path"
                        type="text"
                        class="form-control"
                        placeholder="Nhập path"
                    />
                  </div>
                  <div class="mb-3">
                    <label class="text-left">Action</label>
                    <input
                        id="action"
                        v-model="model.action"
                        type="text"
                        class="form-control"
                        placeholder="Nhập action"
                    />
                  </div>
                  <div class="mb-3">
                    <label class="text-left">Icon</label>
                    <input
                        id="icon"
                        v-model="model.icon"
                        type="text"
                        class="form-control"
                        placeholder="Nhập icon"
                    />
                  </div>
                  <div class="mb-3">
                    <label class="text-left">Menu cha</label>
                    <treeselect
                        v-on:select="addCoQuanToModel"
                        :normalizer="normalizer"
                        :options="treeView"
                        :value="model.parentId"
                        :searchable="true"
                        :show-count="true"
                        :default-expand-level="1"

                    >
                      <label slot="option-label" slot-scope="{ node, shouldShowCount, count, labelClassName, countClassName }" :class="labelClassName">
                        {{ node.label }}
                        <span v-if="shouldShowCount" :class="countClassName">({{ count }})</span>
                      </label>
                    </treeselect>
                  </div>
                  <div class="mb-3">
                    <label class="text-left">Thứ tự</label>
                    <input
                        id="sort"
                        v-model="model.sort"
                        type="number"
                        class="form-control"
                        placeholder="Nhập thứ tự"
                    />
                  </div>
                </div>
              </div>
              <div class="text-end pt-2 mt-3">
                <b-button v-if="model._id" type="button" variant="warning" class="ms-1"
                          v-on:click="handleResetForm"
                > Đặt lại
                </b-button>
                <b-button v-if="model._id" type="button" variant="danger" class="ms-1"
                v-on:click="handleShowDeleteModal(model._id)"
                > Xóa Menu
                </b-button>
                <b-button type="submit" variant="success" class="ms-1 cs-btn-primary px-3"
                >Lưu Menu
                </b-button>
              </div>
            </form>
            <b-modal
                v-model="showDeleteModal"
                centered
                title="Xóa dữ liệu"
                title-class="font-18"
                no-close-on-backdrop
            >
              <p>
                Dữ liệu xóa sẽ không được phục hồi !
              </p>
              <template #modal-footer>
                <button v-b-modal.modal-close_visit
                        class="btn btn-outline-info m-1"
                        v-on:click="showDeleteModal = false">
                  Đóng
                </button>
                <button v-b-modal.modal-close_visit
                        class="btn btn-danger btn m-1"
                        v-on:click="handleDelete">
                  Xóa
                </button>
              </template>
            </b-modal>
          </div>
        </div>
      </div>
    </div>
  </Layout>
</template>
