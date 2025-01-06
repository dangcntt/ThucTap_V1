const toJson = (item) => {
    console.log("LOG TO JSON ", item)
    return {
        id: item.id,
        name: item.label,
    }
}

const fromJson = (item) => {
    console.log("LOG FROM JSON ", item)
    return {
        id: item.id,
        name: item.label,
    }
}

const baseJson = () => {
    return {
        id: null,
        name: null,
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
export const itemTreeModel = {
    toJson, fromJson, baseJson, toListModel
}
