<script>
import Layout from "../../layouts/main";
import PageHeader from "@/components/page-header";
import appConfig from "@/app.config";
import {notifyModel} from "@/models/notifyModel";
import {unitRoleModel} from "@/models/unitRoleModel";
export default {
  page: {
    title: "Thêm quyền vào vai trò",
    meta: [{name: "description", content: appConfig.description}],
  },
  components: {Layout, PageHeader},
  data() {
    return {
      node: [],
      model : unitRoleModel.baseJson() ,
      title: "Thiết lập quyền",
      items: [
        {
          text: "Vai trò",
          active: true,
        },
        {
          text: "Thêm quyền",
          active: true,
        }
      ],
      showModal: false,
      showDeleteModal: false,
      submitted: false,
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
  created() {
    this.GetTreeList();
    this.GetPermissionsByRoleId();
  },
  methods: {
      async GetTreeList(){
        await this.$store.dispatch("menuStore/getTreeListAndAction").then((res) => {
          this.treeView = res.data;
          // console.log("LOG TREE VIEW : " , this.treeView)
        })
    },
    async itemClick (node) {
      this.node = node.model
    //  console.log("LOG CLICK : ", this.node)
    },
    async GetPermissionsByRoleId() {
      await this.$store.dispatch("unitRoleStore/getById", {_id : this.$route.params.id}).then((res) => {
        if (res.code === 0) {
               this.model = res.data;
       //   console.log("LOG LIST : " , this.model)
        } else {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      });
    },
    statusPermission(permission) {
      let check = this.model.listAction.findIndex(x => x == permission);
    //  console.log("LOG NGUYE NTUAN KIET ", this.model.listAction)
      if (check == undefined || check == -1 )
      {
        return  false;
      }

      return true
    },
    onChecked(item) {
      let check = this.model.listAction.findIndex(x => x == item);
    //  console.log("LOG BEFORE : " ,  check)
      if (check == undefined || check == -1 )
      {
        this.model.listAction.push(item)
    //    console.log("LOG IF NE : " , this.model.listAction)
      }else {
        this.model.listAction = this.model.listAction?.filter(x => x !== item);
  //      console.log("LOG ELSE NE AFTER : " , this.model.listAction.length)
      }
     // console.log("LOG GIA TRI PERMISSIONS :  ", this.model.permissions)
    },
    async handleSubmitRole() {
      await this.$store.dispatch("unitRoleStore/updateAction", this.model).then((res) => {
        if (res.code === 0) {
          this.$store.dispatch("snackBarStore/addNotify", notifyModel.addMessage(res));
        }
      })
    }
  },
};
</script>
<template>
  <Layout>
    <PageHeader   :title="title" :items="items"/>
     <div class="row">
       <div class="col-lg-11">
       </div>
       <div class="col-1">
         <b-button
             type="button "
             class="btn-label mb-3 me-2 cs-btn-primary"
             @click="handleSubmitRole" size="xl"
         >
           <i class="mdi mdi-plus me-1 label-icon"></i> Lưu
         </b-button>
       </div>
     </div>
    <div class="row">
      <div class="col-4">
        <div class="card">
          <div class="card-body">
            <div class="font-weight-bold text-danger" style="margin-bottom: 5px" >
              {{this.model.label}}
            </div>
            <v-jstree  :data = "treeView"   text-field-name="label" @item-click="itemClick"></v-jstree>
          </div>
        </div>
      </div>
      <div class="col-8">
        <div class="card">
          <div class="card-body">
            <h4 class=" text-danger font-weight-bold font-size-20">{{this.node.label}}</h4>
            <div class="table-responsive">
              <table class="table mb-0">
                <thead>
                <tr>
                  <th>STT</th>
                  <th>Hành động</th>
                  <th>Xử lý</th>
                </tr>
                </thead>
                <tbody>
                <tr v-for="(item, index) in this.node.listAction" :key="index">
                  <td scope="row">{{index}}</td>
                  <td>{{item}}</td>
                  <td>
                    <div class="form-check form-check-danger">
                      <input
                          class="form-check-input"
                          type="checkbox"
                          id="formCheckcolor1"
                          :checked="statusPermission(item)"
                          @click = "onChecked(item)"
                      />
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
  </Layout>
</template>
<style>
.hidden-sortable:after, .hidden-sortable:before {
  display: none !important;
}
.table>tbody> tr >td{
  padding: 0px;
  line-height: 30px;
}
</style>
