import { ApiClient } from "../../../shared/helpers/ApiClient";
import { MsgResponse } from "../../../shared/model";
import { FunctionaryModel } from "./FuntionaryModel";

export const getFunctionary = async (): Promise<MsgResponse<FunctionaryModel[]>> => {
    const url = "api/functionaries";
    const response = await ApiClient.get<MsgResponse<FunctionaryModel[]>>(url);
    if (response.status !== 200) {
        return {
            isSuccess: false,
            message: "Error al obtener los funcionarios",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const createFunctionaryServices = async (
    model: FunctionaryModel
): Promise<MsgResponse<FunctionaryModel>> => {
    const url = "api/functionaries";
    const response = await ApiClient.post<MsgResponse<FunctionaryModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear el funcionario",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const updateFunctionaryServices = async (
    data: FunctionaryModel
): Promise<MsgResponse<FunctionaryModel>> => {
    const url = `api/functionaries/${data.id}`;
    const response = await ApiClient.put<MsgResponse<FunctionaryModel>>(url, data);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al actualizar el funcionario",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};

export const deleteFunctionaryServices = async (
    id: number
): Promise<MsgResponse<FunctionaryModel>> => {
    const url = `api/functionaries/${id}`;
    const response = await ApiClient.delete<MsgResponse<FunctionaryModel>>(url);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al eliminar el funcionario",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}
