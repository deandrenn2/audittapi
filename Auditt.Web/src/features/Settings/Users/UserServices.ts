import { UserResponseModel } from "../../../routes/Login/LoginModel";
import { ApiClient } from "../../../shared/helpers/ApiClient";
import { MsgResponse } from "../../../shared/model";

export const getUser = async (): Promise<MsgResponse<UserResponseModel[]>> => {
    const url = `api/users`;
    const response = await ApiClient.get<MsgResponse<UserResponseModel[]>>(url);
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

