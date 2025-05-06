import { ApiClient } from "../../../shared/helpers/ApiClient";
import { MsgResponse } from "../../../shared/model";
import { PatientsModel } from "./PantientsModel";
export const getPatients = async (): Promise<MsgResponse<PatientsModel[]>> => {
	const url = "api/patients";
	const response = await ApiClient.get<MsgResponse<PatientsModel[]>>(url);

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

export const createPatientsServices = async (
	model: PatientsModel
): Promise<MsgResponse<PatientsModel>> => {
	const url = "api/patients";
	const response = await ApiClient.post<MsgResponse<PatientsModel>>(url, model);

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
};

export const updatePatientsServices = async (
	model: PatientsModel
): Promise<MsgResponse<PatientsModel>> => {
	const url = `api/patients/${model.id}`;
	const response = await ApiClient.put<MsgResponse<PatientsModel>>(url, model);

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
};

export const deletePatientsServices = async (
	id: number
): Promise<MsgResponse<PatientsModel>> => {
	const url = `api/patients/${id}`;
	const response = await ApiClient.delete<MsgResponse<PatientsModel>>(url);

	if (response.status !== 200) {
		return {
			isSuccess: false,
			message: "Error al eliminar el paciente",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}
	return response.data;
};

export const getPatientById = async (
	id: number
): Promise<MsgResponse<PatientsModel>> => {
	const url = `api/patients/${id}`;
	const response = await ApiClient.get<MsgResponse<PatientsModel>>(url);

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

export const getPatientByDocument = async (
	identity: string
): Promise<MsgResponse<PatientsModel>> => {
	const url = `api/patients/history/${identity}`;
	const response = await ApiClient.get<MsgResponse<PatientsModel>>(url);

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
