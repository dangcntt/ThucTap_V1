<script>
import MetisMenu from "metismenujs/dist/metismenujs";

import Vue from "vue";
import {mapState} from "vuex";

/**
 * Sidenav component
 */
export default {
  data() {
    return {
      menuData: null,
      checkMounted: true,
      menu : []
    };
  },
  created() {
   //
    var currentUser = localStorage.getItem("auth-user");

    if (currentUser) {
      let data = JSON.parse(currentUser)
      if (data && data.menu)
        this.menu = data.menu;
      // console.log("LOG ON CREATED : SIDE - NAV ", this.menu)
    }
  },
  computed: {
    ...mapState('snackBarStore', ['isMenuAction'])
  },
  watch: {
    isMenuAction: {
      deep: true,
      handler(val) {
        var currentMenu = localStorage.getItem("currentMenuId")
        var menuRef = new MetisMenu("#side-menu");
        var links = document.getElementsByClassName("side-nav-link-ref active");
        var chil = links[0]
        if (chil) {
          chil.classList.remove("active")
          var parent = chil.parentElement;
          if (parent)
            parent.classList.remove("mm-active");
        }
        this.handleMenuActive()
      }
    },
  },
  mounted: function () {
    var currentMenu = localStorage.getItem("currentMenuId")

    var menuRef = new MetisMenu("#side-menu");
    var links = document.getElementsByClassName("side-nav-link-ref");

    var matchingMenuItem = null;
    const paths = [];
    for (var i = 0; i < links.length; i++) {
      paths.push({link: links[i].getAttribute("pathname"), menuId: links[i].getAttribute("menuId")});
    }
    var itemIndex = paths.findIndex(x => x.link = window.location.pathname && x.menuId == currentMenu);
    if (itemIndex === -1) {
      const strIndex = window.location.pathname.lastIndexOf("/");
      const item = window.location.pathname.substr(0, strIndex).toString();
      matchingMenuItem = links[paths.findIndex(x => x.link == item)];
    } else {
      matchingMenuItem = links[itemIndex];
    }
    if (matchingMenuItem) {
      matchingMenuItem.classList.add("active");
      var parent = matchingMenuItem.parentElement;

      /**
       * TODO: This is hard coded way of expading/activating parent menu dropdown and working till level 3.
       * We should come up with non hard coded approach
       */
      if (parent) {
        parent.classList.add("mm-active");
        const parent2 = parent.parentElement.closest("ul");
        if (parent2 && parent2.id !== "side-menu") {
          parent2.classList.add("mm-show");

          const parent3 = parent2.parentElement;
          if (parent3) {
            parent3.classList.add("mm-active");
            var childAnchor = parent3.querySelector(".has-arrow");
            var childDropdown = parent3.querySelector(".has-dropdown");
            if (childAnchor) childAnchor.classList.add("mm-active");
            if (childDropdown) childDropdown.classList.add("mm-active");

            const parent4 = parent3.parentElement;
            if (parent4 && parent4.id !== "side-menu") {
              parent4.classList.add("mm-show");
              const parent5 = parent4.parentElement;
              if (parent5 && parent5.id !== "side-menu") {
                parent5.classList.add("mm-active");
                const childanchor = parent5.querySelector(".is-parent");
                if (childanchor && parent5.id !== "side-menu") {
                  childanchor.classList.add("mm-active");
                }
              }
            }
          }
        }
      }
    }
  },
  methods: {
    toggleCollapse(item) {
      item.collapsed = !item.collapsed;
    },
    handleMenuActive() {
      var currentMenu = localStorage.getItem("currentMenuId")

      var menuRef = new MetisMenu("#side-menu");
      var links = document.getElementsByClassName("side-nav-link-ref");

      var matchingMenuItem = null;
      const paths = [];
      for (var i = 0; i < links.length; i++) {
        paths.push({link: links[i].getAttribute("pathname"), menuId: links[i].getAttribute("menuId")});
      }
      var itemIndex = paths.findIndex(x => x.link = window.location.pathname && x.menuId == currentMenu);
      if (itemIndex === -1) {
        const strIndex = window.location.pathname.lastIndexOf("/");
        const item = window.location.pathname.substr(0, strIndex).toString();
        matchingMenuItem = links[paths.findIndex(x => x.link == item)];
      } else {
        matchingMenuItem = links[itemIndex];
      }

      if (matchingMenuItem) {
        matchingMenuItem.classList.add("active");
        var parent = matchingMenuItem.parentElement;

        /**
         * TODO: This is hard coded way of expading/activating parent menu dropdown and working till level 3.
         * We should come up with non hard coded approach
         */
        if (parent) {
          parent.classList.add("mm-active");
          const parent2 = parent.parentElement.closest("ul");
          if (parent2 && parent2.id !== "side-menu") {
            parent2.classList.add("mm-show");

            const parent3 = parent2.parentElement;
            if (parent3) {
              parent3.classList.add("mm-active");
              var childAnchor = parent3.querySelector(".has-arrow");
              var childDropdown = parent3.querySelector(".has-dropdown");
              if (childAnchor) childAnchor.classList.add("mm-active");
              if (childDropdown) childDropdown.classList.add("mm-active");

              const parent4 = parent3.parentElement;
              if (parent4 && parent4.id !== "side-menu") {
                parent4.classList.add("mm-show");
                const parent5 = parent4.parentElement;
                if (parent5 && parent5.id !== "side-menu") {
                  parent5.classList.add("mm-active");
                  const childanchor = parent5.querySelector(".is-parent");
                  if (childanchor && parent5.id !== "side-menu") {
                    childanchor.classList.add("mm-active");
                  }
                }
              }
            }
          }
        }
      }
    },
    /**
     * Returns true or false if given menu item has child or not
     * @param item menuItem
     */
    hasItems(item) {
      return item.children !== undefined ? item.children?.length > 0 : false;
    },
    toggleMenu(event) {
      event.currentTarget.nextElementSibling.classList.toggle("mm-show");
    },
    handleGetIdMenu(path, id) {
      Vue.prototype.menuId = id;
      localStorage.setItem("currentMenuId", id);
      if (path != window.location.pathname)
        this.$router.push(path);
      this.$store.dispatch("snackBarStore/clickMenu", !this.$store.state.snackBarStore.isMenuAction)
    },
    getCount(id) {
      var resultData = this.menu.filter(x => {
        return x.id == id;
      })
    //  console.log("LOG ID GET COUNT : ", id , resultData)
      if (resultData.length > 0)
      {
        return resultData[0].soLuong;
      }
      return 0 ;
    },
  },
};
</script>

<template>
  <div id="sidebar-menu">
    <!-- Left Menu Start -->
    <ul id="side-menu" class="metismenu list-unstyled">
      <template v-for="item in menu">
        <li class="menu-title" v-if="item != null && item.isTitle" :key="item._id" style="color: #204b03n">
          <i class="mdi mdi-star" aria-hidden="true"></i>
          {{ item.name}}
          <i class="mdi mdi-star" aria-hidden="true"></i>
        </li>
        <template v-if="hasItems(item)">
          <li v-for="(value) in item.children" :key="value._id">
            <a :menuId="value._id" :pathname="value.link" style="cursor: pointer"
               @click="handleGetIdMenu(value.link, value._id)" class="side-nav-link-ref">
              <i :class="`bx ${value.icon}`" v-if="value.icon"></i>
              <span>{{ value.name}}</span>
              <span :class="`badge rounded-pill bg-${value.badge.variant} float-end`"
                    v-if="value.badge">{{ value.badge.text }}</span>
            </a>
          </li>
        </template>
      </template>
    </ul>
  </div>
<!--  <div id="sidebar-menu">-->
<!--    &lt;!&ndash; Left Menu Start &ndash;&gt;-->
<!--    <ul id="" class="metismenu list-unstyled" v-if="menu && menu.length > 0">-->
<!--      <li v-for="item in menu" :key="item._id">-->
<!--        <a :menuId="item._id" style="cursor: pointer; text-transform: uppercase">-->
<!--          <i v-if="item.icon" :class="`bx ${item.icon}`"></i>-->
<!--          <span>{{ item.name }}</span>-->
<!--        </a>-->
<!--        <ul v-if="item.children && item.children.length" class="list-unstyled">-->
<!--          <li v-for="value in item.children" :key="value.id" style="padding-left: 20px;">-->
<!--            <a :menuId="value.id" :pathname="value.link" style="cursor: pointer" @click="handleGetIdMenu(value.link, value.id)" class="side-nav-link-ref">-->
<!--              <i v-if="value.icon" :class="`bx ${value.icon}`"></i>-->
<!--              <span>{{ value.name }}</span>-->
<!--              <span v-if="value.badge" :class="`badge rounded-pill bg-${value.badge.variant} float-end`">{{ value.badge.text }}</span>-->
<!--            </a>-->
<!--          </li>-->
<!--        </ul>-->
<!--      </li>-->
<!--    </ul>-->

<!--  </div>-->

  <!-- Sidebar -->
</template>
<style type="text/css" scoped>
.mm-active > a {
  color: #000 !important;
}
#sidebar-menu ul li a:hover {
  color: rgb(181, 0, 39);
}
.mm-active > a i {
  color: #000 !important;
}
#sidebar-menu > ul > li > a:hover i {
  color: rgb(181, 0, 39);
}
#sidebar-menu ul li a:hover i {
  color: rgb(181, 0, 39);
}
.side-nav-link-ref {
  padding-left: 35px !important;
}
.menu-title{
  font-size: 14px !important;
  color: #000;
}
</style>

