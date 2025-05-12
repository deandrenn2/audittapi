import { useMutation, useQuery } from "@tanstack/react-query";
import {
	createAssessmentServices,
	deleteAssessmentServices,
	getAssessmentByDocument,
	GetAssessmentById,
	GetAssessments,
	saveAssessmentServices,
} from "./AssessmentServices";
import { toast } from "react-toastify";
import useUserContext from "../../shared/context/useUserContext";

export const useAssessments = () => {
	const { client } = useUserContext();
	const queryAssessments = useQuery({
		queryKey: ["Assessments", client?.id],
		queryFn: () => GetAssessments(client?.id ?? 0),
	});

	const createAssessment = useMutation({
		mutationFn: createAssessmentServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) {
					toast.info(data.message);
				}
				if (data?.error) {
					toast.info(data.error.message);
				}
			} else {
				toast.success(data.message);
				queryAssessments.refetch();
			}
		},
	});

	const deleteAssessment = useMutation({
		mutationFn: deleteAssessmentServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) {
					toast.info(data.message);
				}
				if (data?.error) {
					toast.info(data.error.message);
				}
			} else {
				toast.success(data.message);
				queryAssessments.refetch();
			}
		},
	});

	return {
		queryAssessments,
		assessments: queryAssessments.data?.data,
		createAssessment,
		deleteAssessment,
	};
};

export const useAssessmentById = (id: number) => {
	const queryAssessment = useQuery({
		queryKey: ["Assessment", id],
		queryFn: () => GetAssessmentById(id),
	});
	return { queryAssessment, assessment: queryAssessment.data?.data };
};

export const useAssessmentByDocumentMutation = () => {
	const getAssessmentByDocumentMutation = useMutation({
		mutationFn: getAssessmentByDocument,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) {
					toast.info(data.message);
				}
				if (data?.error) {
					toast.info(data.error.message);
				}
			} else {
				if (data.isSuccess) {
					toast.success(data.message);
				}
			}
		},
	});

	return { getAssessmentByDocumentMutation };
};

export const useSaveAssessment = () => {
	const saveAssessment = useMutation({
		mutationFn: saveAssessmentServices,
		onSuccess: (data) => {
			if (!data.isSuccess) {
				if (data?.message) {
					toast.info(data.message);
				}
				if (data?.error) {
					toast.info(data.error.message);
				}
			} else {
				toast.success(data.message);
			}
		},
	});

	return { saveAssessment };
};
