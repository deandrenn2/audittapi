import { ApiClient } from "../../../shared/helpers/ApiClient";
import { ScaleModel } from "./ScaleModel";




export const createScale = async (model: ScaleModel) => {
    const url = "api/scales";
    const response = await ApiClient.post(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear la escala",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}
export const updateScale = async (model: ScaleModel) => {
    const url = "api/scales";
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
export const deleteScale = async (id: number) => {
    const url = `api/scales/${id}`;
    const response = await ApiClient.delete(url);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al eliminar la escala",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}
