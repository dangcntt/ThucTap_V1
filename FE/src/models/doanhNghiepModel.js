const toJson = (item, listMenu) => {
  return {
    _id: item._id,
    name: item.name,
    diaChi: item.diaChi,
    sdt: item.sdt,
    content: item.content,
    link: item.link,
  };
};
const fromJson = (item) => {
  return {
    _id: item._id,
    name: item.name,
    diaChi: item.diaChi,
    sdt: item.sdt,
    content: item.content,
    link: item.link,
  };
};

const baseJson = () => {
  return {
    _id: null,
    name: null,
    diaChi: null,
    sdt: null,
    content: null,
    link: null,
  };
};

const toListModel = (items) => {
  if (items.length > 0) {
    let data = [];
    items.map((value, index) => {
      data.push({
        id: value.id,
        name: value.label != null ? value.label : value.name,
      });
    });
    return data ?? [];
  }
  return [];
};

export const doanhNghiepModel = {
  toJson,
  fromJson,
  baseJson,
  toListModel,
};
