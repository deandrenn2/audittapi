import { ApiClient } from "../../../../shared/helpers/ApiClient";
import { MsgResponse } from "../../../../shared/model";
import { EquivalenceModel } from "./EquivalenceModel";

export const getEquivalence = async (): Promise<MsgResponse<EquivalenceModel[]>> => {
    const url = `api/equivalents`;
    const response = await ApiClient.get<MsgResponse<EquivalenceModel[]>>(url);
    if (response.status !== 200) {
        return {
            isSuccess: false,
            message: "Error al obtener la Equivalencia",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const createEquivalenceServices = async (model: EquivalenceModel) => {
    const url = "api/equivalents";
    const response = await ApiClient.post(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear la Equivalncia",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}
export const updateEquivalenceServices = async (model: EquivalenceModel) => {
    const url = "api/equivalents";
    const response = await ApiClient.put(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al actualizar la escala",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const deleteEqvalenceServives = async (id: number) => {
    const url = `api/equivalents/${id}`;
    const response = await ApiClient.delete(url);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al eliminar la Equivalencia",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}
