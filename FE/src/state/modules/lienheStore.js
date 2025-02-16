import {apiClient} from "@/state/modules/apiClient";
const controller = "LienHe";
export const actions = {
    async getAll({commit}) {
        return apiClient.get(controller +"/get-all");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-core", params);
    },
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async update({commit, dispatch}, values) {
        return apiClient.put(controller + "/update", values);
    },
    async delete({commit}, id) {
        return await apiClient.delete(controller + "/delete/" + id);
    },
    async getById({commit}, id) {
        return apiClient.post(controller + "/get-by-id-core" , id);
    }
};