import {apiClient} from "@/state/modules/apiClient";

const controller = "Header";
export const actions = {
    async getAll({commit}) {
        return apiClient.get(controller + "/get-all");
    },
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async delete({commit}, id) {
        return await apiClient.post(controller + "/delete" , id);
    },
};
