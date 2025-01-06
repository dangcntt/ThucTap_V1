import store from "@/state/store";
export default [
  {
    path: "/quan-ly-doanh-nghiep",
    name: "Quản lý doanh nghiệp",
    meta: {},
    component: () => import("../pages/quantri/quanlydoanhnghiep"),
  },
  {
    path: "/danh-sach-doanh-nghiep",
    name: "Danh sách doanh nghiệp",
    meta: {},
    component: () => import("../pages/quantri/danhsachdoanhnghiep"),
  },
  {
    path: "/hoa/chi-tiet/:code?",
    name: "Chi tiết hoa",
    meta: {},
    component: () => import("../pages/congdan/chitiet"),
  },

  {
    path: "/",
    name: "default",
    meta: {},
    component: () => import("../pages/congdan/home"),
  },
  {
    path: "/bg",
    name: "default",
    meta: {},
    component: () => import("../pages/congdan/background"),
  },

  // QUAN TRI

  {
    path: "/menu",
    name: "menu",
    meta: {},
    component: () => import("../pages/quantri/menu"),
  },

  {
    path: "/tai-khoan",
    name: "Tài khoản",
    meta: {},
    component: () => import("../pages/quantri/user"),
  },
  {
    path: "/tai-khoan/doi-mat-khau",
    name: "Đổi mật khẩu",
    meta: {},
    component: () => import("../pages/quantri/user/ChangePass"),
  },
  {
    path: "/menu-cong-dan",
    name: "menu",
    meta: {},
    component: () => import("../pages/quantri/menucongdan"),
  },
  {
    path: "/vai-tro",
    name: "Quyền",
    meta: {},
    component: () => import("../pages/quantri/unitRole"),
  },
  {
    path: "/vai-tro/thiet-lap-quyen/:id?",
    name: "Vai trò",
    meta: {},
    component: () => import("../pages/quantri/unitRole/addPermissions"),
  },

  {
    path: "/nhom-quyen",
    name: "Quản lý nhóm quyền",
    meta: {},
    component: () => import("../pages/quantri/module"),
  },
  {
    path: "/nhom-quyen/action/:id?",
    name: "Hành động",
    // meta: {},
    component: () => import("../pages/quantri/module/action"),
  },
  {
    path: "/thong-tin-ca-nhan",
    name: "Thông tin cá nhân",
    // meta: {},
    component: () => import("../pages/quantri/login/profile"),
  },

  {
    path: "/404",
    name: "404",
    component: require("../pages/utility/404").default,
  },
  {
    path: "/unauthorized",
    name: "401",
    component: require("../pages/utility/401").default,
  },
  {
    path: "*",
    redirect: "404",
  },
  {
    path: "/tao-bai-viet",
    name: "Tạo bài viết",
    // meta: {},
    component: () => import("../pages/quantri/taobaiviet"),
  },

  {
    path: "/danh-sach",
    name: "Danh sách bài viết",
    // meta: {},
    component: () => import("../pages/quantri/danhsachbaiviet"),
  },
  {
    path: "/danh-sach/chi-tiet/:id?",
    name: "Chi tiết bài viết",
    // meta: {},
    component: () =>
        import("../pages/quantri/danhsachbaiviet/capnhatbaiviet"),
  },
  {
    path: "/book",
    name: "Book",
    // meta: {},
    component: () => import("../pages/congdan/booklet"),
  },

];
