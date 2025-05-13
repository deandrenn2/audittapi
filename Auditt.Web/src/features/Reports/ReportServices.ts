import { ApiClient } from "../../shared/helpers/ApiClient";
import { MsgResponse } from "../../shared/model";
import { ReportGlobalModel, ReportQuestionAdherenceModel } from "./ReportModel";

export const GetReportGlobal = async (
	idDataCut: number,
	idInstitution: number,
	idGuide: number
): Promise<MsgResponse<ReportGlobalModel>> => {
	const url = `api/reports/${idDataCut}`;
	const params = new URLSearchParams();
	params.append("idInstitution", idInstitution.toString());
	params.append("idGuide", idGuide.toString());
	const response = await ApiClient.get<MsgResponse<ReportGlobalModel>>(url, {
		params,
	});

	if (response.status !== 200) {
		return {
			isSuccess: false,
			message: "Error al obtener los datos",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};

export const GetReportQuestionAdherence = async (
	idDataCut: number,
	idInstitution: number,
	idGuide: number
): Promise<MsgResponse<ReportQuestionAdherenceModel[]>> => {
	const url = `api/reports/question/${idDataCut}`;
	const params = new URLSearchParams();
	params.append("idInstitution", idInstitution.toString());
	params.append("idGuide", idGuide.toString());
	const response = await ApiClient.get<
		MsgResponse<ReportQuestionAdherenceModel[]>
	>(url, {
		params,
	});

	if (response.status !== 200) {
		return {
			isSuccess: false,
			message: "Error al obtener los datos",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};

export const GetReportFunctionaryAdherence = async (
	idDataCut: number,
	idFunctionary: number,
	idInstitution: number,
	idGuide: number
): Promise<MsgResponse<ReportQuestionAdherenceModel[]>> => {
	const url = `api/reports/functionaries/${idDataCut}/${idFunctionary}`;
	const params = new URLSearchParams();
	params.append("idInstitution", idInstitution.toString());
	params.append("idGuide", idGuide.toString());
	const response = await ApiClient.get<
		MsgResponse<ReportQuestionAdherenceModel[]>
	>(url, {
		params,
	});

	if (response.status !== 200) {
		return {
			isSuccess: false,
			message: "Error al obtener los datos",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};
