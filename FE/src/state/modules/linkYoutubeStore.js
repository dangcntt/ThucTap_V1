import {apiClient} from "@/state/modules/apiClient";

const controller = "LinkYtb";
export const actions = {
    async getAll({commit}) {
        return apiClient.get(controller + "/get-all-core");
    },
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async delete({commit}, id) {
        return await apiClient.post(controller + "/delete" , id);
    },
    async getById({commit}, id) {
        return apiClient.post(controller + "/get-by-id-core" , id);
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller +"/get-paging-params-core", params);
    },
    async update({commit, dispatch}, values) {
        return apiClient.put(controller +"/update", values);
    },
};
