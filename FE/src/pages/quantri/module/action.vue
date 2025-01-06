<script>
import Layout from "@/layouts/main";
import PageHeader from "@/components/page-header";
import {required} from "vuelidate/lib/validators";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {actionModel} from "@/models/actionModel";
import {moduleModel} from "@/models/moduleModel";
import {menuModel} from "@/models/menuModel";
import {unitRoleModel} from "@/models/unitRoleModel";
export default {
  page: {
    title: "Danh sách quyền",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader},
  data() {
    return {
      title: "Danh sách quyền",
      items: [
        {
          text: "Danh sách quyền",
          active: true,
        },
        {
          text: "Danh sách",
          active: true,
        }
      ],
      data: [],
      idModule : this.$route.params.id,
      isDetail: false,
      showModal: false,
      showDeleteModal: false,
      submitted: false,
      model: null,
      modelMenu: menuModel.baseJson(),
      sortDesc: false,
      currentPage: 1,
      perPage: 5,
      name : null,
      fields: [
        { key: 'STT',
          label: 'STT',
          class : "text-center td-stt",
          thClass: 'hidden-sortable' },
        {
          key: "name",
          label: "Tên quyền",
          sortable: true,
          thStyle:"text-align:center",
        },
        {
          key: "action",
          label: "Tên hành động",
          thStyle:"text-align:center",
          sortable: true,
        },
        {
          key: "sort",
          label: "Thứ tự",
          class : "text-center",
          thStyle: {width: '110px', minWidth: '110px'},
          thClass: 'hidden-sortable'
        },
        {
          key: 'process',
          label: 'Xử lý',
          class : "text-center",
          thStyle: {width: '110px', minWidth: '110px'},
          thClass: 'hidden-sortable'
        }
      ],
      tenModule : null,
    };
  },
  validations: {
    model: {
          listAction: {required},
    },
  },
  created() {
    this.handleDetail();
    this.model = actionModel.baseJson(this.$route.params.id)
    this.name = this.modelMenu.name;
  },
  watch: {
    model: {
      deep: true,
      handler(val) {
      }
    },
    // showModal(status) {
    //   if (status == false)
    //   {
    //     this.model = actionModel.baseJson();
    //     this.isDetail = false;
    //   }else {
    //     this.getModule()
    //   }
    // },
    showDeleteModal(val) {
      if (val == false) {
        this.model.id = null;
      }
    },
  },
  methods: {
    async handleDelete(){
      console.log("LOG HANDLE DELETED : ", this.model)
        await this.$store.dispatch("menuStore/deleteAction",this.model ).then((res) => {
          if (res.code === 0) {
         //   this.modelMenu.listAction?.filter(x => x !== this.model.listAction);
            this.modelMenu.listAction = res.data.listAction;
            this.model = actionModel.baseJson(this.$route.params.id)
            this.showDeleteModal= false;
          }
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
        });
      },
    async handleShowDeleteModal(item) {
      //console.log("LOG SHOW DELTED : ",item)
      this.model =  {_id : this.$route.params.id , listAction: item};
      this.showDeleteModal = true;
    },
    async handleSubmit(e) {
      e.preventDefault();
      console.log("LOG NGUYEN TUAN KIET SUBMIT :")
      this.submitted = true;
      this.$v.$touch();
      if (this.$v.model.$invalid) {
        return;
      } else {
        let loader = this.$loading.show({
          container: this.$refs.formQuyenContainer,
        });
        if (
            this.model._id != 0 &&
            this.model._id != null &&
            this.model._id
        ) {
          // Update model
          await this.$store.dispatch("menuStore/addAction", this.model).then((res) => {
            if (res.code === 0) {
              this.modelMenu.listAction = res.data.listAction;
              this.model = actionModel.baseJson();
              console.log("LOG DATA MODEL MENU : ", this.modelMenu)
              this.showModal = false;
            //  this.$refs.tblList.refresh();
            }
            this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res))
          });
        }
        loader.hide();
      }
      this.submitted = false;
    },
    async handleDetail() {
      await this.$store.dispatch("menuStore/getById", {_id : this.$route.params.id}).then((res) => {
        if (res.code === 0) {
          this.modelMenu = menuModel.fromJson(res.data);
      //    console.log("LOG MODEL DETAIL ", this.modelMenu)
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
  }
}
</script>
<template>
  <Layout>
    <PageHeader :title="title" :items="items"/>
    <div class="row">
      <div class="col-12">
        <div class="card">
          <div class="card-body">
            <div class="row mb-2">
              <div class="col-sm-12" style="float: right">
                <div class="text-sm-end">
                  <b-button type="button" class="btn-label cs-btn-primary mb-2 me-2" @click="showModal = true" size="sm">
                    <i class="mdi mdi-plus me-1 label-icon"></i> Thêm
                  </b-button>
                  <b-modal
                      v-model="showModal"
                      title="Thông tin hành động"
                      title-class="text-black font-18"
                      body-class="p-3"
                      hide-footer
                      centered
                      no-close-on-backdrop
                  >
                    <form @submit.prevent="handleSubmit"
                          ref="formQuyenContainer"
                    >
                      <div class="row">
                        <div class="col-12">
                          <div class="mb-3">
                            <label class="text-left">Tên nhóm quyền</label>
                            <input
                                id="idModule"
                                v-model="name"
                                type="text"
                                class="form-control"
                                disabled
                            />
                          </div>
                          <div class="mb-3">
                            <label class="text-left">Tên hành động</label>
                            <input
                                id="quyenName"
                                v-model="model.listAction"
                                type="text"
                                class="form-control"
                                placeholder="Nhập tên quyền"
                            />
                          </div>
                        </div>
                      </div>
                      <div class="text-end pt-2 mt-3">
                        <b-button variant="light" @click="showModal = false">
                          Đóng
                        </b-button>
                        <b-button
                            v-if="!isDetail"
                            type="submit" variant="success" class="ms-1 btn-red cs-btn-primary">Lưu
                        </b-button>
                      </div>
                    </form>
                  </b-modal>
                </div>
              </div>
            </div>
            <div class="row">
              <div class="col-12">
                    <div class="table-responsive mb-5">
                      <table class="table table-admin mb-0">
                        <thead>
                        <tr class="text-center">
                          <th style="width: 6%;">STT</th>
                          <th>Tên quyền</th>
                          <th>Hành động</th>
                          <th>Xử lý</th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr v-for="(item, index) in this.modelMenu.listAction" :key="index">
                          <td scope="row" class="text-center">{{index}}</td>
                          <td>{{name}}</td>
                          <td>{{item}}</td>
                          <td style="width: 80px; text-align: center!important;">
                            <div class=" form-check-danger ">
                              <button
                                  type="button"
                                  size="sm"
                                  class="btn btn-outline btn-sm"
                                  data-toggle="tooltip" data-placement="bottom" title="Xóa"
                                  v-on:click="handleShowDeleteModal(item)">
                                <i class="fas fa-trash-alt text-danger me-1"></i>
                              </button>
                            </div>
                          </td>
                        </tr>
                        </tbody>
                      </table>
                    </div>
                  </div>
                </div>
              </div>
            </div>
            <b-modal
                v-model="showDeleteModal"
                centered
                title="Xóa dữ liệu"
                title-class="font-18"
                no-close-on-backdrop
            >
              <p>
                Dữ liệu xóa sẽ không được phục hồi!
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
  </Layout>
</template>
<style>
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
.table>tbody> tr >td{
  padding: 10px;
  border: 1px solid #000;
}
</style>
