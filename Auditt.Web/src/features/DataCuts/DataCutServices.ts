import { ApiClient } from "../../shared/helpers/ApiClient";
import { MsgResponse } from "../../shared/model";
import { DataCutModel } from "./DataCutModels";

export const GetDataCuts = async (): Promise<MsgResponse<DataCutModel[]>> => {
	const url = `api/datacuts`;
	const response = await ApiClient.get<MsgResponse<DataCutModel[]>>(url);
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
};

export const GetDataCutById = async (
	id: number
): Promise<MsgResponse<DataCutModel>> => {
	const url = `api/datacut/${id}`;
	const response = await ApiClient.get<MsgResponse<DataCutModel>>(url);
	if (response.status !== 200) {
		return {
			isSuccess: false,
			message: "Error al obtener el paciente",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};

export const createDataCutServices = async (
	model: DataCutModel
): Promise<MsgResponse<DataCutModel>> => {
	const url = "api/datacut";
	const response = await ApiClient.post<MsgResponse<DataCutModel>>(url, model);
	if (response.status !== 200 && response.status !== 201) {
		return {
			isSuccess: false,
			message: "Error al crear el corte",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};

export const updateDataCutServices = async (
	model: DataCutModel
): Promise<MsgResponse<DataCutModel>> => {
	const url = `api/datacut/${model.id}`;
	const response = await ApiClient.put<MsgResponse<DataCutModel>>(url, model);
	if (response.status !== 200 && response.status !== 201) {
		return {
			isSuccess: false,
			message: "Error al actualizar el corte",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}
	return response.data;
};

export const deleteDataCutServices = async (
	id: number
  ): Promise<MsgResponse<string>> => {
	const url = `api/datacut/${id}`;
	const response = await ApiClient.delete<MsgResponse<string>>(url);
  
	if (response.status !== 200) {
	  return {
		isSuccess: false,
		message: "Error al eliminar el corte",
		isFailure: true,
		error: {
		  code: response.status.toString(),
		  message: response.statusText,
		},
	  };
	}
  
	return response.data;
  };
  
