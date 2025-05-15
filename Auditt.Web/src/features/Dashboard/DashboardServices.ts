import { ApiClient } from "../../shared/helpers/ApiClient";
import { MsgResponse } from "../../shared/model";
import { DashboardMoldel } from "./DashboardModel";

export const getDashboard = async (): Promise<MsgResponse<DashboardMoldel>> => {
	const url = `api/dashboard/count `;
	const response = await ApiClient.get<MsgResponse<DashboardMoldel>>(url);
	if (response.status !== 200) {
		return {
			isSuccess: false,
			message: "Error al obtener contador ",
			isFailure: true,
			error: {
				code: response.status.toString(),
				message: response.statusText,
			},
		};
	}

	return response.data;
};
