import { ApiClient } from "../../../../shared/helpers/ApiClient";
import { MsgResponse } from "../../../../shared/model";
import { permissionsModel } from "./PermissionModel";

export const getPermission = async (): Promise<MsgResponse<permissionsModel[]>> => {
    const url = `api/permissions`;
    const response = await ApiClient.get<MsgResponse<permissionsModel[]>>(url);
    if (response.status !== 200) {
        return {
            isSuccess: false,
            message: "Error al obtener los permisos",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};

export const createPermissionServices = async (
    model: permissionsModel
): Promise<MsgResponse<permissionsModel>> => {
    const url = "api/permissions";
    const response = await ApiClient.post<MsgResponse<permissionsModel>>(url, model);
    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al crear el permiso",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }

    return response.data;
};

export const updatePermissionServices = async (
    model: permissionsModel
): Promise<MsgResponse<permissionsModel>> => {
    const url = `api/permissions${model.id}`;
    const response = await ApiClient.put<MsgResponse<permissionsModel>>(url, model);
    if (response.status !== 200 && response.status !== 201) {
        return {
            isSuccess: false,
            message: "Error al actualizar el permiso",
            isFailure: true,
            error: {
                code: response.status.toString(),
                message: response.statusText,
            },
        };
    }
    return response.data;
};

export const deletePermissionServices = async (
    id: number
  ): Promise<MsgResponse<string>> => {
    const url = `api/permissions/${id}`;
    const response = await ApiClient.delete<MsgResponse<string>>(url);
  
    if (response.status !== 200) {
      return {
        isSuccess: false,
        message: "Error al eliminar el permiso",
        isFailure: true,
        error: {
          code: response.status.toString(),
          message: response.statusText,
        },
      };
    }
  
    return response.data;
  };