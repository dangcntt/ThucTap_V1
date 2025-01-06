const addMessage = (item, show, code) => {
    console.log("LOG ADD MESSAGE : " , item)
    return {
        message: item.message,
        code: item.code
    }
}

export const notifyModel = {addMessage};
