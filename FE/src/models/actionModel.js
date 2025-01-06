const toJson = (item) => {
    return {
        _id: item._id,
        listAction: item.listAction,
    }
}

const fromJson = (item) => {
    return {
        _id: item._id,
        listAction: item.listAction,
    }
}

const baseJson = (id) => {
    return {
        _id: id,
        listAction: null,
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

export const actionModel = {
    toJson, fromJson, baseJson, toListModel
}
