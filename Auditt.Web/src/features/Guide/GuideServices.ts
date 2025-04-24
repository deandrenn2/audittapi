import { ApiClient } from "../../shared/helpers/ApiClient";
import { MsgResponse } from "../../shared/model";
import { GuideModel } from "./GuideModel";

export const getGuide = async (): Promise<MsgResponse<GuideModel[]>> => {
    const url = `api/guides`;
    const response = await ApiClient.get<MsgResponse<GuideModel[]>>(url);

    if (response.status !== 200) {
        return {
            isSuccess: false,
            message: "Error al obtener la guia",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const createGuideServices = async (
    model: GuideModel
): Promise<MsgResponse<GuideModel>> => {
    const url = "api/guides";
    const response = await ApiClient.post<MsgResponse<GuideModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear la guia",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};

export const updateGuideServices = async (
    model: GuideModel
): Promise<MsgResponse<GuideModel>> => {
    const url = "api/guides";
    const response = await ApiClient.put<MsgResponse<GuideModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al actualizar la guia",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};

export const deleteGuideServices = async (
    id: number
): Promise<MsgResponse<GuideModel>> => {
    const url = `api/guides/${id}`;
    const response = await ApiClient.delete<MsgResponse<GuideModel>>(url);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al eliminar la guia",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};
