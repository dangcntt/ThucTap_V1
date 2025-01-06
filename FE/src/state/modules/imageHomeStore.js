import {apiClient} from "@/state/modules/apiClient";

const controller = "ImageHome";
export const actions = {
    async getAll({commit}) {
        return apiClient.get(controller + "/get-all-core");
    },
}