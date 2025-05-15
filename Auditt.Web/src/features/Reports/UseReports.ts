import {
	GetReportFunctionaryAdherence,
	GetReportGlobal,
	GetReportQuestionAdherence,
} from "./ReportServices";
import useUserContext from "../../shared/context/useUserContext";
import { useQuery } from "@tanstack/react-query";

export const useReportsGlobal = (dataCut: number, idGuide: number) => {
	const { client } = useUserContext();
	const queryReportGlobal = useQuery({
		queryKey: ["ReportGlobal", dataCut, client?.id ?? 0, idGuide],
		queryFn: () => GetReportGlobal(dataCut, client?.id ?? 0, idGuide),
		enabled:
			dataCut !== undefined &&
			client?.id !== undefined &&
			idGuide !== undefined,
	});

	return {
		reportGlobal: queryReportGlobal.data?.data,
	};
};

export const useReportsQuestionAdherence = (
	dataCut: number,
	idGuide: number
) => {
	const { client } = useUserContext();
	const queryReportQuestionAdherence = useQuery({
		queryKey: ["ReportQuestionAdherence", dataCut, client?.id ?? 0, idGuide],
		queryFn: () =>
			GetReportQuestionAdherence(dataCut, client?.id ?? 0, idGuide),
		enabled:
			dataCut !== undefined &&
			client?.id !== undefined &&
			idGuide !== undefined,
	});

	return {
		reportQuestionAdherence: queryReportQuestionAdherence.data?.data,
	};
};

export const useReportsFunctionaryAdherence = (
	dataCut: number,
	idFunctionary: number,
	idGuide: number
) => {
	const { client } = useUserContext();
	const queryReportFunctionaryAdherence = useQuery({
		queryKey: [
			"ReportFunctionaryAdherence",
			dataCut,
			idFunctionary,
			client?.id ?? 0,
			idGuide,
		],
		queryFn: () =>
			GetReportFunctionaryAdherence(
				dataCut,
				idFunctionary,
				client?.id ?? 0,
				idGuide
			),
		enabled:
			dataCut !== undefined &&
			client?.id !== undefined &&
			idGuide !== undefined &&
			idFunctionary !== undefined,
	});

	return {
		reportFunctionaryAdherence: queryReportFunctionaryAdherence.data?.data,
	};
};
