const toJson = (item) => {
  return {
    _id: item._id,
    name: item.name,
    path: item.path,
    icon: item.icon,
    parentId: item.parentId,
    level: item.level,
    sort: item.sort,
    listAction: item.listAction,
    isTieuDe :  item.isTieuDe,
    isMoTa:  item.isMoTa,
    isTrichYeu:  item.isTrichYeu,
    isKyHieu:  item.isKyHieu,
    isNgayXuatBan: item.isNgayXuatBan,
    isAnhDaiDien:  item.isAnhDaiDien,
    isNgayKy: item.isNgayKy,
    isNoiDung:  item.isNoiDung,
    isTepTin:  item.isTepTin,
    isTrangChu:  item.isTrangChu,
    soLuongTin: item.soLuongTin,
    isMenu:  item.isMenu,
    isDanhMuc: item.isDanhMuc,
  };
};

const fromJson = (item) => {
  return {
    _id: item._id,
    name: item.name,
    path: item.path,
    icon: item.icon,
    parentId: item.parentId,
    level: item.level,
    sort: item.sort,
    listAction: item.listAction,
    label: item.label,
    subItems: item.subItems || [],
    isTieuDe :  item.isTieuDe,
    isMoTa:  item.isMoTa,
    isTrichYeu:  item.isTrichYeu,
    isKyHieu:  item.isKyHieu,
    isNgayXuatBan: item.isNgayXuatBan,
    isAnhDaiDien:  item.isAnhDaiDien,
    isNgayKy: item.isNgayKy,
    isNoiDung:  item.isNoiDung,
    isTepTin:  item.isTepTin,
    isTrangChu:  item.isTrangChu,
    soLuongTin: item.soLuongTin,
    isMenu:  item.isMenu,
    isDanhMuc: item.isDanhMuc,
  };
};

const baseJson = () => {
  return {
    _id: null,
    name: null,
    path: null,
    icon: null,
    parentId: null,
    listAction: null,
    level: 0,
    sort: 0,
    soLuongTin: null,
    isTieuDe :  false,
    isMoTa:  false,
    isTrichYeu:  false,
    isKyHieu:  false,
    isNgayXuatBan: false,
    isAnhDaiDien:  false,
    isNgayKy: false,
    isNoiDung:  false,
    isTepTin:  false,
    isTrangChu:  false,
    isMenu:  false,
    isDanhMuc: false,


  };
};

const toListModel = (items) => {
  if (items.length > 0) {
    let data = [];
    items.map((value, index) => {
      data.push(fromJson(value));
    });
    return data ?? [];
  }
  return [];
};

export const menuCongDanModel = {
  toJson,
  fromJson,
  baseJson,
  toListModel,
};
