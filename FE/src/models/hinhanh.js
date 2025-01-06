const toJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        name:item.name,
        moTa: item.moTa
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        file: item.file,
        name:item.name,
        moTa: item.moTa
    }
}

const baseJson = () => {
    return {
        _id: null,
        file: null,
        name:null,
        moTa: null
    }
}


export const  hinhAnhModel = {
    toJson, fromJson, baseJson
}
