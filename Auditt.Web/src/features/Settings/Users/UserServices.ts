
import { ApiClient } from "../../../shared/helpers/ApiClient";
import { MsgResponse } from "../../../shared/model";
import { UsersResponseModel } from "./UsersModel";

export const getUser = async (): Promise<MsgResponse<UsersResponseModel[]>> => {
    const url = `api/users`;
    const response = await ApiClient.get<MsgResponse<UsersResponseModel[]>>(url);
    if (response.status !== 200) {
        return {
            isSuccess: false,
            message: "Error al obtener los usuario",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
}

export const createUsertServices = async (
    model: UsersResponseModel
): Promise<MsgResponse<UsersResponseModel>> => {
    const url = "api/users";
    const response = await ApiClient.post<MsgResponse<UsersResponseModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear el usuario",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }
    return response.data;
};

export const updateUserServices = async (
    model: UsersResponseModel 
): Promise<MsgResponse<UsersResponseModel>> => {
    const url = "api/users";
    const response = await ApiClient.put<MsgResponse<UsersResponseModel>>(url, model);

    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al actualizar el usuario",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }
    return response.data;
};

