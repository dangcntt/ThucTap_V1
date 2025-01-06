import {apiClient} from "@/state/modules/apiClient";

const controller = "UnitRole";
export const actions = {
    async getAll({commit}) {
        return apiClient.get(controller + "/get-all-core");
    },
    async getPagingParams({commit}, params) {
        return apiClient.post(controller + "/get-paging-params-core", params);
    },
    async create({commit}, values) {
        return apiClient.post(controller + "/create", values);
    },
    async update({commit, dispatch}, values) {
        return apiClient.post(controller + "/update", values);
    },
    async updateAction({commit, dispatch}, values) {
        return apiClient.post(controller + "/update-action", values);
    },
    async delete({commit}, id) {
        return await apiClient.post(controller +"/delete" , id);
    },
    async getById({commit}, id) {
        return apiClient.post(controller + "/get-by-id-core" , id);
    }
};
