import { ApiClient } from "../../shared/helpers/ApiClient";
import { MsgResponse } from "../../shared/model";
import {
	AssessmentCreateModel,
	AssessmentDetailModel,
	AssessmentListModel,
} from "./AssessmentModel";

export const GetAssessments = async (): Promise<
	MsgResponse<AssessmentListModel[]>
> => {
	const url = `api/assessments`;
	const response = await ApiClient.get<MsgResponse<AssessmentListModel[]>>(url);

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

export const GetAssessmentById = async (
	id: number
): Promise<MsgResponse<AssessmentDetailModel>> => {
	const url = `api/assessments/${id}`;
	const response = await ApiClient.get<MsgResponse<AssessmentDetailModel>>(url);

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

export const getAssessmentByDocument = async (
	identity: string
): Promise<MsgResponse<AssessmentDetailModel>> => {
	const url = `api/assessments/patients/${identity}`;
	const response = await ApiClient.get<MsgResponse<AssessmentDetailModel>>(url);

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

export const createAssessmentServices = async (
	model: AssessmentCreateModel
): Promise<MsgResponse<AssessmentCreateModel>> => {
	const url = "api/assessments";
	const response = await ApiClient.post<MsgResponse<AssessmentCreateModel>>(
		url,
		model
	);

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

export const deleteAssessmentServices = async (
	id: number
): Promise<MsgResponse<AssessmentCreateModel>> => {
	const url = `api/assessments/${id}`;
	const response = await ApiClient.delete<MsgResponse<AssessmentCreateModel>>(
		url
	);

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
