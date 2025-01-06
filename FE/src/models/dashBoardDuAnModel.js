import moment from "moment";
const toJson = (item) => {
    return {
      id: item.id,
      tongDA : item.tongDA,
      tongDAHoanThanh : item.tongDAHoanThanh,
      tongDATreHan : item.tongDATreHan,
      tongDAKhoKhan : item.tongDAKhoKhan,
      duAnTungNam:  item.duAnTungNam,
      duAnTheoNam :  item.duAnTheoNam,
      duAnTheoCoQuan :  item.duAnTheoCoQuan,
      duAnTheoGiaiDoan:  item.duAnTheoGiaiDoan,
      duAnTheoNhomDuAn :  item.duAnTheoNhomDuAn,
      duAnTheoLinhVuc : item.duAnTheoLinhVuc
    }
}
const fromJson = (item) => {
    return {
        id: item.id,
       tongDA : item.tongDA,
       tongDAHoanThanh : item.tongDAHoanThanh,
       tongDATreHan : item.tongDATreHan,
       tongDAKhoKhan : item.tongDAKhoKhan,
       duAnTungNam:  item.duAnTungNam,
       duAnTheoNam :  item.duAnTheoNam,
       duAnTheoCoQuan :  item.duAnTheoCoQuan,
       duAnTheoGiaiDoan:  item.duAnTheoGiaiDoan,
       duAnTheoNhomDuAn :  item.duAnTheoNhomDuAn,
       duAnTheoLinhVuc : item.duAnTheoLinhVuc
    }
}

const baseJson = () => {
    return {
      id: null,
      tongDA : null,
      tongDAHoanThanh : null,
      tongDATreHan : null,
      tongDAKhoKhan : null,
      duAnTungNam:  null,
      duAnTheoNam :  null,
      duAnTheoCoQuan :  null,
      duAnTheoGiaiDoan:  null,
      duAnTheoNhomDuAn : null,
      duAnTheoLinhVuc : null,
    }
}

const toListModel = (items) =>{
    if(items.length > 0){
        let data = [];
        items.map((value, index) =>{
            data.push(fromJson(value));
        })
        return data??[];
    }
    return [];
}

export const dashBoardDuAnModel = {
    toJson, fromJson, baseJson, toListModel
}
