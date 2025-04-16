import { ApiClient } from "../../shared/helpers/ApiClient";
import { MsgResponse } from "../../shared/model";
import { ClientModel } from "./ClientModel";

export const getClients = async (): Promise<MsgResponse<ClientModel[]>> => {
	const url = `api/institutions`;
	const response = await ApiClient.get<MsgResponse<ClientModel[]>>(url);
	if (response.status !== 200) {
		return {
			isSuccess: false,
			message: "Error al obtener la organización",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};

// export const getPagerClients = async (
// 	page: number = 1,
// 	pageSize: number = 5
// ): Promise<MsgResponse<ClientPagerModel[]>> => {
// 	const url = `api/clients/pager?pageIndex=${page}&pageSize=${pageSize}`;
// 	const response = await ApiClient.get<MsgResponse<ClientPagerModel[]>>(url);
// 	if (response.status !== 200 && response.status !== 201) {
// 		return {
// 			isSuccess: false,
// 			message: "Error al obtener clientes",
// 			isFailure: true,
// 			error: {
// 				code: response.status.toString(),
// 				message: response.statusText,
// 			},
// 		};
// 	}
// 	return response.data;
// };

export const createClientServices = async (
	model: ClientModel
): Promise<MsgResponse<ClientModel>> => {
	const url = "api/institutions";
	const response = await ApiClient.post<MsgResponse<ClientModel>>(url, model);

	if (response.status !== 200 && response.status !== 201) {
		return {
			isSuccess: false,
			message: "Error al crear el cliente",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};

export const updateClientServices = async (
	model: ClientModel
): Promise<MsgResponse<ClientModel>> => {
	const url = "api/institutions";
	const response = await ApiClient.put<MsgResponse<ClientModel>>(url, model);

	if (response.status !== 200 && response.status !== 201) {
		return {
			isSuccess: false,
			message: "Error al actualizar el cliente",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};

export const deleteClientServices = async (
	id: number
): Promise<MsgResponse<ClientModel>> => {
	const url = `api/institutions/${id}`;
	const response = await ApiClient.delete<MsgResponse<ClientModel>>(url);

	if (response.status !== 200) {
		return {
			isSuccess: false,
			message: "Error al eliminar el cliente",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};
