import moment from "moment";
const toJson = (item) => {
    return {
        _id: item._id,
        name: item.name,
        code : item.code,
        sort: item.sort,
        listAction : item.listAction,
    }
}
const fromJson = (item) => {
    console.log("LOG FROM JSON UNIT ROLE: ",item)
    return {
        _id: item._id,
        name: item.name,
        code : item.code,
        sort: item.sort,
        listAction : item.listAction,
        createdAtShow: item.createdAtShow,
        lastModifiedShow: item.lastModifiedShow,
        createdBy: item.createdBy,
        modifiedBy: item.modifiedBy,
    }
}

const baseJson = () => {
    return {
        _id: null,
        name: null,
        code : null,
        sort: null,
        listAction : null,
        createdAtShow: null,
        lastModifiedShow: null,
        createdBy: null,
        modifiedBy: null,
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

export const  unitRoleModel = {
    toJson, fromJson, baseJson, toListModel
}
