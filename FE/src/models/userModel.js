const toJson = (item) => {
    return {
        _id: item._id,
        userName: item.userName,
        fullName: item.fullName,
        phone: item.phone,
        email: item.email,
        note: item.note,
        avatar: item.avatar,
        unitRole: item.unitRole,
        permissions: item.permissions,
        menu: item.menu
    }
}

const fromJson = (item) => {
    return {
        _id: item._id,
        usrName: item.userName,
        firstName: item.firstName,
        lastName: item.lastName,
        fullName: item.fullName,
        phone: item.phone,
        email: item.email,
        note: item.note,
        avatar: item.avatar,
        unitRole: item.unitRole,
        permissions: item.permissions,
        menu: item.menu
    }
}

const baseJson = () => {
    return {
        _id: null,
        usrName: null,
        fullName: null,
        phone: null,
        email: null,
        note: null,
        avatar: null,
        unitRole: null,
        permissions: null,
        menu: null
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
export const userModel = {
    toJson, fromJson, baseJson, toListModel
}
