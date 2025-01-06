
const toJson = (item , listMenu) => {
    return {
        _id: item._id,
        name : item.name,
        phanLoai: item.phanLoai,
        noiDung: item.noiDung,
        moTa: item.moTa,
        files: item.files,
        avatar : item.avatar,
        doanhNghiep : item.doanhNghiep,
        engName: item.engName,
        cayGiong: item.cayGiong,
        sauBenh: item.sauBenh,
        giaThe: item.giaThe,
        trongChau: item.trongChau,
        tuoiNuoc: item.tuoiNuoc,
        bonPhan: item.bonPhan,
        catTia: item.catTia,
        chamSoc: item.chamSoc,
        sort: item.sort,
    }
}
const fromJson = (item) => {
    return {
        _id: item._id,
        name : item.name,
        phanLoai: item.phanLoai,
        moTa: item.moTa,
        noiDung: item.noiDung,
        files: item.files,
        avatar : item.avatar,
        doanhNghiep : item.doanhNghiep,
        engName: item.engName,
        cayGiong: item.cayGiong,
        sauBenh: item.sauBenh,
        giaThe: item.giaThe,
        trongChau: item.trongChau,
        tuoiNuoc: item.tuoiNuoc,
        bonPhan: item.bonPhan,
        catTia: item.catTia,
        chamSoc: item.chamSoc,
        sort: item.sort,
    }
}

const baseJson = () => {
    return {
        _id: null,
        name: null,
        phanLoai: null,
        moTa: null,
        noiDung: null,
        files: [],
        avatar : null,
        doanhNghiep : null,
        engName: null,
        cayGiong: null,
        sauBenh: null,
        giaThe: null,
        trongChau: null,
        tuoiNuoc: null,
        bonPhan: null,
        catTia: null,
        chamSoc: [],
        sort: null,
    }
}

export const  hoaModel = {
    toJson, fromJson, baseJson
}
