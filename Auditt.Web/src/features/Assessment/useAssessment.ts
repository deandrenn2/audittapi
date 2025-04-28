import { useMutation, useQuery } from "@tanstack/react-query";
import {
	createAssessmentServices,
	deleteAssessmentServices,
	GetAssessmentById,
	GetAssessments,
} from "./AssessmentServices";
import { toast } from "react-toastify";

export const useAssessments = () => {
	const queryAssessments = useQuery({
		queryKey: ["Assessments"],
		queryFn: GetAssessments,
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
