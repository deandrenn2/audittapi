import { ApiClient } from "../../shared/helpers/ApiClient";
import { MsgResponse } from "../../shared/model";
import { QuarterlyModel } from "./QuarterlyModel";

export const getQuartely = async (): Promise<MsgResponse<QuarterlyModel[]>> => {
    const url = `api/datacuts`;
    const response = await ApiClient.get<MsgResponse<QuarterlyModel[]>>(url);

    if (response.status !== 200) {
        return {
            isSuccess: false,
            message: "Error al obtener los pacientes",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const createQuarterlyServices = async ( 
    model: QuarterlyModel
): Promise<MsgResponse<QuarterlyModel>> => {
    const url = "api/datacuts";
    const response = await ApiClient.post<MsgResponse<QuarterlyModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear el paciente",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const updateQuarterlyServices = async (
    model: QuarterlyModel
): Promise<MsgResponse<QuarterlyModel>> => {
    const url = `api/datacut/{id}`;
    const response = await ApiClient.put<MsgResponse<QuarterlyModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al actualizar el paciente",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }
    return response.data;
}

export const deleteQuarterlyServices = async (
): Promise<MsgResponse<QuarterlyModel>> => {
    const url = `api/datacut/{id}`;
    const response = await ApiClient.delete<MsgResponse<QuarterlyModel>>(url);
    
    if (response.status !== 200){
        return{
            isSuccess: false,
            message: "Error al eliminar el paciente",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        }
    }
    return response.data;
};
