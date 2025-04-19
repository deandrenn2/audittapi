import { ApiClient } from "../../../shared/helpers/ApiClient";
import { MsgResponse } from "../../../shared/model";
import { RolesModel } from "./RolesModel";

export const getRoles = async (): Promise<MsgResponse<RolesModel[]>> => {
    const url = `api/roles`;
    const response = await ApiClient.get<MsgResponse<RolesModel[]>>(url);
    if (response.status !== 200) {
        return {
            isSuccess: false,
            message: "Error al obtener los roles",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const createRoleServices = async (
    model: RolesModel
): Promise<MsgResponse<RolesModel>> => {
    const url = "api/roles";
    const response = await ApiClient.post<MsgResponse<RolesModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear el rol",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};

export const updateRoleServices = async (
    model: RolesModel
): Promise<MsgResponse<RolesModel>> => {
    const url = "api/roles";
    const response = await ApiClient.put<MsgResponse<RolesModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al actualizar el rol",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};

export const deleteRoleServices = async (
    id: number
): Promise<MsgResponse<RolesModel>> => {
    const url = `api/roles/${id}`;
    const response = await ApiClient.delete<MsgResponse<RolesModel>>(url);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al eliminar el rol",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};
